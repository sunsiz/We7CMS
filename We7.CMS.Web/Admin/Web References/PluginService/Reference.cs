﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace We7.CMS.Web.Admin.PluginService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.79.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PluginInfomationSoap", Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PluginInfo))]
    public partial class PluginInfomation : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback LoadServerInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback LoadRemotePluginInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback CheckTempFileOperationCompleted;
        
        private System.Threading.SendOrPostCallback AddDownLoadsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PluginInfomation() {
            this.Url = global::We7.CMS.Web.Admin.Properties.Settings.Default.WebEngine2007_CD_Web_Admin_PluginService_PluginInfomation;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event LoadServerInfoCompletedEventHandler LoadServerInfoCompleted;
        
        /// <remarks/>
        public event LoadRemotePluginInfoCompletedEventHandler LoadRemotePluginInfoCompleted;
        
        /// <remarks/>
        public event CheckTempFileCompletedEventHandler CheckTempFileCompleted;
        
        /// <remarks/>
        public event AddDownLoadsCompletedEventHandler AddDownLoadsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LoadServerInfo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public RemotePluginInfo[] LoadServerInfo(PluginType pluginType) {
            object[] results = this.Invoke("LoadServerInfo", new object[] {
                        pluginType});
            return ((RemotePluginInfo[])(results[0]));
        }
        
        /// <remarks/>
        public void LoadServerInfoAsync(PluginType pluginType) {
            this.LoadServerInfoAsync(pluginType, null);
        }
        
        /// <remarks/>
        public void LoadServerInfoAsync(PluginType pluginType, object userState) {
            if ((this.LoadServerInfoOperationCompleted == null)) {
                this.LoadServerInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoadServerInfoOperationCompleted);
            }
            this.InvokeAsync("LoadServerInfo", new object[] {
                        pluginType}, this.LoadServerInfoOperationCompleted, userState);
        }
        
        private void OnLoadServerInfoOperationCompleted(object arg) {
            if ((this.LoadServerInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoadServerInfoCompleted(this, new LoadServerInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LoadRemotePluginInfo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public RemotePluginInfo LoadRemotePluginInfo(string pluginName, PluginType pluginType) {
            object[] results = this.Invoke("LoadRemotePluginInfo", new object[] {
                        pluginName,
                        pluginType});
            return ((RemotePluginInfo)(results[0]));
        }
        
        /// <remarks/>
        public void LoadRemotePluginInfoAsync(string pluginName, PluginType pluginType) {
            this.LoadRemotePluginInfoAsync(pluginName, pluginType, null);
        }
        
        /// <remarks/>
        public void LoadRemotePluginInfoAsync(string pluginName, PluginType pluginType, object userState) {
            if ((this.LoadRemotePluginInfoOperationCompleted == null)) {
                this.LoadRemotePluginInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoadRemotePluginInfoOperationCompleted);
            }
            this.InvokeAsync("LoadRemotePluginInfo", new object[] {
                        pluginName,
                        pluginType}, this.LoadRemotePluginInfoOperationCompleted, userState);
        }
        
        private void OnLoadRemotePluginInfoOperationCompleted(object arg) {
            if ((this.LoadRemotePluginInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoadRemotePluginInfoCompleted(this, new LoadRemotePluginInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CheckTempFile", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void CheckTempFile(string pluginName, PluginType pluginType) {
            this.Invoke("CheckTempFile", new object[] {
                        pluginName,
                        pluginType});
        }
        
        /// <remarks/>
        public void CheckTempFileAsync(string pluginName, PluginType pluginType) {
            this.CheckTempFileAsync(pluginName, pluginType, null);
        }
        
        /// <remarks/>
        public void CheckTempFileAsync(string pluginName, PluginType pluginType, object userState) {
            if ((this.CheckTempFileOperationCompleted == null)) {
                this.CheckTempFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckTempFileOperationCompleted);
            }
            this.InvokeAsync("CheckTempFile", new object[] {
                        pluginName,
                        pluginType}, this.CheckTempFileOperationCompleted, userState);
        }
        
        private void OnCheckTempFileOperationCompleted(object arg) {
            if ((this.CheckTempFileCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckTempFileCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AddDownLoads", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void AddDownLoads(string pluginName, PluginType pluginType) {
            this.Invoke("AddDownLoads", new object[] {
                        pluginName,
                        pluginType});
        }
        
        /// <remarks/>
        public void AddDownLoadsAsync(string pluginName, PluginType pluginType) {
            this.AddDownLoadsAsync(pluginName, pluginType, null);
        }
        
        /// <remarks/>
        public void AddDownLoadsAsync(string pluginName, PluginType pluginType, object userState) {
            if ((this.AddDownLoadsOperationCompleted == null)) {
                this.AddDownLoadsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddDownLoadsOperationCompleted);
            }
            this.InvokeAsync("AddDownLoads", new object[] {
                        pluginName,
                        pluginType}, this.AddDownLoadsOperationCompleted, userState);
        }
        
        private void OnAddDownLoadsOperationCompleted(object arg) {
            if ((this.AddDownLoadsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddDownLoadsCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.79.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum PluginType {
        
        /// <remarks/>
        PLUGIN,
        
        /// <remarks/>
        CONSTROL,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.79.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class RemotePluginInfo : PluginInfo {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemotePluginInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.79.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class PluginInfo {
        
        private PluginType pluginTypeField;
        
        private string directoryField;
        
        private string nameField;
        
        private string authorField;
        
        private string versionField;
        
        private bool enableField;
        
        private bool isInstalledField;
        
        private string urlField;
        
        private string descriptionField;
        
        private string summaryField;
        
        private string defaultPageField;
        
        private UrlItem[] pagesField;
        
        private UrlItem[] controlsField;
        
        private Deployment deploymentField;
        
        private string thumbnailField;
        
        private string[] snapshotField;
        
        private string othersField;
        
        private System.DateTime updateTimeField;
        
        private System.DateTime createTimeField;
        
        private string compatibleField;
        
        private bool isSpecialField;
        
        private int clicksField;
        
        private bool isLocalField;
        
        /// <remarks/>
        public PluginType PluginType {
            get {
                return this.pluginTypeField;
            }
            set {
                this.pluginTypeField = value;
            }
        }
        
        /// <remarks/>
        public string Directory {
            get {
                return this.directoryField;
            }
            set {
                this.directoryField = value;
            }
        }
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string Author {
            get {
                return this.authorField;
            }
            set {
                this.authorField = value;
            }
        }
        
        /// <remarks/>
        public string Version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        public bool Enable {
            get {
                return this.enableField;
            }
            set {
                this.enableField = value;
            }
        }
        
        /// <remarks/>
        public bool IsInstalled {
            get {
                return this.isInstalledField;
            }
            set {
                this.isInstalledField = value;
            }
        }
        
        /// <remarks/>
        public string Url {
            get {
                return this.urlField;
            }
            set {
                this.urlField = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        public string Summary {
            get {
                return this.summaryField;
            }
            set {
                this.summaryField = value;
            }
        }
        
        /// <remarks/>
        public string DefaultPage {
            get {
                return this.defaultPageField;
            }
            set {
                this.defaultPageField = value;
            }
        }
        
        /// <remarks/>
        public UrlItem[] Pages {
            get {
                return this.pagesField;
            }
            set {
                this.pagesField = value;
            }
        }
        
        /// <remarks/>
        public UrlItem[] Controls {
            get {
                return this.controlsField;
            }
            set {
                this.controlsField = value;
            }
        }
        
        /// <remarks/>
        public Deployment Deployment {
            get {
                return this.deploymentField;
            }
            set {
                this.deploymentField = value;
            }
        }
        
        /// <remarks/>
        public string Thumbnail {
            get {
                return this.thumbnailField;
            }
            set {
                this.thumbnailField = value;
            }
        }
        
        /// <remarks/>
        public string[] Snapshot {
            get {
                return this.snapshotField;
            }
            set {
                this.snapshotField = value;
            }
        }
        
        /// <remarks/>
        public string Others {
            get {
                return this.othersField;
            }
            set {
                this.othersField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime UpdateTime {
            get {
                return this.updateTimeField;
            }
            set {
                this.updateTimeField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime CreateTime {
            get {
                return this.createTimeField;
            }
            set {
                this.createTimeField = value;
            }
        }
        
        /// <remarks/>
        public string Compatible {
            get {
                return this.compatibleField;
            }
            set {
                this.compatibleField = value;
            }
        }
        
        /// <remarks/>
        public bool IsSpecial {
            get {
                return this.isSpecialField;
            }
            set {
                this.isSpecialField = value;
            }
        }
        
        /// <remarks/>
        public int Clicks {
            get {
                return this.clicksField;
            }
            set {
                this.clicksField = value;
            }
        }
        
        /// <remarks/>
        public bool IsLocal {
            get {
                return this.isLocalField;
            }
            set {
                this.isLocalField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.79.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class UrlItem {
        
        private string nameField;
        
        private string urlField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string Url {
            get {
                return this.urlField;
            }
            set {
                this.urlField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.79.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Deployment {
        
        private string[] installField;
        
        private string[] updateField;
        
        private string[] unstallField;
        
        private string introductionField;
        
        /// <remarks/>
        public string[] Install {
            get {
                return this.installField;
            }
            set {
                this.installField = value;
            }
        }
        
        /// <remarks/>
        public string[] Update {
            get {
                return this.updateField;
            }
            set {
                this.updateField = value;
            }
        }
        
        /// <remarks/>
        public string[] Unstall {
            get {
                return this.unstallField;
            }
            set {
                this.unstallField = value;
            }
        }
        
        /// <remarks/>
        public string Introduction {
            get {
                return this.introductionField;
            }
            set {
                this.introductionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.79.0")]
    public delegate void LoadServerInfoCompletedEventHandler(object sender, LoadServerInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.79.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoadServerInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LoadServerInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RemotePluginInfo[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RemotePluginInfo[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.79.0")]
    public delegate void LoadRemotePluginInfoCompletedEventHandler(object sender, LoadRemotePluginInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.79.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoadRemotePluginInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LoadRemotePluginInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RemotePluginInfo Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RemotePluginInfo)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.79.0")]
    public delegate void CheckTempFileCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.79.0")]
    public delegate void AddDownLoadsCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591