namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using System.Formats.Tar;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Carts;
using WebApi.Repositories;

public interface ICartService
{
    Task<IEnumerable<Cart>> GetAll();
    Task<Cart> GetById(int id);
    Task<IEnumerable<CartInfo>> GetCartItems(int id);
    Task Create(CreateCart model);
    Task Update(int id, UpdateCart model);
    Task Delete(int id);
}

public class CartService : ICartService
{
    private ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CartService(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Cart>> GetAll()
    {
        return await _cartRepository.GetAll();
    }

    public async Task<Cart> GetById(int id)
    {
        var cart = await _cartRepository.GetById(id);

        if (cart == null)
            throw new KeyNotFoundException("Cart not found");

        return cart;
    }

    public async Task<IEnumerable<CartInfo>> GetCartItems(int id)
    {
        var cart = await _cartRepository.GetCartItems(id);

        if (cart == null)
            throw new KeyNotFoundException("Cart Items(s) not found");

        return cart;
    }

    public async Task Create(CreateCart model)
    {

        // map model to new cart object
        var cart = _mapper.Map<Cart>(model);

        // save cart
        await _cartRepository.Create(cart);
    }

    public async Task Update(int id, UpdateCart model)
    {
        var cart = await _cartRepository.GetById(id);

        if (cart == null)
            throw new KeyNotFoundException("Cart not found");

        // copy model props to cart
        _mapper.Map(model, cart);

        // save cart
        await _cartRepository.Update(cart);
    }

    public async Task Delete(int id)
    {
        await _cartRepository.Delete(id);
    }
}