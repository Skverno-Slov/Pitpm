using FluentAssertions;
using Moq;
using OrderProcessing.Interfaces;
using OrderProcessing.Models;
using OrderProcessing.Services;

namespace TestProgram
{
    public class RepositiryFixture : IDisposable
    {
        public Mock<ICustomerRepository> mockCustomerRepository = new ();
        public Mock<IMessageBus> mockMessageBus = new ();
        public Mock<IOrderRepository> mockOrdersRepository = new ();

        public OrderService orderService;

        public Guid CustomerId { get; set; }
        public Order Order { get; set; }

        public RepositiryFixture()
        {
            orderService = new
            (
                mockOrdersRepository.Object,
                mockCustomerRepository.Object,
                mockMessageBus.Object
            );

            CustomerId = Guid.NewGuid();

            Order = new()
            {
                Id = Guid.NewGuid(),
                CustomerId = CustomerId,
            };

            mockOrdersRepository.Setup(o => o.UpdateAsync(Order));
            mockOrdersRepository.Setup(o => o.GetByIdAsync(Order.Id)).ReturnsAsync(Order);
            mockOrdersRepository.Setup(o => o.GetByIdAsync(It.IsNotIn(Order.Id))).ReturnsAsync(() => null);

            mockCustomerRepository.Setup(c => c.GetByIdAsync(CustomerId)).ReturnsAsync(new Customer { Id = CustomerId });
            mockCustomerRepository.Setup(c => c.GetByIdAsync(It.IsNotIn(CustomerId))).ReturnsAsync(() => null);

            mockMessageBus.Setup(c => c.PublishAsync("order.created", new { Order.Id, Order.CustomerId }));
            mockMessageBus.Setup(c => c.PublishAsync("order.paid", new { Order.Id }));
            mockMessageBus.Setup(c => c.PublishAsync("order.cancelled", new { Order.Id }));
            mockMessageBus.Setup(c => c.PublishAsync("order.shipped", new { Order.Id }));
        }

        public void Dispose() { }
    }
}