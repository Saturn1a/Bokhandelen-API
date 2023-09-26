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
            // Logging
            _logger.LogDebug("Adding a new book:{@Book}", book);

            // Connect to DB , open connection
            using MySqlConnection conn = new(_connectionString);
            conn.Open();

            // Query
            MySqlCommand cmd = new("INSERT INTO Book (Title, Author, PublicationYear, ISBN, InStock)" +
                "values (@Title, @Author, @PublicationYear, @ISBN, @InStock)", conn);

            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
            cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            cmd.Parameters.AddWithValue("@InStock", book.InStock);


            // execute
            var rowsAffected = cmd.ExecuteNonQuery();

            // find last ID
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            var lastIdDB = cmd.ExecuteScalar();


            // ?????? 
            book.Id = (int)Convert.ToInt64(lastIdDB);
            return book;

        }

        public Book? Delete(int id)
        {
            // Logging
            _logger.LogDebug("Deleting book with id:{@Id}", id);


            // Match book Id from DB
            var bookToDelete = GetById(id);

            if (bookToDelete == null)
                return null;

            // Connect to DB , open connection
            using MySqlConnection conn = new(_connectionString);
            conn.Open();

            // Query
            MySqlCommand cmd = new("DELETE FROM Book WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
                return null;

            
            return bookToDelete;

        }

        public  IEnumerable<Book> GetAll()
        {
            // Logging
            _logger.LogDebug("Getting all books");


            // Create A list for all books 
            var BookList = new List<Book>();

            // Connect to DB , open connection
            using MySqlConnection conn = new(_connectionString);
            conn.Open();

            //Query
            MySqlCommand cmd = new("SELECT Id, Title, Author, PublicationYear, ISBN, InStock FROM Book", conn);

            // Read from DB
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

            // Return Booklist with added books from DB
            return BookList;

        }

        public Book? GetById(int id)
        {
            // Logging
            _logger.LogDebug("Getting book with id:{@Id}", id);

            // Connect to DB , open connection
            using MySqlConnection conn = new(_connectionString);
            conn.Open();

            //Query
            MySqlCommand cmd = new("SELECT Id, Title, Author, PublicationYear, ISBN, InStock FROM Book where id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            // Read book from DB with id
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Book()
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Author = reader.GetString("Author"),
                    PublicationYear = reader.GetInt32("PublicationYear"),
                    ISBN = reader.GetString("ISBN"),
                    InStock = reader.GetInt32("InStock"),
                };
            }
            return null;
        }

        public Book? Update(int id, Book book)
        {
            // Logging
            _logger.LogDebug("Updated book with Id:{@Id}", id);


            // Connect to DB , open connection
            using MySqlConnection conn = new (_connectionString);
            conn.Open();

            // Query
            MySqlCommand cmd = new("UPDATE Book SET Title=@Title, Author=@Author, PublicationYear=@PublicationYear, ISBN=@ISBN, InStock=@InStock WHERE Id=@Id", conn);

            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
            cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            cmd.Parameters.AddWithValue("@InStock", book.InStock);
            cmd.Parameters.AddWithValue("@Id", book.Id);

            var effectedRows = cmd.ExecuteNonQuery();

            if (effectedRows == 0)
                return null;

            book.Id = id;
            return book;
        }
    }
}
