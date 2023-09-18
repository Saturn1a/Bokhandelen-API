using MySql.Data.MySqlClient;
using BokhandelensRESTApi.DATA;
using System;

namespace BokhandelensRESTApi.Repository
{
    public class BookHandler : IBookRepository
    {
        private readonly string? _connectionString;
        private readonly ILogger<BookHandler> _logger;


        public BookHandler(IConfiguration config, ILogger<BookHandler> logger)
        {
            
            _connectionString = config.GetConnectionString("DefaultConnection");
            _logger = logger;
        }
        public Book Add(Book book)
        {
            throw new NotImplementedException();
        }

        public Book Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetAll()
        {
            // Create A list for all books 
            var BookList = new List<Book>();

            // koppla upp mot databasen, öppna connection
            using MySqlConnection conn = new(_connectionString);
            conn.Open();

            //Query
            MySqlCommand cmd = new("SELECT Id, Title, Author, PublicationYear, ISBN, InStock FROM Book", conn);

            // Reader
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var book = new Book
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Author = reader.GetString("Author"),
                    PublicationYear = reader.GetInt32("PublicationYear"),
                    ISBN = reader.GetString("ISBN"),
                    InStock = reader.GetInt32("InStock"),


                };
                BookList.Add(book);
            }

            return BookList;

        }

        public Book GetBookById(int id)
        {
            throw new NotImplementedException();
        }

        public Book Update(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
