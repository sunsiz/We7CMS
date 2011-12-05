using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Web.Services.Protocols;
using System.Data.SQLite;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using We7.Framework.Config;

namespace We7
{
    /// <summary>
    /// ���þ�̬������
    /// </summary>
    public static class We7Helper
    {
        /// <summary>
        /// ���ַ�������ΪBase64�ַ���
        /// </summary>
        /// <param name="str">Ҫ������ַ���</param>
        public static string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// ��Base64�ַ�������Ϊ��ͨ�ַ���
        /// </summary>
        /// <param name="str">Ҫ������ַ���</param>
        public static string Base64Decode(string str)
        {
            byte[] barray;
            barray = Convert.FromBase64String(str);
            return Encoding.Default.GetString(barray);
        }

        public static string HtmlEncode(string text)
        {
            return text.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public static string HtmlDecode(string text)
        {
            return text.Replace("&quot;", "\"").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
        }

        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            if (Directory.Exists(sourcePath))
            {
                DirectoryInfo di = new DirectoryInfo(sourcePath);
                FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories);
                foreach (FileInfo file in files)
                {
                    string fn = file.FullName.Replace(di.FullName + "\\", String.Empty);
                    string targetFile = Path.Combine(targetPath, fn);
                    string path = Path.GetDirectoryName(targetFile);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    try
                    {
                        file.CopyTo(targetFile, true);
                    }
                    catch (UnauthorizedAccessException uex)
                    {
                        //�Ѿ�������UnauthorizedAccessException����ɾ���͸���һ����ûȨ��
                        //File.Delete(targetFile);
                        //file.CopyTo(targetFile, true);
                        We7.Framework.LogHelper.WriteLog(typeof(We7Helper), uex);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        public static void DeleteFileTree(System.IO.DirectoryInfo path)
        {
            foreach (System.IO.DirectoryInfo d in path.GetDirectories())
            {
                DeleteFileTree(d);
            }

            foreach (System.IO.FileInfo f in path.GetFiles())
            {
                File.SetAttributes(f.FullName, FileAttributes.Normal);
                f.Delete();
            }
            Directory.Delete(path.FullName);
        }

        public static void DeleteFileTree(System.IO.DirectoryInfo path, bool DeleteMe)
        {
            foreach (System.IO.DirectoryInfo d in path.GetDirectories())
            {
                DeleteFileTree(d);
            }

            foreach (System.IO.FileInfo f in path.GetFiles())
            {
                File.SetAttributes(f.FullName, FileAttributes.Normal);
                f.Delete();
            }

            if (DeleteMe)
            {
                Directory.Delete(path.FullName);
            }
        }

        public static void AssertNotNull(object obj, string name)
        {
            string mesage = String.Format("{0} in null.", name);
            Assert(obj != null && obj != DBNull.Value, mesage);
        }

        public static void Assert(bool b, string message)
        {
            if (!b)
            {
                throw new Exception(message);
            }
        }

        public static string CreateNewID()
        {
            return "{" + Guid.NewGuid().ToString() + "}";
        }

        public static bool IsNumber(string s)
        {
            return !String.IsNullOrEmpty(s) && Regex.IsMatch(s, "^[-]?\\d+$");
        }

        public static bool IsGUID(string s)
        {
            return Regex.IsMatch(s, "^{\\w{8}\\-\\w{4}\\-\\w{4}\\-\\w{4}\\-\\w{12}}$");
        }

        public static bool IsValidIP(string ip)
        {
            bool result = false;
            try
            {
                if (Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
                {
                    string[] ips = ip.Split('.');
                    if (ips.Length == 4 || ips.Length == 6)
                    {
                        if (Int32.Parse(ips[0]) < 256 && Int32.Parse(ips[1]) < 256 & Int32.Parse(ips[2]) < 256 & Int32.Parse(ips[3]) < 256)
                            result = true;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        } 

        public static bool HasValue(string[] list, string value)
        {
            if (list != null)
            {
                foreach (string s in list)
                {
                    if (s == value)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
                return false;

        }

        public static string EmptyGUID
        {
            get
            {
                return "{00000000-0000-0000-0000-000000000000}";
            }
        }

        public static string NotFoundID
        {
            get
            {
                //return "-1"; by thehim 2009-10-28
                return "";
            }
        }

        public static bool IsEmptyID(string id)
        {
            return id == null || id == String.Empty || id == EmptyGUID || id == EmptyWapGUID;
        }

        /// <summary>
        /// {00000000-1111-0000-0000-000000000000}
        /// </summary>
        public static string EmptyWapGUID
        {
            get
            {
                return "{00000000-1111-0000-0000-000000000000}";
            }
        }

        //public static bool IsEmptyWapID(string id)
        //{
        //    return id == null || id == String.Empty || id == EmptyWapGUID;
        //}

        /// <summary>
        /// url·��ȥ��Ӧ��·��������Ŀ¼������
        /// </summary>
        /// <param name="path"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public static string RemoveAppFromPath(string path, string app)
        {
            path = Regex.Replace(path, "^" + app, string.Empty, RegexOptions.IgnoreCase);
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            return path;
        }

        /// <summary>
        /// ��GUID��ʽ���ַ���ת��Ϊ�Ϸ���������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GUIDToFormatString(string id)
        {
            if (id == null || id == "")
            {
                return "";
            }
            string idd = id.Replace("{", "").Replace("}", "");
            return idd.Replace("-", "_");
        }

        /// <summary>
        /// ��׼�� ID ��ʽ����Ϊ{00000000-0000-0000-0000-000000000000}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string FormatToGUID(string id)
        {
            if (!id.StartsWith("{")) id = "{" + id;
            if (!id.EndsWith("}")) id = id + "}";
            return id.Replace("_", "-");
        }

        public static string FilterXMLChars(string instring)
        {
            if (String.IsNullOrEmpty(instring))
                return String.Empty;

            string source = instring;
            while (source.IndexOf("&amp;") > -1)
            {
                source = source.Replace("&amp;", "&");
            }

            source = source.Replace("&amp;nbsp;", "&nbsp;");
            source = source.Replace("&amp;lt;", "&lt;");
            source = source.Replace("&amp;gt; ", "&gt;");
            return source;
        }

        /// <summary>
        /// ���JS�еĵ�������˫����
        /// </summary>
        /// <param name="instring"></param>
        /// <returns></returns>
        public static string FilterJSChars(string instring)
        {
            string source = instring;

            source = source.Replace("\"", "&quot;");
            source = source.Replace("'", "\\\'");
            return source;
        }

        public static long IPToINT(string ip)
        {
            string[] parts = ip.Split('.');
            long[] ids = new long[] { 0, 0, 0, 0 };
            if (parts.Length == 4)
            {
                ids[0] = long.Parse(parts[0]);
                ids[1] = long.Parse(parts[1]);
                ids[2] = long.Parse(parts[2]);
                ids[3] = long.Parse(parts[3]);

                long id = ids[0] * 256 * 256 * 256 + ids[1] * 256 * 256 + ids[2] * 256 + ids[3];
                return id;
            }
            else
                return -1;
        }

        /// <summary>
        /// ��IP������ڳ���
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetCityNameByIP(string ip)
        {
            string city = "";
            string strCon = GeneralConfigs.GetConfig().IPDBConnection;//�����ַ���
            HttpContext Context = HttpContext.Current;
            string dataPath = Context.Server.MapPath("~/App_Data/DB");
            strCon = strCon.Replace("{$Current}", dataPath);
            SQLiteConnection myConnection = new SQLiteConnection(strCon);//�������Ӷ���
            string strSel = "select * from [IPToCity] where [IP_Start]<= {0} and [IP_End] >={0}";
            strSel = string.Format(strSel, IPToINT(ip).ToString());
            SQLiteCommand cmd = new SQLiteCommand(strSel, myConnection);
            try
            {
                cmd.Connection.Open();
                SQLiteDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    city = city = myReader["IP_Province"].ToString() + " " + myReader["IP_City"].ToString();
                }
            }
            catch (Exception)
            {

            }
            if (myConnection != null && myConnection.State == ConnectionState.Open)
            {
                myConnection.Close();
            }
            return city;
        }

        static string GetCityNameByIP_Access(string ip)
        {
            string city = "";
            string strCon = GeneralConfigs.GetConfig().IPDBConnection;//�����ַ���
            HttpContext Context = HttpContext.Current;
            string dataPath = Context.Server.MapPath("~/App_Data/DB");
            strCon = strCon.Replace("{$Current}", dataPath);
            OleDbConnection myConnection = new OleDbConnection(strCon);//�������Ӷ���
            string strSel = "select * from [IPToCity] where [IP_Start]<= {0} and [IP_End] >={0}";
            strSel = string.Format(strSel, IPToINT(ip).ToString());
            OleDbCommand cmd = new OleDbCommand(strSel, myConnection);
            try
            {
                cmd.Connection.Open();
                OleDbDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    city = city = myReader["IP_Province"].ToString() + " " + myReader["IP_City"].ToString();
                }
            }
            catch (Exception)
            {

            }
            if (myConnection != null && myConnection.State == ConnectionState.Open)
            {
                myConnection.Close();
            }
            return city;
        }

        /// <summary>
        /// ��URL��ȡ����
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomainFromUrl(string url)
        {
            if (url == null || url == "") return "";

            if(url.ToLower().StartsWith("http://"))
                url=url.Remove(0,7);
            string[] parts = url.Split('/');
            string domain="";
            List<string> dotcoms=new List<string>(){".com",".org",".net",".gov"};
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] != "" && parts[i].IndexOf(".") > 0)
                {
                    domain = parts[0];
                    break;
                }
            }
            if (domain.Length > 0)
            {
                string[] ds = domain.Split('.');
                if (ds.Length == 3 && !dotcoms.Contains(ds[1]))
                    domain = ds[1] + "." + ds[2];
                else if(ds.Length == 4 && dotcoms.Contains(ds[2]))
                    domain = ds[1] + "." + ds[2]+"."+ds[3];
                else if (ds.Length > 4)
                {
                    domain = ds[ds.Length - 3] + "." + ds[ds.Length - 2] + "." + ds[ds.Length - 1];
                }
            }
            return domain;
        }

        /// <summary>
        /// ����url��ȥ���ļ������֡���/news/2.html �е�/news/
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetChannelUrlFromUrl(string url)
        {
            if (url.IndexOf("?") > 0)
                url = url.Remove(url.IndexOf("?"));

            if (url.IndexOf('.') > 0)
                return url.Substring(0, url.LastIndexOf('/') + 1);
            else
            {
                if (!url.EndsWith("/")) url = url + "/";
                return url;
            }
        }

        public static string RemoveHtml(string content)
        {
            string newstr = FilterScript(content);
            string regexstr = @"<[^>]*>";
            newstr = Regex.Replace(newstr, regexstr, string.Empty, RegexOptions.IgnoreCase);
            return newstr;
        }

        public static string RemoveGUID(string content)
        {
            string newstr = FilterScript(content);
            string regexstr = @"{[^}]*}";
            newstr = Regex.Replace(newstr, regexstr, string.Empty, RegexOptions.IgnoreCase);
            return FilterHtmlChars(newstr);

        }

        public static string FilterScript(string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return content;
            }
            string regexstr = @"(?i)<script([^>])*>(\w|\W)*</script([^>])*>";//@"<script.*</script>";
            content = Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<script([^>])*>", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Replace(content, "</script>", string.Empty, RegexOptions.IgnoreCase);
        }

        public static string FilterHtmlChars(string instring)
        {
            string source = instring;
            source = source.Replace("<br>", "\n");
            source = source.Replace("&nbsp;", " ");
            source = source.Replace("&lt;", "<");
            source = source.Replace("&gt;", ">");
            source = source.Replace(" ", "");
            source = source.Replace("&#8220;", "��");
            source = source.Replace("&#8221;", "��");
            source = source.Replace("&amp;", "&");
            return source;
        }

        /// <summary>
        /// ��ͨ�ı�ת��Ϊhtml�������س����з�\n\t��
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string ConvertTextToHtml(string txt)
        {
            string dst = txt;
            dst = dst.Replace("<", "&lt;");
            dst = dst.Replace(">", "&rt;");
            dst = dst.Replace("'", "\"");
            dst = dst.Replace(" ", "&nbsp;");
            dst = dst.Replace("\r\n", "<br>");
            dst = dst.Replace("\r", "<br>");
            dst = dst.Replace("\n", "<br>");
            dst = dst.Replace("\"", "&quot;");
            return dst;
        }

        /// <summary>
        /// ��GUID��ʽ���ַ���ת��Ϊ�Ϸ���������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string RemoveBrarket(string id)
        {
            if (id==null)
            {
                return null;
            }
            string idd = id.Replace("{", "").Replace("}", "");
            return idd.Replace("-", "_").Trim();
        }

        /// <summary>
        /// ��ҳ��ת������divת��Ϊ�ַ�����ҳ����
        /// </summary>
        /// <param name="content">����</param>
        /// <returns></returns>
        public static string ConvertPageBreakFromVisualToChar(string content)
        {
            string regexstr = @"<div[^>]*page-break-after[^>]*\>([\w\W]*?)\</div\>";
            string targetstr = "����ҳ����";
            string newstr = Regex.Replace(content, regexstr, targetstr, RegexOptions.IgnoreCase);
            return newstr;
        }


        /// <summary>
        /// ��ҳ����ʱתΪJAVASCRIPT������ʾЧ��
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ConvertPageBreakToHtml(string content)
        {
			if (string.IsNullOrEmpty(content)) return content;
			string target = "����ҳ����";
            if (content.Contains(target))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.Text.StringBuilder pageStr = new System.Text.StringBuilder();
                string[] list = We7.Framework.Util.Utils.SplitString(content, target);
                if (list.Length > 0)
                {
                    sb.Append(@"<script type='text/javascript' language='javascript'>
                                function PageBreakChange(index) {
                                    $('.pageBreak').hide();
                                    $('#pb' + index).show();
                                }
                            </script>");

                    int count = 1;
                    pageStr.Append("��:"); ;
                    foreach (string s in list)
                    {
                        sb.Append("<div class='pageBreak' id='pb" + count + "' style='display:" + (count == 1 ? "block" : "none") + ";'>");
                        sb.Append(s);
                        sb.Append("</div>");
                        pageStr.Append("<a href='javascript:void(0)'onclick=\"PageBreakChange('"+ count + "')\" class='pblink'><b> " + count + " </b></a>");
                        count++;
                    }
                    pageStr.Append(" ҳ");
                }
                sb.Append(pageStr.ToString());

                return sb.ToString();
            }
            return content;
        }

        /// <summary>
        /// ��ҳ��ת�����ɡ���ҳ����ת��Ϊ�ַ�div
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ConvertPageBreakFromCharToVisual(string content)
        {
            string oldstr = @"����ҳ����";
            string targetstr = @"<div style=""page-break-after: always;""><span style=""display: none;"">&nbsp;</span></div>";
            string newstr = content.Replace(oldstr, targetstr);
            string regStr = @"(At|��)(\s)([0-9]{4}-(((0[13578]|(10|12))-(0[1-9]|[1-2][0-9]|3[0-1]))|(02-(0[1-9]|[1-2][0-9]))|((0[469]|11)-(0[1-9]|[1-2][0-9]|30)))($|\s([0-1]\d|[2][0-3])\:[0-5]\d\:[0-5]\d))(,|��)(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)(\s)(wrote:|д����|д��:)";
            if (Regex.IsMatch(newstr, regStr, RegexOptions.IgnoreCase))
            {
                newstr = Regex.Replace(newstr, regStr,"", RegexOptions.IgnoreCase);
            }
            return newstr;
        }

        public static string AddParamToUrl(string url, string paramName, string paramValue)
        {
            string regexstr, regexstr2, fileNameExt;
            fileNameExt ="html";
            regexstr = "(/?)$";
            regexstr2 = string.Format(@"(\.(aspx|{0}))$", fileNameExt);
            if (Regex.IsMatch(url, regexstr, RegexOptions.IgnoreCase) && !Regex.IsMatch(url, regexstr2, RegexOptions.IgnoreCase) && url.IndexOf("?") < 0)
            {
                if (!url.EndsWith("/"))
                {
                    url += "/";
                }
                url += "default." + fileNameExt;
            }
            regexstr = string.Format(@"{0}=\s*(.*?)&", paramName);
            if (Regex.IsMatch(url, regexstr, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(url, regexstr, paramName + "=" + paramValue + "&", RegexOptions.IgnoreCase);
            }
            regexstr = string.Format(@"{0}=\s*(.*?)$", paramName);
            if (Regex.IsMatch(url, regexstr, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(url, regexstr, paramName + "=" + paramValue, RegexOptions.IgnoreCase);
            }
            if (url.IndexOf("?") > -1)
            {
                return url + "&" + paramName + "=" + paramValue;
            }
            return url + "?" + paramName + "=" + paramValue;
        }

        public static string RemoveParamFromUrl(string url, string paramName)
        {
            string regexstr;
            //regexstr = string.Format(@"{0}=\s*(.*)&", paramName);
            regexstr = string.Format(@"{0}=[^&]*&", paramName);
            if (Regex.IsMatch(url, regexstr, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(url, regexstr, string.Empty, RegexOptions.IgnoreCase);
            }
            //regexstr = string.Format(@"([&]|[?]){0}=\s*(.*)$", paramName);
            regexstr = string.Format(@"([&]|[?]){0}=[^&]*$", paramName);
            if (Regex.IsMatch(url, regexstr, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(url, regexstr, string.Empty, RegexOptions.IgnoreCase);

            }

            return url;
        }

        public static string GetParamValueFromUrl(string url, string paramName)
        {
            string qs = paramName + "=";
            if (url.Length > 0)
            {
                int begin = url.IndexOf(qs);
                if (begin != -1)
                {
					if (url.Substring(begin - 1, 1) == "?" || url.Substring(begin - 1, 1) == "&")
					{
						begin += qs.Length;
						int end = url.IndexOf("&", begin);
						if (end == -1) end = url.Length;
						return (url.Substring(begin, end - begin));
					}
                }
            }
            return "";
        }

        /// <summary>
        /// URL�������Ƿ����ȫ����MenuUrl.QueryKey
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool UrlContainKeys(HttpContext HttpContext, string keys)
        {
            string[] queryStrings = HttpContext.Request.QueryString.AllKeys;
            if (keys == null || keys.Trim() == "")
                return true;

            string[] keyList = keys.Split('&');
            foreach (string key in keyList)
            {
                if (key.Trim() != null && key.Trim() != "")
                {
                    bool isFit = false;
                    string[] ks = key.Split('=');
                    for (int i = 0; i < queryStrings.Length; i++)
                    {
                        if (key.IndexOf('=') >= 0)
                        {
                            if (queryStrings[i] != null && ks[0].ToLower().Trim() == queryStrings[i].Trim().ToLower() &&
                                ks[1].ToLower().Trim() == HttpContext.Request.QueryString[queryStrings[i].Trim()].ToString().ToLower())
                            {
                                isFit = true;
                                break;
                            }
                        }
                        else
                        {
                            if (queryStrings[i] != null && key.ToLower().Trim() == queryStrings[i].Trim().ToLower() )
                            {
                                isFit = true;
                                break;
                            }
                        }
                    }
                    if (!isFit) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ��һ������������ת��Ϊһ������
        /// </summary>
        /// <param name="bytes">bytesΪ�ǿ�����,������׳��쳣</param>
        /// <returns></returns>
        public static object BytesToObject(byte[] bytes)
        {
            if (bytes==null || bytes.Length==0)
            {
                return null;
            }
            IFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            object obj = formatter.Deserialize(ms);

            return obj;
        }

        /// <summary>
        /// ��һ��objectת��Ϊ����������
        /// </summary>
        /// <param name="obj">�����л��Ķ���</param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            if (obj == null)
                return null;
            else
            {
                //ת������ΪByte[]
                MemoryStream ms = new MemoryStream();
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                byte[] data = ms.ToArray();
                ms.Close();
                return data;
            }
        }

        public static void SetDropdownList(DropDownList list, string value)
        {
            int i = 0;
            foreach (ListItem item in list.Items)
            {
                if (item.Value == value)
                {
                    list.SelectedIndex = i;
                    return;
                }
                i++;
            }
        }

        /// <summary>
        ///�ļ���С��ʽ��
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public static String FormatFileSize(Int64 fileSize)
        {
            if (fileSize < 0)
            {
                throw new ArgumentOutOfRangeException("fileSize");
            }
            else if (fileSize >= 1024 * 1024 * 1024)
            {
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            }
            else if (fileSize >= 1024 * 1024)
            {
                return string.Format("{0:####0.00} MB", ((Double)fileSize) / (1024 * 1024));
            }
            else if (fileSize >= 1024)
            {
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            }
            else
            {
                return string.Format("{0} bytes", fileSize);
            }
        }

        public static string  FormatTimeNote(DateTime longtime)
        {
            TimeSpan ts = DateTime.Now - longtime;
            if (ts >= new TimeSpan(1, 0, 0, 0) && ts.TotalDays<6 )
            {
                return String.Format("{0:N0}����ǰ", ts.TotalDays);
            }
            else if (ts >= new TimeSpan(1, 0, 0, 0) && ts.TotalDays>=6 )
            {
                return longtime.ToLongDateString();
            }
            else if (ts >= new TimeSpan(1, 0, 0))
            {
                return String.Format("{0:N0}Сʱ��ǰ", ts.TotalHours);
            }
            else if (ts >= new TimeSpan(0, 1, 0))
            {
                return String.Format("{0:N0}������ǰ", ts.TotalMinutes);
            }
            else
            {
                return "�ղ�";
            }
        }

        /// <summary>
        /// ���ƶ�����������Ե���һ������
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Object CopyObjectPropertiesTo(Object source, Object target)
        {
            PropertyInfo[] pis = target.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.CanWrite)
                    pi.SetValue(target, pi.GetValue(source, null), null);
            }

            return target;
        }

        public static Object ConvertToObject(Object source, Type objClass)
        {
            Object target = objClass.Assembly.CreateInstance(objClass.FullName);
            PropertyInfo[] pis = target.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.CanWrite)
                    pi.SetValue(target, pi.GetValue(source, null), null);
            }

            return target;
        }

        #region ����WebService�쳣��׽�봦��
        /// <summary>
        /// �쳣����
        /// </summary>
        public enum FaultCode
        {
            Client = 0,
            Server = 1
        }

        /// <summary>
        /// ��װ�쳣ΪSoapException
        /// </summary>
        /// <param name="errorFunction">�����쳣�ķ�������</param>
        /// <param name="errorMessage">������Ϣ</param>
        /// <param name="errorNumber">�����</param>
        /// <param name="errorSource">����Դ</param>
        /// <param name="code">�쳣����</param>
        /// <returns>��װ���SoapException</returns>
        public static SoapException RaiseException(
                                                string errorFunction,
                                                string errorMessage,
                                                string errorNumber,
                                                string errorSource,
                                                FaultCode code
                                            )
        {
            //��ʼ���޶���
            XmlQualifiedName faultCodeLocation = null;

            //�쳣���ʹ���ת��
            switch (code)
            {
                case FaultCode.Client:
                    faultCodeLocation = SoapException.ClientFaultCode;
                    break;
                case FaultCode.Server:
                    faultCodeLocation = SoapException.ServerFaultCode;
                    break;
            }

            //�����쳣��Ϣ�ṹ��
            string strXmlOut = @"<detail>"
                             + "<Error>"
                             + "<ErrorNumber>" + errorNumber + "</ErrorNumber>"
                             + "<ErrorMessage>" + errorMessage + "</ErrorMessage>"
                             + "<ErrorSource>" + errorSource + "</ErrorSource>"
                             + "</Error>"
                             + "</detail>";

            //װ��ΪXml�ĵ�
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXmlOut);

            //ʵ����SoapException
            SoapException soapEx = new SoapException(errorMessage, faultCodeLocation, errorFunction, xmlDoc.DocumentElement);

            //����SoapException
            return soapEx;
        }

        /// <summary>
        /// ��ȡSoaException������Ϣ
        /// �˷��������ڵ���WebService�Ĵ�����
        /// </summary>
        /// <param name="soapEx"></param>
        /// <returns>����ǰ������Ϣ</returns>
        public static string SoapExceptionInfo(SoapException soapEx)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(soapEx.Detail.OuterXml);
            //XmlNode categoryNode = doc.DocumentElement.SelectSingleNode("Error");

            ////string ErrorNumber = categoryNode.SelectSingleNode("ErrorNumber").InnerText;
            //string errorMessage = categoryNode.SelectSingleNode("ErrorMessage").InnerText;
            //string errorSource = categoryNode.SelectSingleNode("ErrorSource").InnerText;

            string errorMessage = soapEx.Message;
            string errorSource = soapEx.Source;

            return errorMessage + "\r\n" + errorSource;
        }

        /// <summary>
        /// ��ȡSoaException������Ϣ
        /// �˷��������ڵ���WebService�Ĵ�����
        /// </summary>
        /// <param name="soapEx"></param>
        /// <param name="key">��Ϣ���͹ؼ��֣�message��source��code��һ</param>
        /// <returns>����ǰ������Ϣ</returns>
        public static string SoapExceptionInfo(SoapException soapEx, string key)
        {
            string result = "Զ��δ������Ϣ��";

            XmlDocument doc = new XmlDocument();
            string errorNumber = "";
            string errorMessage = "";
            string errorSource = "";
            if (soapEx.Detail.OuterXml != null && soapEx.Detail.OuterXml != "")
            {
                doc.LoadXml(soapEx.Detail.OuterXml);
                XmlNode categoryNode = doc.DocumentElement.SelectSingleNode("Error");
                try
                {
                    errorNumber = categoryNode.SelectSingleNode("ErrorNumber").InnerText;
                }
                catch (Exception)
                {
                }

                try
                {
                    errorMessage = categoryNode.SelectSingleNode("ErrorMessage").InnerText;
                }
                catch (Exception)
                {
                }

                try
                {
                    errorSource = categoryNode.SelectSingleNode("ErrorSource").InnerText;
                }
                catch (Exception)
                {
                }
            }

            switch (key)
            {
                case "message":
                    result = errorMessage;
                    break;
                case "source":
                    result = errorSource;
                    break;
                case "code":
                    result = errorNumber;
                    break;
                default:
                    result= errorMessage + "\r\n" + errorSource;
                    break;
            }

            return result;
        }
       #endregion
    }
}
