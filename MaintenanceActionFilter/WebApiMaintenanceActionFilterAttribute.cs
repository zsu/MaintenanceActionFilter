using SessionMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace MaintenanceActionFilter
{
	public class WebApiMaintenanceActionFilterAttribute : ActionFilterAttribute
	{
		private const string KEYS_MAINTENANCE_WARNING_MESSAGE = "maintenance.warningmessage";
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
						var difference = (startTime - DateTime.UtcNow);
						if (difference.TotalSeconds < warningLead)
						{
							SessionMessageManager.SetMessage(MessageType.Warning, MessageBehaviors.StatusBar, maintenanceWarningMessage, KEYS_MAINTENANCE_WARNING_MESSAGE);
						}
					}
				}
				base.OnActionExecuting(filterContext);
			}
		}
	}
}
