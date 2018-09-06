using BackendApi.DB.DataModel;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.DB
{
    public class ABD_DbContext: DbContext
    {
        public ABD_DbContext(DbContextOptions<ABD_DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ORDER_LIST_MST>().HasKey(c => new { c.ORDER_NO });
            builder.Entity<ORDER_LIST_DETAIL>().HasKey(c => new { c.ORDER_NO, c.BUMP_ID });
            builder.Entity<INNER_ORDER_BASIC_SEAL_MST>().HasKey(c => new { c.ORDER_NO, c.BUMP_ID });
            builder.Entity<INNER_ORDER_OTHER_COMPONENT_MST>().HasKey(c => new { c.ORDER_NO, c.BUMP_ID });
            builder.Entity<OTHER_COMPONENT_MODEL_MST>().HasKey(c => new { c.BUMP_TYPE });
            builder.Entity<ABD_DOUBLE_SEAL_MST>().HasKey(c => new { c.ID });
            builder.Entity<ABD_INTEGRATE_SEAL_MST>().HasKey(c => new { c.ID });
            builder.Entity<ABD_SINGLE_SEAL_MST>().HasKey(c => new { c.ID });
            builder.Entity<BOM_ITEM_BASE>().HasKey(c => new { c.BOM_ID, c.ITEM_NO });
            builder.Entity<BOM_ITEM_STANDARD>().HasKey(c => new { c.BOM_ID, c.ITEM_NO });
            builder.Entity<INNER_ORDER_BOM_ITEM_BASE>().HasKey(c => new { c.ORDER_NO, c.BUMP_ID, c.BOM_ID, c.ITEM_NO });
            builder.Entity<INNER_ORDER_BOM_ITEM_STANDARD>().HasKey(c => new { c.ORDER_NO, c.BUMP_ID, c.BOM_ID, c.ITEM_NO });
        }

        public virtual DbSet<ORDER_LIST_MST> ORDER_LIST_MST { get; set; }
        public virtual DbSet<ORDER_LIST_DETAIL> ORDER_LIST_DETAIL { get; set; }
        public virtual DbSet<INNER_ORDER_BASIC_SEAL_MST> INNER_ORDER_BASIC_SEAL_MST { get; set; }
        public virtual DbSet<INNER_ORDER_OTHER_COMPONENT_MST> INNER_ORDER_OTHER_COMPONENT_MST { get; set; }
        public virtual DbSet<OTHER_COMPONENT_MODEL_MST> OTHER_COMPONENT_MODEL_MST { get; set; }
        public virtual DbSet<ABD_DOUBLE_SEAL_MST> ABD_DOUBLE_SEAL_MST { get; set; }
        public virtual DbSet<ABD_INTEGRATE_SEAL_MST> ABD_INTEGRATE_SEAL_MST { get; set; }
        public virtual DbSet<ABD_SINGLE_SEAL_MST> ABD_SINGLE_SEAL_MST { get; set; }
        public virtual DbSet<BOM_ITEM_BASE> BOM_ITEM_BASE { get; set; }
        public virtual DbSet<BOM_ITEM_STANDARD> BOM_ITEM_STANDARD { get; set; }
        public virtual DbSet<INNER_ORDER_BOM_ITEM_BASE> INNER_ORDER_BOM_ITEM_BASE { get; set; }
        public virtual DbSet<INNER_ORDER_BOM_ITEM_STANDARD> INNER_ORDER_BOM_ITEM_STANDARD { get; set; }
    }
}
