namespace WMate1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Entry")]
    public partial class Entry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Entry()
        {
            Comments = new HashSet<Comment>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public int UserId { get; set; }

        [StringLength(50)]
        public string DropOffLoc { get; set; }

        [StringLength(50)]
        public string PickUpLoc { get; set; }

        public DateTime? SetDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual UserCredential UserCredential { get; set; }
    }
}
