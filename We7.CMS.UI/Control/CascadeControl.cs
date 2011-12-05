using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;

namespace We7.CMS.Controls
{
	/// <summary>
	/// CascadeControl ��ժҪ˵����
	/// ʡ��������ѡ��ؼ�
	/// </summary>

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:CascadeControl runat=server></{0}:CascadeControl>")]
	public class CascadeControl :  WebControl, IPostBackDataHandler
	{
		#region ����

        [Bindable(true), Category("����"), DefaultValue(""), Description("��ȡѡ���ı�")]
        public string Text 
		{
			get
			{
                return this.TextProvince + this.TextCity + this.TextCounty;
			}
		}
        [Bindable(true), Category("����"), DefaultValue(""), Description("��ȡ������ʡ")]
        public string TextProvince 
		{
			get
			{
                if (ViewState["TextProvince"] != null)
				{
                    return ViewState["TextProvince"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState["TextProvince"] = value;
			}
		}

        [Bindable(true), Category("����"), DefaultValue(""), Description("��ȡ��������")]
        public string TextCity 
		{
			get
			{
                if (ViewState["TextCity"] != null)
				{
                    return ViewState["TextCity"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState["TextCity"] = value;
			}
		}

        [Bindable(true), Category("����"), DefaultValue(""), Description("��ȡ��������")]
        public string TextCounty 
		{
			get
			{
                if (ViewState["TextCounty"] != null)
				{
                    return ViewState["TextCounty"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState["TextCounty"] = value;
			}
		}		
        #endregion

		//ʡ��
		private ArrayList parr = new ArrayList();
		//����
		private ArrayList carr = new ArrayList();
		//��
		private ArrayList darr = new ArrayList();
		
		/// <summary>
		/// ���˿ؼ����ָ�ָ�����������
		/// </summary>
		/// <param name="writer"> Ҫд������ HTML ��д��</param>
		protected override void Render(HtmlTextWriter output)
		{
			GetCascadingInfo();

			#region	ʡ������ѡ�����javascript����

            /*****************ʡ �� �� ����*********/
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(typeof(string),"CascadeControl_clientScript"))
			{
				output.WriteLine("<script language=\"JavaScript\">");
				output.WriteLine("var groups="+(parr.Count+1)+";");
				output.WriteLine("var group=new Array(groups);");
				output.WriteLine("for (i=0; i<groups; i++)");
				output.WriteLine("group[i]=new Array();");
				output.WriteLine("group[0][0]=new Option(\"��ѡ��\",\"\");");

                for (int i = 0; i < carr.Count; i++)
                {
                    output.WriteLine("group[" + (i + 1) + "][0]=new Option(\"��ѡ��\",\"" + parr[i].ToString() + "\");");
                    ArrayList arr = (ArrayList)carr[i];
                    for (int j = 0; j < arr.Count; j++)
                    {
                        output.WriteLine("group[" + (i + 1) + "][" + (j + 1) + "]=new Array();");
                        output.WriteLine("group[" + (i + 1) + "][" + (j + 1) + "][0]=new Option(\"" + arr[j].ToString() + "\",\"" + arr[j].ToString() + "\");");
                        ArrayList drr = (ArrayList)((ArrayList)darr[i])[j];
                        for (int m = 0; m < drr.Count; m++)
                        {
                            output.WriteLine("group[" + (i + 1) + "][" + (j + 1) + "][" + (m + 1) + "]=new Option(\"" + drr[m].ToString() + "\",\"" + drr[m].ToString() + "\");");
                        }
                    }
                }
                /********************************************************************************/


				output.WriteLine("function redirectff(x,obj,objc){");						  
				output.WriteLine("for (m=obj.options.length-1;m>0;m--)");
				output.WriteLine("obj.options[m]=null;");
				output.WriteLine("for (i=1;i<group[x].length;i++){");
				output.WriteLine("obj.options[i]=new Option(group[x][i][0].text,group[x][i][0].value);");
				output.WriteLine("obj.options[0].selected=true;");
				output.WriteLine("}}");
                output.WriteLine("function redirectffcounty(x,y,obj){");						  
				output.WriteLine("for (m=obj.options.length-1;m>0;m--)");
				output.WriteLine("obj.options[m]=null;");
				output.WriteLine("for (i=1;i<group[x][y].length;i++){");
				output.WriteLine("obj.options[i]=new Option(group[x][y][i].text,group[x][y][i].value);");
				output.WriteLine("obj.options[0].selected=true;");
				output.WriteLine("}}");
				output.WriteLine("</script>");
				this.Page.ClientScript.RegisterClientScriptBlock(typeof(string),"CascadeControl_clientScript", "",true);
			}

			#endregion

			#region дselect����

			//��ʼ��
            output.Write("<table style=\"border: solid 0px #fff;width:100%\"><tr><td>");
            output.Write("<select id=\"" + this.UniqueID + "_p\" name=\"" + this.UniqueID + "_p\" onChange=\"redirectff(this.options.selectedIndex,document.all." + this.UniqueID + "_c,document.all." + this.UniqueID + ")\" ");
			//select����������(��ν�WebControl�����Ժܷ����ȫ��Ӧ�õ��ؼ�����)
			if (this.CssClass != "")
			{
				output.Write(" class=\""+this.CssClass+"\"");
			}
			output.Write(">");
			//option
			output.Write("<option>��ѡ��</option>");
			//�ҳ���ʼѡ����ʡ
			int pindex = 0;
			for (int i=0;i<parr.Count;i++)
			{
				if(this.TextProvince != null)
				{
					//����ѡ��ֵ����Ĭ����
                    if (this.TextProvince.IndexOf(parr[i].ToString()) > -1)
					{
						output.Write("<option selected>"+parr[i].ToString()+"</option>");
						pindex = i+1;
					}
					else
					{
						output.Write("<option>"+parr[i].ToString()+"</option>");
					}
				}
				else
				{
					output.Write("<option>"+parr[i].ToString()+"</option>");
				}
				
			}
			output.Write("</select>");
			output.Write("ʡ");
            output.Write("<select id=\"" + this.UniqueID + "_c\" name=\"" + this.UniqueID + "_c\" onChange=\"redirectffcounty(document.all." + this.UniqueID + "_p.options.selectedIndex,this.options.selectedIndex,document.all." + this.UniqueID + ")\" ");
			if (this.CssClass != "")
			{
				output.Write(" class=\""+this.CssClass+"\"");
			}
			output.Write(">");
			output.Write("<option value=\"\">��ѡ��</option>");
            int cindex = 0;
			//���ݳ�ʼ��ѡ����ʡ��ѡ����
			if (pindex > 0)
			{
				ArrayList arr = (ArrayList)carr[pindex-1];
				for(int i=0;i<arr.Count;i++)
				{
                    if (this.TextCity != null)
                    {
                        //����ѡ��ֵ����Ĭ����
                        if (this.TextCity.IndexOf(arr[i].ToString()) > -1)
                        {
                            output.Write("<option selected>" + arr[i].ToString() + "</option>");
                            cindex = i + 1;
                        }
                        else
                        {
                            output.Write("<option>" + arr[i].ToString() + "</option>");
                        }
                    }
                    else
                    {
                        output.Write("<option>" + arr[i].ToString() + "</option>");
                    }
                }
			}
			output.Write("</select>");
			output.Write("��");
            output.Write("<select id=\"" + this.UniqueID + "\" name=\"" + this.UniqueID + "\"");
            if (this.CssClass != "")
            {
                output.Write(" class=\"" + this.CssClass + "\"");
            }
            output.Write(">");
            output.Write("<option value=\"\">��ѡ��</option>");
            //���ݳ�ʼ��ѡ��������ѡ����
            if (cindex > 0)
            {
                ArrayList arr = (ArrayList)((ArrayList)darr[pindex-1])[cindex-1];
                for (int i = 0; i < arr.Count; i++)
                {
                    if (this.TextCounty != null)
                    {
                        //����ѡ��ֵ����Ĭ����
                        if (this.TextCounty.IndexOf(arr[i].ToString()) > -1)
                        {
                            output.Write("<option selected>" + arr[i].ToString() + "</option>");
                        }
                        else
                        {
                            output.Write("<option>" + arr[i].ToString() + "</option>");
                        }
                    }
                    else
                    {
                        output.Write("<option>" + arr[i].ToString() + "</option>");
                    }
                }
            }
            output.Write("</select>");
            output.Write("��");
            output.Write("</td></tr></table>");
            #endregion
		}

		/// <summary>
		/// ��XML�ļ��ж�ȡʡ������Ϣ
		/// </summary>
		/// <returns></returns>
		private void GetCascadingInfo()
		{
			ArrayList arr = new ArrayList();
			ArrayList drr = new ArrayList();
			ArrayList zrr = new ArrayList();

			//����Դ�л�ȡ
			System.Reflection.Assembly thisExe;
			thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream file = thisExe.GetManifestResourceStream("We7.CMS.Controls.CascadeControl.xml");
			XmlTextReader xr = new XmlTextReader(file);

			while(xr.Read())
			{
				if(xr.NodeType == XmlNodeType.Element && xr.Name == "province")
				{
					parr.Add(xr.GetAttribute("name"));
					arr = new ArrayList();
                    drr = new ArrayList();
				}
				if(xr.NodeType == XmlNodeType.Element && xr.Name == "city")
				{
					arr.Add(xr.GetAttribute("name"));
                    zrr = new ArrayList();
				}
                if (xr.NodeType == XmlNodeType.Element && xr.Name == "county")
				{
                    zrr.Add(xr.GetAttribute("name"));
				}
                if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "city")
				{
                    drr.Add(zrr);
				}
				if(xr.NodeType == XmlNodeType.EndElement && xr.Name == "province")
				{
					carr.Add(arr);
					darr.Add(drr);
				}
			}
			xr.Close();
		}

		#region IPostBackDataHandler ��Ա

		public event EventHandler TextChanged;

		/// <summary>
		/// ������ʵ��ʱ�����ź�Ҫ��������ؼ�����֪ͨ ASP.NET Ӧ�ó���ÿؼ���״̬�Ѹ��ġ�
		/// </summary>
		public virtual void RaisePostDataChangedEvent() 
		{
			OnTextChanged(EventArgs.Empty);
		}
      

		protected virtual void OnTextChanged(EventArgs e) 
		{
			if (TextChanged != null)
				TextChanged(this,e);
		}

		/// <summary>
		/// ������ʵ��ʱ��Ϊ ASP.NET �������ؼ�����ط����ݡ�
		/// </summary>
		/// <param name="postDataKey">�ؼ�����Ҫ��ʶ��</param>
		/// <param name="postCollection">���д�������ֵ�ļ���</param>
		/// <returns>����������ؼ���״̬�ڻط���������ģ���Ϊ true������Ϊ false��</returns>
        public virtual bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) 
		{
            bool blReturn = false;
            //ʡ
            if (this.TextProvince == null || !this.Text.Equals(postCollection[this.UniqueID + "_p"]))
            {
                ViewState["TextProvince"] = postCollection[this.UniqueID + "_p"];
                blReturn = true;
            }
            //��
            if (this.TextCity == null || !this.Text.Equals(postCollection[this.UniqueID + "_c"]))
            {
                ViewState["TextCity"] = postCollection[this.UniqueID + "_c"];
                blReturn = true;
            }
            //��
            if (this.TextCounty == null || !this.Text.Equals(postCollection[this.UniqueID]))
            {
                ViewState["TextCounty"] = postCollection[this.UniqueID];
                blReturn = true;
            }

            return blReturn;
		}

		#endregion
	}
}
