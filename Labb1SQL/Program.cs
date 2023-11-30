using System.Data.SqlClient;
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

        // View 1 Method for Case 1
        static void View1(SqlConnection connection)
        {
            // Doing a join select so i can display the ClassName from ClassTable together with each student.
            using (SqlCommand command = new SqlCommand("SELECT s.FirstName, s.LastName, c.ClassName FROM Students s JOIN Class c ON s.FK_ClassID = c.ClassID", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.Clear();
                    Console.WriteLine("List of all students and their class:");

                    // While the reader is reading it will loop thru and print out all the information
                    while (reader.Read())
                    {
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName")).TrimEnd();
                        string lastName = reader.GetString(reader.GetOrdinal("LastName")).TrimEnd();
                        string className = reader.GetString(reader.GetOrdinal("ClassName")).TrimEnd();

                        Console.WriteLine($"Student: {firstName} {lastName} - {className}");
                    }
                }
            }
            // Using my return to menu method 
            ReturntoMenu(connection);
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
                "[7] Add Student\n");
            Console.Write("Input: ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    View1(connection);
                    break;

                case "2":
                    break;

                case "3":
                    break;

                case "4":
                    break;

                case "5":
                    break;

                case "6":
                    break;

                case "7":
                    break;


            }
        }
        // Method that is usable to return to menu when u want to exit.
        static void ReturntoMenu(SqlConnection connection)
        {
            Console.WriteLine("Press Enter to go back to the main menu...");
            // Wait for the Enter key
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            // Clear the console before going back to the StartMenu
            Console.Clear();
            StartMenu(connection);
        }
    }
}