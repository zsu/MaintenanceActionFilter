using SessionMessages.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace MaintenanceActionFilter
{
	public class WebApiMaintenanceActionFilterAttribute : ActionFilterAttribute
	{
		private const string KeyMaintenanceWarningMessage = "maintenance.warningmessage";
        private const string KeyMaintenanceWarningCookie = "maintenancewarning";
        private IMaintenanceSettingProvider _settingProvider;
		public bool Disabled { get; set; }
		public WebApiMaintenanceActionFilterAttribute(IMaintenanceSettingProvider settingProvider)
		{
			_settingProvider = settingProvider;
		}
		public WebApiMaintenanceActionFilterAttribute()
		{
		}
		public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext filterContext)
		{
			if (!Disabled)
			{
				if (!Disabled)
				{
					if (_settingProvider == null)
						throw new Exception("IMaintenanceSettingProvider must be provided when Disabled is false.");
					var startTime = _settingProvider.StartTimeUtc;
					var endTime = _settingProvider.EndTimeUtc;
					var warningLead = _settingProvider.WarningLeadTime;
					var maintenanceWarningMessage = _settingProvider.MaintenanceWarningMessage;
					bool canBypass = _settingProvider.CanByPass(HttpContext.Current.User);
					var requestUrl = filterContext.Request.RequestUri;

					if (!canBypass && startTime != default(DateTime) && DateTime.UtcNow >= startTime)
					{
						if (endTime == default(DateTime) || DateTime.UtcNow <= endTime)
						{
							filterContext.Response = new HttpResponseMessage(HttpStatusCode.OK); //response doesn't work
							string fullyQualifiedUrl = _settingProvider.GetMaintenanceUrl(requestUrl);
							//response.Headers.Location = new Uri(fullyQualifiedUrl);
							filterContext.Response.Headers.Add("FORCE_REDIRECT", fullyQualifiedUrl);
							return;
						}
					}
					if (startTime != default(DateTime) && startTime > DateTime.UtcNow && warningLead > 0)
					{
                        CookieHeaderValue cookie = filterContext.Request.Headers.GetCookies(KeyMaintenanceWarningCookie).FirstOrDefault();
                        if (cookie == null || cookie[KeyMaintenanceWarningCookie].Value!="1")
                        {
                            var difference = (startTime - DateTime.UtcNow);
                            if (difference.TotalSeconds < warningLead)
                            {
                                SessionMessageManager.SetMessage(MessageType.Warning, MessageBehaviors.Modal, maintenanceWarningMessage, KeyMaintenanceWarningMessage);
                                CookieHeaderValue newCookie = new CookieHeaderValue(KeyMaintenanceWarningCookie,"1");
                                newCookie.HttpOnly = true;
                                filterContext.Response.Headers.AddCookies(new CookieHeaderValue[] { newCookie });
                            }
                        }
                    }
				}
				base.OnActionExecuting(filterContext);
			}
		}
	}
}
