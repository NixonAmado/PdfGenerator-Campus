using Dominio.Entities;
using Microsoft.AspNetCore.Http;


namespace Dominio.Interfaces
{
    public interface IArchivo : IGenericRepository<Archivo>
    {
        
        Archivo SaveDocument (IFormFile fichero, string rutaDestino);
        public Task<byte[]> GetDocument(string nombreFichero);
    }
}