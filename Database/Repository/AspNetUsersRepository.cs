

using Database.Repository.BaseRepository;
using DC;
using EntityFrameworkExtras.EF6;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repository
{
    public class AspNetUsersRepository : BaseRepository<AspNetUser>
    {
        public AspNetUsersRepository() : base(new DCEntities())
        {

        }
        public AspNetUser ActiInactive(string Id)
        {
            try
            {
                var user = Get(Id);
                if (user.Status == 1)
                {
                    user.Status = 0;
                }
                else
                {
                    user.Status = 1;
                }
                user.dtmUpdate = DateTime.Now;
                return Update(user);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }

        }

        public AspNetUser GetByUserName(string UserName)
        {
            return GetAll().FirstOrDefault(m => m.UserName == UserName);
        }
        public new AspNetUser Get(string Id)
        {
            return Get(m => m.Id == Id);
        }

        public IEnumerable<AspNetUser> Gets()
        {
            return GetAll().OrderByDescending(m => m.dtmCreate);
        }
        public AspNetUser UpdateUser(AspNetUser model)
        {
            var entityuser = Get(model.Id);
            entityuser.dtmUpdate = DateTime.Now;
            entityuser.FirstName = model.FirstName;
            entityuser.LastName = model.LastName;
            entityuser.Email = model.Email;
            entityuser.DOB = model.DOB;
            entityuser.EmailId = model.Email;
            entityuser.EmailId = model.Email;
            entityuser.Gender = model.Gender.GetHashCode();

            return Update(entityuser);
        }
        public AspNetUser UpdateUserMobileNo(AspNetUser model)
        {
            try
            {
                var entityuser = Get(model.Id);
                entityuser.dtmUpdate = DateTime.Now;
                entityuser.MobNo = model.PhoneNumber;
                entityuser.PhoneNumber = model.PhoneNumber;
                return Update(entityuser);
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public AspNetUser UpdateUserImage(AspNetUser model)
        {
            try
            {
                var entityuser = Get(model.Id);
                var oldImage = entityuser.Image;
                entityuser.dtmUpdate = DateTime.Now;
                entityuser.Image = model.Image;
                var rsult = Update(entityuser);
                rsult.Image = oldImage;
                return rsult;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public AspNetUser FindByPhoneNo(string PhoneNo)
        {
            if (string.IsNullOrWhiteSpace(PhoneNo))
                return null;
            return Get(t => t.eUserName == PhoneNo || (t.PhoneNumber.ToLower() == PhoneNo.ToLower()));
        }
        public AspNetUser FindByUserName(string PhoneNo)
        {
            if (string.IsNullOrWhiteSpace(PhoneNo))
                return null;
            return Get(t => t.UserName.ToLower() == PhoneNo.ToLower());
        }
        public async Task<bool> IsEmailExist(string Id, string Email)
        {
            var IsExist = false;
            IEnumerable<AspNetUser> result;

            if (string.IsNullOrWhiteSpace(Id))
            {
                result = await GetAllAsync(t => t.Email.ToLower() == Email.ToLower());

            }
            else
            {
                result = await GetAllAsync(t => t.Email.ToLower() == Email.ToLower() && t.Id != Id);


            }

            if (result != null && result.Count() > 0)
                IsExist = true;
            else
                IsExist = false;
            return IsExist;
        }
        /// <summary>
        /// Check Phonenumber and user name
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PhoneNo"></param>
        /// <returns></returns>
        public async Task<bool> IsPhoneNoExist(string Id, string PhoneNo)
        {
            var IsExist = false;
            IEnumerable<AspNetUser> result;

            if (string.IsNullOrWhiteSpace(Id))
            {
                result = await GetAllAsync(t => (t.PhoneNumber.ToLower() == PhoneNo.ToLower()));

            }
            else
            {
                result = await GetAllAsync(t => t.PhoneNumber.ToLower() == PhoneNo.ToLower() && t.Id != Id);
            }

            if (result != null && result.Count() > 0)
                IsExist = true;
            else
                IsExist = false;
            return IsExist;
        }
        //public async Task<bool> IsPhoneNoExist(string Id, string PhoneNo)
        //{
        //    var IsExist = false;
        //    dynamic result;

        //    if (string.IsNullOrWhiteSpace(Id))
        //    {
        //        result = Gets().FirstOrDefault(t => t.PhoneNumber.ToLower() == PhoneNo.ToLower());
        //    }
        //    else
        //    {
        //        result = Gets().FirstOrDefault(t => t.PhoneNumber.ToLower() == PhoneNo.ToLower() && t.Id != Id);
        //    }
        //    if (result != null)
        //        IsExist = true;
        //    else
        //        IsExist = false;
        //    return IsExist;
        //}
        //public async Task<bool> IsUserExist(string Id, string UserName)
        //{
        //    var result = false;
        //    dynamic CMSContainer;

        //    if (string.IsNullOrWhiteSpace(Id))
        //    {
        //        CMSContainer = Gets().FirstOrDefault(t => t.UserName.ToLower() == UserName.ToLower());
        //    }
        //    else
        //    {
        //        CMSContainer = Gets().FirstOrDefault(t => t.UserName.ToLower() == UserName.ToLower() && t.Id != Id);
        //    }
        //    if (CMSContainer != null)
        //        result = true;
        //    else
        //        result = false;
        //    return result;
        //}
        public async Task<bool> IsUserExist(string Id, string UserName)
        {
            var IsExist = false;
            IEnumerable<AspNetUser> result;

            if (string.IsNullOrWhiteSpace(Id))
            {
                result = await GetAllAsync(t => t.UserName.ToLower() == UserName.ToLower().ToLower());

            }
            else
            {
                result = await GetAllAsync(t => t.UserName.ToLower() == UserName.ToLower() && t.Id != Id);


            }

            if (result != null && result.Count() > 0)
                IsExist = true;
            else
                IsExist = false;
            return IsExist;
        }
        public IEnumerable<GetUsers_Result> GetUsers(string roleId, int maxRows = 50, int page = 1, int currentRow = 0)
        {
            try
            {
                var result = new DCEntities().GetUsers(roleId, maxRows, page, currentRow).AsEnumerable();
                return result;
            }
            catch (System.Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        public IEnumerable<GetSchoolStudents_Result> GetSchoolStudents(string roleId, string schoolId, int maxRows = 50, int page = 1, int currentRow = 0)
        {
            try
            {
                var result = new DCEntities().GetSchoolStudents(roleId, schoolId, maxRows, page, currentRow).AsEnumerable();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }

        public GetStudent_Result GetStudent(string AspNetUserId)
        {
            try
            {
                var result = new DCEntities().GetStudent(AspNetUserId).FirstOrDefault();
                return result;
            }
            catch (System.Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }


        public ResultModel InserUserExcel(InserUserExcelMetaData model)
        {
            var message = new ResultModel
            {
                message = "Request initialize.",
                status = 201,
            };
            try
            {
                if (model == null)
                {
                    message.message = "Please provide excel data to be insert school students.";
                    message.status = 202;
                }
                var context = new DCEntities();


                context.Database.ExecuteStoredProcedure(model);
                if (model.result == 1)
                {
                    message.message = model.Message;
                    message.status = 200;
                    message.strid = model.UserExcelMetaDataId;
                }
                else if (model.result == 2)
                {
                    message.message = model.Message;
                    message.status = 204;
                }
                else
                {
                    message.message = model.Message;
                    message.status = 203;
                }
                return message;
            }
            catch (Exception ex)
            {
                message.message = ex.Message;
                message.status = 400;
            }
            return message;
        }

        public ResultModel InsertUpdateSchoolStudent(dynamic model)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter IId = new ObjectParameter("IId", typeof(string));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                ObjectParameter IImage = new ObjectParameter("IImage", typeof(string));
            
           var spresult = new DCEntities().InsertUpdateStudent(model.Id, model.MasterClassId, model.MasterBoardId, model.SchoolId, model.FirstName, model.RollNo,
                    model.Session, model.MobNo, model.Address, model.EmailId, DateTime.Now.AddDays(-600), model.iGender, model.DOB, model.Password, model.CreatedBy,
                    model.Status.GetHashCode(), model.IPAddress,  model.AdmissionNo, model.AccountCode, model.ParentName, model.Action.GetHashCode(), result, IId, IImage, Message);

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

        public ResultModel InsertUpdateDelete(string userExcelMetaDataId, string schoolId, string createdBy, string iPaddress)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter IId = new ObjectParameter("IId", typeof(long));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                ObjectParameter IImage = new ObjectParameter("IImage", typeof(string));
                ObjectParameter rowcount = new ObjectParameter("rowcount", typeof(int));
                var context = new DCEntities();
                context.Database.CommandTimeout = 300;
                var coupresult = context.InsertUpdateBulkStudent(userExcelMetaDataId, schoolId, createdBy, iPaddress, result, rowcount, IId, IImage, Message);
                var rid = Convert.ToInt32(result.Value);
                if (rid == 1)
                {
                    return new ResultModel
                    {
                        id = Convert.ToInt32(rowcount.Value),
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
                        id = -1,
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
                    id = -1,
                    message = v1,
                    status = 400,
                    strid = "",
                    image = ""
                };
            }
        }
        public ResultModel InsertUpdateBulkStudentsBoard(string userExcelMetaDataId, string schoolId, string createdBy, string iPaddress)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                var context = new DCEntities();
                context.Database.CommandTimeout = 300;
                var coupresult = context.InsertUpdateBulkStudentsBoard(userExcelMetaDataId, schoolId, createdBy, iPaddress, result, Message);
                var rid = Convert.ToInt32(result.Value);
                if (rid == 1)
                {
                    return new ResultModel
                    {

                        strid = userExcelMetaDataId,
                        message = Convert.ToString(Message.Value),
                        status = 200,

                    };
                }
                else
                {
                    return new ResultModel
                    {

                        strid = userExcelMetaDataId,
                        message = Convert.ToString(Message.Value),
                        status = 201,


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
        public ResultModel InsertUpdateBulkStudentsClass(string userExcelMetaDataId, string schoolId, string createdBy, string iPaddress)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                var context = new DCEntities();
                context.Database.CommandTimeout = 300;
                var coupresult = context.InsertUpdateBulkStudentsClass(userExcelMetaDataId, schoolId, createdBy, iPaddress, result, Message);
                var rid = Convert.ToInt32(result.Value);
                if (rid == 1)
                {
                    return new ResultModel
                    {

                        strid = userExcelMetaDataId,
                        message = Convert.ToString(Message.Value),
                        status = 200,

                    };
                }
                else
                {
                    return new ResultModel
                    {

                        strid = userExcelMetaDataId,
                        message = Convert.ToString(Message.Value),
                        status = 201,


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

        public ResultModel CleanUploadData(string userExcelMetaDataId, string schoolId)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("result", typeof(int));
                ObjectParameter Message = new ObjectParameter("Message", typeof(string));
                var context = new DCEntities();
                context.Database.CommandTimeout = 300;
                var coupresult = context.CleanUploadData(userExcelMetaDataId, schoolId, result, Message);
                var rid = Convert.ToInt32(result.Value);
                if (rid == 1)
                {
                    return new ResultModel
                    {

                        strid = userExcelMetaDataId,
                        message = Convert.ToString(Message.Value),
                        status = 200,

                    };
                }
                else
                {
                    return new ResultModel
                    {

                        strid = userExcelMetaDataId,
                        message = Convert.ToString(Message.Value),
                        status = 201,


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



    }
    [UserDefinedTableType("udtStudent")]
    public class udtStudent
    {
        [UserDefinedTableTypeColumn(1)]
        public long Id { get; set; }
        [UserDefinedTableTypeColumn(2)]
        public string AdmissionNo { get; set; }
        [UserDefinedTableTypeColumn(3)]
        public string AccountCode { get; set; }
        [UserDefinedTableTypeColumn(4)]
        public string AccountName { get; set; }
        [UserDefinedTableTypeColumn(5)]
        public string ParentName { get; set; }
        [UserDefinedTableTypeColumn(6)]
        public string DateOfBirth { get; set; }


        [UserDefinedTableTypeColumn(7)]

        public string Name { get; set; }
        [UserDefinedTableTypeColumn(8)]
        public string Class { get; set; }
        [UserDefinedTableTypeColumn(9)]
        public string RollNo { get; set; }
        [UserDefinedTableTypeColumn(10)]
        public string Session { get; set; }
        [UserDefinedTableTypeColumn(11)]
        public string MobNo { get; set; }
        [UserDefinedTableTypeColumn(12)]
        public string AltMobNo { get; set; }
        [UserDefinedTableTypeColumn(13)]
        public string Board { get; set; }
        [UserDefinedTableTypeColumn(14)]
        public string Password { get; set; }
        [UserDefinedTableTypeColumn(15)]
        public string ContactPerson { get; set; }
        [UserDefinedTableTypeColumn(16)]
        public string Address { get; set; }
        [UserDefinedTableTypeColumn(17)]
        public string City { get; set; }
        [UserDefinedTableTypeColumn(18)]
        public string State { get; set; }

    }
    [StoredProcedure("InserUserExcelMetaData")]
    public class InserUserExcelMetaData
    {
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "AspNetUserId")]
        public string AspNetUserId { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "FileName")]
        public string FileName { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "SheetName")]
        public string SheetName { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "UploadedFileName")]
        public string UploadedFileName { get; set; }
        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "Rows")]
        public int Rows { get; set; }
        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "Cols")]
        public int Cols { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "FileMetaData")]
        public string FileMetaData { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "Description")]
        public string Description { get; set; }
        [StoredProcedureParameter(SqlDbType.Udt, ParameterName = "Students")]
        public List<udtStudent> Students { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "CreatedBy")]
        public string CreatedBy { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "IPAddress")]
        public string IPAddress { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "UserExcelMetaDataId", Direction = ParameterDirection.Output)]
        public string UserExcelMetaDataId { get; set; }
        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "result", Direction = ParameterDirection.Output)]
        public int result { get; set; }
        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "Message", Direction = ParameterDirection.Output)]
        public string Message { get; set; }
    }
}
