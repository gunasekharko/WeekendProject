using Microsoft.Data.SqlClient;
using System.Configuration;

public class Program
{
    List<Books> Books = new List<Books>();

    string conn = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
    public void AddBook(List<Books> books)
    {
        string sqlquery = "Insert into Books(Title,Author,IsAvailable) values(@Title,@Author,@IsAvailable)";
        // books.Add(new Books { BookId = 1, Title = "C# Book", Author = "Guna", isAvailable = true });
        // books.
        using (SqlConnection connection = new SqlConnection(conn))
        {
            using (SqlCommand cmd = new SqlCommand(sqlquery, connection))
            {
                connection.Open();
                cmd.Parameters.AddWithValue("@Title", "C# Book");
                cmd.Parameters.AddWithValue("@Author", "Guna");
                cmd.Parameters.AddWithValue("@IsAvailable", true);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Connected to DB successfully");
            }
        }
    }
    public static void Main()
    {
        List<Books> books = new List<Books>();
        Program program = new Program();
        program.AddBook(books);
    }

}




