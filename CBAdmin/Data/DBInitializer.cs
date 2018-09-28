
using CBAdmin.Models;
using Enums;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Data
{
    public static class DBInitializer
    {
        public static void Initialize(IDocumentStore store)
        {

            var resetDatabase = CheckForResetDatabase(store);

            if (resetDatabase)
            {

                Clear(store);

                Fill(store);

            }

        }

        private static bool CheckForResetDatabase(IDocumentStore store)
        {
            var session = store.OpenSession();

            var settings = new SystemSetting("CBAdmin");

            if (session.Query<SystemSetting>().Any())
            {
                var s = session.Load<SystemSetting>(settings.Id);
                session.Dispose();

                writeSettings(store, settings);


                return s.ResetDatabase;

            }
            else
            {
                writeSettings(store, settings);

                return true;
            }



        }

        private static void writeSettings(IDocumentStore store, SystemSetting settings)
        {

            using (var session = store.OpenSession())
            {
                settings.CreateTime = DateTime.Now;
                settings.ResetDatabase = false;

                session.Store(settings);
                session.SaveChanges();

            }
        }

        private static void Fill(IDocumentStore store)
        {
            Console.WriteLine("Writing Default Data into Database");
            var session = store.OpenSession();

            var student = new Student[]
            {
                new Student{LastName="Müller", FirstMidName="Franz", Id = string.Empty, Gender = GenderType.Male, DateOfBirth = DateTime.Parse("1989-1-11")},
                new Student{LastName="Müller", FirstMidName="Heinz", Id = string.Empty, Gender = GenderType.Male, DateOfBirth = DateTime.Parse("1990-1-11")},
                new Student{LastName="Musterman", FirstMidName="Veronika", Id = string.Empty, Gender = GenderType.Female, DateOfBirth = DateTime.Parse("1991-2-12")}
            };

            foreach (Student s in student)
            {
                session.Store(s, null);
                session.SaveChanges();
            }

            var teacher = new Teacher[]
            {
                new Teacher{LastName="Bingen", FirstMidName="Hildegard", Id = string.Empty, Gender = GenderType.Female, DateOfBirth = DateTime.Parse("1958-03-31"), Salary = SalaryType.A6, Employment_Commence = DateTime.Parse("2018-01-01")},
                new Teacher{LastName="Diesel", FirstMidName="Rudolf", Id = string.Empty, Gender = GenderType.Male, DateOfBirth = DateTime.Parse("1900-01-31"), Salary = SalaryType.A8, Employment_Commence = DateTime.Parse("2017-01-01")},
                new Teacher{LastName="Fischer", FirstMidName="Helene", Id = string.Empty, Gender = GenderType.Female, DateOfBirth = DateTime.Parse("1980-01-31"), Salary = SalaryType.A8, Employment_Commence = DateTime.Parse("2016-01-01")},
                new Teacher{LastName="Humboldt", FirstMidName="Alexander", Id = string.Empty, Gender = GenderType.Male, DateOfBirth = DateTime.Parse("1905-01-31"), Salary = SalaryType.A8, Employment_Commence = DateTime.Parse("2015-01-01")},
            };

            foreach (Teacher t in teacher)
            {
                session.Store(t, null);
                session.SaveChanges();
            }

            var course = new Course[]
            {
                new Course{Id  = string.Empty, Subject ="Deutsch", Abbreviation ="De"},
                new Course{Id  = string.Empty, Subject ="Englisch", Abbreviation ="En"},
                new Course{Id  = string.Empty, Subject ="Physik", Abbreviation ="Phy"},
            };

            foreach (Course c in course)
            {
                session.Store(c, null);
                session.SaveChanges();
            }

            var coursesDatabase = session.Query<Course>().ToList();
            var teachersDataBase = session.Query<Teacher>().ToList();

            var classes = new Class[]
            {
                new Class{Id = string.Empty, Name ="8a", CourseID = coursesDatabase[0].Id, TeacherID = teachersDataBase[0].Id, Students = student},
                new Class{Id = string.Empty, Name ="8b", CourseID = coursesDatabase[1].Id, TeacherID = teachersDataBase[1].Id},
                new Class{Id = string.Empty, Name ="8c", CourseID = coursesDatabase[2].Id, TeacherID = teachersDataBase[2].Id},
            };

            foreach (Class cl in classes)
            {
                session.Store(cl, null);
                session.SaveChanges();
            }

        }

        private static void Clear(IDocumentStore store)
        {
            var session = store.OpenSession();
            session.Advanced.MaxNumberOfRequestsPerSession = 1000;


            var students = session.Query<Student>().ToList();

            foreach (Student s in students)
            {
                session.Delete(s);
                session.SaveChanges();
            }

            var teacher = session.Query<Teacher>().ToList();

            foreach (Teacher t in teacher)
            {
                session.Delete(t);
                session.SaveChanges();
            }

            var claz = session.Query<Class>().ToList();

            foreach (Class c in claz)
            {
                session.Delete(c);
                session.SaveChanges();
            }

            var course = session.Query<Course>().ToList();

            foreach (Course co in course)
            {
                session.Delete(co);
                session.SaveChanges();
            }
        }
    }
}
