using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.DB;
using BackendApi.DB.DataModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderListDetailTableController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public OrderListDetailTableController(ABD_DbContext context)
        {
            myContext = context;
        }

        // GET api/orderListDetailTable
        [HttpGet]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get()
        {
            var orderListDetailEntity = from a in myContext.ORDER_LIST_DETAIL
                                        join b in myContext.M_DICT
                                        on a.MOTOR_BRAND equals b.DICT_ID into b1
                                        from b2 in b1.DefaultIfEmpty()

                                        join c in myContext.M_DICT
                                        on a.SEAL_FORM equals c.DICT_ID into c1
                                        from c2 in c1.DefaultIfEmpty()

                                        join d in myContext.M_DICT
                                        on a.SEAL_BRAND equals d.DICT_ID into d1
                                        from d2 in d1.DefaultIfEmpty()

                                        join e in myContext.M_DICT
                                        on a.ROLLER_BRAND equals e.DICT_ID into e1
                                        from e2 in e1.DefaultIfEmpty()

                                        join f in myContext.M_DICT
                                        on a.COUPLING equals f.DICT_ID into f1
                                        from f2 in f1.DefaultIfEmpty()

                                        join g in myContext.M_DICT
                                        on a.SEAL_COOLER equals g.DICT_ID into g1
                                        from g2 in g1.DefaultIfEmpty()

                                        join h in myContext.M_DICT
                                        on a.FLANGES_STANDARD equals h.DICT_ID into h1
                                        from h2 in h1.DefaultIfEmpty()

                                        join i in myContext.M_DICT
                                        on a.FLANGES_LEVEL equals i.DICT_ID into i1
                                        from i2 in i1.DefaultIfEmpty()

                                        join j in myContext.M_DICT
                                        on a.COUPLING_HOOD equals j.DICT_ID into j1
                                        from j2 in j1.DefaultIfEmpty()

                                        join k in myContext.M_DICT
                                        on a.BASE equals k.DICT_ID into k1
                                        from k2 in k1.DefaultIfEmpty()

                                        join l in myContext.M_DICT
                                        on a.ANCHOR_BOLT equals l.DICT_ID into l1
                                        from l2 in l1.DefaultIfEmpty()

                                        join m in myContext.M_DICT
                                        on a.PAINT equals m.DICT_ID into m1
                                        from m2 in m1.DefaultIfEmpty()

                                        join n in myContext.M_DICT
                                        on a.SURFACE_TREATMENT equals n.DICT_ID into n1
                                        from n2 in n1.DefaultIfEmpty()

                                        join o in myContext.M_DICT
                                        on a.PACKAGE equals o.DICT_ID into o1
                                        from o2 in o1.DefaultIfEmpty()

                                        join p in myContext.M_DICT
                                        on a.TRANSPORT equals p.DICT_ID into p1
                                        from p2 in p1.DefaultIfEmpty()

                                        select new ORDER_LIST_DETAIL_DICT_NAME
                                        {
                                            ORDER_NO = a.ORDER_NO,
                                            BUMP_ID = a.BUMP_ID,
                                            BUMP_NM = a.BUMP_NM,
                                            BUMP_TYPE = a.BUMP_TYPE,
                                            NUMBER = a.NUMBER,

                                            UNIT = a.UNIT,
                                            PRICE = a.PRICE,
                                            AMOUNT = a.AMOUNT,
                                            BUMP_SERIAL_NO = a.BUMP_SERIAL_NO,
                                            FLOW = a.FLOW,
                                            LIFT = a.LIFT,

                                            MATERIAL_BUMP = a.MATERIAL_BUMP,
                                            MATERIAL_FAN = a.MATERIAL_FAN,
                                            MATERIAL_ROLLER = a.MATERIAL_ROLLER,
                                            MOTOR_BRAND = a.MOTOR_BRAND,
                                            MOTOR_DEMAND = a.MOTOR_DEMAND,

                                            SEAL_FORM = a.SEAL_FORM,
                                            SEAL_BRAND = a.SEAL_BRAND,
                                            ROLLER_BRAND = a.ROLLER_BRAND,
                                            COUPLING = a.COUPLING,
                                            SEAL_COOLER = a.SEAL_COOLER,

                                            CAVITATION_ALLOWANCE = a.CAVITATION_ALLOWANCE,
                                            ACTUAL_BUMP_SPEED = a.ACTUAL_BUMP_SPEED,
                                            STATION = a.STATION,
                                            TEMPERATURE = a.TEMPERATURE,
                                            DENSITY = a.DENSITY,

                                            IN_PRESSURE = a.IN_PRESSURE,
                                            MEDIUM = a.MEDIUM,
                                            VISCOSITY = a.VISCOSITY,
                                            PARTICULATES = a.PARTICULATES,
                                            WORKING_PRESSURE = a.WORKING_PRESSURE,

                                            FLANGES_STANDARD = a.FLANGES_STANDARD,
                                            FLANGES_LEVEL = a.FLANGES_LEVEL,
                                            COUPLING_HOOD = a.COUPLING_HOOD,
                                            BASE = a.BASE,
                                            ANCHOR_BOLT = a.ANCHOR_BOLT,

                                            PAINT = a.PAINT,
                                            SURFACE_TREATMENT = a.SURFACE_TREATMENT,
                                            PACKAGE = a.PACKAGE,
                                            TRANSPORT = a.TRANSPORT,

                                            MOTOR_BRAND_NAME = b2.DICT_NAME,
                                            SEAL_FORM_NAME = c2.DICT_NAME,
                                            SEAL_BRAND_NAME = d2.DICT_NAME,
                                            ROLLER_BRAND_NAME = e2.DICT_NAME,
                                            COUPLING_NAME = f2.DICT_NAME,
                                            SEAL_COOLER_NAME = g2.DICT_NAME,

                                            FLANGES_STANDARD_NAME = h2.DICT_NAME,
                                            FLANGES_LEVEL_NAME = i2.DICT_NAME,
                                            COUPLING_HOOD_NAME = j2.DICT_NAME,
                                            BASE_NAME = k2.DICT_NAME,
                                            ANCHOR_BOLT_NAME = l2.DICT_NAME,

                                            PAINT_NAME = m2.DICT_NAME,
                                            SURFACE_TREATMENT_NAME = n2.DICT_NAME,
                                            PACKAGE_NAME = o2.DICT_NAME,
                                            TRANSPORT_NAME = p2.DICT_NAME,

                                            SEAL_BRAND_MANUAL = String.IsNullOrEmpty(a.SEAL_BRAND_MANUAL) ? "" : a.SEAL_BRAND_MANUAL,
                                            SEAL_BRAND_SHOW = d2.DICT_NAME + (String.IsNullOrEmpty(a.SEAL_BRAND_MANUAL) ? "" :"-" + a.SEAL_BRAND_MANUAL) ,   //手动添加显示内容，否则显示数据字典名称
                                        };

            return JsonConvert.SerializeObject(orderListDetailEntity.ToList());
        }

        // GET api/orderListDetailTable/ORDER_NO_001
        [HttpGet("{ORDER_NO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string ORDER_NO)
        {
            if (String.IsNullOrEmpty(ORDER_NO))
            {
                return "";
            }
            var orderListDetailEntity = from a in myContext.ORDER_LIST_DETAIL
                                        join b in myContext.M_DICT
                                        on a.MOTOR_BRAND equals b.DICT_ID into b1
                                        from b2 in b1.DefaultIfEmpty()

                                        join c in myContext.M_DICT
                                        on a.SEAL_FORM equals c.DICT_ID into c1
                                        from c2 in c1.DefaultIfEmpty()

                                        join d in myContext.M_DICT
                                        on a.SEAL_BRAND equals d.DICT_ID into d1
                                        from d2 in d1.DefaultIfEmpty()

                                        join e in myContext.M_DICT
                                        on a.ROLLER_BRAND equals e.DICT_ID into e1
                                        from e2 in e1.DefaultIfEmpty()

                                        join f in myContext.M_DICT
                                        on a.COUPLING equals f.DICT_ID into f1
                                        from f2 in f1.DefaultIfEmpty()

                                        join g in myContext.M_DICT
                                        on a.SEAL_COOLER equals g.DICT_ID into g1
                                        from g2 in g1.DefaultIfEmpty()

                                        join h in myContext.M_DICT
                                        on a.FLANGES_STANDARD equals h.DICT_ID into h1
                                        from h2 in h1.DefaultIfEmpty()

                                        join i in myContext.M_DICT
                                        on a.FLANGES_LEVEL equals i.DICT_ID into i1
                                        from i2 in i1.DefaultIfEmpty()

                                        join j in myContext.M_DICT
                                        on a.COUPLING_HOOD equals j.DICT_ID into j1
                                        from j2 in j1.DefaultIfEmpty()

                                        join k in myContext.M_DICT
                                        on a.BASE equals k.DICT_ID into k1
                                        from k2 in k1.DefaultIfEmpty()

                                        join l in myContext.M_DICT
                                        on a.ANCHOR_BOLT equals l.DICT_ID into l1
                                        from l2 in l1.DefaultIfEmpty()

                                        join m in myContext.M_DICT
                                        on a.PAINT equals m.DICT_ID into m1
                                        from m2 in m1.DefaultIfEmpty()

                                        join n in myContext.M_DICT
                                        on a.SURFACE_TREATMENT equals n.DICT_ID into n1
                                        from n2 in n1.DefaultIfEmpty()

                                        join o in myContext.M_DICT
                                        on a.PACKAGE equals o.DICT_ID into o1
                                        from o2 in o1.DefaultIfEmpty()

                                        join p in myContext.M_DICT
                                        on a.TRANSPORT equals p.DICT_ID into p1
                                        from p2 in p1.DefaultIfEmpty()

                                        where a.ORDER_NO == ORDER_NO.Replace("|SLASH|", "/")
                                        select new ORDER_LIST_DETAIL_DICT_NAME
                                        {
                                            ORDER_NO = a.ORDER_NO,
                                            BUMP_ID = a.BUMP_ID,
                                            BUMP_NM = a.BUMP_NM,
                                            BUMP_TYPE = a.BUMP_TYPE,
                                            NUMBER = a.NUMBER,

                                            UNIT = a.UNIT,
                                            PRICE = a.PRICE,
                                            AMOUNT = a.AMOUNT,
                                            BUMP_SERIAL_NO = a.BUMP_SERIAL_NO,
                                            FLOW = a.FLOW,
                                            LIFT = a.LIFT,

                                            MATERIAL_BUMP = a.MATERIAL_BUMP,
                                            MATERIAL_FAN = a.MATERIAL_FAN,
                                            MATERIAL_ROLLER = a.MATERIAL_ROLLER,
                                            MOTOR_BRAND = a.MOTOR_BRAND,
                                            MOTOR_DEMAND = a.MOTOR_DEMAND,

                                            SEAL_FORM = a.SEAL_FORM,
                                            SEAL_BRAND = a.SEAL_BRAND,
                                            ROLLER_BRAND = a.ROLLER_BRAND,
                                            COUPLING = a.COUPLING,
                                            SEAL_COOLER = a.SEAL_COOLER,

                                            CAVITATION_ALLOWANCE = a.CAVITATION_ALLOWANCE,
                                            ACTUAL_BUMP_SPEED = a.ACTUAL_BUMP_SPEED,
                                            STATION = a.STATION,
                                            TEMPERATURE = a.TEMPERATURE,
                                            DENSITY = a.DENSITY,

                                            IN_PRESSURE = a.IN_PRESSURE,
                                            MEDIUM = a.MEDIUM,
                                            VISCOSITY = a.VISCOSITY,
                                            PARTICULATES = a.PARTICULATES,
                                            WORKING_PRESSURE = a.WORKING_PRESSURE,

                                            FLANGES_STANDARD = a.FLANGES_STANDARD,
                                            FLANGES_LEVEL = a.FLANGES_LEVEL,
                                            COUPLING_HOOD = a.COUPLING_HOOD,
                                            BASE = a.BASE,
                                            ANCHOR_BOLT = a.ANCHOR_BOLT,

                                            PAINT = a.PAINT,
                                            SURFACE_TREATMENT = a.SURFACE_TREATMENT,
                                            PACKAGE = a.PACKAGE,
                                            TRANSPORT = a.TRANSPORT,

                                            MOTOR_BRAND_NAME = b2.DICT_NAME,
                                            SEAL_FORM_NAME = c2.DICT_NAME,
                                            SEAL_BRAND_NAME = d2.DICT_NAME,
                                            ROLLER_BRAND_NAME = e2.DICT_NAME,
                                            COUPLING_NAME = f2.DICT_NAME,
                                            SEAL_COOLER_NAME = g2.DICT_NAME,

                                            FLANGES_STANDARD_NAME = h2.DICT_NAME,
                                            FLANGES_LEVEL_NAME = i2.DICT_NAME,
                                            COUPLING_HOOD_NAME = j2.DICT_NAME,
                                            BASE_NAME = k2.DICT_NAME,
                                            ANCHOR_BOLT_NAME = l2.DICT_NAME,

                                            PAINT_NAME = m2.DICT_NAME,
                                            SURFACE_TREATMENT_NAME = n2.DICT_NAME,
                                            PACKAGE_NAME = o2.DICT_NAME,
                                            TRANSPORT_NAME = p2.DICT_NAME,

                                            SEAL_BRAND_MANUAL = String.IsNullOrEmpty(a.SEAL_BRAND_MANUAL) ? "" : a.SEAL_BRAND_MANUAL,
                                            SEAL_BRAND_SHOW = d2.DICT_NAME + (String.IsNullOrEmpty(a.SEAL_BRAND_MANUAL) ? "" : "-" + a.SEAL_BRAND_MANUAL),   //手动添加显示内容，否则显示数据字典名称
                                        };
            if (orderListDetailEntity.Equals(null))
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(orderListDetailEntity.ToList());
            }
        }

        // DELETE api/orderListDetailTable/BUMP_ID
        [HttpDelete("{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Delete(string BUMP_INFO)
        {
            Boolean isSuccess = true;
            string data = "Delete success";
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Replace("|SLASH|", "/").Split("|DASH|");
                string[] BUMP_ID_LIST = { };
                string ORDER_NO_T = String.Empty;

                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_LIST = ORDER_NO_AND_BUMP_IDS[1].Split(",");
                }
                else
                {
                    isSuccess = false;
                    data = "Need more arguments.";
                }

                foreach (string BUMP_ID_T in BUMP_ID_LIST)
                {
                    ORDER_LIST_DETAIL delEntity = new ORDER_LIST_DETAIL() { BUMP_ID = BUMP_ID_T, ORDER_NO = ORDER_NO_T };

                    if (delEntity.Equals(null))
                    {
                        isSuccess = false;
                        data = "No such data.";
                        break;
                    }
                    else
                    {
                        // FIND DB ITEM
                        myContext.ORDER_LIST_DETAIL.Attach(delEntity);
                        // DELETE DB ITEM
                        var delRes = myContext.ORDER_LIST_DETAIL.Remove(delEntity);
                        if (delRes.State == EntityState.Deleted)
                        {
                            // SAVE CHANGES AND DO NOT RETURN
                            myContext.SaveChanges();
                        }
                        else
                        {
                            isSuccess = false;
                            data = "No such data.";
                            break;
                        }
                    }
                }
            }
            else
            {
                isSuccess = false;
                data = "No such data.";
            }
            JsonRes res = new JsonRes();
            res.isSuccess = isSuccess;
            res.data = data;
            return JsonConvert.SerializeObject(res);
        }

        // POST api/orderListDetailTable
        [HttpPost]
        [EnableCors("CorsPolicy")]
        // JObject here is very very important...
        public ActionResult<string> Post([FromBody]JObject entityObj)
        {

            ORDER_LIST_DETAIL_DICT_NAME postedEntity = entityObj.ToObject<ORDER_LIST_DETAIL_DICT_NAME>();

            // 校验主键
            if (String.IsNullOrEmpty(postedEntity.ORDER_NO))
            {
                return "";
            }

            var orderListEntity = myContext.ORDER_LIST_DETAIL
                .Where(d => d.ORDER_NO.Equals(postedEntity.ORDER_NO) && d.BUMP_ID.Equals(postedEntity.BUMP_ID));

            if (orderListEntity.Count() == 0)
            {
                // INSERT
                ORDER_LIST_DETAIL postedEntity1 = new ORDER_LIST_DETAIL();
                postedEntity1.ORDER_NO = postedEntity.ORDER_NO;
                // MAKE BUMP ID
                postedEntity1.BUMP_ID = postedEntity.BUMP_TYPE + "_" + postedEntity.MATERIAL_BUMP + "_" + postedEntity.MATERIAL_FAN + "_" + postedEntity.MATERIAL_ROLLER;

                postedEntity1.BUMP_NM = postedEntity.BUMP_NM;
                postedEntity1.BUMP_TYPE = postedEntity.BUMP_TYPE;
                postedEntity1.NUMBER = postedEntity.NUMBER;
                postedEntity1.UNIT = postedEntity.UNIT;
                postedEntity1.PRICE = postedEntity.PRICE;
                postedEntity1.AMOUNT = postedEntity.AMOUNT;
                postedEntity1.BUMP_SERIAL_NO = postedEntity.BUMP_SERIAL_NO;
                postedEntity1.FLOW = postedEntity.FLOW;
                postedEntity1.LIFT = postedEntity.LIFT;
                postedEntity1.MATERIAL_BUMP = postedEntity.MATERIAL_BUMP;
                postedEntity1.MATERIAL_FAN = postedEntity.MATERIAL_FAN;
                postedEntity1.MATERIAL_ROLLER = postedEntity.MATERIAL_ROLLER;
                postedEntity1.MOTOR_BRAND = postedEntity.MOTOR_BRAND;
                postedEntity1.MOTOR_DEMAND = postedEntity.MOTOR_DEMAND;
                postedEntity1.SEAL_FORM = postedEntity.SEAL_FORM;
                postedEntity1.SEAL_BRAND = postedEntity.SEAL_BRAND;
                postedEntity1.ROLLER_BRAND = postedEntity.ROLLER_BRAND;
                postedEntity1.COUPLING = postedEntity.COUPLING;
                postedEntity1.SEAL_COOLER = postedEntity.SEAL_COOLER;
                postedEntity1.CAVITATION_ALLOWANCE = postedEntity.CAVITATION_ALLOWANCE;
                postedEntity1.ACTUAL_BUMP_SPEED = postedEntity.ACTUAL_BUMP_SPEED;
                postedEntity1.STATION = postedEntity.STATION;
                postedEntity1.TEMPERATURE = postedEntity.TEMPERATURE;
                postedEntity1.DENSITY = postedEntity.DENSITY;
                postedEntity1.IN_PRESSURE = postedEntity.IN_PRESSURE;
                postedEntity1.MEDIUM = postedEntity.MEDIUM;
                postedEntity1.VISCOSITY = postedEntity.VISCOSITY;
                postedEntity1.PARTICULATES = postedEntity.PARTICULATES;
                postedEntity1.WORKING_PRESSURE = postedEntity.WORKING_PRESSURE;
                postedEntity1.FLANGES_STANDARD = postedEntity.FLANGES_STANDARD;
                postedEntity1.FLANGES_LEVEL = postedEntity.FLANGES_LEVEL;
                postedEntity1.BASE = postedEntity.BASE;
                postedEntity1.COUPLING_HOOD = postedEntity.COUPLING_HOOD;
                postedEntity1.ANCHOR_BOLT = postedEntity.ANCHOR_BOLT;
                postedEntity1.PAINT = postedEntity.PAINT;
                postedEntity1.SURFACE_TREATMENT = postedEntity.SURFACE_TREATMENT;
                postedEntity1.PACKAGE = postedEntity.PACKAGE;
                postedEntity1.TRANSPORT = postedEntity.TRANSPORT;
                postedEntity1.SEAL_BRAND_MANUAL = String.IsNullOrEmpty(postedEntity.SEAL_BRAND_MANUAL) ? "" : postedEntity.SEAL_BRAND_MANUAL;
                myContext.ORDER_LIST_DETAIL.Add(postedEntity1);
                myContext.SaveChanges();
                return "";
            }
            else
            {
                orderListEntity.First().BUMP_NM = postedEntity.BUMP_NM;
                orderListEntity.First().BUMP_TYPE = postedEntity.BUMP_TYPE;
                orderListEntity.First().NUMBER = postedEntity.NUMBER;

                orderListEntity.First().UNIT = postedEntity.UNIT;
                orderListEntity.First().PRICE = postedEntity.PRICE;
                orderListEntity.First().AMOUNT = postedEntity.AMOUNT;
                orderListEntity.First().BUMP_SERIAL_NO = postedEntity.BUMP_SERIAL_NO;
                orderListEntity.First().FLOW = postedEntity.FLOW;
                orderListEntity.First().LIFT = postedEntity.LIFT;

                orderListEntity.First().MATERIAL_BUMP = postedEntity.MATERIAL_BUMP;
                orderListEntity.First().MATERIAL_FAN = postedEntity.MATERIAL_FAN;
                orderListEntity.First().MATERIAL_ROLLER = postedEntity.MATERIAL_ROLLER;
                orderListEntity.First().MOTOR_BRAND = postedEntity.MOTOR_BRAND;
                orderListEntity.First().MOTOR_DEMAND = postedEntity.MOTOR_DEMAND;

                orderListEntity.First().SEAL_FORM = postedEntity.SEAL_FORM;
                orderListEntity.First().SEAL_BRAND = postedEntity.SEAL_BRAND;
                orderListEntity.First().ROLLER_BRAND = postedEntity.ROLLER_BRAND;
                orderListEntity.First().COUPLING = postedEntity.COUPLING;
                orderListEntity.First().SEAL_COOLER = postedEntity.SEAL_COOLER;

                orderListEntity.First().CAVITATION_ALLOWANCE = postedEntity.CAVITATION_ALLOWANCE;
                orderListEntity.First().ACTUAL_BUMP_SPEED = postedEntity.ACTUAL_BUMP_SPEED;
                orderListEntity.First().STATION = postedEntity.STATION;
                orderListEntity.First().TEMPERATURE = postedEntity.TEMPERATURE;
                orderListEntity.First().TEMPERATURE = postedEntity.TEMPERATURE;

                orderListEntity.First().IN_PRESSURE = postedEntity.IN_PRESSURE;
                orderListEntity.First().MEDIUM = postedEntity.MEDIUM;
                orderListEntity.First().VISCOSITY = postedEntity.VISCOSITY;
                orderListEntity.First().PARTICULATES = postedEntity.PARTICULATES;
                orderListEntity.First().WORKING_PRESSURE = postedEntity.WORKING_PRESSURE;

                orderListEntity.First().FLANGES_STANDARD = postedEntity.FLANGES_STANDARD;
                orderListEntity.First().FLANGES_LEVEL = postedEntity.FLANGES_LEVEL;
                orderListEntity.First().BASE = postedEntity.BASE;
                orderListEntity.First().COUPLING_HOOD = postedEntity.COUPLING_HOOD;
                orderListEntity.First().ANCHOR_BOLT = postedEntity.ANCHOR_BOLT;

                orderListEntity.First().PAINT = postedEntity.PAINT;
                orderListEntity.First().SURFACE_TREATMENT = postedEntity.SURFACE_TREATMENT;
                orderListEntity.First().PACKAGE = postedEntity.PACKAGE;
                orderListEntity.First().TRANSPORT = postedEntity.TRANSPORT;

                orderListEntity.First().SEAL_BRAND_MANUAL = String.IsNullOrEmpty(postedEntity.SEAL_BRAND_MANUAL) ? "" : postedEntity.SEAL_BRAND_MANUAL;
                myContext.SaveChanges();
                return "";
            }
        }
    }
}