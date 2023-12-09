using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Labb1SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Puts the connection string in a string that is used to open connection.
            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=Labb1;Integrated Security=True;Pooling=False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StartMenu(connection);
            }
        }

        // Case 1 - View all Students
        static void ViewAllStudents(SqlConnection connection)
        {
            // Doing a join select so i can display the ClassName from ClassTable together with each student.
            using (SqlCommand command = new SqlCommand("SELECT s.FirstName, s.LastName, c.ClassName FROM Students s JOIN Class c ON s.FK_ClassID = c.ClassID", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.Clear();
                    Console.WriteLine("List of all students and their class:");

                    while (reader.Read())
                    {
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName")).TrimEnd();
                        string lastName = reader.GetString(reader.GetOrdinal("LastName")).TrimEnd();
                        string className = reader.GetString(reader.GetOrdinal("ClassName")).TrimEnd();

                        Console.WriteLine($"Student: {firstName} {lastName} - {className}");
                    }
                }
            }
            ReturntoMenu(connection);
        }

        // Case 2 - View Students in specific class
        static void ViewStudentsIn(SqlConnection connection)
        {
            Console.Clear();
            Console.WriteLine("All Classes: ");
            using (SqlCommand command1 = new SqlCommand("SELECT ClassName FROM Class", connection))
            {
                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string className = reader.GetString(reader.GetOrdinal("ClassName")).TrimEnd();
                        Console.WriteLine($"- {className}");
                    }
                }
            }
            Console.Write("Enter name of Class to list their students: ");
            string input = Console.ReadLine();

            string query = "Select s.FirstName, s.LastName, c.ClassName " +
                "FROM Students s " +
                "JOIN Class c ON s.FK_ClassID = c.ClassID " +
                "WHERE c.ClassName = @ClassName";

            using (SqlCommand command1 = new SqlCommand(query, connection))
            {
                command1.Parameters.AddWithValue("@ClassName", input);

                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName")).TrimEnd();
                        string lastName = reader.GetString(reader.GetOrdinal("LastName")).TrimEnd();
                        string className = reader.GetString(reader.GetOrdinal("ClassName")).TrimEnd();

                        Console.WriteLine($"Student: {firstName} {lastName} - {className}");
                    }
                }
            }
            ReturntoMenu(connection);
        }

        // Case 3 - Add Staff members
        static void AddStaff(SqlConnection connection)
        {
            Console.Clear();
            Console.WriteLine("Adding Staff Members");
            Console.WriteLine("What kind of staff do you want to add?");
            Console.WriteLine("[1] Teacher");
            Console.WriteLine("[2] Principal");
            Console.WriteLine("[3] Cleaner");
            Console.WriteLine("[Q] Quit\n");
            Console.Write("Input: ");
            string input = Console.ReadLine();
            string caseInput ="";
            switch (input)
            {
                case "1":
                    caseInput = "Teacher";
                    Console.WriteLine(caseInput);
                    break;

                case "2":
                    caseInput = "Principal";
                    Console.WriteLine(caseInput);
                    break;

                case "3":
                    caseInput = "Cleaner";
                    Console.WriteLine(caseInput);
                    break;
            }

            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Enter phone number:");
            string phoneNumber = Console.ReadLine();

            Console.WriteLine("Enter hire date (YYYY-MM-DD):");
            string hireDate = Console.ReadLine();

            string query = $"INSERT INTO StaffMember (FirstName, LastName, PhoneNumber, HireDate, Category) " +
                           $"VALUES ('{firstName}', '{lastName}', '{phoneNumber}', '{hireDate}', '{caseInput}')";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Add parameters with values
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@HireDate", hireDate);
                command.Parameters.AddWithValue("@Category", caseInput);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) affected.");
            }
            ReturntoMenu(connection);
        }

        // Case 4 - View Staff members
        static void ViewStaff(SqlConnection connection)
        {
            Console.Clear();
            Console.WriteLine("Viewing staff...");
            using (SqlCommand command1 = new SqlCommand("SELECT DISTINCT Category FROM StaffMember", connection))
            {
                Console.WriteLine("All");
                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Category = reader.GetString(reader.GetOrdinal("Category")).TrimEnd();
                        Console.WriteLine($"{Category}");
                    }
                }
            }
            Console.Write("\nEnter name of Category what you wanna view ([All] to view all categories): ");
            string input = Console.ReadLine();
            string query;
            if (input.ToLower() == "all")
            {
                Console.Clear();
                Console.WriteLine($"List of All Staff\n");
                query = "Select FirstName, LastName, PhoneNumber, HireDate, Category " +
                "FROM StaffMember "+
                "ORDER BY Category DESC, HireDate";
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"List of {input}'s\n");
                query = "Select FirstName, LastName, PhoneNumber, HireDate, Category " +
                "FROM StaffMember " +
                "WHERE Category = @Category";

            }

            using (SqlCommand command1 = new SqlCommand(query, connection))
            {
                command1.Parameters.AddWithValue("@Category", input);

                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No records found.");
                        reader.Close();
                        ReturntoMenu(connection);
                        
                    }
                    

                    while (reader.Read())
                    {
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName")).TrimEnd();
                        string lastName = reader.GetString(reader.GetOrdinal("LastName")).TrimEnd();
                        string phoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")).TrimEnd();
                        DateTime hireDate = reader.GetDateTime(reader.GetOrdinal("HireDate"));
                        string category = reader.GetString(reader.GetOrdinal("Category")).TrimEnd();

                        Console.WriteLine($"{category}: {firstName} {lastName}, Hired on: {hireDate}, Phone: {phoneNumber}");
                    }
                }
            }
            ReturntoMenu(connection);

        }

        // Case 5 - View Grades set in spec month
        static void GradesThisMonth(SqlConnection connection)
        {
            Console.Clear();

            Console.Write("Enter the month to see grades set in that month (1-12): ");
            int inputMonth = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.WriteLine($"All grades set in month [{inputMonth}]: ");
            string query = "Select s.FirstName, s.LastName, c.CourseName, g.Grade, g.GradeDate " +
                "FROM Grades g " +
                "JOIN Courses c ON g.FK_CourseID = c.CourseID " +
                "JOIN Students s ON g.FK_StudentID = s.StudentID " +
                "WHERE MONTH(g.GradeDate) = @InputMonth " +
                "AND YEAR(g.GradeDate) = YEAR(GETDATE()) " +
                "ORDER BY g.GradeDate DESC";

            using (SqlCommand command1 = new SqlCommand(query, connection))
            {
                command1.Parameters.AddWithValue("@InputMonth", inputMonth);
                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName")).TrimEnd();
                        string lastName = reader.GetString(reader.GetOrdinal("LastName")).TrimEnd();
                        string courseName = reader.GetString(reader.GetOrdinal("CourseName")).TrimEnd();
                        int grade = reader.GetInt32(reader.GetOrdinal("Grade"));
                        DateTime gradeDate = reader.GetDateTime(reader.GetOrdinal("GradeDate"));

                        Console.WriteLine($"{firstName} {lastName} - Grade {grade} in {courseName} set in {gradeDate}");
                    }
                }
            }
            ReturntoMenu(connection);
        }

        // Case 6 - Average grade for each course
        static void AverageGrade(SqlConnection connection)
        {
            string query = "Select c.CourseName, AVG(g.Grade) AS AverageGrade, " +
                "MAX(g.Grade) AS MaxGrade, MIN(g.Grade) AS MinGrade " +
                "FROM Grades g " +
                "JOIN Courses c ON g.FK_CourseID = c.CourseID " +
                "GROUP BY c.CourseName " +
                "ORDER BY c.CourseName ";

            using (SqlCommand command1 = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string courseName = reader.GetString(reader.GetOrdinal("CourseName")).TrimEnd();
                        // Had to convert to a double to display it correctly.
                        double averageGrade = Convert.ToDouble(Convert.ToDecimal(reader["AverageGrade"]));
                        int maxGrade = reader.GetInt32(reader.GetOrdinal("MaxGrade"));
                        int minGrade = reader.GetInt32(reader.GetOrdinal("MinGrade"));
                        Console.WriteLine($"- {courseName} -\nAverage grade: {averageGrade}\nHighest grade: {maxGrade}\nLowest grade: {minGrade}\n");
                       
                    }
                }
            }
            ReturntoMenu(connection);
        }

        // Case 7 - Add Student to the student table
        static void AddStudent(SqlConnection connection)
        {

        }

        // Method for the StartMenu that includes switch case
        static void StartMenu(SqlConnection connection)
        {
            Console.WriteLine("Welcome\n" +
                "[1] View all Students\n" +
                "[2] View students in specific Class\n" +
                "[3] Add Staffmember\n" +
                "[4] View Staffmembers\n" +
                "[5] View Grades set for specific Month\n" +
                "[6] Average grade for each Course\n" +
                "[7] Add Student\n" +
                "[Q] To Quit Program\n");
            Console.Write("Input: ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ViewAllStudents(connection);
                    break;

                case "2":
                    ViewStudentsIn(connection);
                    break;

                case "3":
                    AddStaff(connection);
                    break;

                case "4":
                    ViewStaff(connection);
                    break;

                case "5":
                    GradesThisMonth(connection);
                    break;

                case "6":
                    AverageGrade(connection);
                    break;

                case "7":
                    AddStudent(connection);
                    break;
                case "Q":
                    Environment.Exit(0);
                    break;

            }
        }
        // Method that is usable to return to menu when u want to exit.
        static void ReturntoMenu(SqlConnection connection)
        {
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Press [Enter] to go back to the main menu");
            Console.WriteLine("---------------------------------------------------------");
            // Wait for the Enter key
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            Console.Clear();
            StartMenu(connection);
        }
    }
}