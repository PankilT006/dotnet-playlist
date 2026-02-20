using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Data.Config;

public class StudentConfig : IEntityTypeConfiguration<Students>
{
    public void Configure(EntityTypeBuilder<Students> builder)
    {
     builder.ToTable("Students");   
     builder.HasKey(s=>s.Id);
     builder.Property(s=>s.Id).UseIdentityColumn();
     builder.Property(s=>s.StudentName).IsRequired();
     builder.Property(s=>s.Email).IsRequired().HasMaxLength(200);
     builder.Property(s=>s.Address).HasMaxLength(500).IsRequired(false);
     builder.HasIndex(s=>s.Email).IsUnique();
    builder.Property(s=>s.MobileNumber).HasMaxLength(15);
    builder.HasIndex(s => s.MobileNumber).IsUnique();
        builder.HasOne(s => s.Department)
            .WithMany(d => d.Students)
            .HasForeignKey(s => s.DepartmentId)
            .HasConstraintName("FK_Students_Department");
   
    }
}
