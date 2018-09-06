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
	public class AbdDoubleSealMstController : Controller
	{
		private readonly ABD_DbContext myContext;
		public AbdDoubleSealMstController(ABD_DbContext context)

		{
			myContext = context;
		}

		// GET api/abddoublesealmst
		[HttpGet]
		[EnableCors("CorsPolicy")]
		public ActionResult<string> Get()
		{
			String returnJson = JsonConvert.SerializeObject(myContext.ABD_DOUBLE_SEAL_MST.ToList());
			return returnJson;
		}

        // POST api/abddoublesealmst
        [HttpPost("searchWithCond")]
        [HttpPost]
			  [EnableCors("CorsPolicy")]
			  // JObject here is very very important...
			  public ActionResult<string> Post1([FromBody]JObject searchCondObj)
			  {

				AbdDoubleSealMstSearchObject searchObj = searchCondObj.ToObject<AbdDoubleSealMstSearchObject>();

				  var orderListEntity = myContext.ABD_DOUBLE_SEAL_MST.Where(d => d.ID.ToString() != null);

				  if (!String.IsNullOrEmpty(searchObj.MEDIUM_END_MOVABLE_RING))
				  {
					  orderListEntity = orderListEntity.Where(d => d.MEDIUM_END_MOVABLE_RING.Contains(searchObj.MEDIUM_END_MOVABLE_RING));
				  }
				  if (!String.IsNullOrEmpty(searchObj.MEDIUM_END_STATIC_RING))
				  {
					  orderListEntity = orderListEntity.Where(d => d.MEDIUM_END_STATIC_RING.Contains(searchObj.MEDIUM_END_STATIC_RING));
				  }
				  if (!String.IsNullOrEmpty(searchObj.ATMOSPHERIC_END_MOVABLE_RING))
				  {
					  orderListEntity = orderListEntity.Where(d => d.ATMOSPHERIC_END_MOVABLE_RING.Contains(searchObj.ATMOSPHERIC_END_MOVABLE_RING));
				  }
				  if (!String.IsNullOrEmpty(searchObj.ATMOSPHERIC_END_STATIC_RING))
				  {
					  orderListEntity = orderListEntity.Where(d => d.ATMOSPHERIC_END_STATIC_RING.Contains(searchObj.ATMOSPHERIC_END_STATIC_RING));
				  }
				if (!String.IsNullOrEmpty(searchObj.METAL_BASE))
				{
					orderListEntity = orderListEntity.Where(d => d.METAL_BASE.Contains(searchObj.METAL_BASE));
				}
				if (!String.IsNullOrEmpty(searchObj.O_TYPE_RING))
				{
					orderListEntity = orderListEntity.Where(d => d.O_TYPE_RING.Contains(searchObj.O_TYPE_RING));
				}
				if (!String.IsNullOrEmpty(searchObj.SPRING))
				{
					orderListEntity = orderListEntity.Where(d => d.SPRING.Contains(searchObj.SPRING));
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

		// DELETE api/abddoublesealmst/5
		[HttpDelete("{ID}")]
		[EnableCors("CorsPolicy")]
		public ActionResult<string> Delete(string ID)
		{
			Boolean isSuccess = true;
			string data = "Delete success";
			if (!String.IsNullOrEmpty(ID))
			{
				String[] ID_LIST = ID.Split(",");

				foreach (string RESULTS_ITEM in ID_LIST)
				{
					ABD_DOUBLE_SEAL_MST delMstEntity = new ABD_DOUBLE_SEAL_MST() { ID = Convert.ToInt32(RESULTS_ITEM) };
					if (delMstEntity.Equals(null))
					{
						isSuccess = false;
						data = "No such data.";
						break;
					}
					else
					{
						// FIND DB ITEM
						myContext.ABD_DOUBLE_SEAL_MST.Attach(delMstEntity);
						// DELETE DB ITEM
						var delRes = myContext.ABD_DOUBLE_SEAL_MST.Remove(delMstEntity);
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

        // POST api/abddoublesealmst
        [HttpPost("save")]
        [HttpPost]
		[EnableCors("CorsPolicy")]
		// JObject here is very very important...
		public ActionResult<string> Post2([FromBody]JObject entityObj)
		{
			//声明要插入的对象
			ABD_DOUBLE_SEAL_MST postedEntity = entityObj.ToObject<ABD_DOUBLE_SEAL_MST>();
			//获取表格中已有的数据
			var orderListEntity = myContext.ABD_DOUBLE_SEAL_MST.Where(d => d.ID.ToString() != null);

			if (!String.IsNullOrEmpty(postedEntity.ID.ToString()))  //修改已有数据
			{
				orderListEntity = orderListEntity.Where(d => d.ID.Equals(postedEntity.ID));
			}

			if (orderListEntity.Count() == 0) //如果表中没有任何数据，则插入新数据
			{
				// INSERT
				myContext.ABD_DOUBLE_SEAL_MST.Add(postedEntity);
				myContext.SaveChanges();
				return "";
			}
			else  //如果表中有数据，则插入新数据
			{
				orderListEntity.First().MEDIUM_END_MOVABLE_RING = postedEntity.MEDIUM_END_MOVABLE_RING;
				orderListEntity.First().MEDIUM_END_STATIC_RING = postedEntity.MEDIUM_END_STATIC_RING;
				orderListEntity.First().ATMOSPHERIC_END_MOVABLE_RING = postedEntity.ATMOSPHERIC_END_MOVABLE_RING;
				orderListEntity.First().ATMOSPHERIC_END_STATIC_RING = postedEntity.ATMOSPHERIC_END_STATIC_RING;
				orderListEntity.First().SPRING = postedEntity.SPRING;
				orderListEntity.First().METAL_BASE = postedEntity.METAL_BASE;
				orderListEntity.First().O_TYPE_RING = postedEntity.O_TYPE_RING;
				myContext.SaveChanges();
				return "";
			}
		}
	}
}