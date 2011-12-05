using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace We7.CMS
{
    /// <summary>
    /// Html��ʽ���࣬�����޸��ļ��ĸ�·��
    /// </summary>
    public class HTFormatter
    {
        string fileName;
        string output;
        string root;

        public HTFormatter()
        {
        }

        /// <summary>
        /// �ļ���
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// ��ʽ������
        /// </summary>
        public string Output
        {
            get { return output; }
            private set { output = value; }
        }

        /// <summary>
        /// ��Ŀ¼
        /// </summary>
        public string Root
        {
            get { return root; }
            set { root = value; }
        }

        /// <summary>
        /// ��ʽ��
        /// </summary>
        public void Process()
        {
            string input;
            Encoding thisEncode = EncodingReading.EncodingInfo(FileName);
            using (FileStream fs = new FileStream(FileName, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, thisEncode))
                {
                    input = sr.ReadToEnd();
                }
            }
            output = ProcessImage(input);
            output = ProcessLink(output);
            output = ProcessBackground(output);
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="input">Ҫ�޸ĵ�ֵ</param>
        /// <returns></returns>
        string ProcessBackground(string input)
        {
            string rs = @"\<(.*|.*(\r\n).*|.*)(background|BACKGROUND)\s*=\s*(?:""(?<url>[^""]*)""|'(?<url>[^']*)'|(?<url>[^\>^\s]+))(.*|.*(\r\n).*|.*)\>";
            return Process(input, rs);
        }

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="input">Ҫ�޸ĵ�ֵ</param>
        /// <returns></returns>
        string ProcessImage(string input)
        {
            string rs = @"\<(img|IMG)(.*|.*(\r\n).*|.*)(src|SRC)\s*=\s*(?:""(?<url>[^""]*)""|'(?<url>[^']*)'|(?<url>[^\>^\s]+))(.*|.*(\r\n).*|.*)\>";
            return Process(input, rs);
        }

        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="input">Ҫֵ�ĵ�ֵ</param>
        /// <returns></returns>
        string ProcessLink(string input)
        {
            string rs = @"\<(link|LINK)(.*|.*(\r\n).*|.*)(href|HREF)\s*=\s*(?:""(?<url>[^""]*)""|'(?<url>[^']*)'|(?<url>[^\>^\s]+))(.*|.*(\r\n).*|.*)\>";
            return Process(input, rs);
        }

        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="input">�޸ĵ�ֵ</param>
        /// <param name="rs">ƥ���������ʾ</param>
        /// <returns></returns>
        string Process(string input, string rs)
        {
            Regex reg = new Regex(rs, RegexOptions.IgnoreCase);
            MatchEvaluator me = new MatchEvaluator(Replace);
            return reg.Replace(input, me);
        }

        /// <summary>
        /// �����滻
        /// </summary>
        /// <param name="m">ƥ��</param>
        /// <returns></returns>
        string Replace(Match m)
        {
            string img = m.Groups["url"].Value;
            string full = m.Value;
            if (string.IsNullOrEmpty(img))
                return full;
            return full.Replace(img, String.Format("{0}{1}", root, img));
        }
    }
}
