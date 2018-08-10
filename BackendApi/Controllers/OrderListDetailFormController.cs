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
    public class OrderListDetailFormController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public OrderListDetailFormController(ABD_DbContext context)
        {
            myContext = context;
        }

        // GET api/orderListDetailForm/ORDER_NO_001
        [HttpGet("{ORDER_NO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string ORDER_NO)
        {
            var orderListDetailEntity = myContext.ORDER_LIST_MST.Where(d => d.ORDER_NO == ORDER_NO).First();
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

            ORDER_LIST_MST postedEntity = entityObj.ToObject<ORDER_LIST_MST>();

            var orderListEntity = myContext.ORDER_LIST_MST.Where(d => d.ORDER_NO != null);

            if (!String.IsNullOrEmpty(postedEntity.ORDER_NO))
            {
                orderListEntity = orderListEntity.Where(d => d.ORDER_NO.Contains(postedEntity.ORDER_NO));
            }

            if (orderListEntity.Count() == 0)
            {
                // INSERT
                myContext.ORDER_LIST_MST.Add(postedEntity);
                myContext.SaveChanges();
                return "";
            }
            else
            {
                orderListEntity.First().CONTRACT_NO = postedEntity.CONTRACT_NO;
                orderListEntity.First().PROJECT_NM = postedEntity.PROJECT_NM;
                orderListEntity.First().ORDER_UNIT = postedEntity.ORDER_UNIT;
                orderListEntity.First().SALES_PERSON = postedEntity.SALES_PERSON;
                orderListEntity.First().DEPARTURE_DATE = postedEntity.DEPARTURE_DATE;
                orderListEntity.First().DELIVERY_DATE = postedEntity.DELIVERY_DATE;
                orderListEntity.First().REMARK = postedEntity.REMARK;
                orderListEntity.First().APPLICATION_ENGINEER = postedEntity.APPLICATION_ENGINEER;
                myContext.SaveChanges();
                return "";
            }
        }
    }
}