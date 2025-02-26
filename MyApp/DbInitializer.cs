using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MyApp
{
    public class DbInitializer : DropCreateDatabaseAlways<Context> 
    {
        protected override void Seed(Context context) 
        {
            //Console.WriteLine("seeding");
            var states = new List<State>
            {
                new State { Id = 1, Name = "California" },
                new State { Id = 2, Name = "Texas" }
            };
            context.States.AddRange(states);
            context.SaveChanges();

           
            var cities = new List<City>
            {
                new City { Id = 1, Name = "Los Angeles", StateId = 1 },
                new City { Id = 2, Name = "San Francisco", StateId = 1 },
                new City { Id = 3, Name = "Dallas", StateId = 2 },
                new City { Id = 4, Name = "Austin", StateId = 2 }
            };
            context.Cities.AddRange(cities);
            context.SaveChanges();

            var schools = new List<School>
            {
                new School { Id = 1, Name = "LA High School" },
                new School { Id = 2, Name = "SF Academy" },
                new School { Id = 3, Name = "Dallas Central School" },
                new School { Id = 4, Name = "Austin Public School" }
            };
            context.Schools.AddRange(schools);
            context.SaveChanges();

            var streams = new List<Stream>
            {
                new Stream { Id = 1, Name = "Science", SchoolId = 1 },
                new Stream { Id = 2, Name = "Commerce", SchoolId = 2 },
                new Stream { Id = 3, Name = "Arts", SchoolId = 3 },
                new Stream { Id = 4, Name = "Technology", SchoolId = 4 }
            };
            context.Streams.AddRange(streams);
            context.SaveChanges();

            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Gender = "Male", StateId = 1, CityId = 1, SchoolId = 1, StreamId = 1 },
                new Student { Id = 2, Name = "Alice Johnson", Gender = "Female", StateId = 1, CityId = 2, SchoolId = 2, StreamId = 2 },
                new Student { Id = 3, Name = "Michael Smith", Gender = "Male", StateId = 2, CityId = 3, SchoolId = 3, StreamId = 3 },
                new Student { Id = 4, Name = "Emma Wilson", Gender = "Female", StateId = 2, CityId = 4, SchoolId = 4, StreamId = 4 }
            };
            context.Students.AddRange(students);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
