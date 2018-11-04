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
    public class OrderQueryController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public OrderQueryController(ABD_DbContext context)

        {
            myContext = context;
        }

        // GET api/OrderQuerymodel
        [HttpGet]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get()
        {
            String returnJson = JsonConvert.SerializeObject(myContext.ORDER_QUERY_MST.ToList());
            return returnJson;
        }

        // POST api/orderList
        [HttpPost("searchWithCond")]
        [HttpPost]
        [EnableCors("CorsPolicy")]
        // JObject here is very very important...
        public ActionResult<string> Post1([FromBody]JObject searchCondObj)
        {

            OrderQuerySearchObject searchObj = searchCondObj.ToObject<OrderQuerySearchObject>();

            var orderListEntity = myContext.ORDER_QUERY_MST.Where(d => d.NO.ToString() != null);

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
