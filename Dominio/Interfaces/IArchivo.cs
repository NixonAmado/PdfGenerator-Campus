using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.AspNetCore.Http;

namespace Dominio.Interfaces
{
    public interface IArchivo : IGenericRepository<Archivo>
    {
        Task<IEnumerable<Archivo>> ToListAsync();
        Task<List<Archivo>> FilesToDb(List<IFormFile> files);
    }
}