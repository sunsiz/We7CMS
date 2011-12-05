using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.IO;
using We7.CMS.Common;
using We7.Framework.Zip;

namespace We7.CMS.Web.Admin
{
    public partial class DataControlDownload :BasePage
    {

        string FileName
        {
            get { 
                return "package.DataControl"+System.DateTime.Now.ToShortDateString().Replace("/","_");
            }
        }


        DataControl[] DataControls;

        protected override void Initialize()
        {

            DataControls = TemplateHelper.GetDataControls(null);
            CreateZip(FileName);
        }

        protected void CreateZip(string FileName)
        {
            //Ŀ���ļ���
            string CopyToPath = Server.MapPath("~/_Temp");
            //��Ŀ���ļ��� �����ļ���.Files�����ļ���
            Directory.CreateDirectory(CopyToPath + "/" + FileName + ".Files/controls");
            Directory.CreateDirectory(CopyToPath + "/" + FileName + ".Files/res");

            string ascxShotName = "";
            for (int i = 0; i < DataControls.Length; i++)
            {
                ascxShotName = DataControls[i].FileName.Replace(".xml", "");

                // ascx�ļ�
                string AscxFile = Server.MapPath("~/" + Constants.ControlBasePath.Replace("\\", "/")) + "/" + ascxShotName;
                //xml�ļ� 
                string AscxXmlFile = Server.MapPath("~/" + Constants.ControlBasePath.Replace("\\", "/")) + "/" + DataControls[i].FileName;
                string ascxCsFile = Server.MapPath("~/" + Constants.ControlBasePath.Replace("\\", "/")) + "/" + ascxShotName + ".cs";
                string designerFile = Server.MapPath("~/" + Constants.ControlBasePath.Replace("\\", "/")) + "/" + ascxShotName + ".designer.cs";

                //������Ŀ���ļ���
                if (File.Exists(AscxFile))
                {
                    try
                    {
                        File.Copy(AscxFile, CopyToPath + "/" + FileName + ".Files/controls/" + ascxShotName, true);
                    }
                    catch { }

                }
                if (File.Exists(AscxXmlFile))
                {
                    try   //ԭxml��ʽ����
                    {
                        File.Copy(AscxXmlFile, CopyToPath + "/" + FileName + ".Files/controls/" + DataControls[i].FileName, true);
                    }
                    catch { }
                }
                if (File.Exists(ascxCsFile))
                {
                    try
                    {
                        File.Copy(ascxCsFile, CopyToPath + "/" + FileName + ".Files/controls/" + ascxShotName + ".cs", true);
                    }
                    catch { }
                }
                if (File.Exists(designerFile))
                {
                    try
                    {
                        File.Copy(designerFile, CopyToPath + "/" + FileName + ".Files/controls/" + ascxShotName + ".designer.cs", true);
                    }
                    catch { }
                }
            }

            string[] FileProperties = new string[2];
            FileProperties[0] = CopyToPath + "/" + FileName + ".Files";//ѹ��Ŀ¼
            FileProperties[1] = Server.MapPath(Constants.TempBasePath) + "/" + FileName + ".zip";//ѹ�����Ŀ¼
            //ѹ���ļ�
            try
            {
                ZipClass.ZipFileMain(FileProperties);
                DonwloadHyperLink.NavigateUrl = Constants.TempBasePath + "/" + FileName + ".zip";
            }
            catch
            {

            }
        }
    }
}
