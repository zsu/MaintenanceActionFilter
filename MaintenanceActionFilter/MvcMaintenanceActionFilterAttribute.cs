using SessionMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MaintenanceActionFilter
{
	public class MvcMaintenanceActionFilterAttribute : ActionFilterAttribute
	{
		private const string KEYS_MAINTENANCE_WARNING_MESSAGE = "maintenance.warningmessage";
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
