using Database.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DC;
using System.Data;
using System.Data.SqlClient;

namespace Database.Repository.MasterRepository
{
    public class OrderDetailsRepository : BaseRepository<OrderDetails>
    {
        public OrderDetailsRepository() : base(new DCEntities())
        {

        }

        public void UpdateOrder(DataTable dt)
        {
            SqlParameter[] sqlParameters = new SqlParameter[2];

            var ordertable = new SqlParameter("@tblOrders", SqlDbType.Structured);
            ordertable.Value = dt;
            ordertable.TypeName = "dbo.OrderDetailsType";
            var mode = new SqlParameter("@Mode", SqlDbType.NVarChar);
            mode.Value = "update";
            sqlParameters[0] = ordertable;
            sqlParameters[1] = mode;


            using (var context = new DCEntities())
            {
                context.Database.ExecuteSqlCommand("exec USP_InsertUpdateOrderDetails @tblOrders,@Mode", sqlParameters);
            }
        }
    }
}
