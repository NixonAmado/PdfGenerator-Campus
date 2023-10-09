
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

    [HttpGet("ObtenerDocumentos")]
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


    [HttpPost, Route("SubirDocumento")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]   
    public async Task<ActionResult> SubirDocumento([FromForm] IFormFile fichero)
    {
        try
        {
            string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Files";
            if (!Directory.Exists(rutaDestino)) Directory.CreateDirectory(rutaDestino);
            string rutaDestinoCompleta = Path.Combine(rutaDestino, fichero.FileName);

            if(fichero.Length > 0)
            {
                using var stream = new FileStream(rutaDestinoCompleta, FileMode.Create);
                fichero.CopyTo(stream);  
            }
            
            long tam = fichero.Length;
            var archivo = new Archivo
            {
                Nombre = Path.GetFileNameWithoutExtension(fichero.FileName),
                Extension = Path.GetExtension(fichero.FileName).Substring(1),
                Tamanio = (double)tam / 1024,
                Ubicacion = rutaDestinoCompleta
            };

            // Guardar el archivo en la base de datos
            _unitOfWork.Archivos.Add(archivo);
            await _unitOfWork.SaveAsync();

            return Ok($"{fichero.FileName} se ha subido correctamente");
        }
        catch (Exception) {
            return BadRequest();
        }
    }

    //Subir Documentos en Base64
    [HttpPost, Route("SubirDocumentoB64")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]   
    public  ActionResult SubirDocumentoB64([FromForm] string base64,[FromForm] string nombreFichero)
    {
        try
        {
            string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Files";
            if (!Directory.Exists(rutaDestino)) Directory.CreateDirectory(rutaDestino);
            string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);

            byte[] documento = Convert.FromBase64String(base64);
            System.IO.File.WriteAllBytes(rutaDestinoCompleta, documento);
            
            return Ok("Docuemto se ha subido correctamente");
        }
        catch (Exception) {
            return BadRequest();
        }
    }

    [HttpPost("BajarDocumento")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]   
    public  ActionResult BajarDocumento([FromForm] string nombreFichero)
    {
        try
        {
            string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Files";
            string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);
            
            byte[] bytes = System.IO.File.ReadAllBytes(rutaDestinoCompleta);
            return File(bytes, "application/octet-stream", nombreFichero);
        }
        catch (Exception) {
            return BadRequest();
        }
    }

    //Bajar Documentos en Base64
    [HttpPost("BajarDocumentoB64")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]   
    public  ActionResult BajarDocumentoB64([FromForm] string nombreFichero)
    {
        try
        {
            string rutaDestino = _webHostEnvironment.ContentRootPath + "\\Files";
            string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);

            byte[] bytes = System.IO.File.ReadAllBytes(rutaDestinoCompleta);
            var base64String = Convert.ToBase64String(bytes);
            
            return Ok(base64String);
        }
        catch (Exception) {
            return BadRequest();
        }
    }


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
