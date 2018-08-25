using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BackendApi.DB;
using BackendApi.DB.DataModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderListUploadController : ControllerBase
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly ABD_DbContext myContext;

        private const String CELL_TYP_STRING = "0";
        private const String CELL_TYP_NUMBER = "1";
        private const String CELL_TYP_DATE = "2";

        public OrderListUploadController(IHostingEnvironment hostingEnvironment, ABD_DbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            myContext = context;

        }
        // POST api/orderListUpload
        [HttpPost]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "UploadExcel";
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(contentRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        IWorkbook workbook = WorkbookFactory.Create(stream);
                        ISheet sheet = workbook.GetSheetAt(0);
                        if (sheet != null)
                        {
                            /*
                             * 订单信息
                             */
                            // 订单编号
                            String ORDER_NO = getCellValue(sheet, "C3", CELL_TYP_STRING);
                            if (String.IsNullOrEmpty(ORDER_NO))
                            {
                                // 订单号（主键）为空时，直接返回
                                return "";
                            }
                            // 合同编号
                            String CONTRACT_NO = getCellValue(sheet, "C4", CELL_TYP_STRING);
                            // 项目名称
                            String PROJECT_NM = getCellValue(sheet, "C5", CELL_TYP_STRING);
                            // 销售部门/销售员
                            String SALES_PERSON = getCellValue(sheet, "C6", CELL_TYP_STRING);
                            // 订货单位
                            String ORDER_UNIT = getCellValue(sheet, "C7", CELL_TYP_STRING);
                            // 应用工程师
                            String APPLICATION_ENGINEER = getCellValue(sheet, "F3", CELL_TYP_STRING);
                            // 下发日期
                            String DEPARTURE_DATE_STR = getCellValue(sheet, "F4", CELL_TYP_DATE);
                            // 交货日期
                            String DELIVERY_DATE_STR = getCellValue(sheet, "F5", CELL_TYP_DATE);
                            // 此订单页数
                            String BUMP_DETAIL_NUM = getCellValue(sheet, "F6", CELL_TYP_NUMBER);

                            // 构造JObject，并保存订单信息至数据库中
                            JObject entityObj = new JObject();
                            entityObj.Add("ORDER_NO", ORDER_NO);
                            entityObj.Add("CONTRACT_NO", CONTRACT_NO);
                            entityObj.Add("PROJECT_NM", PROJECT_NM);
                            entityObj.Add("SALES_PERSON", SALES_PERSON);
                            entityObj.Add("ORDER_UNIT", ORDER_UNIT);
                            entityObj.Add("APPLICATION_ENGINEER", APPLICATION_ENGINEER);
                            entityObj.Add("DEPARTURE_DATE", Convert.ToDateTime(DEPARTURE_DATE_STR));
                            entityObj.Add("DELIVERY_DATE", Convert.ToDateTime(DELIVERY_DATE_STR));

                            ORDER_DETAIL_MST excelEntity = entityObj.ToObject<ORDER_DETAIL_MST>();

                            var orderListEntity = myContext.ORDER_LIST_MST.Where(d => d.ORDER_NO.Equals(excelEntity.ORDER_NO));

                            if (orderListEntity.Count() == 0)
                            {
                                // INSERT
                                myContext.ORDER_LIST_MST.Add(excelEntity);
                                myContext.SaveChanges();
                                return "";
                            }
                            else
                            {
                                orderListEntity.First().CONTRACT_NO = excelEntity.CONTRACT_NO;
                                orderListEntity.First().PROJECT_NM = excelEntity.PROJECT_NM;
                                orderListEntity.First().ORDER_UNIT = excelEntity.ORDER_UNIT;
                                orderListEntity.First().SALES_PERSON = excelEntity.SALES_PERSON;
                                orderListEntity.First().DEPARTURE_DATE = excelEntity.DEPARTURE_DATE.ToLocalTime();
                                orderListEntity.First().DELIVERY_DATE = excelEntity.DELIVERY_DATE.ToLocalTime();
                                orderListEntity.First().REMARK = excelEntity.REMARK;
                                orderListEntity.First().APPLICATION_ENGINEER = excelEntity.APPLICATION_ENGINEER;
                                myContext.SaveChanges();
                                return "";
                            }
                            /*
                             * 水泵信息
                             */
                            // 泵名称
                            // 工位
                            // 型号
                            // 数量
                            // 流量
                            // 扬程
                            // 材质
                            // 机封
                            // 序列号
                            // 备注
                        }
                    }
                }
                return "Upload Successful.";
            }
            catch (System.Exception ex)
            {
                return "Upload Failed: " + ex.Message;
            }
        }
        /*
         * 根据单元格编号取得对应位置内容(目前只提供A-Z)
         */
        public String getCellValue(ISheet sheet, string cellNo, string celltyp)
        {
            char letter = cellNo.ToCharArray()[0];
            int rowIndex = Convert.ToInt32(cellNo.Substring(1));
            int cellIndex = letter - 65;
            IRow mRow = sheet.GetRow(rowIndex - 1);
            ICell mCell = mRow.GetCell(cellIndex);

            if (celltyp == CELL_TYP_STRING)
            {
                mCell.SetCellType(CellType.String);
                return mCell.StringCellValue;
            }
            else if (celltyp == CELL_TYP_NUMBER)
            {
                mCell.SetCellType(CellType.Numeric);
                return mCell.NumericCellValue.ToString();
            }
            else
            {
                mCell.SetCellType(CellType.Numeric);
                return mCell.DateCellValue.ToString();
            }
        }
    }
}