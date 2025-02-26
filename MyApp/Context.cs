using System.Data.Entity;
using System.IO;
using System.Data.Entity.Migrations;
using MyApp.Migrations;  // Ensure this is present

namespace MyApp
{
    public class Context : DbContext
    {
        public Context() : base("name=DefaultConnection") { }

        static Context()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }


        public DbSet<UserAuth> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Stream> Streams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 🔹 Student Foreign Keys
            modelBuilder.Entity<Student>()
                .HasRequired(s => s.State)
                .WithMany()
                .HasForeignKey(s => s.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasRequired(s => s.City)
                .WithMany()
                .HasForeignKey(s => s.CityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasRequired(s => s.School)
                .WithMany(sch => sch.Students)
                .HasForeignKey(s => s.SchoolId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasRequired(s => s.Stream)
                .WithMany(st => st.Students)
                .HasForeignKey(s => s.StreamId)
                .WillCascadeOnDelete(false);

            // 🔹 Stream Foreign Key
            modelBuilder.Entity<Stream>()
                .HasRequired(s => s.School)
                .WithMany(sch => sch.Streams)
                .HasForeignKey(s => s.SchoolId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Database.Connection.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
