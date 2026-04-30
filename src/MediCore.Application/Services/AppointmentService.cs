using MediCore.Application.DTOs;
using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;

namespace MediCore.Application.Services;

public class AppointmentService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentService(IAppointmentRepository repository)
    {
        _repository = repository;

    }

    public async Task<Appointment> CreateAppointment(CreateAppointmentDto dto)
    {
        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            AppointmentDate = DateTime.SpecifyKind(dto.AppointmentDate, DateTimeKind.Utc),
            Reason = dto.Reason
        };
        return await _repository.AddAsync(appointment);
    }
    public async Task<List<Appointment>> GetAppointments()
    {
        return await _repository.GetAllAsync();
    }
    public async Task<List<Appointment>> GetPatientAppointments(Guid patientId)
    {
        return await _repository.GetByPatientAsync(patientId);
    }
    public async Task<List<Appointment>> GetDoctorAppointments(string doctorId)
    {
        return await _repository.GetByDoctorIdAsync(doctorId);
    }
    
}



