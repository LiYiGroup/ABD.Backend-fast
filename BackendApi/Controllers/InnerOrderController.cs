using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.DB;
using BackendApi.DB.DataModel;
using BackendApi.DB.SearchModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InnerOrderController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public InnerOrderController(ABD_DbContext context)
        {
            myContext = context;
        }
        // GET api/innerOrder/existBumpInfo/{BUMP_INFO}
        [HttpGet("existBumpInfo/{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string BUMP_INFO)
        {
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;

                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];
                    var tempObj = JsonConvert.SerializeObject(myContext.ORDER_LIST_DETAIL
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T));
                    if (tempObj != "[]")
                    {
                        returnJson = JsonConvert.SerializeObject(myContext.ORDER_LIST_DETAIL
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).First());
                    }
                }
            }
            return returnJson;
        }

        // GET api/innerOrder/basicSealInfo/{BUMP_INFO}
        [HttpGet("basicSealInfo/{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> GetBasicSealInfo(string BUMP_INFO)
        {
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;

                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];
                    var tempObj = JsonConvert.SerializeObject(myContext.INNER_ORDER_BASIC_SEAL_MST
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T));
                    if (tempObj != "[]")
                    {
                        returnJson = JsonConvert.SerializeObject(myContext.INNER_ORDER_BASIC_SEAL_MST
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).First());
                    }
                }
            }
            return returnJson;
        }

        // GET api/innerOrder/otherComponentInfo/{BUMP_INFO}
        [HttpGet("otherComponentInfo/{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> GetOtherComponentInfo(string BUMP_INFO)
        {
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;

                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];
                    var tempObj = JsonConvert.SerializeObject(myContext.INNER_ORDER_OTHER_COMPONENT_MST
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T));
                    if (tempObj != "[]")
                    {
                        returnJson = JsonConvert.SerializeObject(myContext.INNER_ORDER_OTHER_COMPONENT_MST
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).First());
                    }
                }
            }
            return returnJson;
        }

        // POST api/innerOrder/
        [HttpPost]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Post([FromBody]JObject postObj)
        {
            // FROM WEB PAGE
            ORDER_LIST_DETAIL orderListDetailPostEntity = postObj.GetValue("orderListDetailTableModel").ToObject<ORDER_LIST_DETAIL>();
            INNER_ORDER_BASIC_SEAL_MST innerOrderBasicSealPostEntity = postObj.GetValue("basicAndSealModel").ToObject<INNER_ORDER_BASIC_SEAL_MST>();
            INNER_ORDER_OTHER_COMPONENT_MST innerOrderOtherComponentPostEntity = postObj.GetValue("otherComponentModel").ToObject<INNER_ORDER_OTHER_COMPONENT_MST>();

            IList innerOrderBOMItemStandard = postObj.GetValue("componentListTableData").ToList();
            IList innerOrderBOMItemBase = postObj.GetValue("basicPartListTableData").ToList();

            // 校验主键
            if (String.IsNullOrEmpty(orderListDetailPostEntity.ORDER_NO) || String.IsNullOrEmpty(orderListDetailPostEntity.BUMP_ID))
            {
                return "";
            }

            // FROM DB
            // 登录BOM的GRID部分，先用SQL删除对应ID项目，之后批量插入
            myContext.Database.ExecuteSqlCommand("DELETE FROM \"INNER_ORDER_BOM_ITEM_BASE\" WHERE \"ORDER_NO\" = @ORDER_NO AND \"BUMP_ID\" = @BUMP_ID", 
                        new NpgsqlParameter("@ORDER_NO", orderListDetailPostEntity.ORDER_NO), new NpgsqlParameter("@BUMP_ID", orderListDetailPostEntity.BUMP_ID));
            myContext.Database.ExecuteSqlCommand("DELETE FROM \"INNER_ORDER_BOM_ITEM_STANDARD\" WHERE \"ORDER_NO\" = @ORDER_NO AND \"BUMP_ID\" = @BUMP_ID",
                        new NpgsqlParameter("@ORDER_NO", orderListDetailPostEntity.ORDER_NO), new NpgsqlParameter("@BUMP_ID", orderListDetailPostEntity.BUMP_ID));
            foreach (JObject item in innerOrderBOMItemStandard)
            {
                INNER_ORDER_BOM_ITEM_STANDARD itemEntity = item.ToObject<INNER_ORDER_BOM_ITEM_STANDARD>();
                itemEntity.ORDER_NO = orderListDetailPostEntity.ORDER_NO;
                itemEntity.BUMP_ID = orderListDetailPostEntity.BUMP_ID;
                myContext.INNER_ORDER_BOM_ITEM_STANDARD.AddAsync(itemEntity);
            }
            foreach (JObject item in innerOrderBOMItemBase)
            {
                INNER_ORDER_BOM_ITEM_BASE itemEntity = item.ToObject<INNER_ORDER_BOM_ITEM_BASE>();
                itemEntity.ORDER_NO = orderListDetailPostEntity.ORDER_NO;
                itemEntity.BUMP_ID = orderListDetailPostEntity.BUMP_ID;
                myContext.INNER_ORDER_BOM_ITEM_BASE.AddAsync(itemEntity);
            }
            myContext.SaveChanges();

            var innerOrderbBasicSealMstEntity = myContext.INNER_ORDER_BASIC_SEAL_MST
                .Where(d => d.ORDER_NO.Equals(orderListDetailPostEntity.ORDER_NO) && d.BUMP_ID.Equals(orderListDetailPostEntity.BUMP_ID));

            var innerOrderOtherComponentMstEntity = myContext.INNER_ORDER_OTHER_COMPONENT_MST
                .Where(d => d.ORDER_NO.Equals(orderListDetailPostEntity.ORDER_NO) && d.BUMP_ID.Equals(orderListDetailPostEntity.BUMP_ID));

            if (innerOrderbBasicSealMstEntity.Count() == 0 && innerOrderOtherComponentMstEntity.Count() == 0)
            {
                // 补充项目
                innerOrderBasicSealPostEntity.ORDER_NO = orderListDetailPostEntity.ORDER_NO;
                innerOrderBasicSealPostEntity.BUMP_ID = orderListDetailPostEntity.BUMP_ID;
                innerOrderBasicSealPostEntity.BUMP_TYPE = orderListDetailPostEntity.BUMP_TYPE;
                innerOrderBasicSealPostEntity.BUMP_SERIAL_NO = orderListDetailPostEntity.BUMP_SERIAL_NO;
                innerOrderBasicSealPostEntity.NUMBER = orderListDetailPostEntity.NUMBER;
                innerOrderBasicSealPostEntity.FLOW = orderListDetailPostEntity.FLOW;
                innerOrderBasicSealPostEntity.LIFT = orderListDetailPostEntity.LIFT;
                innerOrderBasicSealPostEntity.STATION = orderListDetailPostEntity.STATION;

                innerOrderOtherComponentPostEntity.ORDER_NO = orderListDetailPostEntity.ORDER_NO;
                innerOrderOtherComponentPostEntity.BUMP_ID = orderListDetailPostEntity.BUMP_ID;

                // TODO

                myContext.INNER_ORDER_OTHER_COMPONENT_MST.Add(innerOrderOtherComponentPostEntity);
                myContext.INNER_ORDER_BASIC_SEAL_MST.Add(innerOrderBasicSealPostEntity);
                myContext.SaveChanges();
                return "";
            }
            else
            {
                // 补充项目
                innerOrderbBasicSealMstEntity.First().BUMP_TYPE = orderListDetailPostEntity.BUMP_TYPE;
                innerOrderbBasicSealMstEntity.First().BUMP_SERIAL_NO = orderListDetailPostEntity.BUMP_SERIAL_NO;
                innerOrderbBasicSealMstEntity.First().NUMBER = orderListDetailPostEntity.NUMBER;
                innerOrderbBasicSealMstEntity.First().FLOW = orderListDetailPostEntity.FLOW;
                innerOrderbBasicSealMstEntity.First().LIFT = orderListDetailPostEntity.LIFT;
                innerOrderbBasicSealMstEntity.First().STATION = orderListDetailPostEntity.STATION;
                // 其他项目
                innerOrderbBasicSealMstEntity.First().WORKING_PRESSURE = innerOrderBasicSealPostEntity.WORKING_PRESSURE;
                innerOrderbBasicSealMstEntity.First().TEST_PRESSURE = innerOrderBasicSealPostEntity.TEST_PRESSURE;
                innerOrderbBasicSealMstEntity.First().SUPPRESS_PRESSURE = innerOrderBasicSealPostEntity.SUPPRESS_PRESSURE;
                innerOrderbBasicSealMstEntity.First().FLANGE_STANDARD = innerOrderBasicSealPostEntity.FLANGE_STANDARD;
                innerOrderbBasicSealMstEntity.First().NPSH = innerOrderBasicSealPostEntity.NPSH;
                innerOrderbBasicSealMstEntity.First().BUMP_SPEED = innerOrderBasicSealPostEntity.BUMP_SPEED;
                innerOrderbBasicSealMstEntity.First().MEDIA = innerOrderBasicSealPostEntity.MEDIA;
                innerOrderbBasicSealMstEntity.First().TEMPERATURE = innerOrderBasicSealPostEntity.TEMPERATURE;
                innerOrderbBasicSealMstEntity.First().VISCOSITY = innerOrderBasicSealPostEntity.VISCOSITY;
                innerOrderbBasicSealMstEntity.First().INLET_PRESSURE = innerOrderBasicSealPostEntity.INLET_PRESSURE;
                innerOrderbBasicSealMstEntity.First().PARTICULATES = innerOrderBasicSealPostEntity.PARTICULATES;
                innerOrderbBasicSealMstEntity.First().SEAL_TYPE = innerOrderBasicSealPostEntity.SEAL_TYPE;
                innerOrderbBasicSealMstEntity.First().SEAL_MODEL = innerOrderBasicSealPostEntity.SEAL_MODEL;
                innerOrderbBasicSealMstEntity.First().SEAL_MATERIAL = innerOrderBasicSealPostEntity.SEAL_MATERIAL;
                innerOrderbBasicSealMstEntity.First().ABD_SEAL_INFO = innerOrderBasicSealPostEntity.ABD_SEAL_INFO;
                innerOrderbBasicSealMstEntity.First().OTHER_SEAL_PROVIDER = innerOrderBasicSealPostEntity.OTHER_SEAL_PROVIDER;
                innerOrderbBasicSealMstEntity.First().OTHER_SEAL_INFO = innerOrderBasicSealPostEntity.OTHER_SEAL_INFO;
                innerOrderbBasicSealMstEntity.First().OTHER_SEAL_MODEL = innerOrderBasicSealPostEntity.OTHER_SEAL_MODEL;
                innerOrderbBasicSealMstEntity.First().NEED_SEAL_COOLER_FLG = innerOrderBasicSealPostEntity.NEED_SEAL_COOLER_FLG;
                innerOrderbBasicSealMstEntity.First().SEAL_COOLER_MODEL = innerOrderBasicSealPostEntity.SEAL_COOLER_MODEL;
                innerOrderbBasicSealMstEntity.First().BEARING_BRAND = innerOrderBasicSealPostEntity.BEARING_BRAND;
                innerOrderbBasicSealMstEntity.First().BEARING_OTHER_INFO = innerOrderBasicSealPostEntity.BEARING_OTHER_INFO;
                innerOrderbBasicSealMstEntity.First().INSTALL_DIRECTION = innerOrderBasicSealPostEntity.INSTALL_DIRECTION;

                innerOrderOtherComponentMstEntity.First().BASE_TYPE = innerOrderOtherComponentPostEntity.BASE_TYPE;
                innerOrderOtherComponentMstEntity.First().BASE_SPEC = innerOrderOtherComponentPostEntity.BASE_SPEC;
                innerOrderOtherComponentMstEntity.First().SPECIAL_BASE_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_BASE_DETAIL;
                innerOrderOtherComponentMstEntity.First().COUPLING_HOOD_TYPE = innerOrderOtherComponentPostEntity.COUPLING_HOOD_TYPE;
                innerOrderOtherComponentMstEntity.First().COUPLING_HOOD_SPEC = innerOrderOtherComponentPostEntity.COUPLING_HOOD_SPEC;
                innerOrderOtherComponentMstEntity.First().SPECIAL_HOOD_TYPE_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_HOOD_TYPE_DETAIL;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_TYPE = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_TYPE;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_SPEC = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_SPEC;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_MATERIAL = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_MATERIAL;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_EXTRA_NUT_SPEC = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_EXTRA_NUT_SPEC;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_EXTRA_PAD_SPEC = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_EXTRA_PAD_SPEC;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_NUM = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_NUM;
                innerOrderOtherComponentMstEntity.First().COUPLING_TYPE = innerOrderOtherComponentPostEntity.COUPLING_TYPE;
                innerOrderOtherComponentMstEntity.First().COUPLING_BUMP_COUPLET = innerOrderOtherComponentPostEntity.COUPLING_BUMP_COUPLET;
                innerOrderOtherComponentMstEntity.First().COUPLING_ELECTRIC_COUPLET = innerOrderOtherComponentPostEntity.COUPLING_ELECTRIC_COUPLET;
                innerOrderOtherComponentMstEntity.First().COUPLING_PIN = innerOrderOtherComponentPostEntity.COUPLING_PIN;
                innerOrderOtherComponentMstEntity.First().COUPLING_JUMP_RING = innerOrderOtherComponentPostEntity.COUPLING_JUMP_RING;
                innerOrderOtherComponentMstEntity.First().COUPLING_PROVIDER = innerOrderOtherComponentPostEntity.COUPLING_PROVIDER;
                innerOrderOtherComponentMstEntity.First().COUPLING_SPEC = innerOrderOtherComponentPostEntity.COUPLING_SPEC;
                innerOrderOtherComponentMstEntity.First().COUPLING_NUM = innerOrderOtherComponentPostEntity.COUPLING_NUM;
                innerOrderOtherComponentMstEntity.First().SPECIAL_COUPLING_TYPE_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_COUPLING_TYPE_DETAIL;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_TYPE = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_TYPE;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_PROVIDER = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_PROVIDER;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_POWER = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_POWER;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_SPEED = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_SPEED;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_PFV = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_PFV;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_EXTRA_INFO = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_EXTRA_INFO;
                innerOrderOtherComponentMstEntity.First().COLOR_TYPE = innerOrderOtherComponentPostEntity.COLOR_TYPE;
                innerOrderOtherComponentMstEntity.First().SPECIAL_COLOR_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_COLOR_DETAIL;
                innerOrderOtherComponentMstEntity.First().SURFACE_TREAT_TYPE = innerOrderOtherComponentPostEntity.SURFACE_TREAT_TYPE;
                innerOrderOtherComponentMstEntity.First().SURFACE_TREAT_EXTRA_INFO = innerOrderOtherComponentPostEntity.SURFACE_TREAT_EXTRA_INFO;
                innerOrderOtherComponentMstEntity.First().TRANSPORT_TYPE = innerOrderOtherComponentPostEntity.TRANSPORT_TYPE;
                innerOrderOtherComponentMstEntity.First().TRANSPORT_PLACE = innerOrderOtherComponentPostEntity.TRANSPORT_PLACE;
                innerOrderOtherComponentMstEntity.First().PACKAGING_TYPE = innerOrderOtherComponentPostEntity.PACKAGING_TYPE;
                innerOrderOtherComponentMstEntity.First().SPECIAL_PACK_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_PACK_DETAIL;
                innerOrderOtherComponentMstEntity.First().NEED_FUME_CERTIFICATE = innerOrderOtherComponentPostEntity.NEED_FUME_CERTIFICATE;
                innerOrderOtherComponentMstEntity.First().ADDRESS_INFO = innerOrderOtherComponentPostEntity.ADDRESS_INFO;
                innerOrderOtherComponentMstEntity.First().REMARK = innerOrderOtherComponentPostEntity.REMARK;

                myContext.SaveChanges();
                return "";
            }
        }

        // GET api/innerOrder/modelLibrary/{BUMP_INFO}
        [HttpGet("modelLibrary/{BUMP_TYPE}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> GetModelLibrary(string BUMP_TYPE)
        {
            BUMP_TYPE = BUMP_TYPE.Replace("|SLASH|", "/");
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_TYPE))
            {
                var tempObj = JsonConvert.SerializeObject(myContext.OTHER_COMPONENT_MODEL_MST.Where(d => d.BUMP_TYPE == BUMP_TYPE));

                if (tempObj != "[]")
                {
                    returnJson = JsonConvert.SerializeObject(myContext.OTHER_COMPONENT_MODEL_MST.Where(d => d.BUMP_TYPE == BUMP_TYPE).First());
                }
            }
            return returnJson;
        }

        // GET api/innerOrder/BOMGridData/
        [HttpGet("BOMGridData")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get()
        {
            InnerOrderGridObject gridObj = new InnerOrderGridObject();
            gridObj.innerOrderGridBomItemBase = myContext.BOM_ITEM_BASE.ToList();
            gridObj.innerOrderGridBomItemStandard = myContext.BOM_ITEM_STANDARD.ToList();
            String returnJson = JsonConvert.SerializeObject(gridObj);
            // 清空所有项目
            myContext.RemoveRange(myContext.Set<BOM_ITEM_BASE>());
            myContext.RemoveRange(myContext.Set<BOM_ITEM_STANDARD>());
            myContext.SaveChanges();
            return returnJson;
        }

        // GET api/innerOrder/BOMGridData/{BUMP_INFO}
        [HttpGet("BOMGridData/{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> GetBOMDataForDisplay(string BUMP_INFO)
        {

            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;
                InnerOrderGridObjectForDisplay gridObj = new InnerOrderGridObjectForDisplay();
                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];

                    gridObj.innerOrderGridBomItemBase = myContext.INNER_ORDER_BOM_ITEM_BASE
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).ToList();

                    gridObj.innerOrderGridBomItemStandard = myContext.INNER_ORDER_BOM_ITEM_STANDARD
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).ToList();
                    returnJson = JsonConvert.SerializeObject(gridObj);
                }
            }
            return returnJson;
        }
    }
}