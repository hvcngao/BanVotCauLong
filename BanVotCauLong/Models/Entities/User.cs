namespace BanVotCauLong.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int IdUser { get; set; }

        [StringLength(50)]
        public string NameUser { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string PassWord { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
