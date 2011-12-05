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
	/// ProvinceCityCascade ��ժҪ˵����
	/// ʡ������ѡ��ؼ�
	/// </summary>

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ProvinceCityCascade runat=server></{0}:ProvinceCityCascade>")]
	public class ProvinceCityCascade :  WebControl, IPostBackDataHandler
	{
		#region ����

        [Bindable(true), Category("����"), DefaultValue(""), Description("��ȡѡ���ı�")]
        public string Text 
		{
			get
			{
                string province = "";
                string city = "";

                if (ViewState[this.ID + "_TextProvince"] != null)
                {
                    province = ViewState[this.ID + "_TextProvince"].ToString();
                }
                else
                {
                    province = "";
                }

                if (ViewState[this.ID + "_TextCity"] != null)
                {
                    city = ViewState[this.ID + "_TextCity"].ToString();
                }
                else
                {
                    city = "";
                }

                return province + "-" + city;
			}
            set
            {
                string[] temp = value.Split(new char[] { '-' });
                if (temp.Length>1)
                {
                    ViewState[this.ID + "_TextProvince"] = temp[0];
                    ViewState[this.ID + "_TextCity"] = temp[1];
                }
            }
		}

        [Bindable(true), Category("����"), DefaultValue(""), Description("��ȡ������ʡ")]
        public string TextProvince 
		{
			get
			{
                if (ViewState[this.ID + "_TextProvince"] != null)
				{
                    return ViewState[this.ID + "_TextProvince"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState[this.ID + "_TextProvince"] = value;
			}
		}

        [Bindable(true), Category("����"), DefaultValue(""), Description("��ȡ��������")]
        public string TextCity 
		{
			get
			{
                if (ViewState[this.ID + "_TextCity"] != null)
				{
                    return ViewState[this.ID + "_TextCity"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState[this.ID + "_TextCity"] = value;
			}
		}	
        #endregion

		//ʡ��
		private ArrayList parr = new ArrayList();
		//����
		private ArrayList carr = new ArrayList();
		
		/// <summary>
		/// ���˿ؼ����ָ�ָ�����������
		/// </summary>
		/// <param name="writer"> Ҫд������ HTML ��д��</param>
		protected override void Render(HtmlTextWriter output)
		{
			GetCascadingInfo();

			#region	ʡ����ѡ�����javascript����

            /*****************ʡ �� �� ����*********/
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(typeof(string),"ProvinceCityCascade_clientScript"))
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
                        output.WriteLine("group[" + (i + 1) + "][" + (j + 1) + "]=new Option(\"" + arr[j].ToString() + "\",\"" + arr[j].ToString() + "\");");
                    }
                }
                /********************************************************************************/

				output.WriteLine("function redirectff(x,obj,objc){");						  
				output.WriteLine("for (m=obj.options.length-1;m>0;m--)");
				output.WriteLine("obj.options[m]=null;");
				output.WriteLine("for (i=1;i<group[x].length;i++){");
				output.WriteLine("obj.options[i]=new Option(group[x][i].text,group[x][i].value);");
				output.WriteLine("obj.options[0].selected=true;");
				output.WriteLine("}}");
				output.WriteLine("</script>");
				this.Page.ClientScript.RegisterClientScriptBlock(typeof(string),"ProvinceCityCascade_clientScript", "",true);
			}

			#endregion

			#region дselect����

			//��ʼ��
            output.Write("<table style=\"border: solid 0px #fff;width:100%\"><tr><td>");
            output.Write("<select id=\"" + this.ID + "_p\" name=\"" + this.ID + "_p\" onChange=\"redirectff(this.options.selectedIndex,document.all." + this.ID + "_c,document.all." + this.ID + ")\" ");
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
            output.Write("<select id=\"" + this.ID + "_c\" name=\"" + this.ID+ "_c\" ");
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
				}
				if(xr.NodeType == XmlNodeType.Element && xr.Name == "city")
				{
					arr.Add(xr.GetAttribute("name"));
				}
				if(xr.NodeType == XmlNodeType.EndElement && xr.Name == "province")
				{
					carr.Add(arr);
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
            if (this.TextProvince == null || !this.Text.Equals(postCollection[this.ID+ "_p"]))
            {
                ViewState[this.ID + "_TextProvince"] = postCollection[this.ID + "_p"];
                blReturn = true;
            }
            //��
            if (this.TextCity == null || !this.Text.Equals(postCollection[this.ID + "_c"]))
            {
                ViewState[this.ID + "_TextCity"] = postCollection[this.ID + "_c"];
                blReturn = true;
            }
            return blReturn;
		}

		#endregion
	}
}
