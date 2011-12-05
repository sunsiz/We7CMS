#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using System.Web.UI;
using OpenPOP.POP3;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Net.Mail;
using System.Text;
using System.Net;
using We7.Framework.Config;


namespace We7.Framework.Util
{
	/// <summary>
	/// �ʼ�����������
	/// </summary>
	public class MailHelper
    {
        #region ����

        public MailHelper() { }

        private string _adminEmail;//"master@duanke.com";
        public string AdminEmail
        {
            get { return _adminEmail; }
            set { _adminEmail = value; }
        }

        private string _smtpServer;//"mail.duanke.com";
        public string SmtpServer
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        private string _popServer;//"mail.duanke.com";
        /// <summary>
        /// ���ʼ�����
        /// </summary>
        public string PopServer
        {
            get { return _popServer; }
            set { _popServer = value; }
        }

        private string _password;// "master@duanke.com888";
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _userName;// "master@duanke.com";
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        #endregion

        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Send(string to, string from, string subject, string message, string priority)
        {
            //string SmtpServer = "mail.duanke.cn";
            //string AdminEmail = "webmaster@duanke.cn";
            //string Password = "webmaster8888";
            //string UserName = "webmaster@duanke.cn";

            ////��һ�෽����MailMessage
            //System.Web.Mail .MailMessage mailMessage = new System.Web.Mail.MailMessage();
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(from);//�����˵�ַ
            mailMessage.To.Add(to);//�����˵�ַ
            mailMessage.Subject = subject.Trim().Replace("\r\n"," ").Replace("<br/>"," ");

            mailMessage.SubjectEncoding = Encoding.UTF8;            
            mailMessage.Body = message.Replace("\r\n", "<br/>");
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;             
            switch (priority)       //�ʼ����ȼ�
            {
                case "High":
                    mailMessage.Priority = System.Net.Mail.MailPriority.High;
                    break;
                case "Low":
                    mailMessage.Priority = System.Net.Mail.MailPriority.Low;
                    break;
                case "Normal":
                    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                    break;
                default:
                    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                    break;
            }            
            SmtpClient smtp = new SmtpClient(); // �ṩ�����֤���û��������� // �����ʼ��û�����Ϊ��username password // Gmail �û�����Ϊ��username@gmail.com password 

            smtp.Credentials = new NetworkCredential(UserName, Password);
            smtp.Port = 25; // Gmail ʹ�� 465 �� 587 �˿� 
            smtp.Host = SmtpServer; // �� smtp.163.com, smtp.gmail.com 
            smtp.EnableSsl = false; // ���ʹ��GMail������Ҫ����Ϊtrue
            //smtp.SendCompleted += new SendCompletedEventHandler(SendMailCompleted);
            try
            {
                smtp.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                SendMailMessageToXml(mailMessage);
                throw new Exception("�ʼ�����ʧ�ܣ����¼�����̨����ʼ������Ƿ���ȷ��ԭ��"+ex.Message);
            }            

            //�ڶ��෽����OpenSMTP
            //string smtpHost = SmtpServer; //"smtp.163.com"; 
            //int smtpPort = 25;
            //string senderEmail = AdminEmail; //"thehim@163.com";
            //Smtp smtp = new Smtp(smtpHost, smtpPort);
            //smtp.Password = Password;//"mypass";//�û����� 
            //smtp.Username = UserName; //"thehim"; //�û�����

            ////�����ʼ���Ϣ==========================================================
            //OpenSmtp.Mail.MailMessage msg = new OpenSmtp.Mail.MailMessage();//(senderEmail, recipientEmail);
            //OpenSmtp.Mail.EmailAddress addfrom = new EmailAddress(senderEmail); //������
            //addfrom.Name = "�̿���";
            //msg.From = addfrom;

            //OpenSmtp.Mail.EmailAddress addbcc = new EmailAddress(to);
            //msg.AddRecipient(addbcc, AddressType.To);

            //msg.Subject = subject;
            //msg.Charset = "gb2312";
            //msg.Body = message;

            //smtp.SendMail(msg);

            ////�����෽����Mailserder Using
            //MailSender ms = new MailSender();
            //ms.From = AdminEmail;
            //ms.To = to;
            //ms.Subject = subject;
            //ms.Body = message;
            //ms.UserName = UserName;  
            //ms.Password = Password; 
            //ms.Server = SmtpServer;

            ////ms.Attachments.Add(new MailSender.AttachmentInfo(@"D:\\test.txt"));
            //ms.SendMail();
            return true;
        }

        /// <summary>
        /// �����ʼ�������������ȷ�����ʼ�
        /// </summary>
        public MailResult ReceiveMail(string asmName, string typeName, string methodName ,bool delete)
        {
            MailResult result = new MailResult();
            string strPort = "";
            if (strPort == "" || strPort == string.Empty) strPort = "110";
            POPClient popClient = new POPClient();
            try
            {
                popClient.Connect(PopServer, Convert.ToInt32(strPort));
                popClient.Authenticate(UserName, Password);
                int count = popClient.GetMessageCount();
                                
                int resultCount = 0;
                for (int i = count; i >= 1; i--)
                {
                    OpenPOP.MIMEParser.Message msg = popClient.GetMessage(i, false);
                    if (msg != null)
                    {
                        resultCount++;
                        //��ȡDLL����·��:
                        string dllPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                        dllPath = Path.GetDirectoryName(dllPath);
                        //������Ҫִ��������������ȡ��ʵ����
                        string asmNames = asmName;//�������ƣ�*.dll��
                        string dllFile = Path.Combine(dllPath, asmNames);
                        Assembly asm = Assembly.LoadFrom(dllFile);
                        //��ȡ�෽����ִ�У�
                        object obj = asm.CreateInstance(typeName, false);
                        Type type = obj.GetType();//����
                        MethodInfo method = type.GetMethod(methodName);//��������
                        //�����Ҫ������������
                        object[] args = new object[] { (object)msg ,(object)result};
                        //ִ�в����÷���
                        method.Invoke(obj, args);
                        if (delete)
                        {
                            popClient.DeleteMessage(i); //�ʼ�����ɹ���ɾ������������
                        }
                    }
                }
                result.Count = resultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }  
            finally
            {
                popClient.Disconnect();
            }
            return result;
        }

        /// <summary>
        /// �����ʼ�������������ȷ�����ʼ�
        /// </summary>
        public MailResult ReceiveMail(string asmName, string typeName, string methodName, bool delete,string stateText)
        {
            MailResult result = new MailResult();
            result.StateText = stateText;
            string strPort = "";
            if (strPort == "" || strPort == string.Empty) strPort = "110";
            POPClient popClient = new POPClient();
            try
            {
                popClient.Connect(PopServer, Convert.ToInt32(strPort));
                popClient.Authenticate(UserName, Password);
                int count = popClient.GetMessageCount();

                int resultCount = 0;
                for (int i = count; i >= 1; i--)
                {
                    OpenPOP.MIMEParser.Message msg = popClient.GetMessage(i, false);
                    if (msg != null)
                    {
                        resultCount++;
                        
                        //��ȡDLL����·��:
                        string dllPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                        dllPath = Path.GetDirectoryName(dllPath);
                        //������Ҫִ��������������ȡ��ʵ����
                        string asmNames = asmName;//�������ƣ�*.dll��
                        string dllFile = Path.Combine(dllPath, asmNames);
                        Assembly asm = Assembly.LoadFrom(dllFile);
                        //��ȡ�෽����ִ�У�
                        object obj = asm.CreateInstance(typeName, false);
                        Type type = obj.GetType();//����
                        MethodInfo method = type.GetMethod(methodName);//��������
                        //�����Ҫ������������
                        object[] args = new object[] { (object)msg, (object)result };
                        //ִ�в����÷���
                        method.Invoke(obj, args);
                        if (delete)
                        {
                            popClient.DeleteMessage(i); //�ʼ�����ɹ���ɾ������������
                        }
                    }
                }
                result.Count = resultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                popClient.Disconnect();
            }
            return result;
        }

        /// <summary>
        /// δ����ȷ���͵��ʼ�����XML��ʽת����/_Data/SendEmail/Ŀ¼��
        /// </summary>
        /// <param name="mailMessage"></param>
        public void SendMailMessageToXml(MailMessage mailMessage)
        {
            try
            {
                string subject = mailMessage.Subject.ToString();//�ʼ�����
                string body = (string)mailMessage.Body;//�ʼ�����
                string replyTime = DateTime.Now.ToString();//�ʼ�
                string user = mailMessage.To[0].Address;//�ռ��˵�ַ
                string formUser = mailMessage.From.Address;//�����˵�ַ

                if (subject != "")
                {
                    string filePath = HttpContext.Current.Server.MapPath("/_Data/SendEmail/");
                    DateTime time = Convert.ToDateTime(replyTime);
                    string fileName = subject + DateTime.Now.ToString(".yyyy_MM_dd_HH_mm_ss") + ".xml";
                    string path = Path.Combine(filePath, fileName);

                    //����Ƿ�XML�ļ������ʱ·�����ڣ��������������д���
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    //���XMLSchema�ļ��Ƿ���ڣ��������������д���
                    if (!File.Exists(subject))
                    {
                        XmlDocument doc = new XmlDocument();
                        //ת���ַ�
                        subject = We7Helper.Base64Encode(subject);
                        user = We7Helper.Base64Encode(user);
                        body = We7Helper.Base64Encode(body);
                        formUser = We7Helper.Base64Encode(formUser);

                        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n"
                            + "<root><infoSubject>" + subject + "</infoSubject><infoUser>" +user + "</infoUser><infoFormUser>" +
                            formUser + "</infoFormUser><infoBody>" + body + "</infoBody><infoTime>"
                            + replyTime + "</infoTime></root>";
                        doc.LoadXml(xml);
                        doc.Save(path);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

	}

    public class MailResult
    {
        int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        int success;

        public int Success
        {
            get { return success; }
            set { success = value; }
        }

        string stateText;

        public string StateText
        {
            get { return stateText; }
            set { stateText = value; }
        }

         

    }

    /// <summary>
    /// �ʼ���Ϣģ����
    /// </summary>
    public class MailMessageTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="itemName"></param>
        public MailMessageTemplate(string templateFile, string itemName)
        {
            string configFilePath = HttpContext.Current.Server.MapPath("~/config/"+templateFile);
            if (File.Exists(configFilePath))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(configFilePath);
                XmlNode root = xml.SelectSingleNode("configuration");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "item"
                        && n.Attributes["name"].Value == itemName )
                    {
                        foreach (XmlNode m in n.ChildNodes)
                        {
                            if (m.NodeType != XmlNodeType.Comment && m.Name == "EmailTitle")
                                Subject = m.InnerXml;
                            if (m.NodeType != XmlNodeType.Comment && m.Name == "Emailcontent")
                                Body = m.InnerXml;
                        }
                    }
                }

                FormatTemplateValue();
            }
        }

        void FormatTemplateValue()
        {
            if (!string.IsNullOrEmpty(Subject))
            {
                Subject = Subject.Replace("${SiteName}",SiteConfigs.GetConfig().SiteName);
            }
            
            if (!string.IsNullOrEmpty(Body))
            {
                Body = Body.Replace("${SiteName}", SiteConfigs.GetConfig().SiteName);
                Body = Body.Replace("${SiteUrl}", SiteConfigs.GetConfig().RootUrl);
                Body = Body.Replace("${DateTime.Now}",DateTime.Now.ToString());
            }

        }
    }
}

