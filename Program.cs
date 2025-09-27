namespace Homework;

class Program
{
    static void Main(string[] args)
    {
        List<Student> students = new List<Student>
        {
            new Student { StudentId = 1, Name = "Alice", DateOfBirth = new DateTime(2000, 5, 15) },
            new Student { StudentId = 2, Name = "Bob", DateOfBirth = new DateTime(1999, 8, 25) },
            new Student { StudentId = 3, Name = "Charlie", DateOfBirth = new DateTime(2001, 3, 10) }
        };
        List<Course> courses = new List<Course>
        {
            new Course { CourseId = 101, Title = "Mathematics", Credits = 4 },
            new Course { CourseId = 102, Title = "Computer Science", Credits = 3 },
            new Course { CourseId = 103, Title = "Physics", Credits = 4 }
        };
        List<Enrollment> enrollments = new List<Enrollment>
        {
            new Enrollment { EnrollmentId = 1, StudentId = 1, CourseId = 101, EnrollmentDate = new DateTime(2023, 1, 15) },
            new Enrollment { EnrollmentId = 2, StudentId = 1, CourseId = 102, EnrollmentDate = new DateTime(2023, 1, 20) },
            new Enrollment { EnrollmentId = 3, StudentId = 2, CourseId = 101, EnrollmentDate = new DateTime(2023, 1, 18) },
            new Enrollment { EnrollmentId = 4, StudentId = 3, CourseId = 103, EnrollmentDate = new DateTime(2023, 1, 22) },
            new Enrollment { EnrollmentId = 5, StudentId = 3, CourseId = 101, EnrollmentDate = new DateTime(2023, 1, 25) },
            new Enrollment { EnrollmentId = 6, StudentId = 3, CourseId = 102, EnrollmentDate = new DateTime(2023, 1, 30) }
        };
        //Task1;
        Console.WriteLine("Task1///////////");
        var mathstudents = from s in students
            join e in enrollments on s.StudentId equals e.StudentId
            join c in courses on e.CourseId equals c.CourseId
            where c.Title == "Mathematics"
            select s;
        foreach (var student in mathstudents) Console.WriteLine(student.Name);
        //Task2;
        Console.WriteLine("Task2/////////////");
        var charliescourses =  from s in students
            join e in enrollments on s.StudentId equals e.StudentId
            join c in courses on e.CourseId equals c.CourseId
            where s.Name== "Charlie"
            select c;
        foreach (var course in charliescourses) Console.WriteLine(course.Title);
        //Task3;
        Console.WriteLine("Task3////////////////");
        var studentsWithManyCourses =
            students
                .SelectMany(
                    s => enrollments
                        .Where(e => e.StudentId == s.StudentId)
                        .Select(e => new { Student = s, e.CourseId })
                )
                .GroupBy(x => x.Student)
                .Where(g => g.Count() > 1) 
                .Select(g => g.Key);
        foreach (var student in studentsWithManyCourses) Console.WriteLine(student.Name);
        //Task4;
        Console.WriteLine("Task4//////////////");
        var today = DateTime.Now;
        var groupedByAgeAndCourse =
            from s in students
            let age = today.Year - s.DateOfBirth.Year
            join e in enrollments on s.StudentId equals e.StudentId
            join c in courses on e.CourseId equals c.CourseId
            group age by new
            {
                AgeRange = age < 20 ? "<20" : age <= 22 ? "20-22" : "23+",
                c.Title
            } into g
            select new { g.Key.AgeRange, g.Key.Title, AverageAge = g.Average() };
        foreach (var g in groupedByAgeAndCourse)
            Console.WriteLine($"{g.AgeRange}, {g.Title}: {g.AverageAge:F1}");

        //Task5;
        Console.WriteLine("Task5////////////");
        var filtered =
            from s in students
            join e in enrollments on s.StudentId equals e.StudentId
            join c in courses on e.CourseId equals c.CourseId
            where e.EnrollmentDate >= new DateTime(2023, 1, 20) && c.Credits >= 3
            select new { s.Name, c.Title, e.EnrollmentDate, c.Credits };
        foreach (var item in filtered)
            Console.WriteLine($"{item.Name} -> {item.Title} ({item.Credits} credits) on {item.EnrollmentDate.ToShortDateString()}");
        //Task6;
        Console.WriteLine("Task6/////////////");
        
        var totalCredits =
            from s in students
            join e in enrollments on s.StudentId equals e.StudentId
            join c in courses on e.CourseId equals c.CourseId
            group c.Credits by s.Name into g
            select new { Student = g.Key, TotalCredits = g.Sum() };
        foreach (var item in totalCredits)
            Console.WriteLine($"{item.Student}: {item.TotalCredits}");
        //Task7;
        Console.WriteLine("Task7///////////////");
        var studentsPerCourse =
            from c in courses
            join e in enrollments on c.CourseId equals e.CourseId into ce
            select new { Course = c.Title, Count = ce.Select(x => x.StudentId).Distinct().Count() };
        foreach (var item in studentsPerCourse)
            Console.WriteLine($"{item.Course}: {item.Count}");
        //Task8;
        Console.WriteLine("Task8///////////////");
        var bobId = students.First(s => s.Name == "Bob").StudentId;
        var bobCourses = enrollments.Where(e => e.StudentId == bobId).Select(e => e.CourseId);

        var coursesNotEnrolled =
            from c in courses
            where !bobCourses.Contains(c.CourseId)
            select c;
        foreach (var c in coursesNotEnrolled)
            Console.WriteLine(c.Title);
    }
}

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
}

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
}