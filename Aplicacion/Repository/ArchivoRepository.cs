using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class ArchivoRepository : GenericRepository<Archivo>, IArchivo
{
    private readonly DbAppContext _context;
    public ArchivoRepository(DbAppContext context) : base(context)
    {
        this._context = context;
    }

    public async  Task<IEnumerable<Archivo>> ToListAsync()
    {
        return await _context.Archivos.ToListAsync();
    }
}

