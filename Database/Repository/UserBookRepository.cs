using Database.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DC;



namespace Database.Repository.MasterRepository
{
    public class UserBookRepository : BaseRepository<UserBook>
    {
        public UserBookRepository() : base(new DCEntities())
        {

        } 
        public ResultModel InsertUpdateDelete(UserBookModel model)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter IId = new ObjectParameter("IId", typeof(string));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                var coupresult = new DCEntities().InsertUpdateDeleteUserBooks(model.AspNetUserId, model.MasterBookId, model.CreatedBy, model.IPaddress, model.Action.GetHashCode(), result, IId, Message);
                var rid = Convert.ToInt32(result.Value);
                if (rid == 1)
                {
                    return new ResultModel
                    {
                        id = Convert.ToInt64(IId.Value),
                        strid = Convert.ToInt64(IId.Value).ToString(),
                        message = Convert.ToString(Message.Value),
                        status = 200,
                        image = "",
                    };
                }
                else
                {
                    return new ResultModel
                    {
                        id = Convert.ToInt64(IId.Value),
                        strid = Convert.ToInt64(IId.Value).ToString(),
                        message = Convert.ToString(Message.Value),
                        status = 201,
                        image = "",

                    };
                }
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
        public IEnumerable<GetUserBooks_Result> GetUserBooks(string aspNetUserId, long? masterClassId, long? masterBoardId)
        {
            try
            {
                var results = new DCEntities().GetUserBooks(aspNetUserId, masterClassId, masterBoardId).ToList(); 
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
