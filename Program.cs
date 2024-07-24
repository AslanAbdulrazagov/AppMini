using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using AppMini.Exceptions;
using AppMini.Helpers;
using AppMini;

public class Program
{
    private static List<Classroom> classrooms = new List<Classroom>();

    public static void Main()
    {
        string classroomsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Jsons", "classroom.json");

        try
        {
            if (File.Exists(classroomsPath))
            {
                using (StreamReader reader = new StreamReader(classroomsPath))
                {
                    var jsonData = reader.ReadToEnd();
                    classrooms = JsonConvert.DeserializeObject<List<Classroom>>(jsonData) ?? new List<Classroom>();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading classrooms data: {ex.Message}");
        }

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Create Classroom");
            Console.WriteLine("2. Create Student");
            Console.WriteLine("3. Display All Students");
            Console.WriteLine("4. Display Students in Specific Classroom");
            Console.WriteLine("5. Delete Student");
            Console.WriteLine("6. Display All Classrooms");
            Console.WriteLine("0. Exit");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        CreateClassroom();
                        SaveClassroomsData(classroomsPath);
                        break;
                    case "2":
                        CreateStudent();
                        SaveClassroomsData(classroomsPath);
                        break;
                    case "3":
                        DisplayAllStudents();
                        break;
                    case "4":
                        DisplayClassroomStudents();
                        break;
                    case "5":
                        DeleteStudent();
                        SaveClassroomsData(classroomsPath);
                        break;
                    case "6":
                        DisplayAllClassrooms();
                        break;
                    case "0":
                        SaveClassroomsData(classroomsPath);
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    private static void CreateClassroom()
    {
        while (true)
        {
            Console.Write("Enter classroom name (2 uppercase letters followed by 3 digits): ");
            string name = Console.ReadLine();

            if (!name.ValidClassroomName())
            {
                Console.WriteLine("Invalid classroom name format. It should be 2 uppercase letters followed by 3 digits.");
                continue;
            }

            if (classrooms.Exists(c => c.Name.Equals(name)))
            {
                Console.WriteLine("Classroom with this name already exists. Please try again.");
                continue;
            }

            Console.WriteLine("Select classroom type:");
            Console.WriteLine("1. Backend");
            Console.WriteLine("2. FrontEnd");
            string typeChoice = Console.ReadLine();

            ClassroomType type;
            switch (typeChoice)
            {
                case "1":
                    type = ClassroomType.Backend;
                    break;
                case "2":
                    type = ClassroomType.Frontend;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select 1 for Backend or 2 for FrontEnd.");
                    continue;
            }

            try
            {
                classrooms.Add(new Classroom(name, type));
                Console.WriteLine("Classroom created successfully.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating classroom: {ex.Message}");
            }
        }
    }

    private static void CreateStudent()
    {
        if (classrooms.Count == 0)
        {
            Console.WriteLine("No classrooms available. Create a classroom first.");
            return;
        }

        while (true)
        {
            Console.Write("Enter student name: ");
            string name = Console.ReadLine();

            if (!name.Valid() || !name.StartsWithCapitalLetter())
            {
                Console.WriteLine("Invalid name format. The name must be at least 3 characters long, not contain spaces, and start with a capital letter.");
                continue;
            }

            Console.Write("Enter student surname: ");
            string surname = Console.ReadLine();

            if (!surname.Valid() || !surname.StartsWithCapitalLetter())
            {
                Console.WriteLine("Invalid surname format. The surname must be at least 3 characters long, not contain spaces, and start with a capital letter.");
                continue;
            }

            Console.WriteLine("Select a classroom by ID:");
            DisplayAllClassrooms();

            if (!int.TryParse(Console.ReadLine(), out int classroomId))
            {
                Console.WriteLine("Invalid classroom ID. Please try again.");
                continue;
            }

            var classroom = classrooms.Find(c => c.Id == classroomId);
            if (classroom == null)
            {
                Console.WriteLine("Classroom not found.");
                continue;
            }

            if (classroom.students.Exists(s => s.Name.Equals(name) &&
                                               s.Surname.Equals(surname)))
            {
                Console.WriteLine("Student with this name and surname already exists in the classroom. Please try again.");
                continue;
            }

            try
            {
                var student = new Student(name, surname);
                classroom.Add(student);
                Console.WriteLine("Student added successfully.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding student: {ex.Message}");
            }
        }
    }


    private static void DisplayAllStudents()
    {
        bool studentsExist = false;
        foreach (var classroom in classrooms)
        {
            if (classroom.students.Count > 0)
            {
                studentsExist = true;
                Console.WriteLine($"Classroom ID: {classroom.Id}, Name: {classroom.Name} (Type: {classroom.Type}):");
                foreach (var student in classroom.students)
                {
                    Console.WriteLine($"  Student ID: {student.Id}, Name: {student.Name}, Surname: {student.Surname}");
                }
            }
        }

        if (!studentsExist)
        {
            Console.WriteLine("No students available.");
        }
    }

    private static void DisplayClassroomStudents()
    {
        DisplayAllClassrooms();
        if (classrooms.Count == 0)
        {
            Console.WriteLine("No classrooms available to display students.");
            return;
        }

        Console.Write("Enter classroom ID: ");
        if (int.TryParse(Console.ReadLine(), out int classroomId))
        {
            var classroom = classrooms.Find(c => c.Id == classroomId);
            if (classroom != null)
            {
                if (classroom.students.Count == 0)
                {
                    Console.WriteLine($"No students found in classroom {classroom.Name}.");
                }
                else
                {
                    Console.WriteLine($"Classroom {classroom.Name} (Type: {classroom.Type}):");
                    foreach (var student in classroom.students)
                    {
                        Console.WriteLine($"  {student.Name} {student.Surname}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Classroom not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid classroom ID.");
        }
    }

    private static void DeleteStudent()
    {
        bool studentsExist = false;

       
        foreach (var classroom in classrooms)
        {
            foreach (var student in classroom.students)
            {
                Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name} {student.Surname}, Classroom: {classroom.Name}");
                studentsExist = true;
            }
        }

       
        if (!studentsExist)
        {
            Console.WriteLine("No students available to delete.");
            return;
        }

        Console.Write("Enter student ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int studentId))
        {
            bool studentDeleted = false;

            foreach (var classroom in classrooms)
            {
                var student = classroom.students.FirstOrDefault(s => s.Id == studentId);
                if (student != null)
                {
                    try
                    {
                        classroom.Remove(studentId);
                        studentDeleted = true;
                        Console.WriteLine("Student deleted successfully.");
                        

                        break;
                    }
                    catch (StudentNotFoundException)
                    {
                        Console.WriteLine("An error occurred while trying to delete the student.");
                        return;
                    }
                }
            }

            if (!studentDeleted)
            {
                Console.WriteLine("Student not found in any classroom.");
            }
        }
        else
        {
            Console.WriteLine("Invalid student ID.");
        }
    }


    private static void DisplayAllClassrooms()
    {
        if (classrooms.Count == 0)
        {
            Console.WriteLine("No classrooms available.");
            return;
        }

        foreach (var classroom in classrooms)
        {
            Console.WriteLine($"Classroom ID: {classroom.Id}, Name: {classroom.Name}, Type: {classroom.Type}");
        }
    }

    private static void SaveClassroomsData(string path)
    {
        try
        {
            var json = JsonConvert.SerializeObject(classrooms);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(json);
            }
            Console.WriteLine("Classroom.json data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving classrooms data: {ex.Message}");
        }
    }
}
