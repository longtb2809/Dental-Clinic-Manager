using DataAccess.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TreatmentDetailService
    {
        private readonly TreatmentDetailRepo _repository;

        public TreatmentDetailService()
        {
            _repository = new TreatmentDetailRepo();
        }

        public TreatmentDetail GetTreatmentDetailByTreatmentId(int treatmentId)
        {
            return _repository.GetTreatmentDetailByTreatmentId(treatmentId);
        }
    }

}
