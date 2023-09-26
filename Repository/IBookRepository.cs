using BokhandelensRESTApi.DATA;
namespace BokhandelensRESTApi.Repository;


public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    Book Add(Book book);
    Book? Update(int id, Book book);
    Book? Delete(int id);


}
