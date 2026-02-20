using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Data.Config;

public class DepartmentConfig : IEntityTypeConfiguration<Department>
{
  

    public void Configure(EntityTypeBuilder<Department> builder)
    {
             builder.ToTable("Departments");   
     builder.HasKey(d=>d.Id);
     builder.Property(d=>d.Id).UseIdentityColumn();
     builder.Property(d=>d.DepartmentName).IsRequired().HasMaxLength(150);
        builder.Property(d=>d.Description).HasMaxLength(500).IsRequired(false);
    }
}