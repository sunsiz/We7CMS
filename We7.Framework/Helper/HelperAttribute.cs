using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework
{
    /// <summary>
    /// �������ԣ���������dll���ػ�ȡ��ʵ�����
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class HelperAttribute : Attribute
    {
        string helperName;
        string description;

        public HelperAttribute(string name)
        {
            helperName = name;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public string Name
        {
            get { return helperName; }
            set { helperName = value; }
        }
    }
}
