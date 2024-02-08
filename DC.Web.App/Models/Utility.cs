using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

public static class Utilities
{
    public static string IsActive(this HtmlHelper html,
                                  string control,
                                  string action, string area = "")
    {
        var conte = HttpContext.Current.Request.Url.PathAndQuery;
        dynamic carea = "";
        try
        {
            carea = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];
        }
        catch (Exception ex)
        {
            var v1 = ex.Message;
            carea = "";
        }

        var routeData = html.ViewContext.RouteData;
        var routeAction = (string)routeData.Values["action"];
        var routeControl = (string)routeData.Values["controller"];
        carea=carea ?? "";
        // both must match
        var returnActive = control == routeControl &&
                           action == routeAction;

        if (returnActive && !string.IsNullOrWhiteSpace(area.ToString()) && !string.IsNullOrWhiteSpace(carea.ToString()))
        {
            if (carea.ToUpper() != area.ToUpper())
            {
                returnActive = false;
            }
        }
        return returnActive ? "active" : "";
    }

    public static string SelectText(this HtmlHelper html, string text)
    {
        TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
        return string.Format("Please Select {0}", cultInfo.ToTitleCase(text));
    }
}

namespace DC.Web.App.Models
{
    class Helper
    {

        public static string IpAddress
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                var ipAddress = string.Empty;

                if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                else if (!string.IsNullOrEmpty(request.UserHostAddress))
                {
                    ipAddress = request.UserHostAddress;
                }

                return ipAddress;
            }
        }
    }
    public class GetInfo
    {

        public static string GetVolumeSerial(string strDriveLetter)
        {
            if (strDriveLetter == "" || strDriveLetter == null) strDriveLetter = "C";
            ManagementObject disk =
                new ManagementObject("win32_logicaldisk.deviceid=\"" + strDriveLetter + ":\"");
            disk.Get();
            return disk["VolumeSerialNumber"].ToString();
        }
        public static string GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (MACAddress == String.Empty)  // only return MAC Address from first card
                {
                    if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }
            MACAddress = MACAddress.Replace(":", "");
            return MACAddress;
        }
        public static string GetCPUId()
        {
            string cpuInfo = String.Empty;
            string temp = String.Empty;
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == String.Empty)
                {// only return cpuInfo from first CPU
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
            }
            return cpuInfo;
        }
    }


    public class MachineInfo
    {
        public static string Network
        {
            get
            {

                return "";
            }
        }
        public static string CompName
        {
            get
            {
                HttpBrowserCapabilities browse = HttpContext.Current.Request.Browser;
                string platform = browse.Platform;
                return platform;
            }
        }

        public static string OSVersion
        {
            get
            {
                OperatingSystem os = Environment.OSVersion;
                return $"OS: {os.Platform.ToString() } Version: {os.Version.ToString() } Service Pack: { os.ServicePack.ToString()}";

            }
        }

    }
    public static class Message91
    {


        public static int Send(string mobileNumber, string message, string sender)
        {


            try
            {
                string authKey = ConfigurationManager.AppSettings["authKey"];

                // https://control.msg91.com/api/sendhttp.php?
                //authkey =YourAuthKey&
                //    mobiles=919999999990,919999999999&
                //    message=message&
                //    sender=ABCDEF&
                //    route=4&
                //    country=0 
                StringBuilder sbPostData = new StringBuilder();
                sbPostData.AppendFormat("authkey={0}", authKey);
                sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
                sbPostData.AppendFormat("&message={0}", message);
                sbPostData.AppendFormat("&sender={0}", sender);
                sbPostData.AppendFormat("&route={0}", 4);


                //Call Send SMS API
                string sendSMSUri = "http://api.msg91.com/api/sendhttp.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {

                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response

                response.Close();
            }
            catch (SystemException ex)
            {
                var v1 = ex.Message;
                // MessageBox.Show(ex.Message.ToString());
            }

            return 0;
        }
        //Prepare you post parameters
        // 
    }
    public class objMail
    {

        public int ID { get; set; }
        public string isApplication { get; set; }

        public string isSubject { get; set; }

        public string isBody { get; set; }
        public string filePath { get; set; }
        public string isFrom { get; set; }
        public string isTo { get; set; }
        public string isBcc { get; set; }
        public string isCC { get; set; }
        public string isIP { get; set; }
        public string ext { get; set; }
        public DateTime? dtmCreate { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public int isStatus { get; set; }
        public string ErrorMessage { get; set; }
    }

    public static class Utility
    {
        public static string GetFileName()
        {
            return Guid.NewGuid().ToString();
            //  return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
        }
        public static T ConvertFromXML<T>(string xmlText)
        {
            if (String.IsNullOrWhiteSpace(xmlText)) return default(T);

            using (StringReader stringReader = new StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
        public static string SetSelectDefaultText(string title)
        {

            string theString = title;
            var array = theString.Split(' ');
            var result = array.Select(s => string.IsNullOrEmpty(s) ? null : s.Substring(0, 1)).ToArray();
            return String.Join("", result);
        }
        public static string GetShortCode(string title)
        {

            string theString = title;
            var array = theString.Split(' ');
            var result = array.Select(s => string.IsNullOrEmpty(s) ? null : s.Substring(0, 1)).ToArray();
            return String.Join("", result);
        }
        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
        {

            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {

                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    {
                        var stype = sourceProp.PropertyType.Name;
                        var dtype = p.PropertyType.Name;
                        if (stype == "Boolean" && dtype == "Int32")
                        {
                            var result = (bool)sourceProp.GetValue(source, null) ? 1 : 0;
                            p.SetValue(dest, result, null);
                        }
                        else
                        {
                            p.SetValue(dest, sourceProp.GetValue(source, null), null);
                        }
                    }
                }

            }

        }
        public static string ExtractHtmlInnerText(string htmlText, int length = 150)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return "";
            }
            Regex regex = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);
            string resultText = regex.Replace(htmlText, " ").Trim();
            if (resultText.Length > length)
            {
                return resultText.Substring(0, length) + "...";
            }
            else
            {
                return resultText + "...";
            }
        }
        public static bool supportedTypes(string ext)
        {
            string[] tp = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            return tp.Contains(ext.ToLower());

        }

        public static bool supportedFiles(string ext)
        {
            string[] tp = new[] { ".jpg", ".jpeg", ".png", ".gif", ".doc", ".docx", ".xls", ".xlsx", ".pdf" };
            return tp.Contains(ext.ToLower());

        }

        public static string getFileName()
        {
            return Guid.NewGuid().ToString();
            //  return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
        }

        public static bool CheckAndCreateRoles()
        {
            var Created = true;
            try
            {
                var roleManager = new IdentityRoleManager();
                if (!roleManager.RoleExists(Roles.SuperAdmin))
                {
                    Created = roleManager.CreateRole(Roles.Admin);
                }
                if (!roleManager.RoleExists(Roles.SuperAdmin))
                {
                    Created = roleManager.CreateRole(Roles.SuperAdmin);
                }
                if (!roleManager.RoleExists(Roles.User))
                {
                    Created = roleManager.CreateRole(Roles.User);
                }
                if (!roleManager.RoleExists(Roles.Teacher))
                {
                    Created = roleManager.CreateRole(Roles.Teacher);
                }
                if (!roleManager.RoleExists(Roles.School))
                {
                    Created = roleManager.CreateRole(Roles.School);
                }
                if (!roleManager.RoleExists(Roles.Student))
                {
                    Created = roleManager.CreateRole(Roles.Student);
                }
            }
            catch (Exception ex)
            {

                Created = false;
            }
            return Created;
        }

        public static string SubDomain { get; set; }

        public static int SendMail(string sub, string bdy, string mailto, string mailfrom = "", string bcc = "", string Host = "")
        {
            int i = 0;
            try
            {

                mailfrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                var ApiUrl = "http://mail.eversion.in/mailAPI/mailAPI/Insertmail";
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                try
                {
                    string responsebody = "";
                    using (WebClient client = new WebClient())
                    {
                        System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection
                        {
                            { "isApplication", Host },
                            { "isSubject", sub },
                            { "isBody", bdy },
                            { "isFrom", mailfrom },
                            { "isTo", mailto },
                            { "isBcc", bcc },
                            { "isIP", Utility.IPAddress }
                        };

                        //var objmail = new objMail
                        //{
                        //    isApplication = Host,
                        //    isSubject = sub,
                        //    isBody = bdy,
                        //    isFrom = mailfrom,
                        //    isTo = mailto,
                        //    isBcc = bcc,
                        //    isCC = "",
                        //    filePath = "",
                        //    isIP = "1.0.0.0",
                        //    ext = "",
                        //};
                        //var result = ClientHelper.DoApiPostMail1<string>("SendMail", objmail);

                        byte[] responsebytes = client.UploadValues(ApiUrl, "POST", reqparm);
                        responsebody = Encoding.UTF8.GetString(responsebytes);
                    }
                    var lstObj = json_serializer.Deserialize<List<dynamic>>(responsebody);
                    if (lstObj.Count > 0)
                    {

                        i = lstObj[0].isSuccess;

                    }
                    else
                    {
                        i = 0;
                    }
                }
                catch (Exception ex)
                {
                    i = 0;
                    var v1 = ex.Message;
                }

                return i;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                i = 0;
            }
            return i;
        }


        public static string GetIpAddress()
        {
            var request = HttpContext.Current.Request;
            var ipAddress = string.Empty;

            if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (!string.IsNullOrEmpty(request.UserHostAddress))
            {
                ipAddress = request.UserHostAddress;
            }

            return ipAddress;
        }
        static public string IPAddress => GetIpAddress();
        //static public string IPAddress()
        //{
        //    return GetIpAddress();
        //    //return HttpContext.Current.Request.UserHostAddress;
        //    //string hostName = Dns.GetHostName();
        //    //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();


        //    //String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    //if (string.IsNullOrEmpty(ip))
        //    //{
        //    //    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //    //}
        //    //return myIP;
        //}
        public static string IsActive(this HtmlHelper html, string control, string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }
        public static string ValidateEmail(string Email)
        {

            string mailto = "";
            string[] tmpEmail = Email.Split(',');
            foreach (string ms in tmpEmail)
            {
                bool isEmail = Regex.IsMatch(ms, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail)
                {
                    mailto += ms + ",";
                }
            }
            mailto = mailto + "avashishta@gmail.com";
            return mailto;
        }
        public static string IsActive(this HtmlHelper html, string control, string action, string id)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "text-danger" : "";
        }
        public static string EncodeQueryStringId(string encodeMe)
        {

            var DecodedString = "";
            try
            {

                byte[] encoded = Encoding.UTF8.GetBytes(encodeMe);
                DecodedString = Convert.ToBase64String(encoded);
            }
            catch
            {
                DecodedString = "";
            }
            return DecodedString;


        }
        public static string DecodeQueryStringId(string decodeMe)
        {


            var EncodedString = "";
            try
            {

                byte[] encoded = Convert.FromBase64String(decodeMe);
                EncodedString = System.Text.Encoding.UTF8.GetString(encoded);
            }
            catch
            {
                EncodedString = "";
            }
            return EncodedString;


        }
        public static int RandomNumber()
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            return myRandomNo;


        }
        public static string MPIN()
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(100000, 999999);
            return myRandomNo.ToString();


        }
        public static string Key()
        {
            string st1 = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            st1 = st1.Length > 6 ? st1.Substring(0, 6) : st1;
            return st1.ToUpper();
        }
        public static bool FileExist(string fn)
        {
            try
            {
                if (File.Exists(fn))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public static string GetImage(string fn, string path, string prefix)
        {
            try
            {
                if (File.Exists(path + fn))
                {
                    return prefix + fn;
                }
                else
                {
                    return "/Images/blansquare.jpeg";
                }
            }
            catch
            {
                return "/Images/blansquare.jpeg";
            }
        }



        public static string GetCodeForEmail(string address)
        {
            string hashkey = Guid.NewGuid().ToString();
            int codelength = 6;
            char[] ValidChars = {
                    '2','3','4','5','6','7','8','9',
                    'A','B','C','D','E','F','G','H',
                    'J','K','L','M','N','P','Q',
                    'R','S','T','U','V','W','X','Y','Z'
            }; // len=32

            byte[] hash;
            using (HMACSHA1 sha1 = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(hashkey)))
                hash = sha1.ComputeHash(UTF8Encoding.UTF8.GetBytes(address));
            int startpos = hash[hash.Length - 1] % (hash.Length - codelength);
            StringBuilder passbuilder = new StringBuilder();
            for (int i = startpos; i < startpos + codelength; i++)
                passbuilder.Append(ValidChars[hash[i] % ValidChars.Length]);
            return passbuilder.ToString();
        }


        public static string GetProFileImage(string fn, string path, string prefix)
        {
            try
            {
                if (File.Exists(path + fn))
                {
                    return prefix + fn;
                }
                else
                {
                    return "/Images/profile.jpg";
                }
            }
            catch
            {
                return "/Images/profile.jpg";
            }
        }

        public static string GetImageDetail(string fn, string path, string prefix)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fn))
                {
                    return "";
                }
                if (File.Exists(path + fn))
                {
                    return prefix + fn;
                }
                else
                {
                    return "/Images/blansquare.jpeg";
                }
            }
            catch
            {
                return "/Images/blansquare.jpeg";
            }
        }
        public static string UniqueKey()
        {
            string st1 = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            string st2 = DateTime.Now.ToString().GetHashCode().ToString("x");
            string st3 = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int len = st1.Length > st2.Length ? st1.Length : st2.Length;
            len = len > st3.Length ? len : st3.Length;
            string _key = "";
            for (int i = 0; i < len; i++)
            {
                if (i <= st3.Length - 1)
                {
                    _key += st3[i].ToString();
                }
                if (i <= st2.Length - 1)
                {
                    _key += st2[i].ToString();
                }
                if (i <= st1.Length - 1)
                {
                    _key += st1[i].ToString();
                }
            }

            return _key;
        }
        public static string UniqueKey(string id)
        {
            string st1 = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            string st2 = DateTime.Now.ToString().GetHashCode().ToString("x");
            string st3 = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int len = st1.Length > st2.Length ? st1.Length : st2.Length;
            len = len > st3.Length ? len : st3.Length;
            string _key = "";
            for (int i = 0; i < len; i++)
            {
                if (i <= st3.Length - 1)
                {
                    _key += st3[i].ToString();
                }
                if (i <= st2.Length - 1)
                {
                    _key += st2[i].ToString();
                }
                if (i <= st1.Length - 1)
                {
                    _key += st1[i].ToString();
                }
            }

            return _key + id;
        }
        public static string ToSafeSubString(string obj, int len)
        {
            string res = ToSafeString(obj);
            string ret = "";
            if (res.Length <= len)
            {
                ret = res;
            }
            else
            {
                ret = res.Substring(0, len);
            }
            return ret;
        }
        public static string ToDateTime(DateTime dt)
        {


            return dt.ToString("MMM dd, yyyy");


        }
        public static string ToDateTimeNull(DateTime? dt)
        {
            if (dt == null)
            {
                return "";
            }

            return ((DateTime)dt).ToString("MMM dd, yyyy");


        }
        public static string ToDateTimeDetails(DateTime? dt)
        {
            //string ap = string.Format("{0:t tt}", dt)=="P PM"?"PM":"AM";
            //String.Format("{0:dddd, MMMM d, yyyy}", dt);
            return String.Format("{0:f}", dt);


        }
        public static string ToDateTimeDisplay(DateTime? dt)
        {
            //string ap = string.Format("{0:t tt}", dt)=="P PM"?"PM":"AM";
            //String.Format("{0:dddd, MMMM d, yyyy}", dt);
            if (dt == null)
            {

                return "";
            }
            else
            {
                if (dt.ToString() == "01/01/1900 12:00:00 AM")
                {
                    return "";
                }
                else
                    return String.Format("{0:f}", dt);
            }


        }
        public static string ToDateTime(DateTime? dt)
        {
            //  DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss")


            if (dt == null)
            {

                return "";
            }
            else
            {
                if (dt.ToString() == "01/01/1900 12:00:00 AM")
                {
                    return "";
                }
                else
                    return String.Format("{0:f}", dt);
            }


        }

        public static string ToDateTimeYYMMDD(DateTime dt)
        {


            return dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString();


        }
        public static string ToDateTimeDDMMMYYYY(DateTime dt)
        {


            return dt.ToString("dd") + " " + dt.ToString("MMM") + " " + dt.Year.ToString();


        }
        public static string ToDateTimeDDMMMYYYY(DateTime? dt)
        {
            try
            {
                DateTime dt1 = (DateTime)dt;
                return dt1.ToString("dd") + " " + dt1.ToString("MMM") + " " + dt1.Year.ToString();

            }
            catch (Exception)
            {

                return "";
            }
        }
        public static string FileName
        {
            get
            {
                return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            }
        }

        public static bool Upload(string path, HttpPostedFileBase postedFile, string fn, string ext)
        {
            try
            {
                bool isSaved = false;
                if (postedFile != null)
                {
                    if (File.Exists(path + "\\" + fn + ext))
                    {
                        File.Delete(path + "\\" + fn + ext);
                    }
                    postedFile.SaveAs(path + "\\" + fn + ext);

                    isSaved = true;
                }

                return isSaved;

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public static async Task<bool> Image_resize1(string input_Image_Path, string output_Image_Path, int new_Width = 0)
        {
            var isSuccess = false;
            try
            {

                const long quality = 10L;
                Bitmap source_Bitmap = new Bitmap(input_Image_Path);
                double dblWidth_origial = source_Bitmap.Width;
                double dblHeigth_origial = source_Bitmap.Height;
                double relation_heigth_width = dblHeigth_origial / dblWidth_origial;
                //int new_Height = (int)dblHeigth_origial; //(int)(new_Width * relation_heigth_width);

                new_Width = new_Width == 0 ? (int)dblWidth_origial : new_Width;
                int new_Height = (int)(new_Width * relation_heigth_width);
                var new_DrawArea = new Bitmap((int)dblWidth_origial, (int)dblHeigth_origial);
                using (var graphic_of_DrawArea = Graphics.FromImage(new_DrawArea))

                {
                    graphic_of_DrawArea.CompositingQuality = CompositingQuality.HighSpeed;
                    graphic_of_DrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic_of_DrawArea.CompositingMode = CompositingMode.SourceCopy;
                    graphic_of_DrawArea.DrawImage(source_Bitmap, 0, 0, new_Width, new_Height);
                    using (var output = File.Open(output_Image_Path, FileMode.Create))
                    {
                        var qualityParamId = System.Drawing.Imaging.Encoder.Quality;
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                        var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                        new_DrawArea.Save(output, codec, encoderParameters);
                        output.Close();

                    }
                    graphic_of_DrawArea.Dispose();

                }
                source_Bitmap.Dispose();
                isSuccess = true;
                //File.Delete(input_Image_Path);
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                isSuccess = false;
            }
            return isSuccess;
        }

        public static bool Image_resize(string input_Image_Path, string output_Image_Path, int new_Width)
        {
            var isSuccess = false;
            try
            {
                const long quality = 50L;
                Bitmap source_Bitmap = new Bitmap(input_Image_Path);
                double dblWidth_origial = source_Bitmap.Width;
                double dblHeigth_origial = source_Bitmap.Height;
                double relation_heigth_width = dblHeigth_origial / dblWidth_origial;
                int new_Height = (int)(new_Width * relation_heigth_width);
                var new_DrawArea = new Bitmap(new_Width, new_Height);
                using (var graphic_of_DrawArea = Graphics.FromImage(new_DrawArea))

                {
                    graphic_of_DrawArea.CompositingQuality = CompositingQuality.HighSpeed;
                    graphic_of_DrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic_of_DrawArea.CompositingMode = CompositingMode.SourceCopy;
                    graphic_of_DrawArea.DrawImage(source_Bitmap, 0, 0, new_Width, new_Height);
                    using (var output = System.IO.File.Open(output_Image_Path, FileMode.Create))
                    {

                        var qualityParamId = System.Drawing.Imaging.Encoder.Quality;
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                        var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                        new_DrawArea.Save(output, codec, encoderParameters);
                        output.Close();

                    }
                    graphic_of_DrawArea.Dispose();
                }
                source_Bitmap.Dispose();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                isSuccess = false;
            }
            return isSuccess;
        }

        public static bool Thumbnail(string fileName, string thumbName, int imgHeight = 100, int imgWidth = 100)
        {
            try
            {

                Image img = Image.FromFile(fileName);

                if (img.Width < img.Height)
                {
                    //portrait image  
                    imgHeight = 100;
                    var imgRatio = (float)imgHeight / img.Height;
                    imgWidth = Convert.ToInt32(img.Height * imgRatio);
                }
                else if (img.Height < img.Width)
                {
                    //landscape image  
                    imgWidth = 100;
                    var imgRatio = (float)imgWidth / img.Width;
                    imgHeight = Convert.ToInt32(img.Height * imgRatio);
                }
                Image thumb = img.GetThumbnailImage(imgWidth, imgHeight, () => false, IntPtr.Zero);
                thumb.Save(thumbName);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public static bool Move(string src, string dest, string fn)
        {
            bool isMove = false;

            try
            {
                File.Copy(src + fn, dest + fn);
                File.Delete(src + fn);
                isMove = true;
            }
            catch
            {
                isMove = false;
            };

            return isMove;

        }

        public static bool Delete(string[] path, string BasePath)
        {
            bool isDeleted = false;
            try
            {
                foreach (var item in path)
                {
                    Delete(BasePath + item);
                }
            }
            catch { }
            return isDeleted;
        }
        public static bool Delete(string path)
        {
            bool isDeleted = false;
            try
            {

                if (File.Exists(path))
                {

                    File.Delete(path);
                    //File.Delete(path.Replace(".", "_th."));
                    isDeleted = true;
                }

            }
            catch { }

            return isDeleted;
        }
        public static bool SupportedTypes(string ext)
        {
            string[] tp = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mkv", ".3gp", ".ogg" };
            return tp.Contains(ext.ToLower());

        }
        public static bool SupportedExcelTypes(string ext)
        {
            string[] tp = new[] { ".xlsx", ".xls" };
            return tp.Contains(ext.ToLower());

        }
        #region "Validation"

        public static bool ValidateDate(string stringDateValue)
        {
            try
            {
                System.Globalization.CultureInfo CultureInfoDateCulture = new System.Globalization.CultureInfo("en-US");
                DateTime d = DateTime.ParseExact(stringDateValue, "MM/dd/yyyy", CultureInfoDateCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsItNumber(string inputvalue)
        {
            //            IsItNumber("2"); will return true;
            //IsItNumber("A"); will return false;
            System.Text.RegularExpressions.Regex isnumber = new System.Text.RegularExpressions.Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }
        public static bool IsItDecimalNumber(string inputvalue)
        {

            System.Text.RegularExpressions.Regex isnumber = new System.Text.RegularExpressions.Regex("^([0-9]*)(\\.[0-9]{2})?$");
            return !isnumber.IsMatch(inputvalue);
        }
        public static bool IsItAlphabate(string inputvalue)
        {

            System.Text.RegularExpressions.Regex isnumber = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");
            return !isnumber.IsMatch(inputvalue);
        }
        public static bool IsItAlphaNumeric(string inputvalue)
        {

            System.Text.RegularExpressions.Regex isnumber = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9]*$");
            return !isnumber.IsMatch(inputvalue);
        }
        public static string ToSafeString(object obj)
        {
            return (obj ?? string.Empty).ToString().Trim();
        }


        internal static string ToSafeStringReplaced(object obj)
        {
            try
            {
                var dt = (obj ?? string.Empty).ToString().Trim();
                foreach (var item in UserLoginSourse())
                {
                    dt = dt.Replace(item + "-", "");
                }
                return dt;
            }
            catch (Exception ex)
            {

                return "";
            }
        }
        public static string[] UserLoginSourse()
        {
            try
            {
                string[] ls = { "google", "facebook", "DC", "twitter" };
                return ls;
            }
            catch (Exception ex)
            {
                string[] ls = { "" };
                return ls;
            }
        }


        public static bool ToSafeBool(object obj)
        {
            if (obj == null || Convert.ToBoolean(obj) == false)
                return false;
            else
                return true;
        }


        public static string ToSafeSubString(object obj, short len)
        {
            var result = (obj ?? string.Empty).ToString().Trim();

            if (result.Length > len)
            {
                return result.Substring(0, len);
            }
            else
                return result;

        }



        public static int ISNULL(string str)
        {
            if (str == "")
                return 0;
            else
                return Convert.ToInt32(str);
        }
        public static long ToSafeLong(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;
            else
                return ISNULLLong(Convert.ToString(obj));
        }
        public static long ISNULLLong(string str)
        {
            if (str == "")
                return 0;
            else
                return Convert.ToInt64(str);
        }
        public static int ToSafeInt(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;
            else
                return ISNULL(Convert.ToString(obj));
        }
        public static int ToSafeIntNegative(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return -1;
            else
                return ISNULL(Convert.ToString(obj));
        }
        public static byte[] ToSafeByteFromString(string obj)
        {
            byte[] array = new byte[0];
            if (obj == null || obj == "")
                return array;
            else
                return Encoding.UTF8.GetBytes(obj);
        }
        internal static byte[] ToSafeByte(object p)
        {
            byte[] array = new byte[0];
            if (p == null || p == System.DBNull.Value)
                return array;
            else
                return (byte[])p;
        }
        public static double ToSafeDouble(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;
            else
                return Convert.ToDouble(obj);
        }
        public static decimal ToSafeDecimal(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;
            else
                return Convert.ToDecimal(obj);
        }
        public static DateTime? ToSafeDate(object obj)
        {
            if (obj == null || obj.ToString() == "" || obj == System.DBNull.Value)
                return Convert.ToDateTime("1/1/1900");
            else
                return Convert.ToDateTime(obj);
        }
        
        #endregion


        public static IEnumerable<SelectListItem> ToSelectLists<TEnum>()
        {
            var myEnumDescriptions = from TEnum n in Enum.GetValues(typeof(TEnum))
                                     select new SelectListItem
                                     {
                                         Text = GetEnumDescription(n),
                                         Value = n.GetHashCode().ToString()
                                     };
            return myEnumDescriptions;
        }

        private static string GetEnumDescription<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static byte[] ImageToBinary(string imagePath)
        {
            FileStream fS = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] b = new byte[fS.Length];
            fS.Read(b, 0, (int)fS.Length);
            fS.Close();
            return b;
        }


    }

    public static class Encryption
    {


        private static byte[] _salt = Encoding.ASCII.GetBytes("b9eafac5-27f5-446a-b8fa-f1a9f9b3788e");
        private static string key = "beb65e7b-1daa-4f8c-868a-0cffb08dfa23";


        public static string DecryptStringDES(string cipherText, string _Key)
        {
            string sharedSecret = _Key;
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");
            RijndaelManaged aesAlg = null;
            string plaintext = "";
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plaintext;
        }
        public static string EncryptStringAES(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");
            string outStr = null;                       // Encrypted string to return 
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data. 
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return outStr;
        }
        public static string StringAES(string plainText)
        {
            if (plainText == String.Empty)
            {
                return "";
            }
            if (plainText == null)
            {
                return "";
            }
            string sharedSecret = key;
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return 
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data. 

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return outStr;
        }
        public static string StringDES(string cipherText)
        {
            if (cipherText == String.Empty)
            {
                return "";
            }
            string sharedSecret = key;
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");
            RijndaelManaged aesAlg = null;
            string plaintext = null;
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        public static string DecryptCommon(string textToDecrypt, string _key = "")
        {
            if (string.IsNullOrWhiteSpace(textToDecrypt))
            {
                return "";
            }
            key = _key == "" ? key : _key;
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }
        public static string EncryptCommon(string textToEncrypt, string _key = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textToEncrypt))
                {
                    return "";
                }
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                //key = _key == "" ? key : _key;
                key = "beb65e7b-1daa-4f8c-868a-0cffb08dfa23";
                rijndaelCipher.KeySize = 0x80; // 256bit key
                rijndaelCipher.BlockSize = 0x80;
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
                return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    public static class ReserveDomain
    {

        public static List<string> Reserved = new List<string>
                 {
                         "www",
                         "http",
                         "api",
                         "secure",
                         "help",
                         "live",
                         "mail",
                         "ftp",
                         "text",
                         "txt",
                         "flixsign",
                         "https",
                         "product",
                         "registration",
                         "register",
                         "login",
                         "logout",
                         "signin",
                         "signout",
                         "email",
                         "smtp",
                         "pop",
                         "pop3",
                         "mailserver",
                         "verify",
                         "awverify",
                         "web",
                         "website",
                         "localhost",
                         "averify.www",
                         "calender",
                         "fax",
                         "azure",
                         "imap",
                         "mobilemail",
                         "manage",
                 };
    }
}
