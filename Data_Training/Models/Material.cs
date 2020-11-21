namespace Data_Training.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Material")]
    public partial class Material
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Material name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Upload Date")]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime UploadDate { get; set; }

        [Display(Name = "Existing file")]
        public string Path { get; set; }

        public string TutorId { get; set; }
    }
}
