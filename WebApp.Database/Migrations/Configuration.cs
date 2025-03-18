using MyApp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using WebApp.Models;

namespace WebApp.Database.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Database.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;  // Enable automatic migrations
            AutomaticMigrationDataLossAllowed = true; // Allow data loss in dev mode (be careful)
        }

        protected override void Seed(WebApp.Database.DataContext context)
        {
            // Seed States
            var states = new List<State>
            {
                new State { Name = "Uttar Pradesh" },
                new State { Name = "Punjab" },
                new State { Name = "Maharashtra" }, 
                new State { Name = "Karnataka" },
                new State { Name = "Delhi" }
            };

            context.States.AddOrUpdate(s => s.Name, states.ToArray());
            context.SaveChanges(); // Save and refresh IDs
            states = context.States.ToList();

            // Function to get a state's ID safely
            int GetStateId(string name)
            {
                var state = states.FirstOrDefault(s => s.Name == name);
                if (state == null) throw new Exception($"State '{name}' not found");
                return state.Id;
            }

            // Seed Cities
            var cities = new List<City>
            {
                new City { Name = "Lucknow", StateId = GetStateId("Uttar Pradesh") },
                new City { Name = "Kanpur", StateId = GetStateId("Uttar Pradesh") },
                new City { Name = "Varanasi", StateId = GetStateId("Uttar Pradesh") },
                new City { Name = "Agra", StateId = GetStateId("Uttar Pradesh") },
                new City { Name = "Meerut", StateId = GetStateId("Uttar Pradesh") },

                new City { Name = "Amritsar", StateId = GetStateId("Punjab") },
                new City { Name = "Ludhiana", StateId = GetStateId("Punjab") }, 
                new City { Name = "Jalandhar", StateId = GetStateId("Punjab") },
                new City { Name = "Patiala", StateId = GetStateId("Punjab") },
                new City { Name = "Bathinda", StateId = GetStateId("Punjab") },

                new City { Name = "Andheri", StateId = GetStateId("Maharashtra") },
                new City { Name = "Bandra", StateId = GetStateId("Maharashtra") },
                new City { Name = "Juhu", StateId = GetStateId("Maharashtra") },
                new City { Name = "Dadar", StateId = GetStateId("Maharashtra") },
                new City { Name = "Goregaon", StateId = GetStateId("Maharashtra") },

                new City { Name = "Bangalore", StateId = GetStateId("Karnataka") },
                new City { Name = "Mysore", StateId = GetStateId("Karnataka") },
                new City { Name = "Hubli", StateId = GetStateId("Karnataka") },
                new City { Name = "Belgaum", StateId = GetStateId("Karnataka") },
                new City { Name = "Mangalore", StateId = GetStateId("Karnataka") },

                new City { Name = "Connaught Place", StateId = GetStateId("Delhi") },
                new City { Name = "Dwarka", StateId = GetStateId("Delhi") },
                new City { Name = "Karol Bagh", StateId = GetStateId("Delhi") },
                new City { Name = "Saket", StateId = GetStateId("Delhi") },
                new City { Name = "Rohini", StateId = GetStateId("Delhi") }
            };

            context.Cities.AddOrUpdate(c => new { c.Name, c.StateId }, cities.ToArray());
            context.SaveChanges();
            cities = context.Cities.ToList();

            int GetCityId(string name)
            {
                var city = cities.FirstOrDefault(c => c.Name == name);

                if (city == null) throw new Exception($"City '{name}' not found");
                return city.Id;
            }

            // Seed Schools
            var schools = new List<School>
            {
            new School { Name = "Lucknow Public School", CityId = GetCityId("Lucknow") },
            new School { Name = "Lucknow Central Academy", CityId = GetCityId("Lucknow") },
            new School { Name = "Lucknow International School", CityId = GetCityId("Lucknow") },
            new School { Name = "Lucknow Model School", CityId = GetCityId("Lucknow") },
            new School { Name = "Lucknow Global Academy", CityId = GetCityId("Lucknow") },

                new School { Name = "Kanpur Public School", CityId = GetCityId("Kanpur") },
            new School { Name = "Kanpur Central Academy", CityId = GetCityId("Kanpur") },
            new School { Name = "Kanpur International School", CityId = GetCityId("Kanpur") },
            new School { Name = "Kanpur Model School", CityId = GetCityId("Kanpur") },
            new School { Name = "Kanpur Global Academy", CityId = GetCityId("Kanpur") },


                new School { Name = "Varanasi Public School", CityId = GetCityId("Varanasi") },
            new School { Name = "Varanasi Central Academy", CityId = GetCityId("Varanasi") },
            new School { Name = "Varanasi International School", CityId = GetCityId("Varanasi") },
            new School { Name = "Varanasi Model School", CityId = GetCityId("Varanasi") },
            new School { Name = "Varanasi Global Academy", CityId = GetCityId("Varanasi") },


                new School { Name = "Agra Public School", CityId = GetCityId("Agra") },
            new School { Name = "Agra Central Academy", CityId = GetCityId("Agra") },
            new School { Name = "Agra International School", CityId = GetCityId("Agra") },
            new School { Name = "Agra Model School", CityId = GetCityId("Agra") },
            new School { Name = "Agra Global Academy", CityId = GetCityId("Agra") },


                new School { Name = "Meerut Public School", CityId = GetCityId("Meerut") },
            new School { Name = "Meerut Central Academy", CityId = GetCityId("Meerut") },
            new School { Name = "Meerut International School", CityId = GetCityId("Meerut") },
            new School { Name = "Meerut Model School", CityId = GetCityId("Meerut") },
            new School { Name = "Meerut Global Academy", CityId = GetCityId("Meerut") },



            // Schools in Punjab
            new School { Name = "Ludhiana High School", CityId = GetCityId("Ludhiana") },
            new School { Name = "Ludhiana Public School", CityId = GetCityId("Ludhiana") },
            new School { Name = "RS Model sr.sec. School", CityId = GetCityId("Ludhiana") },
            new School { Name = "Govt. Public School", CityId = GetCityId("Ludhiana") },
            new School { Name = "Ludhiana Model School", CityId = GetCityId("Ludhiana") },


            new School { Name = "Amritsar High School", CityId = GetCityId("Amritsar") },
            new School { Name = "Amritsar Public School", CityId = GetCityId("Amritsar") },
            new School { Name = "RS Model sr.sec. School", CityId = GetCityId("Amritsar") },
            new School { Name = "Govt. Public School", CityId = GetCityId("Amritsar") },
            new School { Name = "Amritsar Model School", CityId = GetCityId("Amritsar") },

            new School { Name = "Jalandhar High School", CityId = GetCityId("Jalandhar") },
            new School { Name = "Jalandhar Public School", CityId = GetCityId("Jalandhar") },
            new School { Name = "RS Model sr.sec. School", CityId = GetCityId("Jalandhar") },
            new School { Name = "Govt. Public School", CityId = GetCityId("Jalandhar") },
            new School { Name = "Jalandhar Model School", CityId = GetCityId("Jalandhar") },

            new School { Name = "Patiala High School", CityId = GetCityId("Patiala") },
            new School { Name = "Patiala Public School", CityId = GetCityId("Patiala") },
            new School { Name = "RS Model sr.sec. School", CityId = GetCityId("Patiala") },
            new School { Name = "Govt. Public School", CityId = GetCityId("Patiala") },
            new School { Name = "Patiala Model School", CityId = GetCityId("Patiala") },


            new School { Name = "Bathinda High School", CityId = GetCityId("Bathinda") },
            new School { Name = "Bathinda Public School", CityId = GetCityId("Bathinda") },
            new School { Name = "RS Model sr.sec. School", CityId = GetCityId("Bathinda") },
            new School { Name = "Govt. Public School", CityId = GetCityId("Bathinda") },
            new School { Name = "Bathinda Model School", CityId = GetCityId("Bathinda") },


            // Schools in Maharashtra
            new School { Name = "Bandra Modern School", CityId = GetCityId("Bandra") },
            new School { Name = "Bandra Prestige Academy", CityId = GetCityId("Bandra") },
            new School { Name = "Bandra Smart Academy", CityId = GetCityId("Bandra") },
            new School { Name = "Bandra Bright Future School", CityId = GetCityId("Bandra") },
            new School { Name = "Bandra Wisdom Academy", CityId = GetCityId("Bandra") },


                  new School { Name = "Juhu Modern School", CityId = GetCityId("Juhu") },
            new School { Name = "Juhu Prestige Academy", CityId = GetCityId("Juhu") },
            new School { Name = "Juhu Smart Academy", CityId = GetCityId("Juhu") },
            new School { Name = "Juhu Bright Future School", CityId = GetCityId("Juhu") },
            new School { Name = "Juhu Wisdom Academy", CityId = GetCityId("Juhu") },

                  new School { Name = "Dadar Modern School", CityId = GetCityId("Dadar") },
            new School { Name = "Dadar Prestige Academy", CityId = GetCityId("Dadar") },
            new School { Name = "Dadar Smart Academy", CityId = GetCityId("Dadar") },
            new School { Name = "Dadar Bright Future School", CityId = GetCityId("Dadar") },
            new School { Name = "Dadar Wisdom Academy", CityId = GetCityId("Dadar") },


                  new School { Name = "Goregaon Modern School", CityId = GetCityId("Goregaon") },
            new School { Name = "Goregaon Prestige Academy", CityId = GetCityId("Goregaon") },
            new School { Name = "Goregaon Smart Academy", CityId = GetCityId("Goregaon") },
            new School { Name = "Goregaon Bright Future School", CityId = GetCityId("Goregaon") },
            new School { Name = "Goregaon Wisdom Academy", CityId = GetCityId("Goregaon") },


                  new School { Name = "Andheri Modern School", CityId = GetCityId("Andheri") },
            new School { Name = "Andheri Prestige Academy", CityId = GetCityId("Andheri") },
            new School { Name = "Andheri Smart Academy", CityId = GetCityId("Andheri") },
            new School { Name = "Andheri Bright Future School", CityId = GetCityId("Andheri") },
            new School { Name = "Andheri Wisdom Academy", CityId = GetCityId("Andheri") },


            // Schools in Karnataka
            new School { Name = "Bangalore Global School", CityId = GetCityId("Bangalore") },
            new School { Name = "Bangalore Royal Academy", CityId = GetCityId("Bangalore") },
            new School { Name = "Bangalore Public School", CityId = GetCityId("Bangalore") },
            new School { Name = "Bangalore Heritage School", CityId = GetCityId("Bangalore") },
            new School { Name = "Bangalore Rising Stars Academy", CityId = GetCityId("Bangalore") },

                        new School { Name = "Mysore Global School", CityId = GetCityId("Mysore") },
            new School { Name = "Mysore Royal Academy", CityId = GetCityId("Mysore") },
            new School { Name = "Mysore Public School", CityId = GetCityId("Mysore") },
            new School { Name = "Mysore Heritage School", CityId = GetCityId("Mysore") },
            new School { Name = "Mysore Rising Stars Academy", CityId = GetCityId("Mysore") },


                        new School { Name = "Hubli Global School", CityId = GetCityId("Hubli") },
            new School { Name = "Hubli Royal Academy", CityId = GetCityId("Hubli") },
            new School { Name = "Hubli Public School", CityId = GetCityId("Hubli") },
            new School { Name = "Hubli Heritage School", CityId = GetCityId("Hubli") },
            new School { Name = "Hubli Rising Stars Academy", CityId = GetCityId("Hubli") },


                        new School { Name = "Belgaum Global School", CityId = GetCityId("Belgaum") },
            new School { Name = "Belgaum Royal Academy", CityId = GetCityId("Belgaum") },
            new School { Name = "Belgaum Public School", CityId = GetCityId("Belgaum") },
            new School { Name = "Belgaum Heritage School", CityId = GetCityId("Belgaum") },
            new School { Name = "Belgaum Rising Stars Academy", CityId = GetCityId("Belgaum") },

                        new School { Name = "Mangalore Global School", CityId = GetCityId("Mangalore") },
            new School { Name = "Mangalore Royal Academy", CityId = GetCityId("Mangalore") },
            new School { Name = "Mangalore Public School", CityId = GetCityId("Mangalore") },
            new School { Name = "Mangalore Heritage School", CityId = GetCityId("Mangalore") },
            new School { Name = "Mangalore Rising Stars Academy", CityId = GetCityId("Mangalore") },


            // Schools in Delhi
            new School { Name = "Connaught Place Elite Academy", CityId = GetCityId("Connaught Place") },
            new School { Name = "Connaught Place Future Leaders School", CityId = GetCityId("Connaught Place") },
            new School { Name = "Connaught Place  Scholarly Institute", CityId = GetCityId("Connaught Place") },
            new School { Name = "Connaught Place Learning School", CityId = GetCityId("Connaught Place") },
            new School { Name = "Connaught Place Tech School", CityId = GetCityId("Connaught Place") },

            new School { Name = "Dwarka Elite Academy", CityId = GetCityId("Dwarka") },
            new School { Name = "Dwarka Future Leaders School", CityId = GetCityId("Dwarka") },
            new School { Name = "Dwarka Scholarly Institute", CityId = GetCityId("Dwarka") },
            new School { Name = "DwarkaLearning School", CityId = GetCityId("Dwarka") },
            new School { Name = "Dwarka School of Divine", CityId = GetCityId("Dwarka") },

             new School { Name = "Karol Bagh Elite Academy", CityId = GetCityId("Karol Bagh") },
            new School { Name = "Dwarka Future Leaders School", CityId = GetCityId("Karol Bagh") },
            new School { Name = "Karol Bagh Scholarly Institute", CityId = GetCityId("Karol Bagh") },
            new School { Name = "Karol Bagh Advanced Learning School", CityId = GetCityId("Karol Bagh") },
            new School { Name = "Karol Bagh Tech School", CityId = GetCityId("Karol Bagh") },

             new School { Name = "Saket Elite Academy", CityId = GetCityId("Saket") },
            new School { Name = "Saket Leaders School", CityId = GetCityId("Saket") },
            new School { Name = "Saket Scholarly Institute", CityId = GetCityId("Saket") },
            new School { Name = "Saket Advanced Learning School", CityId = GetCityId("Saket") },
            new School { Name = "Saket Tech School", CityId = GetCityId("Saket") },

             new School { Name = "Rohini Elite Academy", CityId = GetCityId("Rohini") },
            new School { Name = "Rohini Future Leaders School", CityId = GetCityId("Rohini") },
            new School { Name = "Rohini Scholarly Institute", CityId = GetCityId("Rohini") },
            new School { Name = "Rohini Advanced Learning School", CityId = GetCityId("Rohini") },
            new School { Name = "Rohini Tech School", CityId = GetCityId("Rohini") },

            };

            context.Schools.AddOrUpdate(s => new { s.Name, s.CityId }, schools.ToArray());
            context.SaveChanges();
            schools = context.Schools.ToList();



            // Seed Streams
            // Define school names and their respective streams
            var schoolStreams = new Dictionary<string, List<string>>
{
    { "Lucknow Public School", new List<string> { "Science", "Commerce", "Physics", "Technology", "Medical" } },
     { "Lucknow Central Academy", new List<string> { "Science", "Commerce", "Physics", "Technology", "Medical" } },
      { "Lucknow International School", new List<string> { "Science", "Commerce", "Physics", "Technology", "Medical" } },
       { "Lucknow Model School", new List<string> { "Science", "Commerce", "Physics", "Technology", "Medical" } },
       { "Lucknow Global Academy", new List<string> { "Science", "Commerce", "Physics", "Technology", "Medical" } },

    { "Kanpur Central Academy", new List<string> { "Science", "Commerce", "Dance", "Technology", "Singing" } },
      { "Kanpur Public School", new List<string> { "Science", "Commerce", "Dance", "Technology", "Singing" } },
        { "Kanpur International School", new List<string> { "Science", "Commerce", "Dance", "Technology", "Singing" } },
          { "Kanpur Model School", new List<string> { "Science", "Commerce", "Dance", "Technology", "Singing" } },
            { "Kanpur Global Academy", new List<string> { "Science", "Commerce", "Dance", "Technology", "Singing" } },

    { "Varanasi Public School", new List<string> { "Science", "Commerce", "Hotel Management", "Technology", "Medical" } },
     { "Varanasi Central Academy", new List<string> { "Science", "Commerce", "Hotel Management", "Technology", "Medical" } },
      { "Varanasi International School", new List<string> { "Science", "Commerce", "Hotel Management", "Technology", "Medical" } },
       { "Varanasi Model School", new List<string> { "Science", "Commerce", "Hotel Management", "Technology", "Medical" } },
        { "Varanasi Global Academy", new List<string> { "Science", "Commerce", "Hotel Management", "Technology", "Medical" } },


    { "Agra Public School", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },
     { "Agra Central Academy", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },
      { "Agra International School", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },
       { "Agra Model School", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },
        { "Agra Global Academy", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },

 { "Meerut Public School", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },
  { "Meerut Central Academy", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },
   { "Meerut International School", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },
    { "Meerut Model School", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },
     { "Meerut Global Academy", new List<string> { "Science", "Commerce", "Arts", "Technology", "Medical" } },

     //punjab
    { "Ludhiana High School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Ludhiana Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "RS Model sr.sec. School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
 //   { "Govt. Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Ludhiana Model School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

    { "Amritsar High School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Amritsar Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
 //   { "RS Model sr.sec. School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
  //  { "Govt. Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Amritsar Model School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

    { "Jalandhar High School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Jalandhar Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
 //   { "RS Model sr.sec. School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    //{ "Govt. Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Jalandhar Model School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

    { "Patiala High School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Patiala Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
   // { "RS Model sr.sec. School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
 //   { "Govt. Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Patiala Model School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

    { "Bathinda High School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Bathinda Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
  //  { "RS Model sr.sec. School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
 //   { "Govt. Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
    { "Bathinda Model School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

    //maharastra

       { "Bandra Modern School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
          { "Bandra Prestige Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
             { "Bandra Smart Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                { "Bandra Bright Future School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                   { "Bandra Wisdom Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

       { "Juhu Modern School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
          { "Juhu Prestige Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
             { "Juhu Smart Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                { "Juhu Bright Future School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                   { "Juhu Wisdom Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

       { "Dadar Modern School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
          { "Dadar Prestige Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
             { "Dadar Smart Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                { "Dadar Bright Future School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                   { "Dadar Wisdom Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

       { "Goregaon Modern School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
          { "Goregaon Prestige Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
             { "Goregaon Smart Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                { "Goregaon Bright Future School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                   { "Goregaon Wisdom Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

           { "Andheri Modern School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
              { "Andheri Prestige Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                 { "Andheri Smart Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                    { "Andheri Bright Future School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                       { "Andheri Wisdom Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },


            //karnatka

           { "Bangalore Global School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
              { "Bangalore Royal Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                 { "Bangalore Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                    { "Bangalore Heritage School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                       { "Bangalore Rising Stars Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

                  { "Mysore Global School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                       { "Mysore Royal Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                            { "Mysore Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                 { "Mysore Heritage School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                      { "Mysore Rising Stars Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

             { "Hubli Global School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                  { "Hubli Royal Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                       { "Hubli Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                            { "Hubli Heritage School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                 { "Hubli Rising Stars Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

                 { "Belgaum Global School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                      { "Belgaum Royal Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                           { "Belgaum Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                { "Belgaum Heritage School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                     { "Belgaum Rising Stars Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

                     { "Mangalore Global School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                          { "Mangalore Royal Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                               { "Mangalore Public School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                    { "Mangalore Heritage School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                         { "Mangalore Rising Stars Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

//delhi

                { "Connaught Place Elite Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                    { "Connaught Place Future Leaders School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                        { "Connaught Place  Scholarly Institute", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                            { "Connaught Place Learning School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                { "Connaught Place Tech School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

                { "Dwarka Elite Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                    { "Dwarka Future Leaders School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                        { "Dwarka Scholarly Institute", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                            { "DwarkaLearning School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                { "Dwarka School of Divine", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

                    { "Karol Bagh Elite Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                            { "Karol Bagh Scholarly Institute", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                { "Karol Bagh Advanced Learning School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                    { "Karol Bagh Tech School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

                        { "Saket Elite Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                            { "Saket Leaders School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                { "Saket Scholarly Institute", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                    { "Saket Advanced Learning School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                        { "Saket Tech School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },

                            { "Rohini Elite Academy", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                { "Rohini Future Leaders School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                    { "Rohini Scholarly Institute", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                        { "Rohini Advanced Learning School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },
                                            { "Rohini Tech School", new List<string> { "Science", "Commerce", "Arts", "Law", "Management" } },


};

   
            foreach (var entry in schoolStreams)
            {
                string schoolName = entry.Key;
                List<string> streams = entry.Value;

                int? schoolId = context.Schools
                    .Where(s => s.Name == schoolName)
                    .Select(s => s.Id)
                    .FirstOrDefault();

                if (schoolId == null)
                {
                    throw new Exception($"School '{schoolName}' not found in the database.");
                }

                foreach (var streamName in streams)
                {
                    var existingStream = context.Streams
                        .FirstOrDefault(s => s.Name == streamName && s.SchoolId == schoolId);

                    if (existingStream == null)
                    {
                        context.Streams.Add(new Stream { Name = streamName, SchoolId = schoolId.Value });
                    }
                }
            }

            context.SaveChanges();



        }
    }
}
