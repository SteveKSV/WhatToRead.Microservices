using Basket.API.Entities;

namespace Basket.API.Managers.Interfaces
{
    public interface IBasketManager
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);
    }
}
