using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.DTOs;
using WebApplication1.Data;
namespace WebApplication1.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly PrescriptionDbContext _context;

    public PrescriptionService(PrescriptionDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddPrescriptionAsync(AddPrescriptionDto prescriptionDto)
    {
        if (prescriptionDto.DueDate < prescriptionDto.Date)
            throw new ArgumentException("Data ważności recepty musi być późniejsza lub równa dacie wystawienia.");

        if (prescriptionDto.Medicaments == null || prescriptionDto.Medicaments.Count == 0)
            throw new ArgumentException("Recepta musi zawierać przynajmniej jeden lek.");

        if (prescriptionDto.Medicaments.Count > 10)
            throw new ArgumentException("Recepta może zawierać maksymalnie 10 leków.");

        foreach (var med in prescriptionDto.Medicaments)
        {
            var medicament = await _context.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == med.IdMedicament);
            if (medicament == null)
                throw new ArgumentException($"Lek o ID {med.IdMedicament} nie istnieje.");
        }
        
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == prescriptionDto.IdDoctor);
        if (doctor == null)
            throw new ArgumentException($"Lekarz o ID {prescriptionDto.IdDoctor} nie istnieje.");

        Patient patient;
        if (prescriptionDto.Patient.IdPatient != 0)
        {
            patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == prescriptionDto.Patient.IdPatient);
            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = prescriptionDto.Patient.FirstName,
                    LastName = prescriptionDto.Patient.LastName,
                    BirthDate = prescriptionDto.Patient.BirthDate,
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }
        }
        else
        {
            patient = await _context.Patients.FirstOrDefaultAsync(p =>
                p.FirstName == prescriptionDto.Patient.FirstName &&
                p.LastName == prescriptionDto.Patient.LastName &&
                p.BirthDate == prescriptionDto.Patient.BirthDate);

            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = prescriptionDto.Patient.FirstName,
                    LastName = prescriptionDto.Patient.LastName,
                    BirthDate = prescriptionDto.Patient.BirthDate,
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }
        }

        var prescription = new Prescription
        {
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = prescriptionDto.IdDoctor,
        };
        
        _context.Prescriptions.Add(prescription);
        
        await _context.SaveChangesAsync();

        foreach (var med in prescriptionDto.Medicaments)
        {
            var prescriptionMedicament = new Prescription_Medicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = med.IdMedicament,
                Dose = med.Dose,
                Details = med.Details //TODO : czego to kurwa nie działa xdddd
            };
            
            _context.PrescriptionMedicaments.Add(prescriptionMedicament);
        }
        
        await _context.SaveChangesAsync();

        return prescription.IdPrescription;
    }

    public async Task<PatientDetailsDto> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);
        if (patient == null)
            return null;
        
        var prescriptions = await _context.Prescriptions
            .Where(p => p.IdPatient == idPatient)
            .Include(p => p.Doctor)
            .Include(p => p.Prescription_Medicaments)
            .ThenInclude(pm => pm.Medicament)
            .OrderBy(p => p.DueDate)
            .ToListAsync();

        var patientDto = new PatientDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate
        };

        var prescriptionsDtos = prescriptions.Select(p => new PrescriptionDto
        {
            IdPrescription = p.IdPrescription,
            Date = p.Date,
            DueDate = p.DueDate,
            Doctor = new DoctorDto
            {
                IdDoctor = p.IdDoctor,
                FirstName = p.Doctor.FirstName,
                LastName = p.Doctor.LastName,
                Email = p.Doctor.Email,
            },
            Medicaments = p.Prescription_Medicaments.Select(pm => new PrescriptionMedicamentDto
            {
                IdMedicament = pm.Medicament.IdMedicament,
                Name = pm.Medicament.Name,
                Description = pm.Medicament.Description,
                Dose = pm.Dose
            }).ToList()
        }).ToList();
        
        return new PatientDetailsDto
        {
            Patient = patientDto,
            Prescriptions = prescriptionsDtos,
        };
    }
}