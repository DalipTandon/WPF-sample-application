using MyApp;
using System.Data.Entity;
using WebApp.Models;

namespace WebApp.Database
{
    public class DataContext : DbContext
    {
        public DataContext() : base("name=DefaultConnection")
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, WebApp.Database.Migrations.Configuration>());
        }

        public DbSet<UserAuth> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Stream> Streams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Student - State Relationship
            modelBuilder.Entity<Student>()
                .HasRequired(s => s.State)
                .WithMany()
                .HasForeignKey(s => s.StateId)
                .WillCascadeOnDelete(false);

            // School - City Relationship
            modelBuilder.Entity<School>()
                .HasRequired(s => s.City)
                .WithMany()
                .HasForeignKey(s => s.CityId)
                .WillCascadeOnDelete(false);

            // Student - School Relationship
            modelBuilder.Entity<Student>()
                .HasRequired(s => s.School)
                .WithMany(sch => sch.Students)
                .HasForeignKey(s => s.SchoolId)
                .WillCascadeOnDelete(false);

            // Student - Stream Relationship
                        modelBuilder.Entity<Student>()
                 .HasRequired(s => s.Stream)  // Stream is mandatory
                 .WithMany(st => st.Students)
                 .HasForeignKey(s => s.StreamId)
                 .WillCascadeOnDelete(false);


            // Stream - School Relationship
            modelBuilder.Entity<Stream>()
                .HasRequired(s => s.School)
                .WithMany(sch => sch.Streams)
                .HasForeignKey(s => s.SchoolId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
