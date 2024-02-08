using System;
using System.Collections.Generic;
using System.Linq;

namespace DC.Web.App.Models
{
    public sealed class WFRoles
    {
        private WFRoles() { }
        private static int instances = 0;
        private static readonly Lazy<WFRoles> instance = new Lazy<WFRoles>(() => new WFRoles());
        public static WFRoles GetInstance
        {
            get
            {
                return instance.Value;
            }
        }
        public IEnumerable<KeyValue> GetRoles(IList<string> role)
        {
            try
            {
                var roles = new Database.Repository.AspNetRoleRepository().SelectList().Where(m => !role.Contains(m.Title));
                var targetList = roles.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                }).ToList();
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }

        public IEnumerable<KeyValue> GetRoles()
        {
            try
            {
                var roles = new Database.Repository.AspNetRoleRepository().SelectList().Where(m=>m.Title!="SuperAdmin");
                var targetList = roles.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                }).ToList();
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public IEnumerable<KeyValue> GetSchoolRoles()
        {
            try
            {
                var roles = new Database.Repository.AspNetRoleRepository().SelectList().Where(m => m.Title == "School");
                var targetList = roles.Select(x => new KeyValue()
                {
                    Id = x.Id,
                    Title = x.Title,
                }).ToList();
                return targetList;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }

    }
}
