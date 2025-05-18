namespace WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    public virtual ICollection<Prescription> Prescriptions { get; set; }
    
}