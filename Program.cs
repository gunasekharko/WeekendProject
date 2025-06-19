using Microsoft.Data.SqlClient;
using System.Configuration;

public class Program
{
    List<Books> book = new List<Books>();

    string conn = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

    
    public void AddBook(List<Books> books)
    {
        using (SqlConnection connection = new SqlConnection(conn))
        {
            connection.Open();
            foreach (var book in books)
            {
                string checkquery = "select count(*) from Books where Title=@Title and Author=@Author";
                using (SqlCommand checkcmd = new SqlCommand(checkquery, connection))
                {
                    checkcmd.Parameters.AddWithValue("@Title", book.Title);
                    checkcmd.Parameters.AddWithValue("@Author", book.Author);
                    int count = (int)checkcmd.ExecuteScalar();
                    if (count == 0)
                    {
                        string sqlquery = "Insert into Books(Title,Author,IsAvailable) values(@Title,@Author,@IsAvailable)";
                        using (SqlCommand cmd = new SqlCommand(sqlquery, connection))
                        {
                            cmd.Parameters.AddWithValue("@Title", book.Title);
                            cmd.Parameters.AddWithValue("@Author", book.Author);
                            cmd.Parameters.AddWithValue("@IsAvailable", book.isAvailable);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                                    Console.WriteLine($"⚠️ Book '{book.Title}' by '{book.Author}' already exists. Skipping.");
                    }
                }
            }
            Console.WriteLine("Connected to DB successfully");
        }
    }
    public  List<Books> getAllBooks()
    {
        using (SqlConnection con = new SqlConnection(conn))
        {
            con.Open();
            string sqlquery = "select * From Books";
            using (SqlCommand cmd = new SqlCommand(sqlquery, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        book.Add(new Books
                        {
                            BookId = reader.GetInt32(reader.GetOrdinal("BookID")),
                            Title = reader.IsDBNull(reader.GetOrdinal("Title"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("Title")),
                            Author = reader.IsDBNull(reader.GetOrdinal("Author")) ?
                                     null
                                     : reader.GetString(reader.GetOrdinal("Author")),
                            isAvailable = !reader.IsDBNull(reader.GetOrdinal("IsAvailable"))
                                          &&
                                           reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                        });
                    }
                }
            }
            foreach (var p in book)
        {
            Console.WriteLine($"BookId:{p.BookId}----Author:{p.Author}----Title:{p.Title}---isAvailable:{p.isAvailable}");
        }
        }
        return book;
    }

    public void Delete(int BookID)
    {
        string sqlquery = "DELETE  From books where BookId=@BookID";
        
        using (SqlConnection con = new SqlConnection(conn))
        {
            using (SqlCommand cmd = new SqlCommand(sqlquery, con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@BookId", BookID);
                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Affected: {rows}");
            }
        }
    }

    public static void Main()
    {
        var newBook = new List<Books>
        {
             new Books { Title = "Book 1", Author = "Author A", isAvailable = true },
             new Books { Title = "Book 2", Author = "Author B", isAvailable = true },
        };
        Program program = new Program();
        program.getAllBooks();
        program.AddBook(newBook);
        // var bookid = newBook.Select(t => t.BookId == 2).Any().ToString();
        program.Delete(12);
    }

}




