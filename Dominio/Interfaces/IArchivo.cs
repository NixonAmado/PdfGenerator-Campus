using Dominio.Entities;


namespace Dominio.Interfaces
{
    public interface IArchivo : IGenericRepository<Archivo>
    {
        Task<IEnumerable<Archivo>> ToListAsync();

    }
}