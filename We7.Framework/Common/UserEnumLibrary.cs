using System;
using System.Collections.Generic;
using System.Text;

namespace We7
{
    /// <summary>
    /// ����״̬�����ö�����
    /// �뽫�����ҵ�񣬼����
    /// ��״̬�����ڴˡ�
    /// </summary>
    [Serializable]
    public static class EnumLibrary
    {
        /// <summary>
        /// ״̬�ֶ��ܳ�
        /// </summary>
        public const int StateLength = 20;

        /// <summary>
        /// ״̬λ���ȣ���ÿ��״̬��ռ�ִ�����
        /// </summary>
        public const int PlaceLenth = 2;

        /// <summary>
        /// �����ζ�Ӧ��ҵ��ö�ٽ�����д
        /// ״̬��State�ֶ�����λ�ã�������������Ϊ0,2,4,6,8,��
        /// </summary>
        public static int[] Position ={ 2, 0, 0, 0, 0, 0, 2, 0, 4, 2,2,0,2,4,6,8,0,2,4,0,4,6,2,0,2};

        /// <summary>
        /// ��Ҫ�����ҵ��ö��
        /// </summary>
        public enum Business : int
        {
            //CD����
            ChannelContentType = 0,     //Channel�� ��Ŀ������Ϣ����
            ProductInfoType = 1,        //Article�� ��Ʒ��Ϣ��������
            ProductProviderType = 2,    //Product�� ��Ʒ��Ӧ������
            IndustryType = 3,           //Industry�� ��ҵ����
            CompanyBaseInfoType = 4,    //CompanyBaseInfo�� ��˾�Ƽ�����
            PermissionType = 5,         //EntityPermission�� Ȩ��Ӧ������
            ArticleType = 6,            //Article�� չ����Ϣ�ȷ�������
            ChannelNodeLevel = 8,         //Channel�� ��ǰ��Ŀ�ڵ����
            RecruitingType = 10,        //CompanyBaseInfo�� ������Ƹ��ҵ�Ƽ�
            HomeRecommend = 18,         //Article�� ��������ҳ�����Ƽ�
            //ID����
            SitePartnership = 7,    //SitePartnership�� վ���ϵ����
            SiteValidateStyle = 9,      //SitePartnership�� վ����Ч��ʽ
            SiteSyncType = 20,          //SitePartnership�� ��Ϣ����ʽ
            SiteAutoUsering = 21,       //SitePartnership�� �Զ�ƥ���û�
            //���λ����
            //AdZoneState=11,           //���λ��״̬��0��ʾ���ã�1��ʾվ�����룬2վȺ��ʾͨ�����
            AdZoneTemplate = 12,        //AdZone��  ���λ��ģ��
            AdZoneType = 13,            //AdZone��  ���λ��λ����
            AdZoneShowType = 14,        //AdZone��  ���λ��ʾ��ʽ
            AdZoneDefaultSetting = 15,  //AdZone��  ���λ����
            AdvertisementType = 16,     //Advertisement��   �������
            //AdvertisementEnumState = 17,//���״̬
            AdPublishState = 19,       //AdPublish�� ��淢��״̬��0��ʾ���ã�1��ʾվ�����룬2վȺ��ʾͨ�����
            //��������
            AdviceMode = 22,            //AdviceType�� ����ģʽ
            AdviceDisplay = 23,         //Advice�� ����ǰ̨��ʾģʽ
            AdviceEnum = 24,            //����״̬

            Others = 99
        }
        #region ����λ���ò�������
        public enum AdvertisementType : int
        { 
            AdImage = 1,//ͼƬ
            AdFlash,//����
            AdText, //�ı�
            AdCode,//����
            AdPage //ҳ��
        }

        /// <summary>
        /// ������״̬
        /// 0��ʾ���ã�1��ʾվ�����룬2վȺ��ʾͨ�����
        /// ͬʱ�ѳ����λ�����״̬���
        /// </summary>
        public enum AdPublishState
        {
            NoUsing = 0, //����
            Applying,  //����
            ApplyPassed //ͨ�����
        }

        //public enum AdvertisementEnumState : int
        //{
        //    UnUsed = 0, //����
        //    ApplyAdvertisement,  //����
        //    ThroughAdvertisement, //ͨ�����
        //}

        //public enum AdZoneState : int
        //{
        //    UnUsed = 0, //����
        //    ApplyZone,  //����
        //    ThroughAdZone, //ͨ�����
        //}

        public enum AdZoneTemplate : int
        {
            ChannelTemplate = 1, //��Ŀģ��
            HomepageTemplate,  //��ҳģ��
            ContentpageTemplate, //���º�չ����ϸ��Ϣģ��
            ProductcontentpageTemplate, //��Ʒ��ϸ��Ϣģ��
            DefaultChannelTemplate,    //��Ŀҳģ��
            LoginTemplate,   //��½ҳģ��
            ErrorTemplate,   //����ҳģ��
            Others
        }

        public enum AdZoneType : int
        {
            RectangleBanner = 1, //���κ��
            ShowWindow,  //��������
            MoveAdZone, //�����ƶ�
            FixAdZone, //�̶�λ��
            FloatMoveAdZone, //Ư���ƶ�
            CharacterAdZone, //���ִ���
            CoupletAdZone, //�������
            Others
        }
        public enum AdZoneShowType : int
        {
            ChanceShow = 1, //��Ȩ�������ʾ��Ȩ��Խ����ʾ����Խ��
            FirstShow, //��ʾȨ�����Ĺ��
            CircleShow,  //ѭ����ʾ�ù��λ�Ĺ��
            Others
        }
        public enum AdZoneDefaultSetting : int
        {
            defaultSetting = 1, //Ĭ������
            CustomerSetting,  //�û�����
            Others
        }
        #endregion


        ///// <summary>
        //// ��ҵ������Ӧ��״̬ö�٣�
        ///// ������Ϊ��Ӧ��ҵ��ö����
        ///// </summary>
        #region CD����
        /// <summary>
        /// Ȩ��Ӧ������ EntityPermission��
        /// </summary>
        public enum PermissionType : int
        {
            Manager = 0,     //����Ȩ��
            Member,          //��ԱȨ��
            Public           //����Ȩ��
        }
        public enum HomeRecommend : int
        {
            DefaultArticle=0,    //Ĭ��
            RecommendArticle,    //�Ƽ�
            Others
        }

        public enum RecruitingType : int
        {
            UnUsed = 0, //����
            Using,  //δ�Ƽ�
            UsingToRecruiting //Ӧ�����Ƽ�
        }
        /// <summary>
        /// ������������ Channel��
        /// </summary>
        public enum ChannelContentType : int
        {
            Article = 0,  //����
            Product,   //��Ʒ
            Recruitment,   //��Ƹ
            SeekJob,  //��ְ
            Others
        }
        /// <summary>
        /// ������������ Article��
        /// </summary>
        public enum ArticleType : int
        {
            Article = 0,  //������Ϣ
            Product,    //��Ʒ��Ϣ
            Recruitment,   //��Ƹ
            SeekJob,  //��ְ
            Others
        }
        /// <summary>
        /// ��Ʒ��Ϣ�������� Article��
        /// </summary>
        public enum ProductInfoType : int
        {
            Defaults= 0,//Ĭ��
            Provide ,//��Ӧ
            Buy,         //��
            UrgentBuy,   //���̰�
            Product,     //��Ʒ
            Others
        }

        /// <summary>
        /// ��Ʒ��Ӧ������ Product��
        /// </summary>
        public enum ProductProviderType : int
        {
            Developer = 0,
            Producer,
            Agent,
            Dealer,
            Others
        }
        /// <summary>
        /// ��ҵ���� Industry��
        /// </summary>
        public enum IndustryType : int
        {
            UnUsed = 0, //����
            Using,  //����
            UsingToHomepage, //Ӧ������ҳ
            Others
        }
        /// <summary>
        ///��˾�Ƽ����� CompanyBaseInfo��
        /// </summary>
        public enum CompanyBaseInfoType : int
        {
            UnUsed = 0, //����
            Using,  //����
            UsingToHomepage, //Ӧ�����Ƽ�
            UsingToRecruiting, //Ӧ�����Ƽ�
            Others
        }

        #endregion

        #region ��������
        /// <summary>
        /// ����ģʽ AdviceType��
        /// </summary>
        public enum AdviceMode : int
        {
            Immediate = 0, //ֱ�Ӱ���
            DeliverTo,  //ת������
            Flow, //�ϱ�����
            Others
        }

        public enum AdviceDisplay : int
        {
            DefaultDisplay = 0,//Ĭ����ʾ��ʽ
            DisplayFront = 1,//ǰ̨��ʾ
            UnDisplayFront = 2//ǰ̨����ʾ
        }

        public enum AdviceEnum : int
        {
            OtherHandle =0,//�ǹ���Ա������ʱû���õ���
            AdminHandle = 1//����Ա����
        }

        #endregion

        #region ID����
        /// <summary>
        /// վ���ϵ���ͣ���������
        /// </summary>
        public enum SitePartnership : int
        {
            Sharing = 0,
            Receiving
        }

        /// <summary>
        /// վ���ϵ��Ч��ʽ���Ƿ������պ�
        /// </summary>
        public enum SiteValidateStyle : int
        {
            MustReceived = 0,
            NoMustReceived
        }

        /// <summary>
        /// վ��ͬ����ʽ���Զ�/�ֶ�
        /// </summary>
        public enum SiteSyncType : int
        {
            ManualSync = 0,
            AutoSync
        }

        /// <summary>
        /// �Ƿ��Զ�ƥ���û�
        /// ָͬ�����Ժ���ݷ������Ƿ��Զ�ƥ�������û����
        /// </summary>
        public enum SiteAutoUsering : int
        {
            UnMatchingUser = 0,
            MatchingUser
        }
        #endregion

        #region δ�漰���ݵĽṹ
        /// <summary>
        ///
        /// </summary>
        public enum UserSearchType : int
        {
            ��Ӧ��Ϣ = 0,
            ����Ϣ = 1,
            ʩ������ = 2,
            ��Ʒ��Ϣ = 3,
            ��˾��¼ = 4,
            ��Ƹ��Ϣ = 5,
            չ����Ϣ = 6,
            ������Ϣ = 7
            
        }
        #endregion
    }
}
