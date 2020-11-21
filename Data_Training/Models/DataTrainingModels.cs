namespace Data_Training.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataTrainingModels : DbContext
    {
        public DataTrainingModels()
            : base("name=DataTrainingModels1")
        {
        }

        public virtual DbSet<Classroom> Classroom { get; set; }
        public virtual DbSet<Enrolment> Enrolment { get; set; }
        public virtual DbSet<Lesson> Lesson { get; set; }
        public virtual DbSet<Material> Material { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Classroom>()
                .HasMany(e => e.Lesson)
                .WithRequired(e => e.Classroom)
                .HasForeignKey(e => e.Crid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lesson>()
                .HasMany(e => e.Enrolment)
                .WithRequired(e => e.Lesson)
                .HasForeignKey(e => e.Cid)
                .WillCascadeOnDelete(false);
        }
    }
}
