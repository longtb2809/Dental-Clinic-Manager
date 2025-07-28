using DataAccess.Models;
using ServiceModel = DataAccess.Models.Service; 

namespace Service
{
    public class ServiceService
    {
        public List<ServiceModel> GetAllServices()
        {
            using (var context = new DentalClinicDbContext())
            {
                return context.Services.ToList();
            }
        }

        public ServiceModel? GetServiceById(int id)
        {
            using (var context = new DentalClinicDbContext())
            {
                return context.Services.FirstOrDefault(s => s.ServiceId == id);
            }
        }
        public bool AddService(ServiceModel service)
        {
            using (var context = new DentalClinicDbContext())
            {
                context.Services.Add(service);
                context.SaveChanges();
                return true;
            }
        }
    }
}
