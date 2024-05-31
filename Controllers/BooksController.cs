namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Models.Books;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _bookService.GetAll();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _bookService.GetById(id);
        return Ok(book);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([BindRequired] string searchTerm)
    {
        var book = await _bookService.Search(searchTerm);
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBook model)
    {
        await _bookService.Create(model);
        return Ok(new { message = "Book created" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBook model)
    {
        await _bookService.Update(id, model);
        return Ok(new { message = "Book updated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.Delete(id);
        return Ok(new { message = "Book deleted" });
    }
}