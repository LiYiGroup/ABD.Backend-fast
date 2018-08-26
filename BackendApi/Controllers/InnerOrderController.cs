using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.DB;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BackendApi.Controllers
{
    [Route("api/[controller]/existBumpInfo")]
    [ApiController]
    public class InnerOrderController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public InnerOrderController(ABD_DbContext context)
        {
            myContext = context;
        }
        // GET api/innerOrder/BUMP_INFO
        [HttpGet("{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string BUMP_INFO)
        {
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;

                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];
                    returnJson = JsonConvert.SerializeObject(myContext.ORDER_LIST_DETAIL
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).First());
                }
            }
            return returnJson;
        }
    }
}