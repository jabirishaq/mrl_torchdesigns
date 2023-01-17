using Nop.Core.Plugins;
using Nop.Services.Common;
using System.Collections.Generic;
using System.Web.Routing;
using Nop.Services.Localization;
using Nop.Services.Configuration;
using Nop.Services.Tasks;
using Nop.Core.Domain.Tasks;
using Nop.Core.Domain.Configuration;


namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public class TorchDesignsDynamicsGPWCFPlugin : BasePlugin, IMiscPlugin
    {
        #region Objects

        private readonly ISettingService _settingService;
        private readonly IScheduleTaskService _scheduleTaskService;

        #endregion

        #region Constructors

        public TorchDesignsDynamicsGPWCFPlugin(ISettingService settingService, IScheduleTaskService scheduledTaskService)
        {
            this._settingService = settingService;
            this._scheduleTaskService = scheduledTaskService;
        }

        #endregion

        #region Methods

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "TorchDesigns_DynamicsGPWCF";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.TorchDesigns.DynamicsGPWCF.Controllers" }, { "area", null } };
        }

        public override void Install()
        {
            // Set Default Settings
            this._settingService.SetSetting<string>("gp_enabled", "True");
            //this._settingService.SetSetting<string>("gp_webserviceaddress", "https://maxisql-test.maxijet.local:8765/DynamicsGPWebServices/DynamicsGPService.asmx");
            //this._settingService.SetSetting<string>("gp_webserviceaddress", "http://SAMUELTHAYER:48620/DynamicsGPWebServices/DynamicsGPService.asmx");
            //this._settingService.SetSetting<string>("gp_clientcertenabled", "True");
            //this._settingService.SetSetting<string>("gp_clientcertpath", "D:\\ssl\\2013\\Mrl2013Client.pfx");
            //this._settingService.SetSetting<string>("gp_clientcertkey", "ref1ned");
            //this._settingService.SetSetting<string>("gp_companykey", "4");

            var passwordBytes = System.Text.Encoding.UTF8.GetBytes("YkNkTEhtWDJhZw==");
            //Set payment, sales etc. related settings
            var settings = new TorchDesignsDynamicsGPWCFSettings()
            {
                SalespersonKeyId = "CASH",
                SalesTerritoryKeyId = "TERRITORY 15",
                TaxScheduleId = "NOTAX",
                //TaxScheduleIdForCustomersOutsideFlorida = "NOTAX",
                PaymentCardTypeKeyId = "visa",
                CurrencyKeyISOCode = "USD",
                UserName = "WebClient",
                Password= System.Convert.ToBase64String(passwordBytes),
                PONumberPrefix = "Nop",
                Domain = "maxijet",
                CompanyKey = "4",
                WebServiceAddress = "http://maxiweb-2017.maxijet.local:7777/DynamicsGPWebServices/DynamicsGPService.asmx",

            };

            _settingService.SaveSetting(settings);

            // Add locale resource strings
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.SalespersonKeyId", "Salesperson Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.SalesTerritoryKeyId", "Territory Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.TaxScheduleId", "Default Tax Schedule Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.TaxScheduleIdForCustomersOutsideFlorida", "Default tax schedule id for customers outside of Florida if tax schedule id is null or empty");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.PaymentCardTypeKeyId", "Payment Type Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.CurrencyKeyISOCode", "Currency ISO Code");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.WebServiceAddress", "Web Service URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.DynamicsGPSettings", "Dymanics GP Web Service Configuration");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.UserName", "UserName");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.Password", "Password");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.Domain", "Doamain");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.CompnyNames", "Company Names");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.TestConnection", "Test Connection");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.Back", "Back");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.SuccessSave", "Information save successfully.");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.ErrorNotify", "Please enter credentials.");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.CompanyKey", "Company");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.currencykeyisocode.hint", "Set currency key iso code");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.taxscheduleid.hint", "TaxScheduleKey id of customer address if taxschedule id is null or empty");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.TaxScheduleIdForCustomersOutsideFlorida.hint", "Default tax schedule id for customers outside of Florida if tax schedule id is null or empty");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.paymentcardtypekeyid.hint", "Payment card type key");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.salesterritorykeyid.hint", "Sales territory key");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.salespersonkeyid.hint", "Sales person key");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.password.hint", "Set dynamicsGp password");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.username.hint", "Set dynamicsGP user name");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.domain.hint", "Set dynamicsGP domain");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.webServiceTitle", "Web Services Configuration");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.domainTitle", "Domain Login Information");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.domainDescription", "Please provide login informatrion for the web service. The provided account must be granted the proper permissions in th Dynamics Security Console on the Web Service Server.");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.SaleOrderTitle", "Sales Order Configuration");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.PONumberPrefix", "PO Prefix");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.PONumberPrefix.hint", "Enter PO Prefix");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.status.Connected", "Connected");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.status.NotConnected", "Not Connected");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.status", "Status:");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.companykey.hint", "Company key");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.webserviceaddress.hint", "Web Service URL");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.dynamicsgp.status", "Status");


            // Add Scheduled Tasks
            ScheduleTask gpSendOrdersTask = new ScheduleTask();
            gpSendOrdersTask.Name = "Great Plains Send New Orders DynamicGP2015";
            gpSendOrdersTask.Seconds = 900;
            gpSendOrdersTask.Type = "Nop.Plugin.TorchDesigns.DynamicsGPWCF.SendNewOrdersTaskNew, Nop.Plugin.TorchDesigns.DynamicsGPWCF";
            gpSendOrdersTask.Enabled = false;
            gpSendOrdersTask.StopOnError = false;
            this._scheduleTaskService.InsertTask(gpSendOrdersTask);

            ScheduleTask gpUpdateOrdersTask = new ScheduleTask();
            gpUpdateOrdersTask.Name = "Great Plains Update Order Status DynamicGP2015";
            gpUpdateOrdersTask.Seconds = 900;
            gpUpdateOrdersTask.Type = "Nop.Plugin.TorchDesigns.DynamicsGPWCF.UpdateOrderStatusTaskNew, Nop.Plugin.TorchDesigns.DynamicsGPWCF";
            gpUpdateOrdersTask.Enabled = false;
            gpUpdateOrdersTask.StopOnError = false;
            this._scheduleTaskService.InsertTask(gpUpdateOrdersTask);

            // Add 
            base.Install();
        }

        public override void Uninstall()
        {
            // Delete Settings
            IList<Setting> settings = this._settingService.GetAllSettings();
            foreach (Setting setting in settings)
            {
                if (setting.Name.StartsWith("gp_"))
                {
                    this._settingService.DeleteSetting(setting);
                }
            }

            //Delete payment, sales etc. related settings
            _settingService.DeleteSetting<TorchDesignsDynamicsGPWCFSettings>();

            //Delete locale resource strings
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.SalespersonKeyId");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.SalesTerritoryKeyId");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.TaxScheduleId");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.TaxScheduleIdForCustomersOutsideFlorida");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.PaymentCardTypeKeyId");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.CurrencyKeyISOCode");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.WebServiceAddress");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.DynamicsGPSettings");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.UserName");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.Password");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.Domain");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.CompnyNames");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.TestConnection");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.Back");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.SuccessSave");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.ErrorNotify");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.CompanyKey");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.currencykeyisocode.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.taxscheduleid.hint");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGP.TaxScheduleIdForCustomersOutsideFlorida.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.paymentcardtypekeyid.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.salesterritorykeyid.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.salespersonkeyid.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.password.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.username.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.domain.hint");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.webServiceTitle");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.domainTitle");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.domainDescription");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.SaleOrderTitle");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.status.Connected");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.status.NotConnected");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.DynamicsGp.status");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.companykey.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.webserviceaddress.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.dynamicsgp.status");


            // Delete Scheduled Tasks
            IList<ScheduleTask> scheduledTasks = this._scheduleTaskService.GetAllTasks(true);
            foreach (ScheduleTask scheuledTask in scheduledTasks)
            {
                if (scheuledTask.Name.ToString().Trim() == "Great Plains Send New Orders DynamicGP2015" || scheuledTask.Name.ToString().Trim() == "Great Plains Update Order Status DynamicGP2015")
                {
                    this._scheduleTaskService.DeleteTask(scheuledTask);
                }
            }

            base.Uninstall();
        }

        #endregion



    }
}
