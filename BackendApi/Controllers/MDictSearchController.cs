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
    public class MDictSearchController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public MDictSearchController(ABD_DbContext context)
        {
            myContext = context;
        }

        //GET api/MDictController/ORDER_NO_001
        [HttpGet("{TYPE}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string TYPE)
        {
            if (String.IsNullOrEmpty(TYPE))
            {
                return "";
            }
            if (TYPE.ToUpper().Equals("ALL"))
            {
                String returnJson = JsonConvert.SerializeObject(myContext.M_DICT.ToList());
                return returnJson;
            }
            var orderDictEntity = myContext.M_DICT.Where(d => d.TYPE == TYPE.Replace("|SLASH|", "/")).OrderBy(e => e.DICT_ID);
            if (orderDictEntity.Equals(null))
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(orderDictEntity);
            }
        }

        // POST api/MDictController
        [HttpPost]
        [EnableCors("CorsPolicy")]
        // JObject here is very very important...
        public ActionResult<string> Post([FromBody]JObject entityObj)
        {
            M_DICT postedEntity = entityObj.ToObject<M_DICT>();

            // 校验主键
            if (String.IsNullOrEmpty(postedEntity.DICT_ID))
            {
                return "";
            }

            var orderListEntity = myContext.M_DICT.Where(d => d.DICT_ID.Equals(postedEntity.DICT_ID));

            if (orderListEntity.Count() == 0)
            {
                // INSERT
                myContext.M_DICT.Add(postedEntity);
                myContext.SaveChanges();
                return "";
            }
            else
            {
                orderListEntity.First().DICT_ID = postedEntity.DICT_ID;
                orderListEntity.First().DICT_NAME = postedEntity.DICT_NAME;
                orderListEntity.First().TYPE = postedEntity.TYPE;
                orderListEntity.First().TYPE_NAME = postedEntity.TYPE_NAME;
                myContext.SaveChanges();
                return "";
            }
        }
    }
}