using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using WebApp.Database;
using WebApp.Models;

namespace WebApp.Repo
{
    internal class Student : IStudentInterface
    {
        private readonly DataContext _context;

        public Student(DataContext context)
        {
           this._context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task InsertAsync(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));

            await _context.Students.Add(student); // Use Add() instead of AddAsync()
            await _context.SaveChangesAsync();
        }


        public async Task<Student> ShowRecordAsync(int studentId)
        {
            await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public Task UpdateAsync(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
