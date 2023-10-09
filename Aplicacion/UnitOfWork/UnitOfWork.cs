using Aplicacion.Repository;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Persistencia;

namespace Aplicacion.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly DbAppContext _context;
    public IArchivo _archivo;

    public UnitOfWork(DbAppContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IArchivo Archivos 
    {
        get
        {
            if(_archivo == null){
                return _archivo = new ArchivoRepository(_context, _webHostEnvironment);
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