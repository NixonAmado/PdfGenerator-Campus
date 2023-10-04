using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

public class ArchivoController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public ArchivoController( IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    // public  async IActionResult Get()
    // {
    //     try
    //     {
    //         return  Ok( await _unitOfWork.Archivos.ToListAsync()); // obtenemos todo lo que esta insertado en la tabla archivos
    //     }
    //     catch(Exception ex)//manejo de errores
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }
    [HttpPost]
    //el metodo tiene que ser una action result
    public async Task<IActionResult> PostArchivos([FromForm]List<IFormFile> files) // lo recibimos de un formulario, recibimos una lista IFormFiles que llamamos files
    {
        List<Archivo> archivos = new(); // crea una lista de nuestro modelo ya que podemos recibir multiples archivos
        //lo que le devolvemos al usuario una vez inserte (archivos)
        try
        {
            if (files.Count > 0) // se insertan archivos si existen 
            {
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
                //la incercion a la bd lo cual lo haremos con entity framework
                _unitOfWork.Archivos.AddRange(archivos);
                await _unitOfWork.SaveAsync();//hacemos la persistencia de datos 
            }
        }
        catch (Exception ex)//manejo de errores
        {
            return BadRequest(ex.Message);
        }
        return Ok(archivos);
    }
    
}
