using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using We7.Framework.Config;

namespace We7.CMS
{
    public class EncryptString
    {
        /// <summary>
        /// DES�����ַ���
        /// </summary>
        /// <param name="encryptString">�����ܵ��ַ���</param>
        /// <param name="rgbKey">������Կ,Ҫ��Ϊ8λ</param>
        /// <param name="rgbIV">��Կ����</param>
        /// <returns>���ܳɹ����ؼ��ܺ���ִ���ʧ�ܷ�Null</returns>
        public byte[] DES_Encrypt(string encryptString, byte[] rgbKey, byte[] rgbIV)
        {
            try
            {
                if (encryptString != null && encryptString != "")
                {
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                    cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cryptoStream.FlushFinalBlock();
                    if (memoryStream != null)
                    {
                        return memoryStream.ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
        } 

        ///   <summary> 
        ///   DES�����ַ��� 
        ///   </summary> 
        ///   <param   name= "decryptString "> �����ܵ��ַ��� </param> 
        ///   <param   name= "rgbKey "> ������Կ,Ҫ��Ϊ8λ,�ͼ�����Կ��ͬ </param> 
        ///   <param   name= "rgbIV "> ��Կ���� </param> 
        ///   <returns> ���ܳɹ����ؽ��ܺ���ַ�����ʧ�ܷ�Դ�ַ��� </returns> 
        public string DES_Decrypt(byte[] decryptByteArray, byte[] rgbKey, byte[] rgbIV)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cryptoStream.Write(decryptByteArray, 0, decryptByteArray.Length);
                cryptoStream.FlushFinalBlock();
                if (memoryStream != null)
                {
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                else
                {
                    return "False";
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        
        private static  string skey = "xbdongli";
        public static string Encrypt(string pToEncrypt)
        {
            return pToEncrypt;
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            if (ci != null && ci.JiaMiKey.Length == 8)
            {
                skey = ci.JiaMiKey;
            }            
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //���ַ����ŵ�byte������  
            //ԭ��ʹ�õ�UTF8���룬�Ҹĳ�Unicode�����ˣ�����  
            if (pToEncrypt == null)
            {
                return "";
                throw new Exception();
            }
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]  inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);  

            //�������ܶ������Կ��ƫ����  
            //ԭ��ʹ��ASCIIEncoding.ASCII������GetBytes����  
            //ʹ�����������������Ӣ���ı�  
            des.Key = ASCIIEncoding.ASCII.GetBytes(skey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(skey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream  
            //(It  will  end  up  in  the  memory  stream)  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string  
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex  
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        //���ܷ���  
        public static string Decrypt(string pToDecrypt)
        {
            return pToDecrypt;
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            if (ci != null && ci.JiaMiKey.Length == 8)
            {
                skey = ci.JiaMiKey;
            }
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //Put  the  input  string  into  the  byte  array  
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                //�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�  
                des.Key = ASCIIEncoding.ASCII.GetBytes(skey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(skey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //Get  the  decrypted  data  back  from  the  memory  stream  
                //����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı����������  
                StringBuilder ret = new StringBuilder();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                //throw ex;
                System.Web.HttpContext.Current.Response.Redirect("/Nonexistence.htm");
                return "";
            }
        }

    }
}
