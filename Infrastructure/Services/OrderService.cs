using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IBasketRepository _basketRepo;
        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> dmRepo,
        IGenericRepository<Product> productRepo, IBasketRepository basketRepo)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _dmRepo = dmRepo;
            _orderRepo = orderRepo;
        }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Core.Entities.OrderAggregate.Address shippingAddress)
    {
        // get basket from the repo
        var basket = await _basketRepo.GetBasketAsync(basketId);

        // get items from the product repo
        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _productRepo.GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        // get delivery method from the repo
        var deliveryMethod = await _dmRepo.GetByIdAsync(deliveryMethodId);

        // calc subtotal
        var subtotal = items.Sum(item => item.Price * item.Quantity);

        // create order
        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

        // TODO: save to db
        

        // return order
        return order;
    }

    public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new System.NotImplementedException();
    }
}
}