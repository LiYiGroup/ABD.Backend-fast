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
        private const int BUMP_OFFSET = 18;

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
                            String ORDER_NO = getCellValue(sheet,ColumnIndex.C, 3, CELL_TYP_STRING);
                            if (String.IsNullOrEmpty(ORDER_NO))
                            {
                                // 订单号（主键）为空时，直接返回
                                return "OrderNo is empty.";
                            }
                            // 合同编号
                            String CONTRACT_NO = getCellValue(sheet, ColumnIndex.C,4, CELL_TYP_STRING);
                            // 项目名称
                            String PROJECT_NM = getCellValue(sheet, ColumnIndex.C,5, CELL_TYP_STRING);
                            // 销售部门/销售员
                            String SALES_PERSON = getCellValue(sheet, ColumnIndex.C,6, CELL_TYP_STRING);
                            // 付款方式
                            String PAYMENT = getCellValue(sheet, ColumnIndex.C,7, CELL_TYP_STRING);
                            // 订货单位
                            String ORDER_UNIT = getCellValue(sheet, ColumnIndex.C,8, CELL_TYP_STRING);
                            // 应用工程师
                            String APPLICATION_ENGINEER = getCellValue(sheet, ColumnIndex.F,3, CELL_TYP_STRING);
                            // 下发日期
                            String DEPARTURE_DATE_STR = getCellValue(sheet, ColumnIndex.F,4, CELL_TYP_DATE);
                            // 交货日期
                            String DELIVERY_DATE_STR = getCellValue(sheet, ColumnIndex.F,5, CELL_TYP_DATE);
                            // 交货期
                            String GUARANTEE_DATE = getCellValue(sheet, ColumnIndex.F,6, CELL_TYP_DATE);
                            // 调试
                            String DEBUG = getCellValue(sheet, ColumnIndex.I,3, CELL_TYP_STRING);
                            var Dict_DEBUG = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(DEBUG)).Where(e => e.TYPE.Equals("01"));
                            DEBUG = Dict_DEBUG.Count() == 0 ? "" : Dict_DEBUG.First().DICT_ID;
                            // 总数量
                            String TOTAL_QTY = getCellValue(sheet, ColumnIndex.I,4, CELL_TYP_NUMBER);
                            int iTOTAL_QTY = int.TryParse(TOTAL_QTY, out iTOTAL_QTY) == true ? iTOTAL_QTY : 0;
                            // 税率
                            String TEX_RATE = getCellValue(sheet, ColumnIndex.I,5, CELL_TYP_NUMBER);
                            var Dict_TEX_RATE = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(TEX_RATE)).Where(e => e.TYPE.Equals("02"));
                            TEX_RATE = Dict_TEX_RATE.Count() == 0 ? "" : Dict_TEX_RATE.First().DICT_ID;
                            // 合同总额
                            String TOTAL_AMT = getCellValue(sheet, ColumnIndex.I,6, CELL_TYP_NUMBER);
                            int iTOTAL_AMT = int.TryParse(TOTAL_AMT, out iTOTAL_AMT) == true ? iTOTAL_AMT : 0;
                            // 目标价
                            String TARGET_PRICE = getCellValue(sheet, ColumnIndex.I,7, CELL_TYP_NUMBER);
                            int iTARGET_PRICE = int.TryParse(TARGET_PRICE, out iTARGET_PRICE) == true ? iTARGET_PRICE : 0;
                            // 折扣
                            String DISCOUNT = getCellValue(sheet, ColumnIndex.I,8, CELL_TYP_NUMBER);
                            int iDISCOUNT = int.TryParse(DISCOUNT, out iDISCOUNT) == true ? iDISCOUNT : 0;


                            // 构造JObject，并保存订单信息至数据库中
                            JObject entityObjForOrder = new JObject();
                            entityObjForOrder.Add("ORDER_NO", ORDER_NO);
                            entityObjForOrder.Add("CONTRACT_NO", CONTRACT_NO);
                            entityObjForOrder.Add("PROJECT_NM", PROJECT_NM);
                            entityObjForOrder.Add("SALES_PERSON", SALES_PERSON);
                            entityObjForOrder.Add("PAYMENT", PAYMENT);
                            entityObjForOrder.Add("ORDER_UNIT", ORDER_UNIT);
                            entityObjForOrder.Add("APPLICATION_ENGINEER", APPLICATION_ENGINEER);
                            entityObjForOrder.Add("DEPARTURE_DATE", Convert.ToDateTime(DEPARTURE_DATE_STR));
                            entityObjForOrder.Add("DELIVERY_DATE", Convert.ToDateTime(DELIVERY_DATE_STR));
                            entityObjForOrder.Add("GUARANTEE_DATE", Convert.ToDateTime(GUARANTEE_DATE));
                            entityObjForOrder.Add("DEBUG", DEBUG);
                            entityObjForOrder.Add("TOTAL_QTY", iTOTAL_QTY);
                            entityObjForOrder.Add("TEX_RATE", TEX_RATE);
                            entityObjForOrder.Add("TOTAL_AMT", iTOTAL_AMT);
                            entityObjForOrder.Add("TARGET_PRICE", iTARGET_PRICE);
                            entityObjForOrder.Add("DISCOUNT", iDISCOUNT);

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
                                orderListMstEntity.First().DEBUG = excelOrderListMstEntity.DEBUG;
                                orderListMstEntity.First().TOTAL_QTY = excelOrderListMstEntity.TOTAL_QTY;
                                orderListMstEntity.First().TEX_RATE = excelOrderListMstEntity.TEX_RATE;
                                orderListMstEntity.First().GUARANTEE_DATE = excelOrderListMstEntity.GUARANTEE_DATE.ToLocalTime();
                                orderListMstEntity.First().TOTAL_AMT = excelOrderListMstEntity.TOTAL_AMT;
                                orderListMstEntity.First().PAYMENT = excelOrderListMstEntity.PAYMENT;
                                orderListMstEntity.First().TARGET_PRICE = excelOrderListMstEntity.TARGET_PRICE;
                                orderListMstEntity.First().DISCOUNT = excelOrderListMstEntity.DISCOUNT;
                                orderListMstEntity.First().CHANGE_HIS1 = excelOrderListMstEntity.CHANGE_HIS1;
                                orderListMstEntity.First().CHANGE_HIS2 = excelOrderListMstEntity.CHANGE_HIS2;

                                myContext.SaveChanges();
                            }

                            /*
                             * 随货资料
                             */
                            // 合格证
                            String QUALIFICATION = getCellValue(sheet, ColumnIndex.A,13, CELL_TYP_STRING);
                            var Dict_QUALIFICATION = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(QUALIFICATION)).Where(e => e.TYPE.Equals("18"));
                            QUALIFICATION = Dict_QUALIFICATION.Count() == 0 ? "" : Dict_QUALIFICATION.First().DICT_ID;
                            // 说明书
                            String IDENTIFICATION = getCellValue(sheet, ColumnIndex.B,13, CELL_TYP_STRING);
                            var Dict_IDENTIFICATION = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(IDENTIFICATION)).Where(e => e.TYPE.Equals("19"));
                            IDENTIFICATION = Dict_IDENTIFICATION.Count() == 0 ? "" : Dict_IDENTIFICATION.First().DICT_ID;
                            // 产品样本
                            String PRODUCT_SAMPLE = getCellValue(sheet, ColumnIndex.C,13, CELL_TYP_STRING);
                            var Dict_PRODUCT_SAMPLE = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(PRODUCT_SAMPLE)).Where(e => e.TYPE.Equals("20"));
                            PRODUCT_SAMPLE = Dict_PRODUCT_SAMPLE.Count() == 0 ? "" : Dict_PRODUCT_SAMPLE.First().DICT_ID;
                            // 动平衡报告
                            String DYNAMIC_REPORT = getCellValue(sheet, ColumnIndex.D,13, CELL_TYP_STRING);
                            var Dict_DYNAMIC_REPORT = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(DYNAMIC_REPORT)).Where(e => e.TYPE.Equals("21"));
                            DYNAMIC_REPORT = Dict_DYNAMIC_REPORT.Count() == 0 ? "" : Dict_DYNAMIC_REPORT.First().DICT_ID;
                            // 静压试验报告
                            String STATIC_REPORT = getCellValue(sheet, ColumnIndex.E,13, CELL_TYP_STRING);
                            var Dict_STATIC_REPORT = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(STATIC_REPORT)).Where(e => e.TYPE.Equals("22"));
                            STATIC_REPORT = Dict_STATIC_REPORT.Count() == 0 ? "" : Dict_STATIC_REPORT.First().DICT_ID;
                            // 性能试验报告
                            String PERFORMANCE_REPORT = getCellValue(sheet, ColumnIndex.F,13, CELL_TYP_STRING);
                            var Dict_PERFORMANCE_REPORT = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(PERFORMANCE_REPORT)).Where(e => e.TYPE.Equals("23"));
                            PERFORMANCE_REPORT = Dict_PERFORMANCE_REPORT.Count() == 0 ? "" : Dict_PERFORMANCE_REPORT.First().DICT_ID;
                            // 图纸
                            String BLUEPRINT = getCellValue(sheet, ColumnIndex.G,13, CELL_TYP_STRING);
                            var Dict_BLUEPRINT = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(BLUEPRINT)).Where(e => e.TYPE.Equals("24"));
                            BLUEPRINT = Dict_BLUEPRINT.Count() == 0 ? "" : Dict_BLUEPRINT.First().DICT_ID;
                            // 性能曲线
                            String PERFORMANCE_CURVE = getCellValue(sheet, ColumnIndex.H,13, CELL_TYP_STRING);
                            var Dict_PERFORMANCE_CURVE = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(PERFORMANCE_CURVE)).Where(e => e.TYPE.Equals("25"));
                            PERFORMANCE_CURVE = Dict_PERFORMANCE_CURVE.Count() == 0 ? "" : Dict_PERFORMANCE_CURVE.First().DICT_ID;
                            // 其它可手动添加
                            String OTHER = getCellValue(sheet, ColumnIndex.I,13, CELL_TYP_STRING);


                            // 构造JObject，并保存订单信息至数据库中
                            JObject entityObjForAttach = new JObject();
                            entityObjForAttach.Add("ORDER_NO", ORDER_NO);
                            entityObjForAttach.Add("QUALIFICATION", QUALIFICATION);
                            entityObjForAttach.Add("IDENTIFICATION", IDENTIFICATION);
                            entityObjForAttach.Add("PRODUCT_SAMPLE", PRODUCT_SAMPLE);
                            entityObjForAttach.Add("DYNAMIC_REPORT", DYNAMIC_REPORT);
                            entityObjForAttach.Add("STATIC_REPORT", STATIC_REPORT);
                            entityObjForAttach.Add("PERFORMANCE_REPORT", PERFORMANCE_REPORT);
                            entityObjForAttach.Add("BLUEPRINT", BLUEPRINT);
                            entityObjForAttach.Add("PERFORMANCE_CURVE", PERFORMANCE_CURVE);
                            entityObjForAttach.Add("OTHER", OTHER);


                            ORDER_LIST_ATTACHMENT excelOrderListAttchmentEntity = entityObjForAttach.ToObject<ORDER_LIST_ATTACHMENT>();

                            var orderListAttachmentEntity = myContext.ORDER_LIST_ATTACHMENT.Where(d => d.ORDER_NO.Equals(excelOrderListAttchmentEntity.ORDER_NO));

                            if (orderListAttachmentEntity.Count() == 0)
                            {
                                // INSERT
                                myContext.ORDER_LIST_ATTACHMENT.Add(excelOrderListAttchmentEntity);
                                // 以后有可能做一个FLG，到水泵信息保存完成后再保存更改
                                myContext.SaveChanges();
                            }
                            else
                            {
                                orderListAttachmentEntity.First().QUALIFICATION = excelOrderListAttchmentEntity.QUALIFICATION;
                                orderListAttachmentEntity.First().IDENTIFICATION = excelOrderListAttchmentEntity.IDENTIFICATION;
                                orderListAttachmentEntity.First().PRODUCT_SAMPLE = excelOrderListAttchmentEntity.PRODUCT_SAMPLE;
                                orderListAttachmentEntity.First().DYNAMIC_REPORT = excelOrderListAttchmentEntity.DYNAMIC_REPORT;
                                orderListAttachmentEntity.First().STATIC_REPORT = excelOrderListAttchmentEntity.STATIC_REPORT;
                                orderListAttachmentEntity.First().PERFORMANCE_REPORT = excelOrderListAttchmentEntity.PERFORMANCE_REPORT;
                                orderListAttachmentEntity.First().BLUEPRINT = excelOrderListAttchmentEntity.BLUEPRINT;
                                orderListAttachmentEntity.First().PERFORMANCE_CURVE = excelOrderListAttchmentEntity.PERFORMANCE_CURVE;
                                orderListAttachmentEntity.First().OTHER = excelOrderListAttchmentEntity.OTHER;

                                myContext.SaveChanges();
                            }

                            /*
                             * 水泵信息
                             */
                            int i = 0;
                            while (true)
                            {
                                // 序号（校验用）
                                String NO = getCellValue(sheet, ColumnIndex.A, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 序号校验（还应考虑不是数字Convert失败的情况，未写）
                                if (NO.ToString().Trim().Equals("合计")) { break; }
                                if (Convert.ToInt32(NO) != i + 1) { return ""; }
                                // 泵名称
                                String BUMP_NM = getCellValue(sheet, ColumnIndex.B,(BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 型号
                                String BUMP_TYPE = getCellValue(sheet, ColumnIndex.C, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 数量
                                String NUMBER = getCellValue(sheet, ColumnIndex.D,(BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iNUMBER = int.TryParse(NUMBER, out iNUMBER) == true ? iNUMBER : 0;
                                // 单位
                                String UNIT = getCellValue(sheet, ColumnIndex.E,(BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 单价
                                String PRICE = getCellValue(sheet, ColumnIndex.F,(BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iPRICE = int.TryParse(PRICE, out iPRICE) == true ? iPRICE : 0;
                                // 金额
                                String AMOUNT = getCellValue(sheet, ColumnIndex.G, (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iAMOUNT = int.TryParse(AMOUNT, out iAMOUNT) == true ? iAMOUNT : 0;
                                // 序列号
                                String BUMP_SERIAL_NO = getCellValue(sheet, ColumnIndex.H, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 流量
                                String FLOW = getCellValue(sheet, ColumnIndex.I, (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iFLOW = int.TryParse(FLOW, out iFLOW) == true ? iFLOW : 0;
                                // 扬程
                                String LIFT = getCellValue(sheet, ColumnIndex.J, (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iLIFT = int.TryParse(LIFT, out iLIFT) == true ? iLIFT : 0;
                                // 材质 泵体
                                String MATERIAL_BUMP = getCellValue(sheet, ColumnIndex.K, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 材质 叶轮
                                String MATERIAL_FAN = getCellValue(sheet, ColumnIndex.L, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 材质 轴
                                String MATERIAL_ROLLER = getCellValue(sheet, ColumnIndex.M, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 电机 品牌
                                String MOTOR_BRAND = getCellValue(sheet, ColumnIndex.N, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_MOTOR_BRAND = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(MOTOR_BRAND)).Where(e => e.TYPE.Equals("03"));
                                MOTOR_BRAND = Dict_MOTOR_BRAND.Count() == 0 ? "" : Dict_MOTOR_BRAND.First().DICT_ID;
                                // 电机 要求
                                String MOTOR_DEMAND = getCellValue(sheet, ColumnIndex.O, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 密封/液下深度（BY&SBY） 形式
                                String SEAL_FORM = getCellValue(sheet, ColumnIndex.P, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_SEAL_FORM = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(SEAL_FORM)).Where(e => e.TYPE.Equals("04"));
                                SEAL_FORM = Dict_SEAL_FORM.Count() == 0 ? "" : Dict_SEAL_FORM.First().DICT_ID;
                                // 密封/液下深度（BY&SBY） 品牌/其它
                                String SEAL_BRAND = getCellValue(sheet, ColumnIndex.Q, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_SEAL_BRAND = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(SEAL_BRAND)).Where(e => e.TYPE.Equals("05"));
                                SEAL_BRAND = Dict_SEAL_BRAND.Count() == 0 ? "" : Dict_SEAL_BRAND.First().DICT_ID;
                                // 轴承品牌
                                String ROLLER_BRAND = getCellValue(sheet, ColumnIndex.R, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_ROLLER_BRAND = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(ROLLER_BRAND)).Where(e => e.TYPE.Equals("06"));
                                ROLLER_BRAND = Dict_ROLLER_BRAND.Count() == 0 ? "" : Dict_ROLLER_BRAND.First().DICT_ID;
                                // 联轴器
                                String COUPLING = getCellValue(sheet, ColumnIndex.S, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_COUPLING = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(COUPLING)).Where(e => e.TYPE.Equals("07"));
                                COUPLING = Dict_COUPLING.Count() == 0 ? "" : Dict_COUPLING.First().DICT_ID;
                                // 机封冷却器
                                String SEAL_COOLER = getCellValue(sheet, ColumnIndex.T, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_SEAL_COOLER = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(SEAL_COOLER)).Where(e => e.TYPE.Equals("03"));
                                SEAL_COOLER = Dict_SEAL_COOLER.Count() == 0 ? "" : Dict_SEAL_COOLER.First().DICT_ID;
                                // 装置汽蚀余量
                                String CAVITATION_ALLOWANCE = getCellValue(sheet, ColumnIndex.U, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                int iCAVITATION_ALLOWANCE = int.TryParse(CAVITATION_ALLOWANCE, out iCAVITATION_ALLOWANCE) == true ? iCAVITATION_ALLOWANCE : 0;
                                // 泵实际转速
                                String ACTUAL_BUMP_SPEED = getCellValue(sheet, ColumnIndex.V, (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iACTUAL_BUMP_SPEED = int.TryParse(ACTUAL_BUMP_SPEED, out iACTUAL_BUMP_SPEED) == true ? iACTUAL_BUMP_SPEED : 0;
                                // 工位
                                String STATION = getCellValue(sheet, ColumnIndex.W, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 温度
                                String TEMPERATURE = getCellValue(sheet, ColumnIndex.X, (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iTEMPERATURE = int.TryParse(TEMPERATURE, out iTEMPERATURE) == true ? iTEMPERATURE : 0;
                                // 密度                           
                                String DENSITY = getCellValue(sheet, ColumnIndex.Y, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 入口压力
                                String IN_PRESSURE = getCellValue(sheet, ColumnIndex.Z,(BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iIN_PRESSURE = int.TryParse(IN_PRESSURE, out iIN_PRESSURE) == true ? iIN_PRESSURE : 0;
                                // 介质
                                String MEDIUM = getCellValue(sheet, ColumnIndex.AA, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 粘度
                                String VISCOSITY = getCellValue(sheet, ColumnIndex.AB, (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iVISCOSITY = int.TryParse(VISCOSITY, out iVISCOSITY) == true ? iVISCOSITY : 0;
                                // 含颗粒情况
                                String PARTICULATES = getCellValue(sheet, ColumnIndex.AC,(BUMP_OFFSET + i), CELL_TYP_STRING);
                                // 工作压力(bar)
                                String WORKING_PRESSURE = getCellValue(sheet, ColumnIndex.AD, (BUMP_OFFSET + i), CELL_TYP_NUMBER);
                                int iWORKING_PRESSURE = int.TryParse(WORKING_PRESSURE, out iWORKING_PRESSURE) == true ? iWORKING_PRESSURE : 0;
                                // 法兰标准
                                String FLANGES_STANDARD = getCellValue(sheet, ColumnIndex.AE, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_FLANGES_STANDARD = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(FLANGES_STANDARD)).Where(e => e.TYPE.Equals("03"));
                                FLANGES_STANDARD = Dict_FLANGES_STANDARD.Count() == 0 ? "" : Dict_FLANGES_STANDARD.First().DICT_ID;
                                // 法兰等级（MPH/MPE)
                                String FLANGES_LEVEL = getCellValue(sheet, ColumnIndex.AF, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_FLANGES_LEVEL = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(FLANGES_LEVEL)).Where(e => e.TYPE.Equals("03"));
                                FLANGES_LEVEL = Dict_FLANGES_LEVEL.Count() == 0 ? "" : Dict_FLANGES_LEVEL.First().DICT_ID;
                                // 底座
                                String BASE = getCellValue(sheet, ColumnIndex.AG, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_BASE = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(BASE)).Where(e => e.TYPE.Equals("03"));
                                BASE = Dict_BASE.Count() == 0 ? "" : Dict_BASE.First().DICT_ID;
                                // 联轴器罩
                                String COUPLING_HOOD = getCellValue(sheet, ColumnIndex.AH,(BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_COUPLING_HOOD = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(COUPLING_HOOD)).Where(e => e.TYPE.Equals("03"));
                                COUPLING_HOOD = Dict_COUPLING_HOOD.Count() == 0 ? "" : Dict_COUPLING_HOOD.First().DICT_ID;
                                // 地脚螺栓
                                String ANCHOR_BOLT = getCellValue(sheet, ColumnIndex.AI, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_ANCHOR_BOLT = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(ANCHOR_BOLT)).Where(e => e.TYPE.Equals("03"));
                                ANCHOR_BOLT = Dict_ANCHOR_BOLT.Count() == 0 ? "" : Dict_ANCHOR_BOLT.First().DICT_ID;
                                // 油漆
                                String PAINT = getCellValue(sheet, ColumnIndex.AJ, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_PAINT = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(PAINT)).Where(e => e.TYPE.Equals("03"));
                                PAINT = Dict_PAINT.Count() == 0 ? "" : Dict_PAINT.First().DICT_ID;
                                // 表面特殊处理
                                String SURFACE_TREATMENT = getCellValue(sheet, ColumnIndex.AK, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_SURFACE_TREATMENT = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(SURFACE_TREATMENT)).Where(e => e.TYPE.Equals("03"));
                                SURFACE_TREATMENT = Dict_SURFACE_TREATMENT.Count() == 0 ? "" : Dict_SURFACE_TREATMENT.First().DICT_ID;
                                // 包装
                                String PACKAGE = getCellValue(sheet, ColumnIndex.AL, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_PACKAGE = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(PACKAGE)).Where(e => e.TYPE.Equals("03"));
                                PACKAGE = Dict_PACKAGE.Count() == 0 ? "" : Dict_PACKAGE.First().DICT_ID;
                                // 运输
                                String TRANSPORT = getCellValue(sheet, ColumnIndex.AM, (BUMP_OFFSET + i), CELL_TYP_STRING);
                                var Dict_TRANSPORT = myContext.M_DICT.Where(d => d.DICT_NAME.Equals(TRANSPORT)).Where(e => e.TYPE.Equals("03"));
                                TRANSPORT = Dict_TRANSPORT.Count() == 0 ? "" : Dict_TRANSPORT.First().DICT_ID;


                                // 构造水泵ID
                                String BUMP_ID = "";
                                if (String.IsNullOrEmpty(BUMP_TYPE) || String.IsNullOrEmpty(MATERIAL_BUMP))
                                {
                                    return "BumpType or material is empty.";
                                }
                                else
                                {
                                    BUMP_ID = BUMP_TYPE + "_" + MATERIAL_BUMP;
                                }

                                // 构造JObject，并保存水泵信息至数据库中

                                JObject entityObjForBump = new JObject();

                                entityObjForBump.Add("ORDER_NO", ORDER_NO);
                                entityObjForBump.Add("BUMP_ID", BUMP_ID);
                                entityObjForBump.Add("BUMP_NM", BUMP_NM);
                                entityObjForBump.Add("BUMP_TYPE", BUMP_TYPE);
                                entityObjForBump.Add("NUMBER", iNUMBER);

                                entityObjForBump.Add("UNIT", UNIT);
                                entityObjForBump.Add("PRICE", iPRICE);
                                entityObjForBump.Add("AMOUNT", iAMOUNT);
                                entityObjForBump.Add("BUMP_SERIAL_NO", BUMP_SERIAL_NO);
                                entityObjForBump.Add("FLOW", iFLOW);
                                entityObjForBump.Add("LIFT", iLIFT);

                                entityObjForBump.Add("MATERIAL_BUMP", MATERIAL_BUMP);
                                entityObjForBump.Add("MATERIAL_FAN", MATERIAL_FAN);
                                entityObjForBump.Add("MATERIAL_ROLLER", MATERIAL_ROLLER);
                                entityObjForBump.Add("MOTOR_BRAND", MOTOR_BRAND);
                                entityObjForBump.Add("MOTOR_DEMAND", MOTOR_DEMAND);

                                entityObjForBump.Add("SEAL_FORM", SEAL_FORM);
                                entityObjForBump.Add("SEAL_BRAND", SEAL_BRAND);
                                entityObjForBump.Add("ROLLER_BRAND", ROLLER_BRAND);
                                entityObjForBump.Add("COUPLING", COUPLING);
                                entityObjForBump.Add("SEAL_COOLER", SEAL_COOLER);

                                entityObjForBump.Add("CAVITATION_ALLOWANCE", iCAVITATION_ALLOWANCE);
                                entityObjForBump.Add("ACTUAL_BUMP_SPEED", iACTUAL_BUMP_SPEED);
                                entityObjForBump.Add("STATION", STATION);
                                entityObjForBump.Add("TEMPERATURE", iTEMPERATURE);
                                entityObjForBump.Add("DENSITY", DENSITY);

                                entityObjForBump.Add("IN_PRESSURE", iIN_PRESSURE);
                                entityObjForBump.Add("MEDIUM", MEDIUM);
                                entityObjForBump.Add("VISCOSITY", iVISCOSITY);
                                entityObjForBump.Add("PARTICULATES", PARTICULATES);
                                entityObjForBump.Add("WORKING_PRESSURE", iWORKING_PRESSURE);

                                entityObjForBump.Add("FLANGES_STANDARD", FLANGES_STANDARD);
                                entityObjForBump.Add("FLANGES_LEVEL", FLANGES_LEVEL);
                                entityObjForBump.Add("BASE", BASE);
                                entityObjForBump.Add("COUPLING_HOOD", COUPLING_HOOD);
                                entityObjForBump.Add("ANCHOR_BOLT", ANCHOR_BOLT);

                                entityObjForBump.Add("PAINT", PAINT);
                                entityObjForBump.Add("SURFACE_TREATMENT", SURFACE_TREATMENT);
                                entityObjForBump.Add("PACKAGE", PACKAGE);
                                entityObjForBump.Add("TRANSPORT", TRANSPORT);


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

                                i++;
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
        public String getCellValue(ISheet sheet, int cell,int Row, string celltyp)
        {            
            IRow mRow = sheet.GetRow(Row - 1);
            ICell mCell = mRow.GetCell(cell);

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

        public class ColumnIndex
        {
            public static int A = 0;
            public static int B = 1;
            public static int C = 2;
            public static int D = 3;
            public static int E = 4;
            public static int F = 5;
            public static int G = 6;
            public static int H = 7;
            public static int I = 8;
            public static int J = 9;
            public static int K = 10;
            public static int L = 11;
            public static int M = 12;
            public static int N = 13;
            public static int O = 14;
            public static int P = 15;
            public static int Q = 16;
            public static int R = 17;
            public static int S = 18;
            public static int T = 19;
            public static int U = 20;
            public static int V = 21;
            public static int W = 22;
            public static int X = 23;
            public static int Y = 24;
            public static int Z = 25;
            public static int AA = 26;
            public static int AB = 27;
            public static int AC = 28;
            public static int AD = 29;
            public static int AE = 30;
            public static int AF = 31;
            public static int AG = 32;
            public static int AH = 33;
            public static int AI = 34;
            public static int AJ = 35;
            public static int AK = 36;
            public static int AL = 37;
            public static int AM = 38;
        }
    }
}