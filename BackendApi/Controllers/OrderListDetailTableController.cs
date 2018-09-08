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
            String returnJson = JsonConvert.SerializeObject(myContext.ORDER_LIST_DETAIL.ToList());
            return returnJson;
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
            var orderListDetailEntity = myContext.ORDER_LIST_DETAIL.Where(d => d.ORDER_NO == ORDER_NO.Replace("|SLASH|", "/"));
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
                string[] BUMP_ID_LIST = {};
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

            ORDER_LIST_DETAIL postedEntity = entityObj.ToObject<ORDER_LIST_DETAIL>();

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
                // MAKE BUMP ID
                postedEntity.BUMP_ID = postedEntity.BUMP_TYPE + "_" + postedEntity.MATERIAL;
                myContext.ORDER_LIST_DETAIL.Add(postedEntity);
                myContext.SaveChanges();
                return "";
            }
            else
            {
                orderListEntity.First().BUMP_NM = postedEntity.BUMP_NM;
                orderListEntity.First().STATION = postedEntity.STATION;
                orderListEntity.First().BUMP_TYPE = postedEntity.BUMP_TYPE;
                orderListEntity.First().NUMBER = postedEntity.NUMBER;
                orderListEntity.First().FLOW = postedEntity.FLOW;
                orderListEntity.First().LIFT = postedEntity.LIFT;
                orderListEntity.First().MATERIAL = postedEntity.MATERIAL;
                orderListEntity.First().SEAL = postedEntity.SEAL;
                orderListEntity.First().STATUS = postedEntity.STATUS;
                orderListEntity.First().REMARK = postedEntity.REMARK;
                myContext.SaveChanges();
                return "";
            }
        }
    }
}