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
        // 单元格类型
        private const String CELL_TYP_STRING = "0";
        private const String CELL_TYP_NUMBER = "1";
        private const String CELL_TYP_DATE = "2";
        // 循环起始行
        private const int BUMP_OFFSET = 10;

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
                                return "OrderNo is empty.";
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
                            JObject entityObjForOrder = new JObject();
                            entityObjForOrder.Add("ORDER_NO", ORDER_NO);
                            entityObjForOrder.Add("CONTRACT_NO", CONTRACT_NO);
                            entityObjForOrder.Add("PROJECT_NM", PROJECT_NM);
                            entityObjForOrder.Add("SALES_PERSON", SALES_PERSON);
                            entityObjForOrder.Add("ORDER_UNIT", ORDER_UNIT);
                            entityObjForOrder.Add("APPLICATION_ENGINEER", APPLICATION_ENGINEER);
                            entityObjForOrder.Add("DEPARTURE_DATE", Convert.ToDateTime(DEPARTURE_DATE_STR));
                            entityObjForOrder.Add("DELIVERY_DATE", Convert.ToDateTime(DELIVERY_DATE_STR));

                            ORDER_LIST_MST excelOrderListMstEntity = entityObjForOrder.ToObject<ORDER_LIST_MST>();

                            var orderListMstEntity = myContext.ORDER_LIST_MST.Where(d => d.ORDER_NO.Equals(excelOrderListMstEntity.ORDER_NO));

                            if (orderListMstEntity.Count() == 0)
                            {
                                // INSERT
                                myContext.ORDER_LIST_MST.Add(excelOrderListMstEntity);
                                // 以后有可能做一个FLG，到水泵信息保存完成后再保存更改
                                myContext.SaveChanges();
                            }
                            else
                            {
                                orderListMstEntity.First().CONTRACT_NO = excelOrderListMstEntity.CONTRACT_NO;
                                orderListMstEntity.First().PROJECT_NM = excelOrderListMstEntity.PROJECT_NM;
                                orderListMstEntity.First().ORDER_UNIT = excelOrderListMstEntity.ORDER_UNIT;
                                orderListMstEntity.First().SALES_PERSON = excelOrderListMstEntity.SALES_PERSON;
                                orderListMstEntity.First().DEPARTURE_DATE = excelOrderListMstEntity.DEPARTURE_DATE.ToLocalTime();
                                orderListMstEntity.First().DELIVERY_DATE = excelOrderListMstEntity.DELIVERY_DATE.ToLocalTime();
                                orderListMstEntity.First().REMARK = excelOrderListMstEntity.REMARK;
                                orderListMstEntity.First().APPLICATION_ENGINEER = excelOrderListMstEntity.APPLICATION_ENGINEER;
                                myContext.SaveChanges();
                            }
                            /*
                             * 水泵信息
                             */
                            for (int i = 0; i < Convert.ToInt32(BUMP_DETAIL_NUM); i++)
                            {
                                // 序号（校验用）
                                String NO = getCellValue(sheet, "A" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 序号校验（还应考虑不是数字Convert失败的情况，未写）
                                if (Convert.ToInt32(NO) != i+1) { return ""; }
                                // 泵名称
                                String BUMP_NM = getCellValue(sheet, "B" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 工位
                                String STATION = getCellValue(sheet, "C" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 型号
                                String BUMP_TYPE = getCellValue(sheet, "D" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 数量
                                String NUMBER = getCellValue(sheet, "F" + (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                // 流量
                                String FLOW = getCellValue(sheet, "G" + (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                // 扬程
                                String LIFT = getCellValue(sheet, "H" + (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                // 材质
                                String MATERIAL = getCellValue(sheet, "I" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 机封
                                String SEAL = getCellValue(sheet, "J" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 序列号
                                String BUMP_SERIAL_NO = getCellValue(sheet, "K" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 备注
                                String REMARK = getCellValue(sheet, "L" + (BUMP_OFFSET + i), CELL_TYP_STRING);

                                // 构造水泵ID
                                String BUMP_ID = "";
                                if (String.IsNullOrEmpty(BUMP_TYPE) || String.IsNullOrEmpty(MATERIAL))
                                {
                                    return "BumpType or material is empty.";
                                }
                                else
                                {
                                   BUMP_ID = BUMP_TYPE + "_" + MATERIAL;
                                }

                                // 构造JObject，并保存水泵信息至数据库中

                                JObject entityObjForBump = new JObject();

                                entityObjForBump.Add("ORDER_NO", ORDER_NO);
                                entityObjForBump.Add("BUMP_ID", BUMP_ID);
                                entityObjForBump.Add("BUMP_NM", BUMP_NM);
                                entityObjForBump.Add("STATION", STATION);
                                entityObjForBump.Add("BUMP_TYPE", BUMP_TYPE);
                                entityObjForBump.Add("NUMBER", Convert.ToInt32(NUMBER));
                                entityObjForBump.Add("FLOW", Convert.ToInt32(FLOW));
                                entityObjForBump.Add("LIFT", Convert.ToInt32(LIFT));
                                //entityObjForBump.Add("MATERIAL", MATERIAL);
                                //entityObjForBump.Add("SEAL", SEAL);
                                entityObjForBump.Add("BUMP_SERIAL_NO", BUMP_SERIAL_NO);
                                entityObjForBump.Add("REMARK", REMARK);

                                ORDER_LIST_DETAIL excelOrderListDetailEntity = entityObjForBump.ToObject<ORDER_LIST_DETAIL>();

                                var orderListDetailEntity =
                                    myContext.ORDER_LIST_DETAIL
                                    .Where(d => d.ORDER_NO.Equals(excelOrderListDetailEntity.ORDER_NO))
                                    .Where(d => d.BUMP_ID.Equals(excelOrderListDetailEntity.BUMP_ID));

                                if (orderListDetailEntity.Count() == 0)
                                {
                                    // INSERT
                                    myContext.ORDER_LIST_DETAIL.Add(excelOrderListDetailEntity);
                                    // 以后有可能做一个FLG，到水泵信息保存完成后再保存更改
                                    myContext.SaveChanges();
                                }
                                else
                                {
                                    orderListDetailEntity.First().BUMP_NM = excelOrderListDetailEntity.BUMP_NM;
                                    orderListDetailEntity.First().BUMP_TYPE = excelOrderListDetailEntity.BUMP_TYPE;
                                    orderListDetailEntity.First().NUMBER = excelOrderListDetailEntity.NUMBER;

                                    orderListDetailEntity.First().UNIT = excelOrderListDetailEntity.UNIT;
                                    orderListDetailEntity.First().PRICE = excelOrderListDetailEntity.PRICE;
                                    orderListDetailEntity.First().AMOUNT = excelOrderListDetailEntity.AMOUNT;
                                    orderListDetailEntity.First().BUMP_SERIAL_NO = excelOrderListDetailEntity.BUMP_SERIAL_NO;
                                    orderListDetailEntity.First().FLOW = excelOrderListDetailEntity.FLOW;
                                    orderListDetailEntity.First().LIFT = excelOrderListDetailEntity.LIFT;

                                    orderListDetailEntity.First().MATERIAL_BUMP = excelOrderListDetailEntity.MATERIAL_BUMP;
                                    orderListDetailEntity.First().MATERIAL_FAN = excelOrderListDetailEntity.MATERIAL_FAN;
                                    orderListDetailEntity.First().MATERIAL_ROLLER = excelOrderListDetailEntity.MATERIAL_ROLLER;
                                    orderListDetailEntity.First().MOTOR_BRAND = excelOrderListDetailEntity.MOTOR_BRAND;
                                    orderListDetailEntity.First().MOTOR_DEMAND = excelOrderListDetailEntity.MOTOR_DEMAND;

                                    orderListDetailEntity.First().SEAL_FORM = excelOrderListDetailEntity.SEAL_FORM;
                                    orderListDetailEntity.First().SEAL_BRAND = excelOrderListDetailEntity.SEAL_BRAND;
                                    orderListDetailEntity.First().ROLLER_BRAND = excelOrderListDetailEntity.ROLLER_BRAND;
                                    orderListDetailEntity.First().COUPLING = excelOrderListDetailEntity.COUPLING;
                                    orderListDetailEntity.First().SEAL_COOLER = excelOrderListDetailEntity.SEAL_COOLER;

                                    orderListDetailEntity.First().CAVITATION_ALLOWANCE = excelOrderListDetailEntity.CAVITATION_ALLOWANCE;
                                    orderListDetailEntity.First().ACTUAL_BUMP_SPEED = excelOrderListDetailEntity.ACTUAL_BUMP_SPEED;
                                    orderListDetailEntity.First().STATION = excelOrderListDetailEntity.STATION;
                                    orderListDetailEntity.First().TEMPERATURE = excelOrderListDetailEntity.TEMPERATURE;
                                    orderListDetailEntity.First().DENSITY = excelOrderListDetailEntity.DENSITY;

                                    orderListDetailEntity.First().IN_PRESSURE = excelOrderListDetailEntity.IN_PRESSURE;
                                    orderListDetailEntity.First().MEDIUM = excelOrderListDetailEntity.MEDIUM;
                                    orderListDetailEntity.First().VISCOSITY = excelOrderListDetailEntity.VISCOSITY;
                                    orderListDetailEntity.First().PARTICULATES = excelOrderListDetailEntity.PARTICULATES;
                                    orderListDetailEntity.First().WORKING_PRESSURE = excelOrderListDetailEntity.WORKING_PRESSURE;

                                    orderListDetailEntity.First().FLANGES_STANDARD = excelOrderListDetailEntity.FLANGES_STANDARD;
                                    orderListDetailEntity.First().FLANGES_LEVEL = excelOrderListDetailEntity.FLANGES_LEVEL;
                                    orderListDetailEntity.First().BASE = excelOrderListDetailEntity.BASE;
                                    orderListDetailEntity.First().COUPLING_HOOD = excelOrderListDetailEntity.COUPLING_HOOD;
                                    orderListDetailEntity.First().ANCHOR_BOLT = excelOrderListDetailEntity.ANCHOR_BOLT;

                                    orderListDetailEntity.First().PAINT = excelOrderListDetailEntity.PAINT;
                                    orderListDetailEntity.First().SURFACE_TREATMENT = excelOrderListDetailEntity.SURFACE_TREATMENT;
                                    orderListDetailEntity.First().PACKAGE = excelOrderListDetailEntity.PACKAGE;
                                    orderListDetailEntity.First().TRANSPORT = excelOrderListDetailEntity.TRANSPORT;

                                    myContext.SaveChanges();
                                }

                                var orderAttach = myContext.ORDER_LIST_ATTACHMENT.Where(d => d.ORDER_NO.Equals(excelOrderListDetailEntity.ORDER_NO));
                                if (orderListDetailEntity.Count() == 0)
                                {
                                    // INSERT
                                    ORDER_LIST_ATTACHMENT orderListAttach = new ORDER_LIST_ATTACHMENT();
                                    orderListAttach.ORDER_NO = excelOrderListDetailEntity.ORDER_NO;

                                    myContext.ORDER_LIST_ATTACHMENT.Add(orderListAttach);
                                    myContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
                return String.Empty;
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