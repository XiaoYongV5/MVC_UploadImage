using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MVC图片上传.Models.EFentity
{
    public class ImageMap : EntityTypeConfiguration<ImageModel>
    {
        public ImageMap()
        { 
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IDProofFront)
                .HasMaxLength(100);

            this.Property(t => t.IDProofBack)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ImageModel");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IDProofFront).HasColumnName("IDProofFront");
            this.Property(t => t.IDProofBack).HasColumnName("IDProofBack");
        }
    }
}