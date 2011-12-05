using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

using We7.CMS.Config;
using We7.Framework.Config;


namespace We7.CMS.Install
{
    /// <summary>
    /// SetupPage ��ժҪ˵����
    /// </summary>
    public class SetupPage : System.Web.UI.Page
    {

        private static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        public static string producename = "";  //��ǰ��Ʒ�汾����

        public static string footer = "";

        public static string logo = "<img src=\"images/logo.jpg\" width=\"180\" height=\"300\">"; //��װ��LOGO

        public static string header = ""; //htmlҳ�ĵ�<head>����

        public static string SelectDB = "";

        public static void Init()
        {
            header = "<HEAD><title>��װ " + GetAssemblyProductName() + "</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n";
            header += "<LINK rev=\"stylesheet\" media=\"all\" href=\"css/general.css\" type=\"text/css\" rel=\"stylesheet\">\r\n";
            header += "<script language=\"javascript\" src=\"js/setup.js\"></script>\r\n";
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si != null)
            {
                string copyright = si.CopyrightOfWe7;
                if (si.IsOEM) copyright = si.Copyright;
                footer = string.Format("<div class='pubfooter'><p>{0}</p></div>", copyright);
            }
            else
            {
                footer = "<div class='pubfooter'><p>Powered by <a  href=\"http://we7.cn\" target=\"_blank\">" + GetAssemblyProductName() + "</a>";
                footer += " &nbsp; &copy;" + GetAssemblyCopyright().Split(',')[0] + " <a  href=\"http://www.westengine.com\" target=\"_blank\">WestEngine Inc.</a></p></div>";
            }

            producename = GetAssemblyProductName();
        }


        //���û������ťʱ��������Ϊ��Ч
        public void DisableSubmitBotton(Page mypage, System.Web.UI.WebControls.Button submitbutton)
        {
            RegisterAdminPageClientScriptBlock();

            //��֤ __doPostBack(eventTarget, eventArgument) ��ȷע��
            mypage.ClientScript.GetPostBackEventReference(submitbutton, "");

            StringBuilder sb = new StringBuilder();

            //��֤��֤������ִ��
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { if (Page_ClientValidate() == false) { return false; }}");

            // disable����submit��ť
            sb.Append("disableOtherSubmit();");

            //sb.Append("document.getElementById('Layer5').innerHTML ='�������в���</td></tr></table><BR />';");
            sb.Append("document.getElementById('success').style.display ='block';");

            sb.Append(this.ClientScript.GetPostBackEventReference(submitbutton, ""));
            sb.Append(";");
            submitbutton.Attributes.Add("onclick", sb.ToString());
        }


        public void RegisterAdminPageClientScriptBlock()
        {

            string script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n" +
                          "	<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n" +
                          "		<div id=\"Layer4\" style=\"height:26px;background:#333333;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;color:#fff \">������ʾ</div>\r\n" +
                          "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\"><br />����ִ�в���,���Ե�...</div>\r\n" +
                          "	</div>\r\n" +
                          "	<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #cccccc;\"></div>\r\n" +
                          "</div>\r\n" +
                          "<script> \r\n" +
                          "document.getElementById('success').style.display ='none'; \r\n" +
                          "</script> \r\n" ;


            base.ClientScript.RegisterClientScriptBlock(this.GetType(), "Page", script);
        }

        public new void RegisterStartupScript(string key, string scriptstr)
        {

            string message = "<BR />�����ɹ�!";

            if (key == "PAGE")
            {
                string script = "";

                script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n" +
                       "	<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n" +
                       "		<div id=\"Layer4\" style=\"height:26px;background:#333;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;color:#fff \">������ʾ</div>\r\n" +
                       "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\">" + message + "</div>\r\n" +
                       "	</div>\r\n" +
                       "	<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #cccccc;\"></div>\r\n" +
                       "</div>\r\n" +
                       "<script> \r\n" +
                       "var bar=0;\r\n" +
                       "document.getElementById('success').style.display = \"block\"; \r\n" +
                       "count() ; \r\n" +
                       "function count(){ \r\n" +
                       "bar=bar+4; \r\n" +
                       "if (bar<99) \r\n" +
                       "{setTimeout(\"count()\",100);} \r\n" +
                       "else { \r\n" +
                       "	document.getElementById('success').style.display = \"none\"; \r\n" +
                       scriptstr + "} \r\n" +
                       "} \r\n" +
                       "</script> \r\n"+
                       "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";

                Response.Write(script);
            }
            else
            {
                base.ClientScript.RegisterStartupScript(this.GetType(), key, scriptstr);
            }
        }

        public static string InitialSystemValidCheck(ref bool error)
        {
            error = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellSpacing='0' cellPadding='0' width='90%' align='center' border='0' bgcolor='#666666' style='font-size:12px'>");

            HttpContext context = HttpContext.Current;

            string filename = null;
            if (context != null)
                filename = context.Server.MapPath("/config/db.config");
            else
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/db.config");

            //ϵͳBINĿ¼���
            sb.Append(IISSystemBINCheck(ref error));


            //���db.config�ļ�����Ч��
            if (!SystemConfigCheck())
            {
                sb.Append("<tr style=\"height:15px\"><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'> db.config ����д��û�з�����ȷ, ������������װ�ĵ�!</td></tr>");
                error = true;
            }
            else
            {
                sb.Append("<tr style=\"height:15px\"><td bgcolor='#ffffff' width='5%'><img src='images/ok.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%' style='color:#080'>�� db.config ��֤ͨ��!</td></tr>");
            }

            if (!SystemConfigCheck())
            {
                sb.Append("����ϵͳ�����ļ�db.configû��д��Ȩ��!<br />");
            }

            //�Զ���������Ŀ¼_data,_skins
            checkDataFilePath();
            //���ϵͳĿ¼����Ч��
            string folderstr = "admin,config,app_data,_data,_skins";
            foreach (string foldler in folderstr.Split(','))
            {
                if (!SystemFolderCheck(foldler))
                {
                    sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>�� " + foldler + " Ŀ¼û��д���ɾ��Ȩ��!</td></tr>");
                    error = true;
                }
                else
                {
                    sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/ok.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'  style='color:#080'>�� " + foldler + " Ŀ¼Ȩ����֤ͨ��!</td></tr>");
                }
            }
            string filestr = "install\\systemfile.aspx";
            foreach (string file in filestr.Split(','))
            {
                if (!SystemFileCheck(file))
                {
                    sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>�� " + file.Substring(0,file.LastIndexOf('\\')) + " Ŀ¼û��д���ɾ��Ȩ��!</td></tr>");
                    error = true;
                }
                else
                {
                    sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/ok.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%' style='color:#080'>�� " + file.Substring(0, file.LastIndexOf('\\')) + " Ŀ¼Ȩ����֤ͨ��!</td></tr>");
                }
            }

           if(!TempTest())
           {
               sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>��û�ж� " + Path.GetTempPath() + " �ļ��з���Ȩ�ޣ�����μ���װ�ĵ�.</td></tr>");
               error = true;
           }
           else
           {
            if (!SerialiazeTest())
            {
                sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>��config�ļ������л�ʧ�ܣ��뱣֤config�µ��ļ����п�дȨ�޼���ʽ��ȷ��<br></td></tr>");
                error = true;
            }
            else
            {
                sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/ok.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'  style='color:#080'>�����л���֤ͨ����</td></tr>");
            } 
           }
            sb.Append("</table>");
         
            return sb.ToString();
        }

        /// <summary>
        /// ��������ļ��У����������Զ�����
        /// </summary>
        private static void checkDataFilePath()
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = null;

            if (context != null)
                physicsPath = context.Server.MapPath("/");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;

            string mypath = Path.Combine(physicsPath, "_data");
            if (!Directory.Exists(mypath))
                Directory.CreateDirectory(mypath);

            mypath = Path.Combine(physicsPath, "_skins");
            if (!Directory.Exists(mypath))
                Directory.CreateDirectory(mypath);
        }

        public static bool SystemConfigCheck()
        {
            HttpContext context = HttpContext.Current;

            string physicsPath = null;

            if (context != null)
                physicsPath = context.Server.MapPath("/config");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }

                System.IO.File.Delete(physicsPath + "\\a.txt");

                return true;
            }
            catch
            {
                return false;
            }

        }

        #region ��װ���̱��
        public static bool LockFileExist()
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = null;
            if (context != null)
                physicsPath = context.Server.MapPath("/config");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;
            return File.Exists(physicsPath + "\\db-is-creating.lock");
        }

        public static void DeleteLockFile()
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = null;
            if (context != null)
                physicsPath = context.Server.MapPath("/config");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;
            System.IO.File.Delete(physicsPath + "\\db-is-creating.lock");
        }

        public static void CreateLockFile()
        {
            HttpContext context = HttpContext.Current;

            string physicsPath = null;

            if (context != null)
                physicsPath = context.Server.MapPath("/config");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;

            using (FileStream fs = new FileStream(physicsPath + "\\db-is-creating.lock", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Close();
            }
        }


        #endregion

        public static string IISSystemBINCheck(ref bool error)
        {
            string binfolderpath = HttpRuntime.BinDirectory;

            string result = "";
            try
            {
                string[] assemblylist = new string[] { "We7.CMS.Common.dll", "We7.CMS.Config.dll", 
                    "We7.CMS.Utils.dll", "We7.CMS.Web.Admin.dll", "We7.CMS.Web.dll", "We7.Framework.dll"};
                bool isAssemblyInexistence = false;
                ArrayList inexistenceAssemblyList = new ArrayList();
                foreach (string assembly in assemblylist)
                {
                    if (!File.Exists(binfolderpath + assembly))
                    {
                        isAssemblyInexistence = true;
                        error = true;
                        inexistenceAssemblyList.Add(assembly);
                    }
                }
                if (isAssemblyInexistence)
                {
                    foreach (string assembly in inexistenceAssemblyList)
                    {
                            result += "<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>" + assembly + " �ļ����ò���ȷ<br/>�뽫���е�dll�ļ����Ƶ�Ŀ¼ " + binfolderpath + " ��.</td></tr>";
                    }
                }
            }
            catch
            {
                result += "<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>�뽫���е�dll�ļ����Ƶ�Ŀ¼ " + binfolderpath + " ��.</td></tr>";
                error = true;
            }

            return result;
        }

        public static bool SystemFolderCheck(string foldername)
        {

            string physicsPath = HttpContext.Current.Server.MapPath(@"..\" + foldername);
            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
                if (File.Exists(physicsPath + "\\a.txt"))
                {
                    System.IO.File.Delete(physicsPath + "\\a.txt");
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

        public static bool SystemFileCheck(string filename)
        {
            filename = HttpContext.Current.Server.MapPath(@"..\" + filename);
            try
            {
                if (filename.IndexOf("systemfile.aspx") == -1 && !File.Exists(filename))
                    return false;
                if (filename.IndexOf("systemfile.aspx") != -1)  //��ɾ������
                {
                    File.Delete(filename);
                    return true;
                }
                StreamReader sr = new StreamReader(filename);
                string content = sr.ReadToEnd();
                sr.Close();
                content += " ";
                StreamWriter sw = new StreamWriter(filename, false);
                sw.Write(content);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool SerialiazeTest()
        {
            try
            {
                string configPath = HttpContext.Current.Server.MapPath("/config/general.config");
                GeneralConfigInfo __configinfo=new GeneralConfigInfo();
                if(!File.Exists(configPath))
                    GeneralConfigs.Serialiaze(__configinfo, configPath);
                __configinfo = GeneralConfigs.Deserialize(configPath);
                //__configinfo.IcpInfo ="";
                GeneralConfigs.Serialiaze(__configinfo, configPath);
                return true;
            }
            catch
            {
                return false;
            }
        }


        private static bool TempTest()
        {
            string UserGuid = Guid.NewGuid().ToString();
            string TempPath = Path.GetTempPath();
            string path = TempPath + UserGuid;
            try
            {

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(DateTime.Now);
                }

                using (StreamReader sr = new StreamReader(path))
                {
                    sr.ReadLine();
                    return true;
                }


            }
            catch
            {
                return false;

            }

        }

        protected string ProductBrand
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null)
                {
                    return si.ProductName;
                }
                else
                    return "We7";
            }
        }

        /// <summary>
        /// ���Assembly�汾��
        /// </summary>
        /// <returns></returns>
        public string GetAssemblyVersion()
        {
            return string.Format("{0}.{1}.{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);
        }

        /// <summary>
        /// ���Assembly��Ʒ����
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyProductName()
        {
             GeneralConfigInfo si = GeneralConfigs.GetConfig();
             if (si != null)
             {
                 return si.ProductName;
             }
             else
                 return AssemblyFileVersion.ProductName;
        }

        /// <summary>
        /// ���Assembly��Ʒ��Ȩ
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCopyright()
        {

            return AssemblyFileVersion.LegalCopyright;
        }
    }
}