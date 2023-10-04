namespace Dominio.Entities;

public class Archivo : BaseEntity
{
    public string Nombre { get; set; }
    public string Extension { get; set; }
    public double Tamanio { get; set; }
    public string Ubicacion { get; set; }

}
