namespace WMate1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int CommentId { get; set; }

        [Required]
        public string Description { get; set; }

        public int EntryId { get; set; }

        public int UserId { get; set; }

        public DateTime UploadDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public virtual Entry Entry { get; set; }

        public virtual UserCredential UserCredential { get; set; }
    }
}
