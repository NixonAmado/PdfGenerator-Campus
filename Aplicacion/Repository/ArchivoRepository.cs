using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class ArchivoRepository : GenericRepository<Archivo>, IArchivo
{
    private readonly DbAppContext _context;
    
    public ArchivoRepository(DbAppContext context ) : base(context)
    {
        _context = context;
    }

    public Archivo SaveDocument (IFormFile fichero, string rutaDestino)
    {
        if (!Directory.Exists(rutaDestino)) Directory.CreateDirectory(rutaDestino);
        string rutaDestinoCompleta = Path.Combine(rutaDestino, fichero.FileName);
        if(fichero.Length > 0)
        {
            using var stream = new FileStream(rutaDestinoCompleta, FileMode.Create);
            fichero.CopyTo(stream);  
        }
        
        long tam = fichero.Length;
        Archivo archivo = new()
        {
            Nombre = Path.GetFileNameWithoutExtension(fichero.FileName),
            Extension = Path.GetExtension(fichero.FileName)[1..],
            Tamanio = (double)tam / 1024,
            Ubicacion = rutaDestinoCompleta
        };

        return archivo;
    }


    //Bajar Documentos
    public async Task<byte[]> GetDocument (string nombreFichero)
    {
        var Nombre = Path.GetFileNameWithoutExtension(nombreFichero);

        string rutaDestino = await _context.Archivos
                            .Where(a => a.Nombre == Nombre)
                            .Select(a => a.Ubicacion)
                            .SingleOrDefaultAsync();
        
        string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);
        
        byte[] bytes = File.ReadAllBytes(rutaDestinoCompleta);
        
       return bytes;
    }
}

