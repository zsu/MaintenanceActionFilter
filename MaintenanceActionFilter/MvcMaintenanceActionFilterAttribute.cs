using SessionMessages.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MaintenanceActionFilter
{
	public class MvcMaintenanceActionFilterAttribute : ActionFilterAttribute
	{
		private const string KeyMaintenanceWarningMessage = "maintenance.warningmessage";
        private const string KeyMaintenanceWarningCookie = "maintenancewarning";
		private IMaintenanceSettingProvider _settingProvider;
		public bool Disabled { get; set; }
		public MvcMaintenanceActionFilterAttribute()
		{
		}
		public MvcMaintenanceActionFilterAttribute(IMaintenanceSettingProvider settingProvider)
		{
			_settingProvider = settingProvider;
		}
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!Disabled)
			{
				if (_settingProvider == null)
					throw new Exception("IMaintenanceSettingProvider must be provided when Disabled is false.");
				var startTime = _settingProvider.StartTimeUtc;
				var endTime = _settingProvider.EndTimeUtc;
				var warningLead = _settingProvider.WarningLeadTime;
				var maintenanceWarningMessage = _settingProvider.MaintenanceWarningMessage;
				bool canBypass = _settingProvider.CanByPass(filterContext.HttpContext.User);
				var requestUrl = filterContext.HttpContext.Request.Url;
				if (!canBypass && startTime != default(DateTime) && DateTime.UtcNow >= startTime)
				{
					if (endTime == default(DateTime) || DateTime.UtcNow <= endTime)
					{
						filterContext.Result = new RedirectResult(_settingProvider.GetMaintenanceUrl(requestUrl));
					}
				}
				else if (startTime != default(DateTime) && startTime > DateTime.UtcNow && warningLead > 0)
				{
                    if (filterContext.HttpContext.Request.Cookies[KeyMaintenanceWarningCookie]==null || filterContext.HttpContext.Request.Cookies[KeyMaintenanceWarningCookie].Value !="1")
                    {
                        var difference = (startTime - DateTime.UtcNow);
                        if (difference.TotalSeconds < warningLead)
                        {
                            SessionMessageManager.SetMessage(MessageType.Warning, MessageBehaviors.Modal, maintenanceWarningMessage, KeyMaintenanceWarningMessage);
                            HttpCookie cookie = new HttpCookie(KeyMaintenanceWarningCookie);
                            cookie.Value = "1";
                            cookie.HttpOnly = true;
                            filterContext.HttpContext.Response.Cookies.Add(cookie);
                        }
                    }
				}
			}
			base.OnActionExecuting(filterContext);
		}
	}
}
