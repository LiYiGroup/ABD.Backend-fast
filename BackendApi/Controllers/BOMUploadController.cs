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
    public class BOMUploadController : ControllerBase
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly ABD_DbContext myContext;
        // 单元格类型
        private const String CELL_TYP_STRING = "0";
        private const String CELL_TYP_NUMBER = "1";
        private const String CELL_TYP_DATE = "2";
        // 循环起始行
        private const int BUMP_OFFSET = 3;

        public BOMUploadController(IHostingEnvironment hostingEnvironment, ABD_DbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            myContext = context;

        }
        // POST api/BomItemUpload
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
                            int iRowCount = sheet.LastRowNum;
                            /*
                             * BOM信息
                             */

                            // BOM编号
                            String BOM_ID = DateTime.Now.ToString("yyyyMMddHHmmss");

                            bool bIsStandard = false;
                            bool bIsBase = false;
                            for (int i = 0; i < Convert.ToInt32(iRowCount - BUMP_OFFSET); i++)
                            {
                                String ITEM_NO = getCellValue(sheet, "A" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                String ITEM_NAME = getCellValue(sheet, "B" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                int iITEM_NO = 0;
                                bool ITEM_NO_Parse = int.TryParse(ITEM_NO, out iITEM_NO);
                                if (ITEM_NO_Parse == false)
                                {
                                    if (ITEM_NAME.Equals("标准件明细表"))
                                    {
                                        bIsStandard = true;
                                        bIsBase = false;
                                    }
                                    else if (ITEM_NAME.Equals("基本件明细表"))
                                    {
                                        bIsBase = true;
                                        bIsStandard = false;
                                    }
                                    continue;
                                }

                                // 规格及代号
                                String SPEC = getCellValue(sheet, "C" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 数量
                                String QTY = getCellValue(sheet, "D" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 材料
                                String MATERIAL = getCellValue(sheet, "E" + (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 备注
                                String REMARK = getCellValue(sheet, "F" + (BUMP_OFFSET + i), CELL_TYP_STRING);

                                // 构造JObject，并保存水泵信息至数据库中

                                JObject entityObjForBom = new JObject();

                                entityObjForBom.Add("BOM_ID", BOM_ID);
                                entityObjForBom.Add("ITEM_NO", ITEM_NO);
                                entityObjForBom.Add("ITEM_NAME", ITEM_NAME);
                                entityObjForBom.Add("SPEC", SPEC);
                                entityObjForBom.Add("QTY", QTY);
                                entityObjForBom.Add("MATERIAL", MATERIAL);
                                entityObjForBom.Add("REMARK", REMARK);

                                if (bIsStandard == true)
                                {
                                    BOM_ITEM_STANDARD excelBomItemStdEntity = entityObjForBom.ToObject<BOM_ITEM_STANDARD>();

                                    var BomItemStdEntity =
                                        myContext.BOM_ITEM_STANDARD
                                        .Where(d => d.BOM_ID.Equals(excelBomItemStdEntity.BOM_ID))
                                        .Where(d => d.ITEM_NO.Equals(excelBomItemStdEntity.ITEM_NO));

                                    if (BomItemStdEntity.Count() == 0)
                                    {
                                        // INSERT
                                        myContext.BOM_ITEM_STANDARD.Add(excelBomItemStdEntity);
                                        // 以后有可能做一个FLG，到水泵信息保存完成后再保存更改
                                        myContext.SaveChanges();
                                    }
                                    else
                                    {
                                        BomItemStdEntity.First().BOM_ID = excelBomItemStdEntity.BOM_ID;
                                        BomItemStdEntity.First().ITEM_NO = excelBomItemStdEntity.ITEM_NO;
                                        BomItemStdEntity.First().ITEM_NAME = excelBomItemStdEntity.ITEM_NAME;
                                        BomItemStdEntity.First().SPEC = excelBomItemStdEntity.SPEC;
                                        BomItemStdEntity.First().QTY = excelBomItemStdEntity.QTY;
                                        BomItemStdEntity.First().MATERIAL = excelBomItemStdEntity.MATERIAL;
                                        BomItemStdEntity.First().REMARK = excelBomItemStdEntity.REMARK;
                                        myContext.SaveChanges();
                                    }
                                }
                                else if (bIsBase == true)
                                {
                                    BOM_ITEM_BASE excelBomItemBaseEntity = entityObjForBom.ToObject<BOM_ITEM_BASE>();

                                    var BomItemBaseEntity =
                                        myContext.BOM_ITEM_BASE
                                        .Where(d => d.BOM_ID.Equals(excelBomItemBaseEntity.BOM_ID))
                                        .Where(d => d.ITEM_NO.Equals(excelBomItemBaseEntity.ITEM_NO));

                                    if (BomItemBaseEntity.Count() == 0)
                                    {
                                        // INSERT
                                        myContext.BOM_ITEM_BASE.Add(excelBomItemBaseEntity);
                                        // 以后有可能做一个FLG，到水泵信息保存完成后再保存更改
                                        myContext.SaveChanges();
                                    }
                                    else
                                    {
                                        BomItemBaseEntity.First().BOM_ID = excelBomItemBaseEntity.BOM_ID;
                                        BomItemBaseEntity.First().ITEM_NO = excelBomItemBaseEntity.ITEM_NO;
                                        BomItemBaseEntity.First().ITEM_NAME = excelBomItemBaseEntity.ITEM_NAME;
                                        BomItemBaseEntity.First().SPEC = excelBomItemBaseEntity.SPEC;
                                        BomItemBaseEntity.First().QTY = excelBomItemBaseEntity.QTY;
                                        BomItemBaseEntity.First().MATERIAL = excelBomItemBaseEntity.MATERIAL;
                                        BomItemBaseEntity.First().REMARK = excelBomItemBaseEntity.REMARK;
                                        myContext.SaveChanges();
                                    }
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
                return mCell.StringCellValue.ToString();
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