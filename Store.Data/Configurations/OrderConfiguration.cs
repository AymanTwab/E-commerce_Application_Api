using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.OrderEntities;

namespace Store.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(order => order.ShippingAddress, shippingAddress =>
            {
                shippingAddress.WithOwner();
            });

            builder.HasOne(order => order.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
