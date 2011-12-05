using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace We7
{
    /// <summary>
    /// �������б��״̬����
    /// </summary>
    [Serializable]
    public static class StateMgr
    {
        public static string StateInitialize()
        {
            return "".PadLeft(EnumLibrary.StateLength, '0');
        }

        public static string StateProcess(string oldState, EnumLibrary.Business business, int state)
        {
            //Ϊ�˼����ϳ������ΪNull��""�����ʼ��״̬�롣
            if (oldState == string.Empty || oldState == null)
            {
                oldState = "".PadLeft(EnumLibrary.StateLength, '0');
            }           

            Type enumType = business.GetType();
            int position = 0;
            foreach (int enumItem in Enum.GetValues(enumType))
            {
                if ((int)business == enumItem)
                {
                    position = EnumLibrary.Position[enumItem];
                    break;
                }
            }

            oldState = ReplaceAt(oldState, position, state.ToString().PadLeft(EnumLibrary.PlaceLenth, '0'));

            return oldState;
        }

        public static int GetStateValue(string stateValue, EnumLibrary.Business business)
        {
            //Ϊ�˼����ϳ������ΪNull��""�����ʼ��״̬�롣
            if (stateValue == null || stateValue == string.Empty)
            {
                stateValue = StateInitialize();
            }

            int state = 0;

            Type enumType = business.GetType();
            int position = 0;
            foreach (int enumItem in Enum.GetValues(enumType))
            {
                if ((int)business == enumItem)
                {
                    position = EnumLibrary.Position[enumItem];
                    break;
                }
            }

            state = Convert.ToInt16(stateValue.Substring(position, EnumLibrary.PlaceLenth));
            return state;
        }

        public static string GetStateValueStr(string stateValue, EnumLibrary.Business business)
        {
            //Ϊ�˼����ϳ������ΪNull��""�����ʼ��״̬�롣
            if (stateValue == null || stateValue == string.Empty)
            {
                stateValue = StateInitialize();
            }

            int state = 0;

            Type enumType = business.GetType();
            int position = 0;
            foreach (int enumItem in Enum.GetValues(enumType))
            {
                if ((int)business == enumItem)
                {
                    position = EnumLibrary.Position[enumItem];
                    break;
                }
            }

            state = Convert.ToInt16(stateValue.Substring(position, EnumLibrary.PlaceLenth));
            return state.ToString().PadLeft(EnumLibrary.PlaceLenth, '0');
        }

        public static object GetStateValueEnum(string stateValue, EnumLibrary.Business business)
        {
            //Ϊ�˼����ϳ������ΪNull��""�����ʼ��״̬�롣
            if (stateValue == null || stateValue == string.Empty)
            {
                stateValue = StateInitialize();
            }

            int state = 0;

            Type enumType = business.GetType();
            int position = 0;
            foreach (int enumItem in Enum.GetValues(enumType))
            {
                if ((int)business == enumItem)
                {
                    position = EnumLibrary.Position[enumItem];
                    break;
                }
            }

            state = Convert.ToInt16(stateValue.Substring(position, EnumLibrary.PlaceLenth));

            Type t = typeof(EnumLibrary);
            enumType = t.GetNestedType(business.ToString());

            //foreach (string enumItem in Enum.GetNames(enumType))
            //{
            //    if (enumItem ==Enum.Parse(enumType, state)
            //    {
            //stateName = enumItem;
            //Enum.GetName(enumType, state);
            //}
            //}

            return Enum.Parse(enumType, state.ToString());
        }

        public static string GetStateName(string stateValue, EnumLibrary.Business business)
        {
            //Ϊ�˼����ϳ������ΪNull��""�����ʼ��״̬�롣
            if (stateValue == null || stateValue == string.Empty)
            {
                stateValue = StateInitialize();
            }

            int state = 0;

            Type enumType = business.GetType();
            int position = 0;
            foreach (int enumItem in Enum.GetValues(enumType))
            {
                if ((int)business == enumItem)
                {
                    position = EnumLibrary.Position[enumItem];
                    break;
                }
            }

            state = Convert.ToInt16(stateValue.Substring(position, EnumLibrary.PlaceLenth));

            Type t = typeof(EnumLibrary);
            enumType = t.GetNestedType(business.ToString());

            //foreach (string enumItem in Enum.GetNames(enumType))
            //{
            //    if (enumItem ==Enum.Parse(enumType, state)
            //    {
            //stateName = enumItem;
            //Enum.GetName(enumType, state);
            //}
            //}

            return Enum.GetName(enumType, state);
        }

        /// <summary>
        /// �����ַ��滻�ַ���ָ��λ���ַ�
        /// </summary>
        /// <param name="oldString">ԭ�ַ���</param>
        /// <param name="position">���滻���ַ�λ��</param>
        /// <param name="newString">���ַ�</param>
        /// <returns>���ַ���</returns>
        private static string ReplaceAt(string oldString, int position, string newString)
        {
            string preString = oldString.Substring(0, position);
            string endString = ((position + EnumLibrary.PlaceLenth) < oldString.Length) ? oldString.Substring(position + EnumLibrary.PlaceLenth) : "";
            return preString + newString + endString;
        }

        public static string ConvertEnumToStr(object objEnum)
        {
            int tmpInt = (int)objEnum;
            string tmpStr = tmpInt.ToString().PadLeft(EnumLibrary.PlaceLenth, '0');

            return tmpStr;
        }

    }
}
