using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class Prescription_Medicament
{
    public int IdMedicament { get; set; }
    
    [ForeignKey("IdMedicament")]
    public virtual Medicament Medicament { get; set; }
    
    public int IdPrescription { get; set; }
    
    [ForeignKey("IdPrescription")]
    public virtual Prescription Prescription { get; set; }
    
    [Required]
    public int Dose { get; set; }
    
    [MaxLength(100)]
    public string Details { get; set; }
}