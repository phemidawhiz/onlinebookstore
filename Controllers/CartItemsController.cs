namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Models.CartItems;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class CartItemsController : ControllerBase
{
    private ICartItemService _cartItemService;

    public CartItemsController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cartItems = await _cartItemService.GetAll();
        return Ok(cartItems);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cartItem = await _cartItemService.GetById(id);
        return Ok(cartItem);
    }

    [HttpGet("cart/{id}")]
    public async Task<IActionResult> GetByCartId(int id)
    {
        var cartItems = await _cartItemService.GetByCartId(id);
        return Ok(cartItems);
    }

    [HttpGet("cart/book/{id}")]
    public async Task<IActionResult> GetByCartItemBook(int id)
    {
        var cartItems = await _cartItemService.GetByCartItemBook(id);
        return Ok(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCartItem model)
    {
        await _cartItemService.Create(model);
        return Ok(new { message = "Cart Item created" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCartItem model)
    {
        await _cartItemService.Update(id, model);
        return Ok(new { message = "Cart Item updated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cartItemService.Delete(id);
        return Ok(new { message = "Cart Item deleted" });
    }
}