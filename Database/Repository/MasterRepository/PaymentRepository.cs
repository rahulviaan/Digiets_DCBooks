using Database.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repository.MasterRepository
{
    public class PaymentRepository : BaseRepository<PaymentDetails>
    {
        public PaymentRepository() : base(new DCEntities())
        {

        }
    }
}
