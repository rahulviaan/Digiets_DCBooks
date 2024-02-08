using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace DC.Web.App.Models
{
    public class ReaderBooks
    {

        public string Title { get; set; }
        public string CoverPage { get; set; }
        public string LastPage { get; set; }
        public string Path { get; set; }
        public string DirPath { get; set; }
        public string BookPath { get; set; }
        public string EpubPath { get; set; }
        public DateTime? dtmCreate { get; set; }
        public string IndexTitle { get; set; }
        public string dccreator { get; set; }
        public string dcpublisher { get; set; }
        public string dcidentifier { get; set; }
        public string isSize { get; set; }
        public string dclanguage { get; set; }
        public string dcdate { get; set; }
        public bool Downloaded { get; set; }
        public int DownloadPercentage { get; set; }
        public string EncriptionKey { get; set; }
        public IList<ReaderBookPage> Pages { get; set; }
        public IList<ReaderBookIndex> Indexes { get; set; }
    }
    public class ReaderBookPage
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public bool BookMarked { get; set; }
        public string Path { get; set; }
        public string DirPath { get; set; }
    }
    public class ReaderBookIndex
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string DirPath { get; set; }
        public IList<ReaderBookIndex> Indexes { get; set; }

    }
    public class ReaderBookService
    {

        public IList<ReaderBookIndex> ReadIndexFromXML(out string IndexTitle, string rootpath)
        {
            List<ReaderBookIndex> Indexes = new List<ReaderBookIndex>();
            IndexTitle = "";
            try
            {
                var xmlFile = "";

                foreach (string indexfile in Directory.GetFiles(rootpath, "*.ncx", SearchOption.AllDirectories))
                {

                    StreamReader indexreader = new StreamReader(indexfile);
                    string indexstEn = indexreader.ReadToEnd();
                    indexreader.Close();
                    var navigationindex = indexstEn.Substring(indexstEn.IndexOf("<docTitle>"), indexstEn.IndexOf("</ncx>") - indexstEn.IndexOf("<docTitle>")).Replace("</ncx>", "");

                    StringBuilder sbindex = new StringBuilder(500);
                    sbindex.Append("<?xml version='1.0' encoding='UTF-8' ?>" + Environment.NewLine);
                    sbindex.Append("<root>" + Environment.NewLine);
                    sbindex.Append(navigationindex + Environment.NewLine);
                    sbindex.Append("</root>" + Environment.NewLine);
                    string stxmlindex = sbindex.ToString();
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(stxmlindex);
                    IndexTitle = xmldoc.SelectNodes("/root/docTitle/text")[0].InnerText;
                    XmlNodeList xnList = xmldoc.SelectNodes("/root/navMap/navPoint");
                    foreach (XmlNode x in xnList)
                    {
                        var Index = Utility.ToSafeInt(x.Attributes["playOrder"].InnerText);
                        var Title = x.SelectNodes("navLabel/text")[0].InnerText;
                        var Path = rootpath + "\\" + x.SelectNodes("content")[0].Attributes["src"].InnerText;
                        var navpoint = new ReaderBookIndex
                        {
                            Title = Title,
                            Path = Path,
                            DirPath = rootpath,
                            Index = Index,
                        };
                        navpoint.Indexes = ReadRecuursive(rootpath, x);
                        Indexes.Add(navpoint);
                    }
                }


            }
            catch (Exception ex)
            {
                string st = ex.Message;
                Indexes = null;
            }
            return Indexes;

        }
        public string SetCoverPage(string rootpath)
        {
            string page = "";
            try
            {
                var path = rootpath + "\\images";

                foreach (string fl in Directory.GetFiles(path, "cover.*", SearchOption.AllDirectories))
                {
                    if (File.Exists(fl))
                    {
                        page = path;
                        break;
                    }

                }


            }
            catch
            {
                //  page = Constants.AppImage() + "blankbook.png";

            }
            return page;
        }
        public void CreateBookMark(string BookStaticsPath, int currentindex, string Title)
        {
            if (File.Exists(BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(BookStaticsPath);
                var bookmarknode = xmldoc.SelectNodes("/root/bookmarks/bookmark[@currentpage='" + currentindex + "']");
                if (bookmarknode == null || bookmarknode.Count == 0)
                {
                    XmlElement bookmark = xmldoc.CreateElement("bookmark");
                    bookmark.SetAttribute("currentpage", currentindex.ToString());
                    bookmark.SetAttribute("title", Title);
                    bookmark.SetAttribute("date", DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " : " + DateTime.Now.Hour.ToString() + " : " + DateTime.Now.Minute.ToString() + " : " + DateTime.Now.Second.ToString());
                    XmlNode NodeItem = xmldoc.SelectSingleNode("/root/bookmarks");
                    NodeItem.AppendChild(bookmark);
                    xmldoc.Save(BookStaticsPath);
                }

            }
        }
        public string SetLastPage(string rootpath)
        {
            string page = "";
            try
            {
                var path = rootpath + "\\images";
                foreach (string fl in Directory.GetFiles(path, "back.*", SearchOption.AllDirectories))
                {
                    if (File.Exists(fl))
                    {
                        page = path;
                        break;
                    }

                }

                //page = Constants.AppImage() + "blankbook.png";
            }
            catch
            {
                //page = Constants.AppImage() + "blankbook.png";

            }
            return page;
        }
        public IList<ReaderBookPage> Pages(XmlDocument xmldoc, string rootpath)
        {
            List<ReaderBookPage> pages = new List<ReaderBookPage>();
            try
            {
                int i = 1;
                XmlNodeList xnList = xmldoc.SelectNodes("/root/spine/itemref");
                foreach (XmlNode x in xnList)
                {
                    var Title = Utility.ToSafeString(x.Attributes["idref"].InnerText);
                    XmlNodeList xnPage = xmldoc.SelectNodes("/root/manifest/item [@id ='" + Title + "']");
                    var pageval = xnPage[0].Attributes["href"].InnerText;
                    var page = new ReaderBookPage
                    {
                        Title = (Title.Contains("p") == true) ? i.ToString() : Title,
                        Path = rootpath + "\\" + pageval,
                        DirPath = rootpath,
                        Index = i,
                    };

                    i++;
                    pages.Add(page);
                }
            }
            catch (Exception ex)
            {
                pages = null;
            }
            return pages;
        }
        public IList<ReaderBookIndex> ReadRecuursive(string rootpath, XmlNode xn)
        {
            List<ReaderBookIndex> Indexes = new List<ReaderBookIndex>();
            XmlNodeList xnList = xn.SelectNodes("navPoint");

            if (xnList == null && xnList.Count == 0)
            {
                return Indexes;

            }
            else
            {
                foreach (XmlNode x in xnList)
                {
                    var Index = Utility.ToSafeInt(x.Attributes["playOrder"].InnerText);
                    var Title = x.SelectNodes("navLabel/text")[0].InnerText;
                    var Path = rootpath + "\\" + x.SelectNodes("content")[0].Attributes["src"].InnerText;
                    var navpoint = new ReaderBookIndex
                    {
                        Title = Title,
                        Path = Path,
                        DirPath = rootpath,
                        Index = Index,

                    };
                    navpoint.Indexes = ReadRecuursive(rootpath, x);
                    Indexes.Add(navpoint);
                }
            }
            return Indexes;

        }

    }
    #region xml
    public class AudioModel
    {
        public string start { get; set; }
        public string end { get; set; }
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "smil", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Smil
    {
        [XmlElement(ElementName = "body", Namespace = "http://www.w3.org/ns/SMIL")]
        public Body Body { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "profile")]
        public string Profile { get; set; }
    }

    [XmlRoot(ElementName = "body", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Body
    {
        [XmlElement(ElementName = "par", Namespace = "http://www.w3.org/ns/SMIL")]
        public List<Par> Par { get; set; }
    }

    [XmlRoot(ElementName = "par", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Par
    {
        [XmlElement(ElementName = "text", Namespace = "http://www.w3.org/ns/SMIL")]
        public Text Text { get; set; }
        [XmlElement(ElementName = "audio", Namespace = "http://www.w3.org/ns/SMIL")]
        public Audio Audio { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "text", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Text
    {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
    }

    [XmlRoot(ElementName = "audio", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Audio
    {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
        [XmlAttribute(AttributeName = "clipBegin")]
        public string ClipBegin { get; set; }
        [XmlAttribute(AttributeName = "clipEnd")]
        public string ClipEnd { get; set; }
    }
    #endregion
    public class EbookReader
    {
        #region Props 
        string rootPath = "";
        string encKey = "";
        string sub = "OEBPS/";
        public string pageTitle = "";
        static int lastindex = 0;
        string bookpath = "";
        IList<ReaderBookPage> Pages { get; set; }
        #endregion

        public EbookReader(string _rootPath)
        {
            rootPath = _rootPath;
            bookpath = HttpContext.Current.Server.MapPath("~" + _rootPath); ;
            HttpContext.Current.Session["currentindex"] = 1;
            SetAudioJspath(bookpath);
        }
        public void SetAudioJspath(string bookpath)
        {
            foreach (string fl in Directory.GetFiles(bookpath, "*.js", SearchOption.AllDirectories))
            {
                string line;
                var reader = new StreamReader(fl);
                line = reader.ReadToEnd();
                var dire = rootPath + "/" + sub;
                line = line.Replace("../", dire);
                reader.Close();
                File.Delete(fl);
                File.WriteAllText(fl, line);
            }

        } 
        public string OpenEbook(IList<ReaderBookPage> pages)
        {
            Pages = pages;


            return LoadHtml();

        }
        string LoadHtml()
        {
            //var filename = "p005.xhtml";
            var filename = "cover.xhtml";

            var dire = rootPath + "\\" + sub;
            //string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Books");
            //string filePath = Path.Combine(uploadsFolder, folderPath + "\\" + sub + filename);
            ////var text = System.IO.File.ReadAllText(bookpath);
            return GetCurrentPageHtmlString(dire);
        }
        public ReaderBooks GetReaderBooks(string fileName, string EncKey)
        {
            string IndexTitle;
            var readerBookPages = new List<ReaderBookPage>();
            foreach (string fl in Directory.GetFiles(bookpath, "*.opf", SearchOption.AllDirectories))
            {
                string line;
                var reader = new StreamReader(fl);
                line = reader.ReadToEnd();

                reader.Close();


                //var encKey = "5dd0702c-51b4-483d-9cf8-2db23dc60992";
                var stEn = line;
                //stEn = line = Encryption.DecryptCommon(line, "540f82fe-b0c7-49c4-b80c-1bf143654220");
                if (!string.IsNullOrEmpty(encKey))
                {
                    stEn = line = Encryption.DecryptCommon(line, encKey);
                }

                var metadata = stEn.Substring(stEn.IndexOf("<metadata"), stEn.IndexOf("</metadata>") - stEn.IndexOf("<metadata")) + "<dtmcreate>" + DateTime.Now.ToString() + "</dtmcreate></metadata>";
                metadata = "<metadata" + metadata.Substring(metadata.IndexOf('>'), metadata.Length - metadata.IndexOf('>'));
                metadata = metadata.Replace("dc:", "dc");
                metadata = metadata.Replace(":dc", "dc");

                var pagename = stEn.Substring(stEn.IndexOf("<manifest>"), stEn.IndexOf("</manifest>") - stEn.IndexOf("<manifest>")) + "</manifest>";
                var navigation = stEn.Substring(stEn.IndexOf("<spine toc=\"ncx\">"), stEn.IndexOf("</spine>") - stEn.IndexOf("<spine toc=\"ncx\">")) + "</spine>";

                var sb = new StringBuilder(500);
                sb.Append("<?xml version='1.0' encoding='UTF-8' ?>" + Environment.NewLine);
                sb.Append("<root>" + Environment.NewLine);
                sb.Append(metadata + Environment.NewLine + pagename + Environment.NewLine + navigation + Environment.NewLine);
                sb.Append("</root>" + Environment.NewLine);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(sb.ToString());

                XmlNodeList xnList = xmldoc.SelectNodes("/root/spine/itemref");
                string Title = xmldoc.SelectSingleNode("/root/metadata/dctitle").InnerXml;
                ReaderBooks _readerBooks = new ReaderBooks();
                _readerBooks.Title = Title;
                _readerBooks.DirPath = bookpath + "\\" + "OEBPS";

                _readerBooks.dccreator = xmldoc.SelectSingleNode("/root/metadata/dccreator").InnerXml;
                if (xmldoc.SelectSingleNode("/root/metadata/dcpublisher") != null)
                {
                    _readerBooks.dcpublisher = xmldoc.SelectSingleNode("/root/metadata/dcpublisher").InnerXml;
                }
                else
                {
                    _readerBooks.dcpublisher = string.Empty;
                }
                if (xmldoc.SelectSingleNode("/root/metadata/dcidentifier") != null)
                {
                    _readerBooks.dcidentifier = xmldoc.SelectSingleNode("/root/metadata/dcidentifier").InnerXml;
                }
                else
                {
                    _readerBooks.dcidentifier = string.Empty;
                }

                if (xmldoc.SelectSingleNode("/root/metadata/dclanguage") != null)
                {
                    _readerBooks.dclanguage = xmldoc.SelectSingleNode("/root/metadata/dclanguage").InnerXml;
                }
                else
                {
                    _readerBooks.dclanguage = string.Empty;
                }
                //readerBooks.dclanguage = xmldoc.SelectSingleNode("/root/metadata/dclanguage").InnerXml;
                if (xmldoc.SelectSingleNode("/root/metadata/dcdate") != null)
                {
                    _readerBooks.dcdate = xmldoc.SelectSingleNode("/root/metadata/dcdate").InnerXml;
                }
                else
                {
                    _readerBooks.dcdate = string.Empty;
                }

                var _readerBookService = new ReaderBookService();
                //readerBooks.dcdate = xmldoc.SelectSingleNode("/root/metadata/dcdate").InnerXml;
                _readerBooks.Pages = _readerBookService.Pages(xmldoc, _readerBooks.DirPath);
                _readerBooks.CoverPage = _readerBookService.SetCoverPage(_readerBooks.DirPath);
                _readerBooks.LastPage = _readerBookService.SetLastPage(_readerBooks.DirPath);
                _readerBooks.DirPath = _readerBooks.DirPath;
                _readerBooks.BookPath = bookpath + _readerBooks.Title;
                _readerBooks.Indexes = _readerBookService.ReadIndexFromXML(out IndexTitle, _readerBooks.DirPath);
                _readerBooks.IndexTitle = _readerBooks.Title;
                _readerBooks.Title = _readerBooks.Title;
                _readerBooks.Downloaded = true;
                _readerBooks.DownloadPercentage = 100;

                _readerBooks.BookPath = bookpath;
                _readerBooks.EpubPath = "";
                _readerBooks.EncriptionKey = encKey;
                return (ReaderBooks)_readerBooks;
            }
            return null;
        }

        string GetCurrentPageHtmlString(string dire, string htmlpagename = "")
        {
            Pages = (IList<ReaderBookPage>)(HttpContext.Current.Session["Pages"]);
            //var currentindex = _HttpContextAccessor.HttpContext.Session.GetInt32("currentindex");
            var currentindex = HttpContext.Current.Session["currentindex"];
            int index = 1;
            if (currentindex != null || currentindex.ToString() == "")
            {
                index = Convert.ToInt32(currentindex);
            }

            //this.Title = readerBooks.Title;
            string pagename = string.Empty;
            if (htmlpagename != "")
            {
                pagename = Pages.Where(x => x.Path.Contains(htmlpagename)).FirstOrDefault().Path;
                var Cindex = Pages.Where(x => x.Path.Contains(htmlpagename)).FirstOrDefault().Index;
                HttpContext.Current.Session["currentindex"] = Cindex;

            }
            else
            {
                if (index > 0)
                    pagename = Pages[index - 1].Path;
            }
            lastindex = Pages.Count();
            return GetHtml(dire, pagename, encKey);
        }
        private String getTime(string time)
        {

            string k = "";
            try
            {
                string[] str = time.Split(':');


                int hour = int.Parse(str[0]);
                int Minute = int.Parse(str[1]);
                decimal second = decimal.Parse(str[2]);

                decimal result = second + (Minute * 60) + (hour * 3600);

                result = Math.Truncate(result * 1000m) / 1000m;
                k = result.ToString();
            }
            catch (Exception e)
            {

            }

            return k;

        }
        private void CreateAudioFrame(string fl, out string audioflname, out string sbaudio, out string pagename, string dire)
        {
            audioflname = "";
            pagename = "";
            sbaudio = "";
            string line = "";
            StreamReader reader = new StreamReader(fl);
            StringBuilder sb = new StringBuilder(1000);
            sb.Append("<pre style='display: none' class='timeline'>" + Environment.NewLine);
            while ((line = reader.ReadLine()) != null)
            {
                if (line.ToLower().Trim().Replace(" ", "").Contains("<text"))
                {
                    string pfn = line.Substring(line.IndexOf("<text"), line.IndexOf("/>") - line.IndexOf("<text"));
                    //<text src="../p008.xhtml#word1"
                    int i = pfn.IndexOf("../");
                    int j = pfn.IndexOf("#");
                    pagename = pfn.Substring(i, j - i).Replace("../", "").Trim();
                }
                if (line.ToLower().Trim().Replace(" ", "").Contains("<audio"))
                {
                    //<par id="id1"><text src="../p006.xhtml#word1"/><audio clipBegin="0:00:00.675" clipEnd="0:00:01.875" src="../audio/p006.mp3"/></par>
                    int i = line.IndexOf("<audio");
                    int j = line.LastIndexOf("/>");

                    string[] temp = line.Substring(i, j - i).Replace("<audio", "").Split(' ');


                    foreach (string st in temp)
                    {
                        if (st.Contains("clipBegin"))
                        {
                            string clipBegin = st.Replace("clipBegin", "").Replace('"', ' ').Replace("=", "").Trim();
                            sb.Append(getTime(clipBegin) + " ");
                        }
                        else if (st.Contains("clipEnd"))
                        {
                            string clipEnd = st.Replace("clipEnd", "").Replace('"', ' ').Replace("=", "").Trim();
                            sb.Append(getTime(clipEnd) + " " + Environment.NewLine);
                        }
                        else if (st.Contains("src"))
                        {
                            audioflname = dire + st.Replace("src", "").Replace('"', ' ').Replace("../", "").Replace("=", "").Trim();
                        }
                        //<audio clipBegin="0:00:0.702508" clipEnd="0:00:2.679333" src="../audio/p008.mp3"                           
                    }
                }
            }
            sb.Append("</pre>" + Environment.NewLine);
            sb.Append("<audio class='audioPlayer' id='audioPlayer1' src='" + audioflname + "'></audio><div style='display: none' class='buttons-rw' id='buttons'><button class='play' id='play1' type='button'></button><button class='stop' id='stop1' type='button'></button></div>" + Environment.NewLine);
            sbaudio = sb.ToString();
            reader.Close();
        }
        private string GetHtml(string dire, string pagename, string encryptionKey)
        {

            HttpContext.Current.Session.Remove("Audio");
            HttpContext.Current.Session["Audio"] = false;
            dire = dire.Replace("\\", "/");
            StreamReader reader = new StreamReader(pagename);
            var content = reader.ReadToEnd();
            //string stEn = reader.ReadToEnd();
            reader.Close();
            string stEn = string.Empty;
            if (!string.IsNullOrEmpty(encryptionKey))
            {
                stEn = Encryption.DecryptCommon(content, encryptionKey);
            }
            else
            {
                stEn = content;
            }
            var x = @"<?xml version=""1.0"" encoding=""UTF-8""?>";
            stEn = stEn.Replace(x, "");

            var headstring = stEn.Substring(stEn.IndexOf("<head>"), stEn.IndexOf("</head>") - stEn.IndexOf("<head>"));
            var bodystring = stEn.Substring(stEn.IndexOf("<body"), stEn.IndexOf("</body>") - stEn.IndexOf("<body"));
            bodystring = bodystring.Replace("<body", "<div id='dvBodyMain'class='aie-audio-text-rw col-lg-12' ") + "<div id='dvAudioFooter'></div></div>";

            headstring = headstring.Replace("<head>", "");
            headstring = headstring.Replace(" href=\"", " href=\"" + dire + "");

            headstring = headstring.Replace("../", "");
            pageTitle = headstring.Substring(headstring.IndexOf("<title>"), headstring.IndexOf("</title>") - headstring.IndexOf("<title>"));
            headstring = headstring.Replace(pageTitle, "");
            pageTitle = pageTitle.Replace("<title>", "");
            headstring = headstring.Replace("</title>", "");
            bodystring = bodystring.Replace(" src=\"", " src=\"" + dire);
            headstring = headstring.Replace(" src=\"", " src=\"" + dire);

            bodystring = bodystring.Replace(" url=\"", " url=\"" + dire);
            bodystring = bodystring.Replace("../", "");

            //bodystring= bodystring.Replace(" href=\"", " data-href=\"" + dire);
            bodystring = bodystring.Replace(" href=\"", " data-href=\"");
            bodystring = bodystring.Replace("<a ", "<a onclick= 'Loadpagewihthtml(this)' href='javascript:void(0);'");
            HttpContext.Current.Session["Audio"] = false;
            try
            {
                pagename = pagename.Replace("/", "\\");
                var filename = pagename.Split('\\');
                var adiofn = filename[filename.Length - 1].Split('.');
                var files = Directory.EnumerateFiles(bookpath, adiofn[0] + ".mp3", SearchOption.AllDirectories);
                if (files.Count() > 0)
                {
                    var audioframe = Directory.EnumerateFiles(bookpath, adiofn[0] + ".smil", SearchOption.AllDirectories);

                    //var smileFile = pagename.Replace(".xhtml", ".smil");
                    var text = "";
                    try
                    {
                        HttpContext.Current.Session["Audio"] = true;
                        text = GetAudioScript(AudioStream(audioframe.FirstOrDefault()));
                        // bodystring = bodystring.Replace("<div id='dvAudioFooter'></div>", "<div id='dvAudioFooter'> " + text + "</div>");

                    }
                    catch (Exception ex) { }

                    string audioflname, sbaudio, pagename1;
                    CreateAudioFrame(audioframe.FirstOrDefault(), out audioflname, out sbaudio, out pagename1, dire);
                    HttpContext.Current.Session["Audio"] = true;
                    var audiotag = "<audio  class='audioPlayer' id='audioPlayer1' src='" + dire + "audio/" + adiofn[0] + ".mp3" + "'></audio>  " +
                  " <div class='buttons-rw' id='buttons'><button class='play' id='play1' type='button'></button><button class='stop' id='stop1' type='button'></button></div>";
                    //bodystring = bodystring.Replace("<div id='dvAudioFooter'></div>", "<div id='dvAudioFooter'> " + sbaudio + "</div>");

                    bodystring = bodystring.Replace("<div id='dvAudioFooter'></div>", "<div id='dvAudioFooter'> " + text + "<audio class='audioPlayer' id='audioPlayer1' src='" + dire + "audio/" + adiofn[0] + ".mp3'></audio></div>");

                }
            }
            catch (Exception sss)
            {
                var dd = sss.Message;
            }
            return headstring + bodystring;
        }
        public string LoadPage(int pageID)
        {
            var pageIndex = Utility.ToSafeInt(pageID);
            var dire = rootPath + "\\" + sub;
            HttpContext.Current.Session["currentindex"] = pageIndex;
            var result = GetCurrentPageHtmlString(dire);
            return result;
        }
        public string LoadPageWithdivNaviagaion(string htmlpagewithanchortag)
        {
            var dire = rootPath + "\\" + sub;
            var lstpage = htmlpagewithanchortag.Split('#');
            string htmlpage = string.Empty;
            if (lstpage != null)
            {
                htmlpage = lstpage[0];
            }
            var result = GetCurrentPageHtmlString(dire, htmlpage);
            if (lstpage.Count() != 1)
            {
                result = result.Replace("</html>", "<script src='https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js'></script><script type='text/javascript'>$(document).ready(function () {$('html, body').animate({scrollTop: $('#" + lstpage[1] + "').offset().top}, 'slow');});</script></html>");

            }
            result = result + "<input type='hdCurrentpage' val='" + HttpContext.Current.Session["currentindex"]?.ToString() + "' />";
            return result;
        }

        string AudioStream(string file)
        {
            var results = new List<AudioModel>();

            if (File.Exists(file))
            {
                string text;
                var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }
                var smiles = Utility.ConvertFromXML<Smil>(text);
                if (smiles != null && smiles.Body != null && smiles.Body.Par != null && smiles.Body.Par.Any())
                {
                    foreach (var item in smiles.Body.Par)
                    {
                        var audio = new AudioModel
                        {
                            end = ConvertTomeToSec(item.Audio.ClipEnd).ToString(),
                            start = ConvertTomeToSec(item.Audio.ClipBegin).ToString(),
                            text = item.Text.Src.Split('#')[1]
                        };
                        results.Add(audio);
                    }
                }

            }
            return JsonConvert.SerializeObject(results, Newtonsoft.Json.Formatting.Indented);
        }

        double ConvertTomeToSec(string mnts)
        {
            var allmnts = mnts.Split(':');
            var hrs = Convert.ToDouble(allmnts[0]);
            var mn = Convert.ToDouble(allmnts[1]);
            var sec = Convert.ToDouble(allmnts[2]);
            var seconds = 0D;
            seconds = hrs * 60 * 60;
            seconds = seconds + (mn * 60);
            seconds = seconds + sec;
            return seconds;

        }

        string GetAudioScript(string mediaStream)
        {
            try
            {
                var scriptTag = "<script>var allData = " + mediaStream + "; ";
                StreamReader jsstream = new StreamReader(HttpContext.Current.Server.MapPath("~/Scripts/eBook/zuriets.audio.js"));
                string jsfile = jsstream.ReadToEnd();
                jsstream.Close();
                jsstream.Dispose();
                scriptTag += jsfile + "</script>";
                return scriptTag;
            }
            catch (Exception ex) { }

            return "";
        }

    }


}