using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MyApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MyApp.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;  // Enable automatic migrations
            AutomaticMigrationDataLossAllowed = true; // Allow data loss in dev mode (be careful)
        }

        protected override void Seed(MyApp.Context context)
        {
            // Seed States
            var states = new List<State>
            {
                new State { Name = "California" },
                new State { Name = "Texas" }
            };
            context.States.AddOrUpdate(s => s.Name, states.ToArray());
            context.SaveChanges();
            states = context.States.ToList(); // Refresh state IDs

            // Seed Cities
            var cities = new List<City>
            {
                new City { Name = "Los Angeles", StateId = states.First(s => s.Name == "California").Id },
                new City { Name = "San Francisco", StateId = states.First(s => s.Name == "California").Id },
                new City { Name = "Dallas", StateId = states.First(s => s.Name == "Texas").Id },
                new City { Name = "Austin", StateId = states.First(s => s.Name == "Texas").Id }
            };
            context.Cities.AddOrUpdate(c => c.Name, cities.ToArray());
            context.SaveChanges();
            cities = context.Cities.ToList(); // Refresh city IDs

            // Seed Schools
            var schools = new List<School>
            {
                new School { Name = "LA High School", CityId = cities.First(c => c.Name == "Los Angeles").Id },
                new School { Name = "SF Academy", CityId = cities.First(c => c.Name == "San Francisco").Id },
                new School { Name = "Dallas Central School", CityId = cities.First(c => c.Name == "Dallas").Id },
                new School { Name = "Austin Public School", CityId = cities.First(c => c.Name == "Austin").Id }
            };
            context.Schools.AddOrUpdate(s => s.Name, schools.ToArray());
            context.SaveChanges();
            schools = context.Schools.ToList(); // Refresh school IDs

            // Seed Streams
            var streams = new List<Stream>
            {
                new Stream { Name = "Science", SchoolId = schools.First(s => s.Name == "LA High School").Id },
                new Stream { Name = "Commerce", SchoolId = schools.First(s => s.Name == "SF Academy").Id },
                new Stream { Name = "Arts", SchoolId = schools.First(s => s.Name == "Dallas Central School").Id },
                new Stream { Name = "Technology", SchoolId = schools.First(s => s.Name == "Austin Public School").Id }
            };
            context.Streams.AddOrUpdate(st => st.Name, streams.ToArray());
            context.SaveChanges();
            streams = context.Streams.ToList(); // Refresh stream IDs

            // Seed Students
            var students = new List<Student>
            {
                new Student { Name = "John Doe", Gender = "Male",
                    StateId = states.First(s => s.Name == "California").Id,
                    CityId = cities.First(c => c.Name == "Los Angeles").Id,
                    SchoolId = schools.First(s => s.Name == "LA High School").Id,
                    StreamId = streams.First(st => st.Name == "Science").Id },

                new Student { Name = "Alice Johnson", Gender = "Female",
                    StateId = states.First(s => s.Name == "California").Id,
                    CityId = cities.First(c => c.Name == "San Francisco").Id,
                    SchoolId = schools.First(s => s.Name == "SF Academy").Id,
                    StreamId = streams.First(st => st.Name == "Commerce").Id },

                new Student { Name = "Michael Smith", Gender = "Male",
                    StateId = states.First(s => s.Name == "Texas").Id,
                    CityId = cities.First(c => c.Name == "Dallas").Id,
                    SchoolId = schools.First(s => s.Name == "Dallas Central School").Id,
                    StreamId = streams.First(st => st.Name == "Arts").Id },

                new Student { Name = "Emma Wilson", Gender = "Female",
                    StateId = states.First(s => s.Name == "Texas").Id,
                    CityId = cities.First(c => c.Name == "Austin").Id,
                    SchoolId = schools.First(s => s.Name == "Austin Public School").Id,
                    StreamId = streams.First(st => st.Name == "Technology").Id }
            };
            context.Students.AddOrUpdate(st => st.Name, students.ToArray());
            context.SaveChanges();
        }
    }
}
