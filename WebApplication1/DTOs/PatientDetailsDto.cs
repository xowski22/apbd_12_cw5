using System.Collections.Generic;
namespace WebApplication1.DTOs;

public class PatientDetailsDto
{
    public PatientDto Patient { get; set; }
    public List<PrescriptionDto> Prescriptions { get; set; }
}