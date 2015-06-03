using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AdventureWorksModel
{
    public class SalesPersonQuotaHistoryMap : EntityTypeConfiguration<SalesPersonQuotaHistory>
    {
        public SalesPersonQuotaHistoryMap()
        {
            // Primary Key
            HasKey(t => new { t.SalesPersonID, t.QuotaDate });

            // Properties
            Property(t => t.SalesPersonID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("SalesPersonQuotaHistory", "Sales");
            Property(t => t.SalesPersonID).HasColumnName("SalesPersonID");
            Property(t => t.QuotaDate).HasColumnName("QuotaDate");
            Property(t => t.SalesQuota).HasColumnName("SalesQuota");
            Property(t => t.rowguid).HasColumnName("rowguid");
            Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            // Relationships
            HasRequired(t => t.SalesPerson)
                .WithMany(t => t.QuotaHistory)
                .HasForeignKey(d => d.SalesPersonID);

        }
    }
}
