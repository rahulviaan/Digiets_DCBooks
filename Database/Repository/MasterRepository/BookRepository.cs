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

    public class BookRepository : BaseRepository<MasterBook>
    {
        public BookRepository() : base(new DCEntities())
        {

        }
        public IEnumerable<KeyValue> SelectList(int status = 1)
        {
            try
            {
                var results = (from r in GetAll(m => m.Status == status)
                               orderby r.DisplayOrder
                               select new
                               {
                                   Id = r.Id.ToString(),
                                   Title = r.Title,
                                   r.DisplayOrder,
                                   r.CreateDate
                               }).OrderBy(m => m.Title);
                var targetList = results.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                    DisplayOrder = x.DisplayOrder,
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
        public IEnumerable<KeyValue> SelectList(Expression<Func<MasterBook, bool>> predicate)
        {
            try
            {
                var results = (from r in GetAll(predicate)
                               orderby r.DisplayOrder
                               select new
                               {
                                   Id = r.Id.ToString(),
                                   Title = r.Title,
                                   r.DisplayOrder,
                                   r.CreateDate
                               }).OrderBy(m => m.Title);
                var targetList = results.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                    DisplayOrder = x.DisplayOrder,
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

        public object GetBook(string id, string v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MasterBook> Gets(Expression<Func<MasterBook, bool>> predicate)
        {
            try
            {
                var results = GetAll(predicate).OrderByDescending(m => m.DisplayOrder);
                return results;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public new MasterBook Get(Expression<Func<MasterBook, bool>> predicate)
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
        public BookModel GetBook(Expression<Func<MasterBook, bool>> predicate)
        {
            try
            {
                var book = base.Get(predicate);
                var model = new BookModel();
                new DC.MapObjects<MasterBook, BookModel>().Copy(ref book, ref model);
                return model;

            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }

        public IQueryable<MasterBook> SearchFor(Expression<Func<MasterBook, bool>> predicate)
        {
            return Search(predicate);
        }
        public MasterBook UpdateData(MasterBook entity)
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
        public MasterBook ActiInactive(string Id, string AspnetUserId)
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
                var bookcount = new MyLibraryRepository().BookCount(id);
                if (bookcount > 0)
                {
                    message.Password = "";
                    message.PasswordRequired = false;
                    message.Action = false;
                    message.Status = 301;
                    message.Message = "book not allow to be delete beacuse this is used by another process(" + bookcount + ").";
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
        public ResultModel InsertUpdateDelete(BookModel model)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter IId = new ObjectParameter("IId", typeof(string));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                ObjectParameter IImage = new ObjectParameter("IImage", typeof(string));
                //var spresult = new DCEntities().InsertUpdateDeleteMasterBooks(model.Id, model.MasterCategoryId, model.Title, model.Author, model.ISBN,
                //     model.Edition, model.FileCoverImage, model.Description, model.PageTitle, model.MetaDescription, model.OgTitle, model.OgDescription,
                //     model.TwitterTitle, model.TwitterDescription, model.KeyWords, model.ParentId, model.FileBannerImage, model.ServerId, model.EncriptionKey,
                //     model.isSize, model.EbookPrice, model.PrintPrice, model.Discount, model.Colour, model.EbookSize_MB_, model.Ebook, model.Pbook, model.Audio, model.EBookType.GetHashCode(),
                //     model.CreatedBy, (byte)model.Status.GetHashCode(), model.IPaddress, model.Action.GetHashCode(), result, IId, IImage, Message);


                new DCEntities().InsertUpdateDeleteMasterBooks(model.Id, model.MasterCategoryId, model.MasterClassId, model.MasterSubjectId, model.MasterSeriesId, model.MasterBoardId, model.Title, model.Author, model.ISBN,
                   model.Edition, model.FileCoverImage, model.Description, model.PageTitle, model.MetaDescription, model.OgTitle, model.OgDescription,
                   model.TwitterTitle, model.TwitterDescription, model.KeyWords, model.ParentId, model.FileBannerImage, model.ServerId, model.EncriptionKey,
                   model.isSize, model.EbookPrice, model.PrintPrice, model.Discount, model.Colour, model.EbookSize_MB_, model.Ebook, model.Pbook, model.Audio, model.EBookType.GetHashCode(),
                   model.CreatedBy, (byte)model.Status.GetHashCode(), model.IPaddress, model.Action.GetHashCode(), result, IId, IImage, Message);

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

        public string RemoveImageFile(string Id, string Abbr, string AspNetUserId)
        {
            try
            {
                var model = Get(m => m.Id == Id);
                var deletdfile = "";
                switch (Abbr.ToLower())
                {
                    case "filecoverimage":
                        deletdfile = model.Image;
                        model.Image = "";
                        break;
                    case "filebannerimage":
                        deletdfile = model.BannerImage;
                        model.BannerImage = "";
                        break;
                }
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

        public long BookCount(long? CatId)
        {
            try
            {
                return Count(m => m.MasterCategoryId == CatId);
            }
            catch (Exception ex)
            {

                return 0;
            }

        }
        public long BookCount(Expression<Func<MasterBook, bool>> predicate)
        {
            try
            {
                return Count(predicate);
            }
            catch (Exception ex)
            {

                return 0;
            }

        }
    }
}
