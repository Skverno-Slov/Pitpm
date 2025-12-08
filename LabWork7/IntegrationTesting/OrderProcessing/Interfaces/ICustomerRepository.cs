using OrderProcessing.Models;

namespace OrderProcessing.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с клиентами.
    /// </summary>
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllAsync();
    }
}
