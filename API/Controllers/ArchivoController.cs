using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ArchivoController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ArchivoController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
    {
        _webHostEnvironment = webHostEnvironment;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("ObtenerArchivo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public  async Task<IActionResult> Get()
    {
        try
        {
            return  Ok( await _unitOfWork.Archivos.GetAllAsync()); 
        }
        catch(Exception ex)//manejo de errores
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost, Route("SubirArchivo")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]   
    public async Task<ActionResult> SubirDocumento([FromForm] IFormFile fichero)
    {
        try
        {
            if (fichero == null)
            {
                return BadRequest("No se proporcionó un archivo válido.");
            }
            string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Files";
            Archivo archivo = _unitOfWork.Archivos.SaveDocument(fichero, rutaDestino);
            // Guardar el archivo en la base de datos
            _unitOfWork.Archivos.Add(archivo);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(SubirDocumento), new { id = archivo.Id }, $"{fichero.FileName} se ha subido correctamente");
        }
        catch (Exception e)
        {
            return BadRequest($"Error al subir el archivo: {e.StackTrace}");
        }
    }


    //Bajar Archivo
    [HttpPost("BajarArchivo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]   
    public async Task<ActionResult> BajarDocumento([FromForm] string nombreFichero)
    {
        try
        {
            byte[] bytes = await _unitOfWork.Archivos.GetDocument(nombreFichero);
            return File(bytes, "application/octet-stream", nombreFichero);
        }
        catch (Exception) {
            return BadRequest();
        }
    }


    //Eliminar Archivo
    [HttpDelete("Eliminar/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var archivo = await _unitOfWork.Archivos.GetByIdAsync(id);
        if (archivo == null)
        {
            return NotFound();
        }

        _unitOfWork.Archivos.Remove(archivo);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }

}
