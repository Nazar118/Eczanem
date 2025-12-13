// Interfaces/IPatientService.cs
using Eczanem.Api.Models;

namespace Eczanem.Api.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient?> GetPatientByIdAsync(int id);
        Task<Patient> CreatePatientAsync(Patient patient);
    }
}