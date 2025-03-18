using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Repo
{
    internal interface IStudentInterface
    {
        Task InsertAsync(Student student);
        Task<Student> ShowRecordAsync(int studentId);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);
    }
}
