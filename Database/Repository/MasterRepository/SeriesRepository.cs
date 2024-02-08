using Database;
using Database.Repository.BaseRepository;
using DC;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Database.Repository.MasterRepository
{
    public class SeriesRepository : BaseRepository<MasterSery>
    {
        public SeriesRepository() : base(new DCEntities())
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
                                   r.CreatedDate
                               }).OrderBy(m => m.Title);
                var targetList = results.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                    DisplayOrder = x.DisplayOrder,
                    dtmCreate = x.CreatedDate
                }).ToList();
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }


        }

        public IEnumerable<KeyValue> SelectList(Expression<Func<MasterSery, bool>> predicate)
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
                                   r.CreatedDate
                               }).OrderBy(m => m.Title);
                var targetList = results.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                    DisplayOrder = x.DisplayOrder,
                    dtmCreate = x.CreatedDate
                }).ToList();
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }


        }

        public async Task<IEnumerable<KeyValue>> GetsAsync(Expression<Func<MasterSery, bool>> predicate)
        {
            try
            {
                var results = (from r in await GetAllAsync(predicate)
                               orderby r.DisplayOrder
                               select new
                               {
                                   Id = r.Id.ToString(),
                                   Title = r.Title,
                                   r.DisplayOrder,
                                   r.CreatedDate
                               }).OrderBy(m => m.Title);
                var targetList = results.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                    DisplayOrder = x.DisplayOrder,
                    dtmCreate = x.CreatedDate
                }).ToList();
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }


        }
        public IEnumerable<MasterSery> Gets(Expression<Func<MasterSery, bool>> predicate)
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
        public IEnumerable<MasterSery> Gets()
        {
            try
            {
                var results = GetAll().OrderByDescending(m => m.DisplayOrder);
                return results;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public IEnumerable<MasterSery> GetsActive()
        {
            try
            {
                return GetAll().Where(m => m.Status == 1).OrderByDescending(m => m.DisplayOrder);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }

        }
        public new MasterSery Get(Expression<Func<MasterSery, bool>> predicate)
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
        public MasterSery Get(long Id)
        {
            try
            {
                return base.Get(Id);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public IQueryable<MasterSery> SearchFor(Expression<Func<MasterSery, bool>> predicate)
        {
            return Search(predicate);
        }
        public   MasterSery UpdateData(MasterSery entity)
        {
            try
            {
                var MasterSery = Get(entity.Id);
                MasterSery.Title = entity.Title;
                MasterSery.Description = entity.Description;
                MasterSery.DisplayOrder = entity.DisplayOrder;
                MasterSery.Image = entity.Image;
                MasterSery.DisplayOrder = entity.DisplayOrder;
                MasterSery.UpdatedBy = entity.UpdatedBy;
                MasterSery.UpdatedDate = DateTime.Now;
                MasterSery.IPAddress = entity.IPAddress;
                MasterSery.Status = entity.Status;
                return Update(MasterSery);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }

        }
        public bool IsExist(long Id, string Title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Id.ToString()) || Id == 0)
                {
                    return base.IsExist(t =>t.Title.ToLower() == Title.ToLower() );
                }
                else
                {
                    return base.IsExist(t => t.Title.ToLower() == Title.ToLower() && t.Id != Id);
                }
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return true;
            }

        }
        public MasterSery Create(MasterSery entity)
        {
            try
            {
                return Insert(entity);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }

        }
        public MasterSery ActiInactive(long Id, string AspnetUserId)
        {
            try
            {
                var MasterSery = Get(Id);
                if (MasterSery.Status == 1)
                {
                    MasterSery.Status = 0;
                }
                else
                {
                    MasterSery.Status = 1;
                }
                MasterSery.UpdatedDate = DateTime.Now;
                MasterSery.UpdatedBy = AspnetUserId;
                return UpdateData(MasterSery);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public ResultModel InsertUpdateDelete(SeriesModel model)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter IId = new ObjectParameter("IId", typeof(long));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                ObjectParameter IImage = new ObjectParameter("IImage", typeof(string));
                var coupresult = new DCEntities().InsertUpdateDeleteMasterSeries(model.Id, model.MasterSubjectId, model.MasterPublisherId, model.GlobalId, model.Title, model.DisplayOrder, model.Image, model.Description, model.IPAddress, model.AspNetUserId, model.Status.GetHashCode(), model.ActionTaken.GetHashCode(), result, IId, IImage, Message);
                var rid = Convert.ToInt32(result.Value);
                if (rid == 1)
                {
                    return new ResultModel
                    {
                        id = Convert.ToInt64(IId.Value),
                        strid= Convert.ToInt64(IId.Value).ToString(),
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
                    image=""
                };
            }
        }
        public bool Remove(long id)
        {
            try
            {
                Delete(Get(id));
                return true;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return false;
            }
        }
    }
}
