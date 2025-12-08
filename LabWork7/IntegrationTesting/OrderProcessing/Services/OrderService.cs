using OrderProcessing.Interfaces;
using OrderProcessing.Models;

namespace OrderProcessing.Services
{
    /// <summary>
    /// Сервис для управления заказами, использующий репозитории и шину сообщений.
    /// </summary>
    public class OrderService
    {
        private readonly IOrderRepository _orders;
        private readonly ICustomerRepository _customers;
        private readonly IMessageBus _bus;

        public OrderService(IOrderRepository orders, ICustomerRepository customers, IMessageBus bus)
        {
            _orders = orders;
            _customers = customers;
            _bus = bus;
        }

        /// <summary>
        /// Создает новый заказ для клиента.
        /// </summary>
        public async Task<Order> CreateOrderAsync(Guid customerId, decimal totalAmount)
        {
            var customer = await _customers.GetByIdAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException("Клиент не найден.");

            var order = new Order { CustomerId = customerId, TotalAmount = totalAmount };
            await _orders.AddAsync(order);
            await _bus.PublishAsync("order.created", new { order.Id, order.CustomerId });

            return order;
        }

        /// <summary>
        /// Подтверждает оплату заказа.
        /// </summary>
        public async Task ConfirmPaymentAsync(Guid orderId)
        {
            var order = await _orders.GetByIdAsync(orderId) 
                ?? throw new InvalidOperationException("Заказ не найден.");

            order.Status = OrderStatus.Paid;
            await _orders.UpdateAsync(order);
            await _bus.PublishAsync("order.paid", new { order.Id });
        }

        /// <summary>
        /// Отменяет заказ.
        /// </summary>
        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _orders.GetByIdAsync(orderId) 
                ?? throw new InvalidOperationException("Заказ не найден.");

            order.Status = OrderStatus.Cancelled;
            await _orders.UpdateAsync(order);
            await _bus.PublishAsync("order.cancelled", new { order.Id });
        }

        /// <summary>
        /// Отправляет заказ (имитация отгрузки).
        /// </summary>
        public async Task ShipOrderAsync(Guid orderId)
        {
            var order = await _orders.GetByIdAsync(orderId) 
                ?? throw new InvalidOperationException("Заказ не найден.");

            if (order.Status != OrderStatus.Paid)
                throw new InvalidOperationException("Отправить можно только оплаченные заказы.");

            order.Status = OrderStatus.Shipped;
            await _orders.UpdateAsync(order);
            await _bus.PublishAsync("order.shipped", new { order.Id });
        }
    }
}
