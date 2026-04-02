using MediCore.Domain.Entities;


namespace MediCore.Application.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment> AddAsync(Appointment appointment);
    Task<List<Appointment>> GetAllAsync();
    Task<List<Appointment>> GetByPatientAsync(Guid patientId);
    Task<List<Appointment>> GetByDoctorIdAsync(string doctorId);
}

