using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace MyApp
{
    public class Context : DbContext
    {
        public Context() : base("name=DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, MyApp.Migrations.Configuration>());
        }

        public DbSet<UserAuth> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Stream> Streams { get; set; }
   

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasRequired(s => s.State)
                .WithMany()
                .HasForeignKey(s => s.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<School>()
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

            modelBuilder.Entity<Stream>()
                .HasRequired(s => s.School)
                .WithMany(sch => sch.Streams)
                .HasForeignKey(s => s.SchoolId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
