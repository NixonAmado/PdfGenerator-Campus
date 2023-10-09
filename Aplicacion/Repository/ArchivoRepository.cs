using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class ArchivoRepository : GenericRepository<Archivo>, IArchivo
{
    private readonly DbAppContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ArchivoRepository(DbAppContext context ) : base(context)
    {
        this._context = context;
    }
   
    public ArchivoRepository(DbAppContext context, IWebHostEnvironment webHostEnvironment ) : base(context)
    {
        this._context = context;
        this._webHostEnvironment = webHostEnvironment;
    }

    public async  Task<IEnumerable<Archivo>> ToListAsync()
    {
        return await _context.Archivos.ToListAsync();
    }

    public Archivo SaveDocument (IFormFile fichero, string rutaDestino)
    {
        System.Diagnostics.Debug.WriteLine($"Ruta Destino: {rutaDestino}");
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
            Extension = Path.GetExtension(fichero.FileName).Substring(1),
            Tamanio = (double)tam / 1024,
            Ubicacion = rutaDestinoCompleta
        };

        return archivo;
    }

    public void SaveDocumentB64 (string base64, string nombreFichero)
    {
            string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Files";
            if (!Directory.Exists(rutaDestino)) Directory.CreateDirectory(rutaDestino);
            string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);

            byte[] documento = Convert.FromBase64String(base64);
            System.IO.File.WriteAllBytes(rutaDestinoCompleta, documento);
            
    }

    public byte[] GetDocument (string nombreFichero)
    {
        string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Files";
        string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);
        
        byte[] bytes = System.IO.File.ReadAllBytes(rutaDestinoCompleta);
        
       return bytes;
    }
        public string  GetDocumentB64 (string nombreFichero)
    {
        string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Files";
        string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);
        byte[] bytes = System.IO.File.ReadAllBytes(rutaDestinoCompleta);
        var base64String = Convert.ToBase64String(bytes);
        
       return base64String;
    }
}

