using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.DB;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        // GET api/orderList/ORDER_NO_001
        [HttpGet("{ORDER_NO}")]
        public ActionResult<string> Get(string ORDER_NO)
        {
            var orderListEntity = myContext.ORDER_LIST_MST.Find(ORDER_NO);
            if (orderListEntity.Equals(null))
            {
                return "";
            }
            else
            {
                return orderListEntity.ORDER_NO;
            }
        }

        // POST api/orderList
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/orderList/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/orderList/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
