using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Properties.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    public int IdPatient { get; set; }
    
    [ForeignKey("IdPatient")]
    public Patient Patient { get; set; }
    
    public int IdDoctor { get; set; }
    
    [ForeignKey("IdDoctor")]
    public Doctor Doctor { get; set; }
}