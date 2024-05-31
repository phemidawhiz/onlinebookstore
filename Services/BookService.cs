namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Books;
using WebApi.Repositories;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAll();
    Task<Book> GetById(int id);
    Task<Book> Search(string searchTerm);
    Task Create(CreateBook model);
    Task Update(int id, UpdateBook model);
    Task Delete(int id);
}

public class BookService : IBookService
{
    private IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BookService(
        IBookRepository bookRepository,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        return await _bookRepository.GetAll();
    }

    public async Task<Book> GetById(int id)
    {
        var book = await _bookRepository.GetById(id);

        if (book == null)
            throw new KeyNotFoundException("Book not found");

        return book;
    }

    public async Task<Book> Search(string searchTerm)
    {
        var book = await _bookRepository.Search(searchTerm);

        if (book == null)
            throw new KeyNotFoundException("Book not found");

        return book;
    }

    public async Task Create(CreateBook model)
    {

        // map model to new book object
        var book = _mapper.Map<Book>(model);

        // save book
        await _bookRepository.Create(book);
    }

    public async Task Update(int id, UpdateBook model)
    {
        var book = await _bookRepository.GetById(id);

        if (book == null)
            throw new KeyNotFoundException("Book not found");

        // copy model props to book
        _mapper.Map(model, book);

        // save book
        await _bookRepository.Update(book);
    }

    public async Task Delete(int id)
    {
        await _bookRepository.Delete(id);
    }
}