using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace WebApplication1.DTOs;

public class AddPrescriptionDto
{
    public PatientDto Patient { get; set; }
    public int IdDoctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<PrescriptionMedicamentDto> Medicaments { get; set; }
}