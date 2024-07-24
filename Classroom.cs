using AppMini.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AppMini
{
    public class Classroom
    {
        private static int _id;
        public int Id { get; set; }
        public string Name { get; set; }

        public  ClassroomType Type { get; set; }
        public List<Student> students { get; set; }
        public int Limit { get; set; }

       
        public Classroom(string name,ClassroomType type)
        {
            Id=++_id;
            Name = name;
            Type = type;
            Limit = type==ClassroomType.Backend?20:15;
            students = new List<Student>();
            

        }
        public void Add(Student student)
        {
            if (students.Count <= Limit)
            {

            students.Add(student);
            }

        }
        public Student Find(int id) {
           return students.FirstOrDefault(p => p.Id == id);
        }
        public void Remove( int id)
        {
            var student = Find(id);
            students.Remove(student);
        }


        
    }
}
