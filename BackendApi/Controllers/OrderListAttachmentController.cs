using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.DB;
using BackendApi.DB.DataModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderListAttachmentController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public OrderListAttachmentController(ABD_DbContext context)
        {
            myContext = context;
        }

        // GET api/orderListDetailForm/ORDER_NO_001
        [HttpGet("{ORDER_NO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string ORDER_NO)
        {
            if (String.IsNullOrEmpty(ORDER_NO))
            {
                return "";
            }
            var orderListAttach = myContext.ORDER_LIST_ATTACHMENT.Where(d => d.ORDER_NO == ORDER_NO.Replace("|SLASH|", "/"));
            if (orderListAttach.Count() == 0)
            {
                return "";
            }
            var orderListDetailEntity = myContext.ORDER_LIST_ATTACHMENT.Where(d => d.ORDER_NO == ORDER_NO.Replace("|SLASH|", "/")).First();

            if (orderListDetailEntity.Equals(null))
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(orderListDetailEntity);
            }
        }
        // POST api/orderListDetailForm
        [HttpPost]
        [EnableCors("CorsPolicy")]
        // JObject here is very very important...
        public ActionResult<string> Post([FromBody]JObject entityObj)
        {
            ORDER_LIST_ATTACHMENT postedEntity = entityObj.ToObject<ORDER_LIST_ATTACHMENT>();
            // 校验主键
            if (String.IsNullOrEmpty(postedEntity.ORDER_NO))
            {
                return "";
            }

            var orderListEntity = myContext.ORDER_LIST_ATTACHMENT.Where(d => d.ORDER_NO.Equals(postedEntity.ORDER_NO));

            if (orderListEntity.Count() == 0)
            {
                // INSERT
                myContext.ORDER_LIST_ATTACHMENT.Add(postedEntity);
                myContext.SaveChanges();
                return "";
            }
            else
            {
                orderListEntity.First().QUALIFICATION = postedEntity.QUALIFICATION;
                orderListEntity.First().IDENTIFICATION = postedEntity.IDENTIFICATION;
                orderListEntity.First().PRODUCT_SAMPLE = postedEntity.PRODUCT_SAMPLE;
                orderListEntity.First().DYNAMIC_REPORT = postedEntity.DYNAMIC_REPORT;
                orderListEntity.First().STATIC_REPORT = postedEntity.STATIC_REPORT;
                orderListEntity.First().PERFORMANCE_REPORT = postedEntity.PERFORMANCE_REPORT;
                orderListEntity.First().BLUEPRINT = postedEntity.BLUEPRINT;
                orderListEntity.First().PERFORMANCE_CURVE = postedEntity.PERFORMANCE_CURVE;
                orderListEntity.First().OTHER = postedEntity.OTHER;
                myContext.SaveChanges();
                return "";
            }
        }
    }
    //// GET api/orderListDetailForm/ORDER_NO_001
    //[HttpGet("{ORDER_NO}")]
    //[EnableCors("CorsPolicy")]
    //public ActionResult<string> Get(string ORDER_NO)
    //{
    //    if (String.IsNullOrEmpty(ORDER_NO))
    //    {
    //        return "";
    //    }
    //    var orderListDetailEntity = from a in myContext.ORDER_LIST_ATTACHMENT
    //                                join b in myContext.M_DICT
    //                                on a.QUALIFICATION equals b.DICT_ID into b1
    //                                from b2 in b1.DefaultIfEmpty()

    //                                join c in myContext.M_DICT
    //                                on a.IDENTIFICATION equals c.DICT_ID into c1
    //                                from c2 in c1.DefaultIfEmpty()

    //                                join d in myContext.M_DICT
    //                                on a.PRODUCT_SAMPLE equals d.DICT_ID into d1
    //                                from d2 in d1.DefaultIfEmpty()

    //                                join e in myContext.M_DICT
    //                                on a.DYNAMIC_REPORT equals e.DICT_ID into e1
    //                                from e2 in e1.DefaultIfEmpty()

    //                                join f in myContext.M_DICT
    //                                on a.STATIC_REPORT equals f.DICT_ID into f1
    //                                from f2 in f1.DefaultIfEmpty()

    //                                join g in myContext.M_DICT
    //                                on a.PERFORMANCE_REPORT equals g.DICT_ID into g1
    //                                from g2 in g1.DefaultIfEmpty()

    //                                join h in myContext.M_DICT
    //                                on a.BLUEPRINT equals h.DICT_ID into h1
    //                                from h2 in h1.DefaultIfEmpty()

    //                                join i in myContext.M_DICT
    //                                on a.PERFORMANCE_CURVE equals i.DICT_ID into i1
    //                                from i2 in i1.DefaultIfEmpty()

    //                                where (a.ORDER_NO.ToString() == ORDER_NO.Replace("|SLASH|", "/")
    //                                //&& b2.TYPE == "18"
    //                                //&& c2.TYPE == "19"
    //                                //&& d2.TYPE == "20"
    //                                //&& e2.TYPE == "21"
    //                                //&& f2.TYPE == "22"
    //                                //&& g2.TYPE == "23"
    //                                //&& h2.TYPE == "24"
    //                                //&& i2.TYPE == "25"
    //                                )
    //                                select new ORDER_LIST_ATTACHMENT_RST
    //                                {
    //                                    ORDER_NO = a.ORDER_NO,
    //                                    QUALIFICATION = a.QUALIFICATION,
    //                                    QUALIFICATION_NAME = b2.DICT_NAME,
    //                                    IDENTIFICATION = a.IDENTIFICATION,
    //                                    IDENTIFICATION_NAME = c2.DICT_NAME,
    //                                    PRODUCT_SAMPLE = a.PRODUCT_SAMPLE,
    //                                    PRODUCT_SAMPLE_NAME = d2.DICT_NAME,
    //                                    DYNAMIC_REPORT = a.DYNAMIC_REPORT,
    //                                    DYNAMIC_REPORT_NAME = e2.DICT_NAME,
    //                                    STATIC_REPORT = a.STATIC_REPORT,
    //                                    STATIC_REPORT_NAME = f2.DICT_NAME,
    //                                    PERFORMANCE_REPORT = a.PERFORMANCE_REPORT,
    //                                    PERFORMANCE_REPORT_NAME = g2.DICT_NAME,
    //                                    BLUEPRINT = a.BLUEPRINT,
    //                                    BLUEPRINT_NAME = h2.DICT_NAME,
    //                                    PERFORMANCE_CURVE = a.PERFORMANCE_CURVE,
    //                                    PERFORMANCE_CURVE_NAME = i2.DICT_NAME,
    //                                    OTHER = a.OTHER,
    //                                };

    //    if (orderListDetailEntity.Equals(null))
    //    {
    //        return "";
    //    }
    //    else
    //    {
    //        return JsonConvert.SerializeObject(orderListDetailEntity);
    //    }
    //}

    //    // POST api/orderListDetailForm
    //    [HttpPost]
    //    [EnableCors("CorsPolicy")]
    //    // JObject here is very very important...
    //    public ActionResult<string> Post([FromBody]JObject entityObj)
    //    {
    //        ORDER_LIST_ATTACHMENT_RST postedEntity = entityObj.ToObject<ORDER_LIST_ATTACHMENT_RST>();
    //        // 校验主键
    //        if (String.IsNullOrEmpty(postedEntity.ORDER_NO))
    //        {
    //            return "";
    //        }

    //        var orderListEntity = myContext.ORDER_LIST_ATTACHMENT.Where(d => d.ORDER_NO.Equals(postedEntity.ORDER_NO));

    //        if (orderListEntity.Count() == 0)
    //        {
    //            // INSERT
    //            ORDER_LIST_ATTACHMENT postedEntity1 = new ORDER_LIST_ATTACHMENT();
    //            postedEntity1.QUALIFICATION = postedEntity.QUALIFICATION;
    //            postedEntity1.IDENTIFICATION = postedEntity.IDENTIFICATION;
    //            postedEntity1.PRODUCT_SAMPLE = postedEntity.PRODUCT_SAMPLE;
    //            postedEntity1.DYNAMIC_REPORT = postedEntity.DYNAMIC_REPORT;
    //            postedEntity1.STATIC_REPORT = postedEntity.STATIC_REPORT;
    //            postedEntity1.PERFORMANCE_REPORT = postedEntity.PERFORMANCE_REPORT;
    //            postedEntity1.BLUEPRINT = postedEntity.BLUEPRINT;
    //            postedEntity1.PERFORMANCE_CURVE = postedEntity.PERFORMANCE_CURVE;
    //            postedEntity1.OTHER = postedEntity.OTHER;

    //            myContext.ORDER_LIST_ATTACHMENT.Add(postedEntity1);
    //            myContext.SaveChanges();
    //            return "";
    //        }
    //        else
    //        {
    //            orderListEntity.First().QUALIFICATION = postedEntity.QUALIFICATION;
    //            orderListEntity.First().IDENTIFICATION = postedEntity.IDENTIFICATION;
    //            orderListEntity.First().PRODUCT_SAMPLE = postedEntity.PRODUCT_SAMPLE;
    //            orderListEntity.First().DYNAMIC_REPORT = postedEntity.DYNAMIC_REPORT;
    //            orderListEntity.First().STATIC_REPORT = postedEntity.STATIC_REPORT;
    //            orderListEntity.First().PERFORMANCE_REPORT = postedEntity.PERFORMANCE_REPORT;
    //            orderListEntity.First().BLUEPRINT = postedEntity.BLUEPRINT;
    //            orderListEntity.First().PERFORMANCE_CURVE = postedEntity.PERFORMANCE_CURVE;
    //            orderListEntity.First().OTHER = postedEntity.OTHER;
    //            myContext.SaveChanges();
    //            return "";
    //        }
    //    }
    //}
}