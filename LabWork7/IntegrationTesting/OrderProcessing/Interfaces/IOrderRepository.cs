using OrderProcessing.Models;

namespace OrderProcessing.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с заказами.
    /// </summary>
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
