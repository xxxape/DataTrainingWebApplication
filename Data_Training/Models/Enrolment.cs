namespace Data_Training.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Enrolment")]
    public partial class Enrolment
    {
        public int Id { get; set; }

        [Required]
        public int Cid { get; set; }

        [Required]
        public string StuId { get; set; }

        [Display(Name = "Rating")]
        public int Rating { get; set; }

        public virtual Lesson Lesson { get; set; }
    }
}
