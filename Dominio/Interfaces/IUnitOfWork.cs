

namespace Dominio.Interfaces;

    public interface IUnitOfWork
    {
        IArchivo Archivos {get;}
        Task<int> SaveAsync();
    }

