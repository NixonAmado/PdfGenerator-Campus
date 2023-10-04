

namespace Dominio.Interfaces;

    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }

