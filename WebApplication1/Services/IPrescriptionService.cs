using System.Threading.Tasks;
using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IPrescriptionService
{
    Task<int> AddPrescriptionAsync(AddPrescriptionDto prescriptionDto);
    Task<PatientDetailsDto> GetPatientDetailsAsync(int idPatient);
}