using BokhandelensRESTApi.DATA;
namespace BokhandelensRESTApi.Repository;


public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book GetBookById(int id);
    Book Add(Book book);
    Book Update(Book book);
    Book Delete(int id);


}
