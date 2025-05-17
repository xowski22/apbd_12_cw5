using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Properties.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Type { get; set; }
    
    
}