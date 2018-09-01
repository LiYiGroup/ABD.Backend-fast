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

        }

        public virtual DbSet<ORDER_LIST_MST> ORDER_LIST_MST { get; set; }
        public virtual DbSet<ORDER_LIST_DETAIL> ORDER_LIST_DETAIL { get; set; }
        public virtual DbSet<INNER_ORDER_BASIC_SEAL_MST> INNER_ORDER_BASIC_SEAL_MST { get; set; }
        public virtual DbSet<INNER_ORDER_OTHER_COMPONENT_MST> INNER_ORDER_OTHER_COMPONENT_MST { get; set; }
    }
}
