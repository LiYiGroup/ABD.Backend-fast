using BackendApi.DB.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.SearchModel
{
    public class InnerOrderGridObject
    {
        public List<BOM_ITEM_BASE> innerOrderGridBomItemBase { get; set; }
        public List<BOM_ITEM_STANDARD> innerOrderGridBomItemStandard { get; set; }
    }
}
