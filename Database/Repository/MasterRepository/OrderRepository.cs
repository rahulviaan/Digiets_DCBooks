using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DC;
using System.Data;
using System.Data.SqlClient;
using Database.Repository.BaseRepository;
using System.Collections.Generic;
using System;

namespace Database.Repository.MasterRepository
{
   public class OrderRepository : BaseRepository<Orders>
    {
        public OrderRepository() : base(new DCEntities())
        {

        }


        public IEnumerable<USP_MyOrders_Result> GetOrders(string Userid=null,string OrderId=null)
        {
            try
            {
                var results = new DCEntities().USP_MyOrders(OrderId, Userid).ToList();
                return results;
            }
            catch (Exception ex)
            {
                var v1 = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return null;
            }
        }

        public IEnumerable<USP_MyOrderDetails_Result> GetOrderDetails(string OrderId = null)
        {
            try
            {
                var results = new DCEntities().USP_MyOrderDetails(OrderId).ToList();
                return results;
            }
            catch (Exception ex)
            {
                var v1 = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return null;
            }
        }
    }
}
