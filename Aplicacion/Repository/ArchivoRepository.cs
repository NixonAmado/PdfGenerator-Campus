using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Http;
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

        public async Task<List<Archivo>> FilesToDb(List<IFormFile> files)
        {
             List<Archivo> archivos = new();
             foreach (var file in files)
                {
                    var filePath = "C:\\Users\\APT01-65\\Documents\\skeleton-app-webapi\\Archivos\\" + file.FileName ; // es importante que sea doble diagonal, si no , no será valido
                    //tenemos que establecer la ruta exacta del archivo por eso se añade \\" + file.FileName ;
                    using var stream = System.IO.File.Create(filePath);// guardar el archivo en la carpeta
                    {
                        await file.CopyToAsync(stream);
                    }
                    //guardar la informacion del archivo en la base de datos
                    double tamanio = file.Length;
                    //lo convertimos a megabytes
                    tamanio /= 1000000;
                    tamanio = Math.Round(tamanio,2); // hay que redondearlo a dos decimales
                    Archivo archivo = new()
                    {
                        Extension = Path.GetExtension(file.FileName).Substring(1),//establecer las propiedades dentro del modelo para posteriormente porderlo insertar en la base de datos
                                                                                  //.Substring(1) quitar el punto que esta al inicio para no almacenarlo en la bd
                        Nombre = Path.GetFileNameWithoutExtension(file.FileName),
                        Ubicacion = filePath
                    };
                    archivos.Add(archivo); // se guarda en la lista archivos que es la que le vamos a retornar al usuario
                    //incertar en la base de datos y guardar los cambios
                }
                return archivos;
        }

    }

