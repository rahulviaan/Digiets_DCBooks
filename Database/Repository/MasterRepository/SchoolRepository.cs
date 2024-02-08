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

    public class SchoolRepository : BaseRepository<School>
    {
        public SchoolRepository() : base(new DCEntities())
        {

        }
        public IEnumerable<KeyValue> SelectList(int status = 1)
        {
            try
            {
                var results = (from r in GetAll(m => m.Status == status)
                               orderby r.Title
                               select new
                               {
                                   Id = r.Id.ToString(),
                                   Title = r.Title,
                                   r.CreateDate
                               }).OrderBy(m => m.Title);
                var targetList = results.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                    dtmCreate = x.CreateDate
                }).ToList();
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }


        }
        public IEnumerable<KeyValue> SelectList(Expression<Func<School, bool>> predicate)
        {
            try
            {
                var results = (from r in GetAll(predicate)
                               orderby r.Title
                               select new
                               {
                                   Id = r.Id.ToString(),
                                   Title = r.Title,
                                   r.CreateDate
                               }).OrderBy(m => m.Title);
                var targetList = results.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,

                    dtmCreate = x.CreateDate
                }).ToList();
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }

         
        public IEnumerable<GetSchools_Result> GetSchools(string roleId, int maxRows = 50, int page = 1, int currentRow = 0)
        {
            try
            {
                var result = new DCEntities().GetSchools(roleId, maxRows, page, currentRow).AsEnumerable();
                return result;
            }
            catch (System.Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }

        public IEnumerable<School> Gets(Expression<Func<School, bool>> predicate)
        {
            try
            {
                var results = GetAll(predicate).OrderByDescending(m => m.CreateDate);
                return results;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public new School Get(Expression<Func<School, bool>> predicate)
        {
            try
            {
                return base.Get(predicate);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public BookModel GetSchool(Expression<Func<School, bool>> predicate)
        {
            try
            {
                var book = base.Get(predicate);
                var model = new BookModel();
                new DC.MapObjects<School, BookModel>().Copy(ref book, ref model);
                return model;

            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }

        public IQueryable<School> SearchFor(Expression<Func<School, bool>> predicate)
        {
            return Search(predicate);
        }
        public School UpdateData(School entity)
        {
            try
            {
                var book = Get(entity.Id);
                book.UpdatedBy = entity.UpdatedBy;
                book.UpdatedDate = DateTime.Now;
                book.IPAddress = entity.IPAddress;
                book.Status = entity.Status;
                return Update(book);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }

        }
        public bool IsExist(string Id, string Title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Id))
                {
                    return base.IsExist(t => (t.Title.ToLower() == Title.ToLower()));
                }
                else
                {
                    return base.IsExist(t => (t.Title.ToLower() == Title.ToLower()) && (t.Id != Id));
                }
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return true;
            }

        }
        public School ActiInactive(string Id, string AspnetUserId)
        {
            try
            {
                var model = Get(Id);
                if (model.Status == 1)
                {
                    model.Status = 0; 
                }
                else
                {
                    model.Status = 1;
                }
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = AspnetUserId;
                new AspNetUsersRepository().ActiInactive(model.AspNetUserId);
                return UpdateData(model);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public CustomMessage<string> CheckForDelete(string id, string controller = "", string action = "", string password = "")
        {
            var message = new CustomMessage<string>
            {
                Id = id.ToString(),
                Action = false,
                Status = 201,
                Password = "",
                PasswordRequired = false,
                Message = "Checking  data to be deleted."
            };
            try
            {
                var model = Get(m => m.Id == id);
                var bookcount = new AspNetUsersRepository().Count(m => m.Id == model.AspNetUserId);
                if (bookcount > 0)
                {
                    message.Password = "";
                    message.PasswordRequired = false;
                    message.Action = false;
                    message.Status = 301;
                    message.Message = "school not allow to be delete beacuse this is used by another process(" + bookcount + ").";
                }
                else
                {
                    message.PasswordRequired = true;
                    message.Action = true;
                    message.Status = 200;
                    message.Message = "Data can be deleted now.";
                }
            }
            catch (Exception ex)
            {
                message.Action = false;
                message.Status = 301;
                message.Message = ex.Message;

            }
            return message;
        }
        public ResultModel InsertUpdateSchool(SchoolModel model)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter IId = new ObjectParameter("IId", typeof(string));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                ObjectParameter IImage = new ObjectParameter("IImage", typeof(string));
                 

                var spresult = new DCEntities().InsertUpdateSchool(model.Id, model.AspNetUserId, model.Title,
                    model.EmailId, model.Logo, model.ContactNo, model.AlterNateContactNo, model.Principle,
                    model.PrincipleContactNo, model.AddressLine1, model.AddressLine2, model.AddressLine3,
                    model.State, model.City, model.Pincode,model.Password, model.Description, model.CreatedBy, model.Status.GetHashCode(),
                    model. MasterBoardId, model.Strength, model.ITIncharge,
                    model.IPaddress, model.Action.GetHashCode(), result, IId, IImage, Message);

                var rid = Convert.ToInt32(result.Value);
                if (rid == 1)
                {
                    return new ResultModel
                    {

                        strid = IId.Value.ToString(),
                        message = Convert.ToString(Message.Value),
                        status = 200,
                        image = Convert.ToString(IImage.Value),
                    };
                }
                else
                {
                    return new ResultModel
                    {
                        strid = IId.Value.ToString(),
                        message = Convert.ToString(Message.Value),
                        status = 201,
                        image = Convert.ToString(IImage.Value),
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

        public string RemoveImageFile(string Id, string AspNetUserId)
        {
            try
            {
                var model = Get(m => m.Id == Id);
                var deletdfile = "";
                deletdfile = model.Logo;
                model.Logo = "";
                model.UpdatedBy = AspNetUserId;
                if (Update(model) != null)
                {
                    return deletdfile;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {

                var v1 = ex.Message;
                return "";
            }
        } 
        public long BookCount(string Id)
        {
            try
            {
                return new MyLibraryRepository(). Count(m => m.AspNetUserId == Id);
            }
            catch (Exception ex)
            {

                return 0;
            }

        }
     
    }
}
