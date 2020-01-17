namespace WMate1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserCredential
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserCredential()
        {
            Comments = new HashSet<Comment>();
            Entries = new HashSet<Entry>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(70)]
        public string UserMail { get; set; }

        [Required]
        [StringLength(25)]
        public string UserName { get; set; }

        [Required]
        public string UserPass { get; set; }

        public bool IsActive { get; set; }

        public bool? IsAdmin { get; set; }

        public bool? IsDriver { get; set; }

        public Guid? ActiveGuid { get; set; }

        public string ProfileImage { get; set; }

        public bool? IsArchieved { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entry> Entries { get; set; }
    }
}
