using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.DTOs;
namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PresciptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PresciptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription(AddPrescriptionDto prescriptionDto)
    {
        try
        {
            var prescriptionId = await _prescriptionService.AddPrescriptionAsync(prescriptionDto);
            return CreatedAtAction(nameof(GetPatients), new { idPatient = prescriptionDto.Patient.IdPatient}, prescriptionDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Wystąpił błąd podczas dodania recepty.");
        }
    }

    [HttpGet("patients/{IdPatient}")]
    public async Task<IActionResult> GetPatients(int IdPatient)
    {
        var patientDetails = await _prescriptionService.GetPatientDetailsAsync(idPatient : IdPatient);
        
        if (patientDetails == null)
            return NotFound($"Pacjent o ID {IdPatient} nie istnieje.");
        
        return Ok(patientDetails);
    }
}