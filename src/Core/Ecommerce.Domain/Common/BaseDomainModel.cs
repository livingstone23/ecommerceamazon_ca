namespace Ecommerce.Domain.Common;


/// <summary>
/// Clase base para agregar propiedades de todas las clases de negocio de la aplicacion
/// </summary>
public abstract class BaseDomainModel
{
    public int Id { get; set; }
    
    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }

    public string? LastModifiedBy { get; set; }

}

