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
    public class AccessoriesTemplateController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public AccessoriesTemplateController(ABD_DbContext context)
        {
            myContext = context;
        }

        // GET api/AccessoriesTemplate/ORDER_NO
        [HttpGet("{ORDER_NO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string ORDER_NO)
        {
            if (String.IsNullOrEmpty(ORDER_NO))
            {
                return "";
            }
            var AccessoriesTemplateEntity = myContext.ACCESSORIES_TEMPLATE.Where(d => d.ORDER_NO == ORDER_NO.Replace("|SLASH|", "/")).OrderBy(e => e.SEQ_ID);
            if (AccessoriesTemplateEntity.Equals(null))
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(AccessoriesTemplateEntity);
            }
        }

        [HttpDelete("{SeqID}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Delete(int SeqID)
        {
            Boolean isSuccess = true;
            string data = "Delete success";
            if (!String.IsNullOrEmpty(SeqID.ToString()))
            {
                int ID = Convert.ToInt32(SeqID);
                ACCESSORIES_TEMPLATE delEntity = new ACCESSORIES_TEMPLATE() { SEQ_ID = ID };

                if (delEntity.Equals(null))
                {
                    isSuccess = false;
                    data = "No such data.";
                }
                else
                {
                    // FIND DB ITEM
                    myContext.ACCESSORIES_TEMPLATE.Attach(delEntity);
                    // DELETE DB ITEM
                    var delRes = myContext.ACCESSORIES_TEMPLATE.Remove(delEntity);
                    if (delRes.State == EntityState.Deleted)
                    {
                        // SAVE CHANGES AND DO NOT RETURN
                        myContext.SaveChanges();
                    }
                    else
                    {
                        isSuccess = false;
                        data = "No such data.";
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

        // POST api/orderListDetailForm
        [HttpPost]
        [EnableCors("CorsPolicy")]
        // JObject here is very very important...
        public ActionResult<string> Post([FromBody]JObject entityObj)
        {
            ACCESSORIES_TEMPLATE postedEntity = entityObj.ToObject<ACCESSORIES_TEMPLATE>();

            // 校验主键
            if (String.IsNullOrEmpty(postedEntity.SEQ_ID.ToString()))
            {
                return "";
            }

            var orderListEntity = myContext.ACCESSORIES_TEMPLATE.Where(d => d.SEQ_ID.Equals(postedEntity.SEQ_ID));

            if (postedEntity.SEQ_ID == 0)
            {
                // INSERT
                if (myContext.ACCESSORIES_TEMPLATE.Count() == 0)
                {
                    postedEntity.SEQ_ID = 1;
                }
                else
                {
                    postedEntity.SEQ_ID = myContext.ACCESSORIES_TEMPLATE.Select(m => m.SEQ_ID).Max() + 1;
                }

                myContext.ACCESSORIES_TEMPLATE.Add(postedEntity);
                myContext.SaveChanges();
                return "";
            }
            else
            {
                orderListEntity.First().NAME = postedEntity.NAME;
                orderListEntity.First().SPEC = postedEntity.SPEC;
                orderListEntity.First().QTY = postedEntity.QTY;
                orderListEntity.First().UNIT = postedEntity.UNIT;
                orderListEntity.First().PRICE = postedEntity.PRICE;
                orderListEntity.First().TOTAL_AMT = postedEntity.TOTAL_AMT;
                orderListEntity.First().BUMP_TYPE = postedEntity.BUMP_TYPE;
                orderListEntity.First().REMARK = postedEntity.REMARK;
                myContext.SaveChanges();
                return "";
            }
        }
    }
}