using System;
using System.Collections.Generic;
using System.Text;

using Thinkment.Data;

namespace We7.Framework
{
    /// <summary>
    /// ������ӿ�
    /// </summary>
    public interface IHelper
    {
        string Name { get; set; }

        string Description { get; set; }
        
        ObjectAssistant Assistant { get; set; }

        string Root { get; set; }
    }
}
