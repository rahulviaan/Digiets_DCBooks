using System;
using System.Collections.Generic;
using System.Linq;
using ErrorLogger;
using System.Web;
using LinqToExcel;
using System.IO;
using Database.Repository;

namespace DC.Web.App.Models
{
    public class ValidateStudentExcel
    {
        public string DataId { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string FileName { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public string _FilePath { get; set; }
        public string FilePath { get; set; }
        public string MetaData { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string SheetName { get; set; }
        public string UploadedFileName { get; set; }

        public IEnumerable<ExcelMetaData> Data { get; set; }
        public virtual ICollection<ColumnMapperModel> ExcelCols { get; set; }
        public virtual ICollection<ExcelDataRow> ExcelRows { get; set; }
    }
    public class ExcelMetaData
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public int Code { get; set; }
        public string RollNo { get; set; }
        public int Session { get; set; }

    }
    public class ColumnMapperModel
    {
        public string Id { get; set; }
        public string Col { get; set; }
        public int? ExcelColIndex { get; set; }
    }
    public class ExcelDataRow
    {
        public virtual ICollection<ExcelData> Data { get; set; }
    }
    public class ExcelData
    {
        public string Value { get; set; }
    }
    public class ProcessExcel
    {
        private ILog logerror = ErrorLogger.Logger.GetInstance;
        //public ValidateStudentExcel ValidateExcel(string filepath, string userid)
        //{
        //    var model = new ValidateStudentExcel
        //    {
        //        Title = "Process initialize",
        //        Detail = "Process initialize for validate excel",
        //        Cols=0,
        //        Rows=0,
        //        FileName="",
        //        MetaData="",
        //        Status = 201,
        //        Data = null
        //    };

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(filepath) || !File.Exists(filepath) || )
        //        {
        //            model.Title = "Please upload correct excel";
        //            model.Detail = $"Uploaded excel sheet name is not defined";
        //            model.Status = 202;
        //            model.Data = null;
        //            return model;
        //        }

        //        Excel.Application MyApp = new Excel.Application();
        //        Excel.Workbook xlWorkbook = MyApp.Workbooks.Open(filepath);
        //        try
        //        {
        //            Excel.Worksheet xlSheet = xlWorkbook.Sheets[1]; 
        //            Excel.Range xlRange = xlSheet.UsedRange; // get the entire used range
        //            MyApp.Visible = false;
        //            int numberOfRows = xlRange.Rows.Count;
        //            int numberOfCols = xlRange.Columns.Count;

        //            //ProductId	MRP	DiscountPercent	DiscPrice	Quantity	BatchNo	dtmMfg	dtmExp
        //            var Name = Utility.ToSafeString(xlRange.Cells[2, 1].Value2).Trim() == "Name";
        //            var Class = Utility.ToSafeString(xlRange.Cells[2, 2].Value2).Trim() == "Class";
        //            var Code = Utility.ToSafeString(xlRange.Cells[2, 3].Value2).Trim() == "Code";
        //            var RollNo = Utility.ToSafeString(xlRange.Cells[2, 4].Value2).Trim() == "RollNo";
        //            var Session = Utility.ToSafeString(xlRange.Cells[2, 5].Value2).Trim() == "Session";

        //            if (!Name)
        //            {
        //                model.Title = "Please check uploaded file 'Name' coloumn is not given at position(A1).";
        //                model.Detail = "Please check uploaded file 'Name' coloumn is not given at position(A1).";
        //                model.Status = 203;
        //                model.Data = null;

        //            }
        //            if (!Class)
        //            {
        //                model.Title = "Please check uploaded file 'Class' coloumn is not given at position(B1).";
        //                model.Detail = "Please check uploaded file 'Class' coloumn is not given at position(B1).";
        //                model.Status = 203;
        //                model.Data = null;

        //            }
        //            if (!Code)
        //            {
        //                model.Title = "Please check uploaded file 'Code' coloumn is not given at position(C1).";
        //                model.Detail = "Please check uploaded file 'Code' coloumn is not given at position(C1).";
        //                model.Status = 203;
        //                model.Data = null;

        //            }
        //            if (!RollNo)
        //            {
        //                model.Title = "Please check uploaded file 'RollNo' coloumn is not given at position(D1).";
        //                model.Detail = "Please check uploaded file 'RollNo' coloumn is not given at position(D1).";
        //                model.Status = 203;
        //                model.Data = null;

        //            }
        //            if (!Session)
        //            {
        //                model.Title = "Please check uploaded file 'Session' coloumn is not given at position(E1).";
        //                model.Detail = "Please check uploaded file 'Session' coloumn is not given at position(E1).";
        //                model.Status = 203;
        //                model.Data = null;

        //            }




        //            //else
        //            //{
        //            //    validatehubinventory.Title = "Please check uploaded file some problem occur while inserting data.";
        //            //    validatehubinventory.Detail = "Please check uploaded file some problem occur while inserting data.";
        //            //    validatehubinventory.Status = 203;
        //            //    validatehubinventory.excelmetadata = null;
        //            //    return validatehubinventory;
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            var v1 = ex.Message;
        //            model.Title = ex.Message;
        //            model.Detail = ex.Message;
        //            model.Status = 400;
        //            model.Data = null;

        //        }
        //        finally
        //        {
        //            xlWorkbook.Saved = true;
        //            MyApp.Quit();
        //        } 
        //    }
        //    catch (Exception ex)
        //    { 
        //        var v1 = ex.Message;
        //        model.Title = ex.Message;
        //        model.Detail = ex.Message;
        //        model.Status = 400;
        //        model.Data = null; 
        //    }
        //    return model;
        //}

        public Response<ValidateStudentExcel> ReadExcelHeaderAndData(string filepath)
        {

            var path = HttpContext.Current.Server.MapPath("~/AppLog");
            logerror.Logerror("ReadExcelHeaderAndData Initialize", path);
            var message = new Response<ValidateStudentExcel>
            {
                Title = "Process initialize",
                Detail = "Process initialize for reading excel header  coloumns.",
                Status = 201,
                Data = null
            };
            try
            {

                if (string.IsNullOrWhiteSpace(filepath) || !File.Exists(filepath))
                {
                    message.Title = "Please upload correct excel";
                    message.Detail = $"Uploaded excel sheet name is not defined";
                    message.Status = 202;
                    message.Data = null;
                    return message;
                }
                var validatestudentexcel = new ValidateStudentExcel
                {
                    Title = "Process initialize",
                    Detail = "Process initialize for validate excel",
                    Cols = 0,
                    Rows = 0,
                    FileName = "",
                    MetaData = "",
                    Status = 201,
                    Data = null


                };
                var lstexcelcolumns = new List<ColumnMapperModel>();

                try
                {

                    if (string.IsNullOrWhiteSpace(filepath))
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel sheet name is not defined";
                        message.Status = 202;
                        message.Data = null;
                        return message;
                    }
                    var excel = new ExcelQueryFactory(filepath);
                    var worksheetNames = excel.GetWorksheetNames();
                    if (worksheetNames == null || worksheetNames.Count() == 0 || string.IsNullOrWhiteSpace(worksheetNames.FirstOrDefault()))
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel sheet name is not defined";
                        message.Status = 202;
                        message.Data = null;
                        return message;
                    }
                    var worksheetname = worksheetNames.FirstOrDefault();
                    validatestudentexcel.FileName = worksheetname;
                    var coloumns = from c in excel.GetColumnNames(worksheetname)
                                   select c;
                    var allRows = excel.Worksheet(worksheetNames.FirstOrDefault());

                    if (coloumns == null || coloumns.Count() == 0 || allRows == null || allRows.Count() == 0)
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel sheet: {worksheetname}, have coloumns: {coloumns.Count()}, and rows: {allRows.Count()}";
                        message.Status = 203;
                        message.Data = null;
                        return message;
                    }
                    int numberOfRows = allRows.Count();
                    int numberOfCols = coloumns.Count();
                    var hubcol = "";
                    int i = 1;
                    								 

                    string[] allowedcols = { "AdmissionNo", "AccountCode", "AccountName", "ParentName", "DateOfBirth", "Class" , "RollNo" , "Session", "Board" };
                    foreach (var col in coloumns)
                    {
                        hubcol = Utility.ToSafeString(col);
                        if (!string.IsNullOrWhiteSpace(hubcol))
                        {
                            lstexcelcolumns.Add(new ColumnMapperModel
                            {
                                Id = i.ToString(),
                                ExcelColIndex = i,
                                Col = Utility.ToSafeString(col)
                            });
                        }
                        i++;
                    }

                    if (allowedcols.Length != lstexcelcolumns.Count())
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel does not have coloumns(Name,Class,RollNo,Session,Board)";
                        message.Status = 202;
                        message.Data = null;
                        return message;
                    }
                    var IsValid = true;
                    for (int j = 0; j < allowedcols.Length; j++)
                    {
                        if (allowedcols[j].Trim().ToLower() != lstexcelcolumns[j].Col.Trim().ToLower())
                        {
                            message.Title = "Please upload correct excel";
                            message.Detail = $"Uploaded excel does not have coloumn: {lstexcelcolumns[j].Col} != {allowedcols[j]} ";
                            message.Status = 202;
                            message.Data = null;
                            IsValid = false;
                            break;
                        }
                    }
                    if (!IsValid)
                    {
                        return message;
                    }

                    var length = numberOfRows > 500 ? 500 : numberOfRows;
                    var lstdata = new List<string>();
                    var exceldatarows = new List<ExcelDataRow>();
                    foreach (var r in allRows.Take(length))
                    {
                        var row = new ExcelDataRow();
                        var ddo = new List<ExcelData>();
                        foreach (var col in coloumns)
                        {
                            var dataobject = new ExcelData
                            {
                                Value = Utility.ToSafeString(r[col])
                            };
                            ddo.Add(dataobject);
                        }

                        row.Data = ddo;
                        exceldatarows.Add(row);
                    }


                    if (lstexcelcolumns.Count() > 0)
                    {
                        validatestudentexcel.ExcelRows = exceldatarows;
                        validatestudentexcel.Rows = numberOfRows;
                        validatestudentexcel.Cols = numberOfCols;
                        validatestudentexcel.ExcelCols = lstexcelcolumns;
                        message.Title = "Coloumn header fetched successfully.";
                        message.Detail = $"Excel sheet: {worksheetname},  coloumns: {numberOfCols},  rows: {numberOfRows}";
                        message.Status = 200;
                        message.Data = validatestudentexcel;
                    }
                    else
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel sheet have coloumns: {numberOfCols} and rows: {numberOfRows}";
                        message.Status = 203;
                        message.Data = null;
                    }
                    return message;
                }
                catch (Exception ex)
                {
                    var v1 = ex.Message;
                    message.Title = ex.Message;
                    message.Detail = ex.Message;
                    message.Status = 400;
                    message.Data = null;
                    return message;
                }

            }
            catch (Exception ex)
            {

                logerror.Logerror("Upload Excel: " + DateTime.UtcNow.ToString() + " -> " + ex.Message, path);
                var v1 = ex.Message;
                message.Title = ex.Message;
                message.Detail = ex.Message;
                message.Status = 400;
                message.Data = null;
                return message;
            }
        }

        public Response<List<udtStudent>> ReadExcelHeaderAndAllData(string filepath, out int cols, out int rows)
        {

            cols = 0;
            rows = 0;
            var message = new Response<List<udtStudent>>
            {
                Title = "Process initialize",
                Detail = "Process initialize for reading excel header  coloumns.",
                Status = 201,
                Data = null
            };

            try
            {
                var exceldatarows = new List<udtStudent>();

                if (string.IsNullOrWhiteSpace(filepath) || !File.Exists(filepath))
                {
                    message.Title = "Please upload correct excel";
                    message.Detail = $"Uploaded excel sheet name is not defined";
                    message.Status = 202;
                    message.Data = null;
                    return message;
                }

                var lstexcelcolumns = new List<ColumnMapperModel>();

                try
                {

                    if (string.IsNullOrWhiteSpace(filepath))
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel sheet name is not defined";
                        message.Status = 202;
                        message.Data = null;
                        return message;
                    }
                    var excel = new ExcelQueryFactory(filepath);
                    var worksheetNames = excel.GetWorksheetNames();
                    if (worksheetNames == null || worksheetNames.Count() == 0 || string.IsNullOrWhiteSpace(worksheetNames.FirstOrDefault()))
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel sheet name is not defined";
                        message.Status = 202;
                        message.Data = null;
                        return message;
                    }
                    var worksheetname = worksheetNames.FirstOrDefault();
                    var coloumns = from c in excel.GetColumnNames(worksheetname)
                                   select c;
                    var allRows = excel.Worksheet(worksheetNames.FirstOrDefault());




                    cols = coloumns.Count();
                    rows = allRows.Count();

                    if (coloumns == null || coloumns.Count() == 0 || allRows == null || allRows.Count() == 0)
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel sheet: {worksheetname}, have coloumns: {coloumns.Count()}, and rows: {allRows.Count()}";
                        message.Status = 203;
                        message.Data = null;
                        return message;
                    }
                    int numberOfRows = allRows.Count();
                    int numberOfCols = coloumns.Count();
                    var hubcol = "";
                    int i = 1;

                     
                    string[] allowedcols = { "AdmissionNo", "AccountCode", "AccountName", "ParentName", "DateOfBirth", "Class", "RollNo", "Session", "Board" };

                    foreach (var col in coloumns)
                    {
                        hubcol = Utility.ToSafeString(col);
                        if (!string.IsNullOrWhiteSpace(hubcol))
                        {
                            lstexcelcolumns.Add(new ColumnMapperModel
                            {
                                Id = i.ToString(),
                                ExcelColIndex = i,
                                Col = Utility.ToSafeString(col)
                            });
                        }
                        i++;
                    }

                    if (allowedcols.Length != lstexcelcolumns.Count())
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel does not have coloumns(Name,Class,RollNo,Session,Board)";
                        message.Status = 202;
                        message.Data = null;
                        return message;
                    }
                    var IsValid = true;
                    for (int j = 0; j < allowedcols.Length; j++)
                    {
                        if (allowedcols[j].Trim().ToLower() != lstexcelcolumns[j].Col.Trim().ToLower())
                        {
                            message.Title = "Please upload correct excel";
                            message.Detail = $"Uploaded excel does not have coloumn: {lstexcelcolumns[j].Col} != {allowedcols[j]} ";
                            message.Status = 202;
                            message.Data = null;
                            IsValid = false;
                            break;
                        }
                    }
                    if (!IsValid)
                    {
                        return message;
                    }

                    var length = numberOfRows;
                    var lstdata = new List<string>();

                    foreach (var r in allRows.Take(length))
                    {
                        var row = new udtStudent();
                        // { "Name", "", "", "" }
                        foreach (var col in coloumns)
                        {
                            
                            switch (col)
                            {
                                case "AdmissionNo":
                                    row.AdmissionNo = Utility.ToSafeString(r[col]);
                                    break;
                                case "AccountCode":
                                    row.AccountCode = Utility.ToSafeString(r[col]);
                                    break;
                                case "ParentName":
                                    row.ParentName = Utility.ToSafeString(r[col]);
                                    break;
                                case "DateOfBirth":
                                    row.DateOfBirth = Utility.ToSafeString(r[col]);
                                    break;
                                case "AccountName": 
                                    row.Name = Utility.ToSafeString(r[col]);
                                    row.AccountName = Utility.ToSafeString(r[col]);
                                    break;
                                case "Class":
                                    row.Class = Utility.ToSafeString(r[col]);
                                    break;
                                case "RollNo":
                                    row.RollNo = Utility.ToSafeString(r[col]);
                                    break;
                                case "Session":
                                    row.Session = Utility.ToSafeString(r[col]);
                                    break;
                                case "Board":

                                    //Vheck Here For Board
                                    var fp = Guid.NewGuid().ToString() + "_err.err";
                                    var context = HttpContext.Current;
                                    string logpath = context.Server.MapPath("~/Attatchments/" + fp);

                                    using (StreamWriter writer = new StreamWriter(logpath, true))
                                    {
                                        writer.WriteLine("---------------------");
                                        writer.WriteLine("Board: "+ Utility.ToSafeString(r[col]));
                                        writer.WriteLine("---------------------");
                                    }

                                    row.Board = Utility.ToSafeString(r[col]);
                                    break;
                            }
                        }
                        var pwd= DC.Encryption.PasswordD(6);
                        row.Password = pwd;
                        exceldatarows.Add(row);
                       // System.Threading.Thread.Sleep(5);
                    }


                    if (lstexcelcolumns.Count() > 0)
                    {
                        message.Title = "Coloumn header fetched successfully.";
                        message.Detail = $"Excel sheet: {worksheetname},  coloumns: {numberOfCols},  rows: {numberOfRows}";
                        message.Status = 200;
                        message.Data = exceldatarows;
                    }
                    else
                    {
                        message.Title = "Please upload correct excel";
                        message.Detail = $"Uploaded excel sheet have coloumns: {numberOfCols} and rows: {numberOfRows}";
                        message.Status = 203;
                        message.Data = null;
                    }
                    return message;
                }
                catch (Exception ex)
                {
                    var v1 = ex.Message;
                    message.Title = ex.Message;
                    message.Detail = ex.Message;
                    message.Status = 400;
                    message.Data = null;
                    return message;
                }

            }
            catch (Exception ex)
            { 
                var v1 = ex.Message;
                message.Title = ex.Message;
                message.Detail = ex.Message;
                message.Status = 400;
                message.Data = null;
                return message;
            }
        }


    }
}