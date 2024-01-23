using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice.Models;

namespace Practice.Configurations
{
    public class ServiceConfiguration:IEntityTypeConfiguration<Service> 
    {

        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Property(x=>x.FullName).IsRequired().HasMaxLength(64);
            builder.Property(x=>x.Description).IsRequired().HasMaxLength(256);
        }
    }
}
