using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.DB;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BackendApi.DB.SearchModel;
using BackendApi.DB.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderQueryResultController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public OrderQueryResultController(ABD_DbContext context)

        {
            myContext = context;
        }

        // GET api/OrderQuerymodel
        [HttpGet]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get()
        {
            var orderListEntity = from a in myContext.ORDER_LIST_MST
                                  join b in myContext.ORDER_QUERY_MST
                                  on a.ORDER_NO equals b.ORDER_NO into c
                                  from c1 in c.DefaultIfEmpty()
                                  where a.ORDER_NO.ToString() != null
                                  select new ORDER_QUERY_RESULT
                                  {
                                      SALES_PERSON = a.SALES_PERSON,
                                      ORDER_UNIT = a.ORDER_UNIT,
                                      PROJECT_NM = a.PROJECT_NM,
                                      DEPARTURE_DATE = a.DEPARTURE_DATE,
                                      DELIVERY_DATE = a.DELIVERY_DATE,
                                      ORDER_NO = a.ORDER_NO,
                                      QTY = string.IsNullOrEmpty(c1.QTY.ToString()) ? 0 : c1.QTY,
                                      TOTAL_AMT = string.IsNullOrEmpty(c1.TOTAL_AMT.ToString()) ? 0 : c1.TOTAL_AMT,
                                      DISCOUNT = string.IsNullOrEmpty(c1.DISCOUNT.ToString()) ? 0 : c1.DISCOUNT,
                                      DELIVERY_QTY = string.IsNullOrEmpty(c1.DELIVERY_QTY.ToString()) ? 0 : c1.DELIVERY_QTY,
                                      DOC_AMT = string.IsNullOrEmpty(c1.DOC_AMT.ToString()) ? 0 : c1.DOC_AMT,
                                      ARRIVAL_AMT = string.IsNullOrEmpty(c1.ARRIVAL_AMT.ToString()) ? 0 : c1.ARRIVAL_AMT,
                                      REMAIN_AMT = string.IsNullOrEmpty(c1.QTY.ToString()) ? 0 : c1.REMAIN_AMT,
                                  };
            return JsonConvert.SerializeObject(orderListEntity.ToList());
        }

        // POST api/orderList
        [HttpPost("searchWithCond")]
        [HttpPost]
        [EnableCors("CorsPolicy")]
        // JObject here is very very important...
        public ActionResult<string> Post([FromBody]JObject searchCondObj)
        {

            OrderQueryResultSearchObject searchObj = searchCondObj.ToObject<OrderQueryResultSearchObject>();

            var orderListEntity = from a in myContext.ORDER_LIST_MST
                                  join b in myContext.ORDER_QUERY_MST
                                  on a.ORDER_NO equals b.ORDER_NO into c
                                  from c1 in c.DefaultIfEmpty()
                                  where a.ORDER_NO.ToString() != null
                                  select new ORDER_QUERY_RESULT
                                  {
                                      SALES_PERSON = a.SALES_PERSON,
                                      ORDER_UNIT = a.ORDER_UNIT,
                                      PROJECT_NM = a.PROJECT_NM,
                                      DEPARTURE_DATE = a.DEPARTURE_DATE,
                                      DELIVERY_DATE = a.DELIVERY_DATE,
                                      ORDER_NO = a.ORDER_NO,
                                      QTY = string.IsNullOrEmpty(c1.QTY.ToString()) ? 0 : c1.QTY,
                                      TOTAL_AMT = string.IsNullOrEmpty(c1.TOTAL_AMT.ToString()) ? 0 : c1.TOTAL_AMT,
                                      DISCOUNT = string.IsNullOrEmpty(c1.DISCOUNT.ToString()) ? 0 : c1.DISCOUNT,
                                      DELIVERY_QTY = string.IsNullOrEmpty(c1.DELIVERY_QTY.ToString()) ? 0 : c1.DELIVERY_QTY,
                                      DOC_AMT = string.IsNullOrEmpty(c1.DOC_AMT.ToString()) ? 0 : c1.DOC_AMT,
                                      ARRIVAL_AMT = string.IsNullOrEmpty(c1.ARRIVAL_AMT.ToString()) ? 0 : c1.ARRIVAL_AMT,
                                      REMAIN_AMT = string.IsNullOrEmpty(c1.QTY.ToString()) ? 0 : c1.REMAIN_AMT,
                                  };

            if (!String.IsNullOrEmpty(searchObj.SALES_PERSON))
            {
                orderListEntity = orderListEntity.Where(d => d.SALES_PERSON.Contains(searchObj.SALES_PERSON));
            }
            if (!String.IsNullOrEmpty(searchObj.ORDER_UNIT))
            {
                orderListEntity = orderListEntity.Where(d => d.ORDER_UNIT.Contains(searchObj.ORDER_UNIT));
            }
            if (!String.IsNullOrEmpty(searchObj.PROJECT_NM))
            {
                orderListEntity = orderListEntity.Where(d => d.PROJECT_NM.Contains(searchObj.PROJECT_NM));
            }
            if (searchObj.DEPARTURE_DATE_ST.ToShortDateString().CompareTo("1970-01-01") > 0)
            {
                orderListEntity = orderListEntity.Where(d => d.DEPARTURE_DATE.CompareTo(searchObj.DEPARTURE_DATE_ST) > 0);
            }
            if (searchObj.DEPARTURE_DATE_ED.ToShortDateString().CompareTo("1970-01-01") > 0)
            {
                orderListEntity = orderListEntity.Where(d => d.DEPARTURE_DATE.CompareTo(searchObj.DEPARTURE_DATE_ED) < 0);
            }
            if (searchObj.DELIVERY_DATE_ST.ToShortDateString().CompareTo("1970-01-01") > 0)
            {
                orderListEntity = orderListEntity.Where(d => d.DELIVERY_DATE.CompareTo(searchObj.DELIVERY_DATE_ST) > 0);
            }
            if (searchObj.DELIVERY_DATE_ED.ToShortDateString().CompareTo("1970-01-01") > 0)
            {
                orderListEntity = orderListEntity.Where(d => d.DELIVERY_DATE.CompareTo(searchObj.DELIVERY_DATE_ED) < 0);
            }
            if (!String.IsNullOrEmpty(searchObj.ORDER_NO))
            {
                orderListEntity = orderListEntity.Where(d => d.ORDER_NO.Contains(searchObj.ORDER_NO));
            }
            if (orderListEntity.Equals(null))
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(orderListEntity.ToList());
            }
        }

        // PUT api/orderList/ORDER_NO_001
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

    }
}
