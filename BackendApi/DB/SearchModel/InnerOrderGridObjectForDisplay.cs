using BackendApi.DB.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.SearchModel
{
    public class InnerOrderGridObjectForDisplay
    {
        public List<INNER_ORDER_BOM_ITEM_BASE> innerOrderGridBomItemBase { get; set; }
        public List<INNER_ORDER_BOM_ITEM_STANDARD> innerOrderGridBomItemStandard { get; set; }
    }
}
