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
            if (String.IsNullOrEmpty(ORDER_NO))
            {
                return "";
            }
            var orderListDetailEntity = myContext.ORDER_LIST_MST.Where(d => d.ORDER_NO == ORDER_NO.Replace("|SLASH|", "/")).First();
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

            // 校验主键
            if (String.IsNullOrEmpty(postedEntity.ORDER_NO))
            {
                return "";
            }

            var orderListEntity = myContext.ORDER_LIST_MST.Where(d => d.ORDER_NO.Equals(postedEntity.ORDER_NO));

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
                orderListEntity.First().DEPARTURE_DATE = postedEntity.DEPARTURE_DATE.ToLocalTime();
                orderListEntity.First().DELIVERY_DATE = postedEntity.DELIVERY_DATE.ToLocalTime();
                orderListEntity.First().REMARK = postedEntity.REMARK;
                orderListEntity.First().APPLICATION_ENGINEER = postedEntity.APPLICATION_ENGINEER;
                orderListEntity.First().DEBUG = postedEntity.DEBUG;
                orderListEntity.First().TOTAL_QTY = postedEntity.TOTAL_QTY;
                orderListEntity.First().TEX_RATE = postedEntity.TEX_RATE;
                orderListEntity.First().GUARANTEE_DATE = postedEntity.GUARANTEE_DATE;
                orderListEntity.First().TOTAL_AMT = postedEntity.TOTAL_AMT;
                orderListEntity.First().PAYMENT = postedEntity.PAYMENT;
                orderListEntity.First().TARGET_PRICE = postedEntity.TARGET_PRICE;
                orderListEntity.First().DISCOUNT = postedEntity.DISCOUNT;
                orderListEntity.First().CHANGE_HIS1 = postedEntity.CHANGE_HIS1;
                orderListEntity.First().CHANGE_HIS2 = postedEntity.CHANGE_HIS2;
                myContext.SaveChanges();
                return "";
            }
        }
    }
}