
using Database.Repository.BaseRepository;
using EntityFrameworkExtras.EF6;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using DC;

namespace Database.Repository.MasterRepository
{

    public class CategoryRepository : BaseRepository<MasterCategory>
    {
        public CategoryRepository() : base(new DCEntities())
        {

        }
        public IEnumerable<MasterCategory> Gets(short ParentId = 0, int status = -1)
        {
            if (ParentId == 0)
            {
                if (status == -1)
                {

                    return GetAll(m => (m.ParentId == null || (short)m.ParentId == 0)).OrderByDescending(m => m.CreateDate);
                }
                else
                {
                    return GetAll(m => m.Status == status && (m.ParentId == null || (short)m.ParentId == 0)).OrderByDescending(m => m.CreateDate);
                }
            }
            else
            {
                if (status == -1)
                {
                    return GetAll().Where(m => m.ParentId == ParentId).OrderByDescending(m => m.CreateDate);
                }
                else
                {
                    return GetAll().Where(m => m.Status == status && m.ParentId == ParentId).OrderByDescending(m => m.CreateDate);
                }
            }

        }
        public IEnumerable<fun_CategoryParent_Result> CategoryParent(short Id)
        {
            var result = new DCEntities().fun_CategoryParent(Id).AsEnumerable().Reverse();

            return result;
        }

        public long GetRootCategory(short Id)
        {
            var result = new DCEntities().fun_CategoryParent(Id).FirstOrDefault(m => m.ParentId == 0);

            return result.Id ?? 0;
        }
        public IEnumerable<fun_CategoryChild_Result> CategoryChild(short Id)
        {
            return new DCEntities().fun_CategoryChild(Id);
        }
        public IEnumerable<fun_CategoryHierarchy_Result> CategoryHierarchy(short Id)
        {
            try
            {
                return new DCEntities().fun_CategoryHierarchy(Id);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public IEnumerable<fun_CategoryHierarchy_Result> fun_CategoryHierarchyL2(short id, short parentid)
        {
            try
            {
                var lst = new List<fun_CategoryHierarchy_Result>();
                var result = new DCEntities().fun_CategoryHierarchyL2(id, parentid).ToList();
                new MapObjects<fun_CategoryHierarchyL2_Result, fun_CategoryHierarchy_Result>().Copy(result, lst);
                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<MasterCategory> GetsActive()
        {
            return GetAll().Where(m => m.Status == 1);
        }
        public IEnumerable<MasterCategory> GetsActive(Expression<Func<MasterCategory, bool>> predicate)
        {
            return GetAll(predicate).Take(5);
        }
        public IEnumerable<fun_Category_Result> CategoryActive(short? Id)
        {
            return new DCEntities().fun_Category(Id);
        }
        public IEnumerable<CategoryResultObject> CategoryBookCount(short? Id)
        {
            var lst = new List<CategoryResultObject>();
            var results = new DCEntities().fun_CategoryBookCount(Id);
            new DC.MapObjects<fun_CategoryBookCount_Result, CategoryResultObject>().Copy(results.ToList(), lst);
            return lst.OrderBy(m => m.DisplayOrder);
        }
        public IEnumerable<fun_Category_Result> CategoriesActive(short? Id)
        {

            var result = new DCEntities().fun_Category(Id);
            if (result.Count() > 0)
            {
                return result.OrderBy(m => m.Title);
            }
            else
            {
                var lst = new List<fun_Category_Result>();
                var cat = Get(Id);
                lst.Add(new fun_Category_Result
                {
                    ChildCatCount = 0,
                    DisplayOrder = 0,
                    Id = cat.Id,
                    ParentId = cat.ParentId,
                    Title = cat.Title,
                });
                return lst;
            }
        }
        public new MasterCategory Get(string Id)
        {

            return base.Get(Id);
        }
        public new MasterCategory Get(long? Id)
        {

            return base.Get(Id);
        }
        public IQueryable<MasterCategory> SearchFor(Expression<Func<MasterCategory, bool>> predicate)
        {
            return Search(predicate);
        }
        public new MasterCategory UpdateAsync(MasterCategory entity)
        {
            var MasterCategory = Get(entity.Id);
            MasterCategory.UpdateDate = DateTime.Now;
            MasterCategory.Title = entity.Title;
            MasterCategory.Description = entity.Description;
            if (entity.ParentId == 0)
            {
                short? value = null;
                entity.ParentId = value;
            }

            MasterCategory.ParentId = entity.ParentId;
            MasterCategory.ShortCode = entity.ShortCode;
            MasterCategory.DisplayOrder = entity.DisplayOrder;

            MasterCategory.Image = entity.Image;
            MasterCategory.BannerImage = entity.BannerImage;
            MasterCategory.UpdatedBy = entity.UpdatedBy;
            MasterCategory.UpdateDate = DateTime.Now;
            MasterCategory.IPaddress = entity.IPaddress;
            MasterCategory.PageTitle = entity.PageTitle;
            MasterCategory.MetaDescription = entity.MetaDescription;
            MasterCategory.OgTitle = entity.OgTitle;
            MasterCategory.OgDescription = entity.OgDescription;
            MasterCategory.TwitterTitle = entity.TwitterTitle;
            MasterCategory.TwitterDescription = entity.TwitterDescription;
            MasterCategory.Author = entity.Author;
            MasterCategory.KeyWords = entity.KeyWords;
            return Update(MasterCategory);
        }
        public MasterCategory DeleteImage(short Id, string IPaddress)
        {
            try
            {
                var MasterCategory = Get(Id);
                MasterCategory.UpdateDate = DateTime.Now;
                MasterCategory.Image = "";
                MasterCategory.IPaddress = IPaddress;
                return Update(MasterCategory);
            }
            catch (Exception ex)
            {

                var v1 = ex.Message;
                return null;
            }

        }
        public MasterCategory DeleteBannerImage(short Id, string IPaddress)
        {
            try
            {
                var MasterCategory = Get(Id);
                MasterCategory.UpdateDate = DateTime.Now;
                MasterCategory.BannerImage = "";
                MasterCategory.IPaddress = IPaddress;
                return Update(MasterCategory);
            }
            catch (Exception ex)
            {

                var v1 = ex.Message;
                return null;
            }

        }

        public long Count(short Id)
        {
            return Count(t => t.ParentId == Id);
        }
        public bool IsExist(long Id, string Title, long? ParentId)
        {
            if (ParentId == 0)
            {
                short? value = null;
                ParentId = value;
            }

            if (string.IsNullOrWhiteSpace(Id.ToString()) || Id == 0)
            {
                return base.IsExist(t => (t.Title.ToLower() == Title.ToLower()) && (ParentId == t.ParentId));

            }
            else
            {
                var result = base.IsExist(t => (t.Title.ToLower() == Title.ToLower()) && (ParentId == t.ParentId) && t.Id != Id);
                return base.IsExist(t => (t.Title.ToLower() == Title.ToLower()) && (ParentId == t.ParentId) && t.Id != Id);


            }

        }
        public MasterCategory CreateAsync(MasterCategory entity)
        {
            if (entity.ParentId == 0)
            {
                short? value = null;
                entity.ParentId = value;
            }
            return Insert(entity);
        }
        public MasterCategory ActiInactive(short Id)
        {
            var MasterCategory = Get(Id);
            if (MasterCategory.Status == 1)
            {
                MasterCategory.Status = 0;

            }
            else
            {
                MasterCategory.Status = 1;
            }
            MasterCategory.UpdateDate = DateTime.Now;
            return UpdateAsync(MasterCategory);
        }
        public CustomMessage<string> CheckForDelete(short id, string controller = "", string action = "", string password = "")
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
                var childcount = Count(id);
                if (childcount > 0)
                {
                    message.Password = "";
                    message.PasswordRequired = false;
                    message.Action = false;
                    message.Status = 301;
                    message.Message = "Category not allow to be delete beacuse category is using by another child process(" + childcount + ") .";
                }
                else
                {
                    var productcount = new BookRepository().BookCount(id);
                    if (productcount > 0)
                    {
                        message.Password = "";
                        message.PasswordRequired = false;
                        message.Action = false;
                        message.Status = 301;
                        message.Message = "Category not allow to be delete beacuse category is using by another product process(" + productcount + ") .";
                    }
                    else
                    { 
                        message.Action = true;
                        message.Status = 200;
                        message.Message = "Category can be deleted now.";

                    }
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

        public CustomMessage<MasterCategory> RemoveAsync(short id, string controller, string action, string Password)
        {
            var message = new CustomMessage<string>
            {
                Action = false,
                Status = 201,
                Password = "",
                PasswordRequired = false,
                Message = "Checking  data to be deleted."
            };

            var response = new CustomMessage<MasterCategory>
            {
                Action = false,
                Status = 201,
                Password = "",
                PasswordRequired = false,
                Message = "Checking  data to be deleted."
            };
            try
            {
                message = CheckForDelete(id, controller, action, Password);
                if (message.Action)
                {
                    var model = Get(id);
                    response.Data = model;
                    ObjectParameter result = new ObjectParameter("result", typeof(int));
                    ObjectParameter IId = new ObjectParameter("IId", typeof(long));
                    ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                    ObjectParameter IImage = new ObjectParameter("IImage", typeof(string));
                    var coupresult = new DCEntities().DeleteCategory(id, result, IImage, Message);
                    var rid = Convert.ToInt32(result.Value);
                    if (rid == 1)
                    {
                        response.Message = Convert.ToString(Message.Value);
                        response.Status = 200;
                        response.Action = true;
                    }
                    else
                    {
                        response.Message = Convert.ToString(Message.Value);
                        response.Status = 202;
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
