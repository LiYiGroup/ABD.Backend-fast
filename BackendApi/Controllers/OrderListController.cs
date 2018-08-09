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

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderListController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public OrderListController(ABD_DbContext context)

        {
            myContext = context;
        }

        // GET api/orderList
        [HttpGet]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get()
        {
            String returnJson = JsonConvert.SerializeObject(myContext.ORDER_LIST_MST.ToList());
            return returnJson;
        }

        // GET api/orderList
        [HttpGet("{searchOrderItem}")]
        public ActionResult<string> Get(string searchCondition)
        {
            return "";
        }

        // POST api/orderList
        [HttpPost]
        [EnableCors("CorsPolicy")]
        // JObject here is very very important...
        public ActionResult<string> Post([FromBody]JObject searchCondObj)
        {

            OrderListSearchObject searchObj = searchCondObj.ToObject<OrderListSearchObject>();

            var orderListEntity = myContext.ORDER_LIST_MST.Where(d => d.ORDER_NO != null);

            if (!String.IsNullOrEmpty(searchObj.ORDER_NO))
            {
                orderListEntity = orderListEntity.Where(d => d.ORDER_NO.Contains(searchObj.ORDER_NO));
            }
            if (!String.IsNullOrEmpty(searchObj.CONTRACT_NO))
            {
                orderListEntity = orderListEntity.Where(d => d.CONTRACT_NO.Contains(searchObj.CONTRACT_NO));
            }
            if (!String.IsNullOrEmpty(searchObj.PROJECT_NM))
            {
                orderListEntity = orderListEntity.Where(d => d.PROJECT_NM.Contains(searchObj.PROJECT_NM));
            }
            if (!String.IsNullOrEmpty(searchObj.ORDER_UNIT))
            {
                orderListEntity = orderListEntity.Where(d => d.ORDER_UNIT.Contains(searchObj.ORDER_UNIT));
            }
            if (!String.IsNullOrEmpty(searchObj.SALES_PERSON))
            {
                orderListEntity = orderListEntity.Where(d => d.SALES_PERSON.Contains(searchObj.SALES_PERSON));
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

        // DELETE api/orderList/5
        [HttpDelete("{ORDER_NO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Delete(string ORDER_NO)
        {
            Boolean isSuccess = true;
            string data = "Delete success";
            if (!String.IsNullOrEmpty(ORDER_NO))
            {
                String[] ORDER_NO_LIST = ORDER_NO.Split(",");

                foreach (string ORDER_ITEM in ORDER_NO_LIST)
                {
                    ORDER_LIST_MST delEntity = new ORDER_LIST_MST() { ORDER_NO = ORDER_ITEM };

                    if (delEntity.Equals(null))
                    {
                        isSuccess = false;
                        data = "No such data.";
                        break;
                    }
                    else
                    {
                        // FIND ORDER DETAIL
                        int deteilNum = myContext.ORDER_LIST_DETAIL.Count(d => d.ORDER_NO == ORDER_ITEM);
                        if (deteilNum > 0)
                        {
                            isSuccess = false;
                            data = ORDER_ITEM + " has detail info.";
                            break;
                        }
                        // FIND DB ITEM
                        myContext.ORDER_LIST_MST.Attach(delEntity);
                        // DELETE DB ITEM
                        var delRes = myContext.ORDER_LIST_MST.Remove(delEntity);
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
    }
}
