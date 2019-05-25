namespace BanVotCauLong.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Racket")]
    public partial class Racket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Racket()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        public int IdRacket { get; set; }

        [StringLength(50)]
        public string NameRacket { get; set; }

        public int? IdBrand { get; set; }

        public int? Quantity { get; set; }

        public long? Price { get; set; }

        [StringLength(50)]
        public string ImageLink { get; set; }

        public virtual Brand Brand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
