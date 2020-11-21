namespace Data_Training.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lesson")]
    public partial class Lesson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lesson()
        {
            Enrolment = new HashSet<Enrolment>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Class name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date&Time")]
        public DateTime DateTime { get; set; }

        public string TutorId { get; set; }

        [Required]
        [Display(Name = "Classroom")]
        public int Crid { get; set; }

        public decimal AvgOfRatings { get; set; }

        public int NumOfEnrolment { get; set; }

        public virtual Classroom Classroom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrolment> Enrolment { get; set; }
    }
}
