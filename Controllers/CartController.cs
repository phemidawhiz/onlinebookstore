namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Models.Carts;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class CartsController : ControllerBase
{
    private ICartService _cartService;

    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var carts = await _cartService.GetAll();
        return Ok(carts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cart = await _cartService.GetById(id);
        return Ok(cart);
    }

    [HttpGet("cart/{id}")]
    public async Task<IActionResult> GetCartItems(int id)
    {
        var carts = await _cartService.GetCartItems(id);
        return Ok(carts);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserCartHistory(int id)
    {
        var carts = await _cartService.GetUserCartHistory(id);
        return Ok(carts);
    }

    [HttpGet("history/{id}")]
    public async Task<IActionResult> GetUserPurchaseHistory(int id)
    {
        var carts = await _cartService.GetUserPurchaseHistory(id);
        return Ok(carts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCart model)
    {
        await _cartService.Create(model);
        return Ok(new { message = "Cart created" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCart model)
    {
        await _cartService.Update(id, model);
        return Ok(new { message = "Cart updated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cartService.Delete(id);
        return Ok(new { message = "Cart deleted" });
    }
}