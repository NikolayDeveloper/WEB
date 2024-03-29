﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by wsdl, Version=4.0.30319.1.
// 

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.ExternalPEP {
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [WebServiceBinding(Name="WSExport2_IPP402MUCallbackHttpBinding", Namespace="http://MU.P402.Library/MU/P402/Binding2")]
    public partial class WSExport2_IPP402MUCallbackHttpService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback sendResponseOperationCompleted;
        
        /// <remarks/>
        public WSExport2_IPP402MUCallbackHttpService() {
            this.Url = "http://http.async.test.shep.nitec.kz:80/MU.P402.ModuleWeb/sca/MU.P402.Callback";
        }
        
        /// <remarks/>
        public event sendResponseCompletedEventHandler sendResponseCompleted;
        
        /// <remarks/>
        [SoapDocumentMethod("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: XmlElement("sendResponseResponse", Namespace="http://MU.P402.Library/MU/P402")]
        public sendResponseResponse sendResponse([XmlElement("sendResponse", Namespace="http://MU.P402.Library/MU/P402")] sendResponse sendResponse1) {
            object[] results = this.Invoke("sendResponse", new object[] {
                        sendResponse1});
            return ((sendResponseResponse)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginsendResponse(sendResponse sendResponse1, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("sendResponse", new object[] {
                        sendResponse1}, callback, asyncState);
        }
        
        /// <remarks/>
        public sendResponseResponse EndsendResponse(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((sendResponseResponse)(results[0]));
        }
        
        /// <remarks/>
        public void sendResponseAsync(sendResponse sendResponse1) {
            this.sendResponseAsync(sendResponse1, null);
        }
        
        /// <remarks/>
        public void sendResponseAsync(sendResponse sendResponse1, object userState) {
            if ((this.sendResponseOperationCompleted == null)) {
                this.sendResponseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsendResponseOperationCompleted);
            }
            this.InvokeAsync("sendResponse", new object[] {
                        sendResponse1}, this.sendResponseOperationCompleted, userState);
        }
        
        private void OnsendResponseOperationCompleted(object arg) {
            if ((this.sendResponseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.sendResponseCompleted(this, new sendResponseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType=true, Namespace="http://MU.P402.Library/MU/P402")]
    public partial class sendResponse {
        
        private P40XResponse requestField;
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public P40XResponse request {
            get {
                return this.requestField;
            }
            set {
                this.requestField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace="http://MU.NIIS.Library")]
    public partial class P40XResponse {
        
        private P40XResponseData responseDataField;
        
        private RequestInfo systemInfoField;
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public P40XResponseData responseData {
            get {
                return this.responseDataField;
            }
            set {
                this.responseDataField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RequestInfo systemInfo {
            get {
                return this.systemInfoField;
            }
            set {
                this.systemInfoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace="http://MU.NIIS.Library")]
    public partial class P40XResponseData {
        
        private int documentIdField;
        
        private bool documentIdFieldSpecified;
        
        private int statusField;
        
        private bool statusFieldSpecified;
        
        private string additionalInfoField;
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int documentId {
            get {
                return this.documentIdField;
            }
            set {
                this.documentIdField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool documentIdSpecified {
            get {
                return this.documentIdFieldSpecified;
            }
            set {
                this.documentIdFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool statusSpecified {
            get {
                return this.statusFieldSpecified;
            }
            set {
                this.statusFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string additionalInfo {
            get {
                return this.additionalInfoField;
            }
            set {
                this.additionalInfoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace="http://systeminfo.services.shep.nitec.kz")]
    public partial class SystemInfo {
        
        private string digiSignField;
        
        private System.DateTime messageDateField;
        
        private bool messageDateFieldSpecified;
        
        private string chainIdField;
        
        private string messageIdField;
        
        private string additionalInfoField;
        
        private StatusInfo statusField;
        
        private string senderField;
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DigiSign {
            get {
                return this.digiSignField;
            }
            set {
                this.digiSignField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime MessageDate {
            get {
                return this.messageDateField;
            }
            set {
                this.messageDateField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool MessageDateSpecified {
            get {
                return this.messageDateFieldSpecified;
            }
            set {
                this.messageDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ChainId {
            get {
                return this.chainIdField;
            }
            set {
                this.chainIdField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageId {
            get {
                return this.messageIdField;
            }
            set {
                this.messageIdField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string AdditionalInfo {
            get {
                return this.additionalInfoField;
            }
            set {
                this.additionalInfoField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StatusInfo Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        public string Sender {
            get {
                return this.senderField;
            }
            set {
                this.senderField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace="http://systeminfo.services.shep.nitec.kz")]
    public partial class StatusInfo {
        
        private string codeField;
        
        private string messageField;
        
        private string messageKzField;
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        public string MessageKz {
            get {
                return this.messageKzField;
            }
            set {
                this.messageKzField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace="http://systeminfo.services.shep.nitec.kz")]
    public partial class RequestInfoOptions {
        
        private string notifyProcessField;
        
        private string parentIdField;
        
        private bool isPublicField;
        
        private bool isPublicFieldSpecified;
        
        private bool persistHistoryField;
        
        private bool persistHistoryFieldSpecified;
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string notifyProcess {
            get {
                return this.notifyProcessField;
            }
            set {
                this.notifyProcessField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parentId {
            get {
                return this.parentIdField;
            }
            set {
                this.parentIdField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isPublic {
            get {
                return this.isPublicField;
            }
            set {
                this.isPublicField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool isPublicSpecified {
            get {
                return this.isPublicFieldSpecified;
            }
            set {
                this.isPublicFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool persistHistory {
            get {
                return this.persistHistoryField;
            }
            set {
                this.persistHistoryField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool persistHistorySpecified {
            get {
                return this.persistHistoryFieldSpecified;
            }
            set {
                this.persistHistoryFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace="http://systeminfo.services.shep.nitec.kz")]
    public partial class RequestInfo {
        
        private string requestNumberField;
        
        private string messageIdField;
        
        private string chainIdField;
        
        private System.DateTime messageDateField;
        
        private bool messageDateFieldSpecified;
        
        private string digiSignField;
        
        private StatusInfo statusField;
        
        private RequestInfoOptions optionField;
        
        private string senderField;
        
        private string requestIINBINField;
        
        private string requestedIINBINField;
        
        private string clientIPAddressField;
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requestNumber {
            get {
                return this.requestNumberField;
            }
            set {
                this.requestNumberField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string messageId {
            get {
                return this.messageIdField;
            }
            set {
                this.messageIdField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string chainId {
            get {
                return this.chainIdField;
            }
            set {
                this.chainIdField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime messageDate {
            get {
                return this.messageDateField;
            }
            set {
                this.messageDateField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool messageDateSpecified {
            get {
                return this.messageDateFieldSpecified;
            }
            set {
                this.messageDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string digiSign {
            get {
                return this.digiSignField;
            }
            set {
                this.digiSignField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StatusInfo status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RequestInfoOptions option {
            get {
                return this.optionField;
            }
            set {
                this.optionField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Sender {
            get {
                return this.senderField;
            }
            set {
                this.senderField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string RequestIINBIN {
            get {
                return this.requestIINBINField;
            }
            set {
                this.requestIINBINField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string RequestedIINBIN {
            get {
                return this.requestedIINBINField;
            }
            set {
                this.requestedIINBINField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ClientIPAddress {
            get {
                return this.clientIPAddressField;
            }
            set {
                this.clientIPAddressField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType=true, Namespace="http://MU.P402.Library/MU/P402")]
    public partial class sendResponseResponse {
        
        private SystemInfo responseField;
        
        /// <remarks/>
        [XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public SystemInfo response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    public delegate void sendResponseCompletedEventHandler(object sender, sendResponseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class sendResponseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal sendResponseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public sendResponseResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((sendResponseResponse)(this.results[0]));
            }
        }
    }
}
