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

    [HttpGet("ObtenerArchivos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public  async Task<IActionResult> Get()
    {
        try
        {
            return  Ok( await _unitOfWork.Archivos.ToListAsync()); // obtenemos todo lo que esta insertado en la tabla archivos
        }
        catch(Exception ex)//manejo de errores
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("EnviarArchivos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]    
    //el metodo tiene que ser una action result
    public async Task<IActionResult> PostArchivos([FromForm]List<IFormFile> files) // lo recibimos de un formulario, recibimos una lista IFormFiles que llamamos files
    {
        // crea una lista de nuestro modelo ya que podemos recibir multiples archivos
        //lo que le devolvemos al usuario una vez inserte (archivos)
        try
        {
            if (files.Count > 0) // se insertan archivos si existen 
            {
                var archivos = await _unitOfWork.Archivos.FilesToDb(files); 
                //la incercion a la bd lo cual lo haremos con entity framework
                _unitOfWork.Archivos.AddRange(archivos);
                await _unitOfWork.SaveAsync();//hacemos la persistencia de datos 
                return Ok(archivos);

            }
            else{
                return BadRequest("No se han encotrado archivos para insertar");
            }
        }
        catch (Exception ex)//manejo de errores
        {
            return BadRequest(ex.Message);
        }
    }
    
}
