using Database.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DC;
using Database.Repository.DTO;

namespace Database.Repository.MasterRepository
{
    public class CartRepository : BaseRepository<CustomerCart>
    {
        public CartRepository() : base(new DCEntities())
        {
          
        }
        public ResultModel OperationOnCartItems(CustomerCart model)
        {
            try
            {

                ObjectParameter Message = new ObjectParameter("Message", typeof(string));

                var coupresult = new DCEntities().OperationOnCartItems(model.BookId,model.Id.ToString(), model.Userid, model.CurrentStatus, Message);

                return new ResultModel
                {
                    message = Convert.ToString(Message.Value),
                    status = 200,
                };


            }
            catch (Exception ex)
            {
                var v1 = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResultModel
                {
                    id = 0,
                    message = v1,
                    status = 400,
                    strid = "",
                    image = ""
                };
            }
        }

        public IEnumerable<USP_CustomerCartDetails_Result> GetCustomerCart(string AspNetUserId)
        {

            var results = new DCEntities().USP_CustomerCartDetails(AspNetUserId);
            var lst = new List<USP_CustomerCartDetails_Result>();
            new DC.MapObjects<USP_CustomerCartDetails_Result, USP_CustomerCartDetails_Result>().Copy(results.ToList(), lst);
            return lst;
        }


        //public long CartCount()
        //{
        //    try
        //    {
               
        //        return Count(m => m.Status == 1);
        //    }
        //    catch (Exception ex)
        //    {

        //        return 0;
        //    }

        //}
    }
}
