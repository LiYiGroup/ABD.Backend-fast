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
    }
}