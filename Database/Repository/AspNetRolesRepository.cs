
using Database.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using DC;

namespace Database.Repository
{
    public class AspNetRoleRepository : BaseRepository<AspNetRole>
    {
        public AspNetRoleRepository() : base(new DCEntities())
        {

        }
        public IEnumerable<AspNetRole> Gets()
        {
            return GetAll();
        }
        public IEnumerable<KeyValue> SelectList(int status = 1)
        {
            try
            {
                var results = (from r in GetAll().ToList()
                               select new
                               {
                                   r.Id,
                                   Title = r.Name,
                               }).OrderBy(m => m.Title);
                var targetList = results.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,

                }).ToList() ;
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }


        }



        //public AspNetUser UpdateAsync(RegisterModel entity)
        //{
        //    var user = GetByUserName(entity.UserName);
        //    user.AccessFailedCount = user.AccessFailedCount;
        //    user.AccessLevels = user.AccessLevels;


        //    user.AccessLevels = user.AccessLevels;
        //    user.Address = user.Address;
        //    user.City = user.City;
        //    user.State = user.State;
        //    user.PinCode = user.PinCode;
        //    user.Description = user.Description;
        //    user.Name = entity.Name;

        //    user.eUserName = entity.UserName;
        //    user.ePassword = entity.Password;
        //    user.Company = entity.Company;

        //    user.dtmAdd = DateTime.Now;
        //    user.dtmUpdate = DateTime.Now;           
        //    user.Designation = entity.Designation;
        //    user.lastLoginDate = DateTime.Now;
        //    user.Email = user.Email;
        //    user.EmailConfirmed = user.EmailConfirmed;
        //    user.PasswordHash = user.PasswordHash;
        //    user.SecurityStamp = user.SecurityStamp;
        //    user.PhoneNumber = user.PhoneNumber;
        //    user.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
        //    user.TwoFactorEnabled = user.TwoFactorEnabled;
        //    user.LockoutEndDateUtc = user.LockoutEndDateUtc;
        //    user.LockoutEnabled = user.LockoutEnabled;
        //    user.AccessFailedCount = user.AccessFailedCount;
        //    user.UserName = user.UserName; 
        //    return Update(user);
        //}

    }

}
