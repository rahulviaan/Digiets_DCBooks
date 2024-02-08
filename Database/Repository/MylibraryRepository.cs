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
    public class MyLibraryRepository : BaseRepository<MyLibrary>
    {
        public MyLibraryRepository() : base(new DCEntities())
        {

        }

        public IEnumerable<MyLibrary> Gets(Expression<Func<MyLibrary, bool>> predicate)
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

        public new MyLibrary Get(Expression<Func<MyLibrary, bool>> predicate)
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

        public IQueryable<MyLibrary> SearchFor(Expression<Func<MyLibrary, bool>> predicate)
        {
            return Search(predicate);
        }
        public MyLibrary UpdateData(MyLibrary entity)
        {
            try
            {
                var book = Get(entity.Id);
                book.UpdatedBy = entity.UpdatedBy;
                book.UpdateDate = DateTime.Now;
                book.IPaddress = entity.IPaddress;
                book.Status = entity.Status;
                return Update(book);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }

        }
        public bool IsExist(string Id, string BookId, string AspNetUserId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Id))
                {
                    return base.IsExist(t => t.MasterBookId == BookId && t.AspNetUserId == AspNetUserId);
                }
                else
                {
                    return base.IsExist(t => t.MasterBookId == BookId && t.AspNetUserId == AspNetUserId && t.Id != Id);
                }
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return true;
            }

        }

        public MyLibrary ActiInactive(string Id, string AspnetUserId)
        {
            try
            {
                var book = Get(Id);
                if (book.Status == 1)
                {
                    book.Status = 0;
                }
                else
                {
                    book.Status = 1;
                }
                book.UpdateDate = DateTime.Now;
                book.UpdatedBy = AspnetUserId;
                return UpdateData(book);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public ResultModel InsertUpdateDelete(MyLibraryModel model)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter IId = new ObjectParameter("IId", typeof(string));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                ObjectParameter IImage = new ObjectParameter("IImage", typeof(string));

                var coupresult = new DCEntities().InsertUpdateDeleteMyLibrary(model.Id, model.MasterBookId, model.AspNetUserId, model.CreatedBy,
                   model.Validity, model.LastDate, (byte)model.Status.GetHashCode(), model.IPaddress, model.Action.GetHashCode(), result, IId, Message, IImage);
                var rid = Convert.ToInt32(result.Value);
                if (rid == 1)
                {
                    return new ResultModel
                    {
                        id = Convert.ToInt64(IId.Value),
                        strid = Convert.ToInt64(IId.Value).ToString(),
                        message = Convert.ToString(Message.Value),
                        status = 200,
                        image = Convert.ToString(IImage.Value),
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
        public IEnumerable<ReadBooks_Result> GetUserBooks(string AspNetUserId)
        {
            try
            {
                var results = new DCEntities().ReadBooks(AspNetUserId).ToList();
                return results;
            }
            catch (Exception ex)
            {
                var v1 = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return null;
            }
        }
        public ReadBook_Result GetReadBook(string AspNetUserId, string BookId)
        {
            try
            {
                var results = new DCEntities().ReadBook(AspNetUserId, BookId).ToList();
                var book = results.ToList().FirstOrDefault();
                return book;


            }
            catch (Exception ex)
            {
                var v1 = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return null;
            }
        }
        public ReadBook_Result GetSchoolReadBook(string AspNetUserId, string BookId)
        {
            try
            {
                var results = new DCEntities().SchoolReadBook(AspNetUserId, BookId).ToList();
                var book = results.ToList().FirstOrDefault();

                var model = new ReadBook_Result();
                new DC.MapObjects<SchoolReadBook_Result, ReadBook_Result>().Copy(ref book, ref model);

                return model; 

            }
            catch (Exception ex)
            {
                var v1 = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return null;
            }
        }

        public ReadBook_Result StudentReadBook(string AspNetUserId, string BookId)
        {
            try
            {
                var results = new DCEntities().StudentReadBook(AspNetUserId, BookId).ToList();
                var book = results.ToList().FirstOrDefault();

                var model = new ReadBook_Result();
                new DC.MapObjects<StudentReadBook_Result, ReadBook_Result>().Copy(ref book, ref model);

                return model;

            }
            catch (Exception ex)
            {
                var v1 = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return null;
            }
        }

        

        public IEnumerable<GetLibraryBooks_Result> GetLibraryBooks(string AspNetUserId)
        {
            var result = new DCEntities().GetLibraryBooks(AspNetUserId);

            return result;
        }
        public IEnumerable<GetLibraryBooks_Result> GetStudentLibraryBooks(string AspNetUserId)
        {
            var result = new DCEntities().GetStudentLibraryBooks(AspNetUserId);
            var lst = new List<GetLibraryBooks_Result>();
            new DC.MapObjects<GetStudentLibraryBooks_Result, GetLibraryBooks_Result>().Copy(result.ToList(), lst);

            return lst;
        }
        public IEnumerable<GetLibraryBooks_Result> GetSchoolBooks(string AspNetUserId)
        {
            var results = new DCEntities().GetSchoolBooks(AspNetUserId);
            var lst = new List<GetLibraryBooks_Result>();            
            new DC.MapObjects<GetSchoolBooks_Result, GetLibraryBooks_Result>().Copy(results.ToList(), lst);
            return lst;
        }
        public IEnumerable<GetLibraryBooks_Result> GetAllLibraryBooks(string AspNetUserId)
        {
            
            var results = new DCEntities().GetAllLibraryBooks(AspNetUserId);
            var lst = new List<GetLibraryBooks_Result>();
            new DC.MapObjects<GetAllLibraryBooks_Result, GetLibraryBooks_Result>().Copy(results.ToList(), lst);
            return lst;
        }
        
        public long BookCount(string MasterBookId)
        {
            try
            {
                return Count(m => m.MasterBookId == MasterBookId);
            }
            catch (Exception ex)
            {

                return 0;
            }

        }

        

        public USP_BookDetails_Result GetBookDetails(string BookId)
        {
            try
            {
                var results = new DCEntities().USP_BookDetails(BookId).ToList();
                //BookDetails bookDetails = new BookDetails();
                var book = results.ToList().FirstOrDefault();
                return book;

            }
            catch (Exception ex)
            {
                var v1 = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return null;
            }
        }
    }
}
