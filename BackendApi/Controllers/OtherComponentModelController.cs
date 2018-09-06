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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherComponentModelController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public OtherComponentModelController(ABD_DbContext context)

        {
            myContext = context;
        }

        // GET api/othercomponentmodel
        [HttpGet]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get()
        {
            String returnJson = JsonConvert.SerializeObject(myContext.OTHER_COMPONENT_MODEL_MST.ToList());
            return returnJson;
        }

        // POST api/orderList
        [HttpPost("searchWithCond")]
        [HttpPost]
          [EnableCors("CorsPolicy")]
          // JObject here is very very important...
          public ActionResult<string> Post1([FromBody]JObject searchCondObj)
          {

			OtherComponentModelSearchObject searchObj = searchCondObj.ToObject<OtherComponentModelSearchObject>();

              var orderListEntity = myContext.OTHER_COMPONENT_MODEL_MST.Where(d => d.BUMP_TYPE != null);

              if (!String.IsNullOrEmpty(searchObj.BUMP_TYPE))
              {
                  orderListEntity = orderListEntity.Where(d => d.BUMP_TYPE.Contains(searchObj.BUMP_TYPE));
              }
              if (!String.IsNullOrEmpty(searchObj.BASE_TYPE))
              {
                  orderListEntity = orderListEntity.Where(d => d.BASE_TYPE.Contains(searchObj.BASE_TYPE));
              }
              if (!String.IsNullOrEmpty(searchObj.COUPLING_HOOD_TYPE))
              {
                  orderListEntity = orderListEntity.Where(d => d.COUPLING_HOOD_TYPE.Contains(searchObj.COUPLING_HOOD_TYPE));
              }
              if (!String.IsNullOrEmpty(searchObj.COUPLING_BUMP_COUPLET))
              {
                  orderListEntity = orderListEntity.Where(d => d.COUPLING_BUMP_COUPLET.Contains(searchObj.COUPLING_BUMP_COUPLET));
              }
              if (!String.IsNullOrEmpty(searchObj.COUPLING_ELECTRIC_COUPLET))
              {
                  orderListEntity = orderListEntity.Where(d => d.COUPLING_ELECTRIC_COUPLET.Contains(searchObj.COUPLING_ELECTRIC_COUPLET));
              }
			if (!String.IsNullOrEmpty(searchObj.COUPLING_PIN))
			{
				orderListEntity = orderListEntity.Where(d => d.COUPLING_PIN.Contains(searchObj.COUPLING_PIN));
			}
			if (!String.IsNullOrEmpty(searchObj.COUPLING_JUMP_RING))
			{
				orderListEntity = orderListEntity.Where(d => d.COUPLING_JUMP_RING.Contains(searchObj.COUPLING_JUMP_RING));
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

		// DELETE api/othercomponentmodel/5
		[HttpDelete("{BUMP_TYPE}")]
		[EnableCors("CorsPolicy")]
		public ActionResult<string> Delete(string BUMP_TYPE)
		{
			Boolean isSuccess = true;
			string data = "Delete success";
			if (!String.IsNullOrEmpty(BUMP_TYPE))
			{
				String[] BUMP_TYPE_LIST = BUMP_TYPE.Split(",");
				String[] RESULTS_BUMP_TYPE_LIST =  new String [BUMP_TYPE_LIST.Length] ;
				for(int i = 0 ; i < BUMP_TYPE_LIST.Length ; i++)
				{
					RESULTS_BUMP_TYPE_LIST[i] = BUMP_TYPE_LIST[i].Replace('*','/');
				}


				foreach (string BUMP_ITEM in RESULTS_BUMP_TYPE_LIST)
				{
					OTHER_COMPONENT_MODEL_MST delMstEntity = new OTHER_COMPONENT_MODEL_MST() { BUMP_TYPE = BUMP_ITEM };
					if (delMstEntity.Equals(null))
					{
						isSuccess = false;
						data = "No such data.";
						break;
					}
					else
					{
						// FIND DB ITEM
						myContext.OTHER_COMPONENT_MODEL_MST.Attach(delMstEntity);
						// DELETE DB ITEM
						var delRes = myContext.OTHER_COMPONENT_MODEL_MST.Remove(delMstEntity);
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

        // POST api/othercomponentmodel
        [HttpPost("save")]
        [HttpPost]
		[EnableCors("CorsPolicy")]
		// JObject here is very very important...
		public ActionResult<string> Post2([FromBody]JObject entityObj)
		{
			//声明要插入的对象
			OTHER_COMPONENT_MODEL_MST postedEntity = entityObj.ToObject<OTHER_COMPONENT_MODEL_MST>();
			//获取表格中已有的数据
			var orderListEntity = myContext.OTHER_COMPONENT_MODEL_MST.Where(d => d.BUMP_TYPE != null );

			if (!String.IsNullOrEmpty(postedEntity.BUMP_TYPE))  //修改已有数据
			{
				orderListEntity = orderListEntity.Where(d => d.BUMP_TYPE.Equals(postedEntity.BUMP_TYPE));
			}

			if (orderListEntity.Count() == 0) //如果表中没有任何数据，则插入新数据
			{
				// INSERT
				myContext.OTHER_COMPONENT_MODEL_MST.Add(postedEntity);
				myContext.SaveChanges();
				return "";
			}
			else  //如果表中有数据，则插入新数据
			{
				orderListEntity.First().BUMP_TYPE = postedEntity.BUMP_TYPE;
				orderListEntity.First().BASE_TYPE = postedEntity.BASE_TYPE;
				orderListEntity.First().COUPLING_HOOD_TYPE = postedEntity.COUPLING_HOOD_TYPE;
				orderListEntity.First().COUPLING_BUMP_COUPLET = postedEntity.COUPLING_BUMP_COUPLET;
				orderListEntity.First().COUPLING_ELECTRIC_COUPLET = postedEntity.COUPLING_ELECTRIC_COUPLET;
				orderListEntity.First().COUPLING_PIN = postedEntity.COUPLING_PIN;
				orderListEntity.First().COUPLING_JUMP_RING = postedEntity.COUPLING_JUMP_RING;
				myContext.SaveChanges();
				return "";
			}
		}


	}
}
