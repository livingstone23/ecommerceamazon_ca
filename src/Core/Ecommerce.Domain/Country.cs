using Ecommerce.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain;

public class Country: BaseDomainModel
{
    [Column(TypeName = "NVARCHAR(100)")]
    public string? Name { get; set; }

    public string? Iso2 { get; set; }
    public string? Iso3 { get; set; }
    
}