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
    public class OrderDetailListController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public OrderDetailListController(ABD_DbContext context)
        {
            myContext = context;
        }

        // GET api/orderDetailList
        [HttpGet]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get()
        {
            String returnJson = JsonConvert.SerializeObject(myContext.ORDER_LIST_DETAIL.ToList());
            return returnJson;
        }

        // GET api/orderDetailList/ORDER_NO_001
        [HttpGet("{ORDER_NO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string ORDER_NO)
        {
            var orderListDetailEntity = myContext.ORDER_LIST_DETAIL.Where(d => d.ORDER_NO == ORDER_NO);
            if (orderListDetailEntity.Equals(null))
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(orderListDetailEntity.ToList());
            }
        }
    }
}