namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.CartItems;
using WebApi.Repositories;

public interface ICartItemService
{
    Task<IEnumerable<CartItem>> GetAll();
    Task<CartItem> GetById(int id);
    Task<IEnumerable<CartItem>> GetByCartId(int id);
    Task Create(CreateCartItem model);
    Task Update(int id, UpdateCartItem model);
    Task Delete(int id);
}

public class CartItemService : ICartItemService
{
    private ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;

    public CartItemService(
        ICartItemRepository cartItemRepository,
        IMapper mapper)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CartItem>> GetAll()
    {
        return await _cartItemRepository.GetAll();
    }

    public async Task<CartItem> GetById(int id)
    {
        var cartItem = await _cartItemRepository.GetById(id);

        if (cartItem == null)
            throw new KeyNotFoundException("CartItem not found");

        return cartItem;
    }

    public async Task<IEnumerable<CartItem>> GetByCartId(int id)
    {
        var cartItem = await _cartItemRepository.GetByCartId(id);

        if (cartItem == null)
            throw new KeyNotFoundException("CartItem(s) not found");

        return cartItem;
    }

    public async Task Create(CreateCartItem model)
    {

        // map model to new cartItem object
        var cartItem = _mapper.Map<CartItem>(model);

        // save cartItem
        await _cartItemRepository.Create(cartItem);
    }

    public async Task Update(int id, UpdateCartItem model)
    {
        var cartItem = await _cartItemRepository.GetById(id);

        if (cartItem == null)
            throw new KeyNotFoundException("CartItem not found");

        // copy model props to cartItem
        _mapper.Map(model, cartItem);

        // save cartItem
        await _cartItemRepository.Update(cartItem);
    }

    public async Task Delete(int id)
    {
        await _cartItemRepository.Delete(id);
    }
}