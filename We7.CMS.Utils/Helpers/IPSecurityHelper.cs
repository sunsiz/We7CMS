using System;
using System.Collections.Generic;
using System.Text;


using Thinkment.Data;
using System.Web;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS
{
    /// <summary>
    /// IP���԰�ȫ������Ϊ��
    /// </summary>
    [Serializable]
    [Helper("We7.IPSecurityHelper")]
    public class IPSecurityHelper:BaseHelper
    {
        /// <summary>
        /// ����һ��ArticleHelperʵ��
        /// </summary>
        private ArticleHelper ArticleHelper
        {
            get
            {
                HelperFactory hf = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                return hf.GetHelper<ArticleHelper>();
            }
        }

        /// <summary>
        /// ����һ��ChannelHelperʵ��
        /// </summary>
        private ChannelHelper ChannelHelper
        {
            get
            {
                HelperFactory hf = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                return hf.GetHelper<ChannelHelper>();
            }
        }

        /// <summary>
        /// ���IP�Ƿ���IP������������Χ��
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="ColumnID">��ĿID</param>
        /// <param name="ArticleID">����ID</param>
        /// <returns></returns>
        public bool CheckIPStrategy(string ip, string ColumnID, string ArticleID)
        {
            return checkArticleStrategy(ip, ArticleID, ColumnID) == StrategyState.DENY ? false : true;
        }

        /// <summary>
        /// ��ⳣ��IP��������
        /// </summary>
        /// <param name="ip">IP</param>
        /// <returns></returns>
        private StrategyState checkCommonStrategy(string ip)
        {
            return StrategyConfigs.Instance.IsAllow(ip, GeneralConfigs.GetConfig().IPStrategy);
        }

        /// <summary>
        /// �����ĿIP��������
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="columnId">��ĿID</param>
        /// <returns></returns>
        private StrategyState checkChannelStrategy(string ip,string columnId)
        {
            StrategyState state = StrategyState.DEFAULT;
            if (!String.IsNullOrEmpty(columnId))
            {
                state = StrategyConfigs.Instance.IsAllow(ip, ChannelHelper.QueryStrategy(columnId));

                if (state == StrategyState.DEFAULT)
                {
                    Channel channel=ChannelHelper.GetChannel(columnId, new string[] { "ParentID" });
                    if(channel!=null)
                        state = checkChannelStrategy(ip, channel.ParentID);
                }                   
            }
            return state;
        }

        /// <summary>
        /// �����Ŀ��IP����
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="columnId">��ĿID</param>
        /// <returns></returns>
        private StrategyState checkChannelStrategyWithCommon(string ip, string columnId)
        {
            StrategyState state=checkChannelStrategy(ip, columnId);
            if (state == StrategyState.DEFAULT)
                state = checkCommonStrategy(ip);
            return state;
        }

        /// <summary>
        /// �������IP��������
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="articleId">����ID</param>
        /// <param name="channelID">��ĿID</param>
        /// <returns></returns>
        private StrategyState checkArticleStrategy(string ip, string articleId,string channelID)
        {
            StrategyState state=StrategyState.DEFAULT;

            if(!String.IsNullOrEmpty(articleId))
                state= StrategyConfigs.Instance.IsAllow(ip, ArticleHelper.QueryStrategy(articleId));
            
            if (state == StrategyState.DEFAULT)
                state = checkChannelStrategyWithCommon(ip, channelID);
            return state;
        }
    }

    public enum IPSecurityType
    {
        /// <summary>
        /// ������
        /// </summary>
        Allow = 0,
        /// <summary>
        /// ��ֹ
        /// </summary>
        Forbidden = 1,
        Other = 2
    }
}
