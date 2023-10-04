using Aplicacion.Repository;
using Dominio.Interfaces;
using Persistencia;

namespace Aplicacion.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DbAppContext _context;
    public ArchivoRepository _archivo;

    public UnitOfWork(DbAppContext context)
    {
        _context = context;
    }
    
    public IArchivo Archivos 
    {
        get
        {
            if(_archivo == null){
                return _archivo = new ArchivoRepository(_context);
            }
            return _archivo;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

}