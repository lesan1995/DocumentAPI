using DocumentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.DAL.EntityConfigurations
{
    public class DocumentEntityTypeConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CategoryId)
                .HasColumnName("CategoryId")
                .IsRequired(false);

            builder.Property(x => x.Title)
                .HasColumnName("Title")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("Description")
                .IsRequired();

            builder.Property(x => x.PublishYear)
                .HasColumnName("PublishYear")
                .IsRequired();
        }
    }
}
