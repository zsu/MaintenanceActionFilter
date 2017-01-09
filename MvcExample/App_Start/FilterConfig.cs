using MaintenanceActionFilter;
using System.Web;
using System.Web.Mvc;
using System;
using System.Security.Principal;

namespace MvcExample
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new MvcMaintenanceActionFilterAttribute(new MaintenanceSettingProvider()));
		}
		public static void RegisterWebApiGlobalFilters()
		{
			System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new WebApiMaintenanceActionFilterAttribute(new MaintenanceSettingProvider()));
		}
	}
	public class MaintenanceSettingProvider : IMaintenanceSettingProvider
	{
		public DateTime EndTimeUtc
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		public string GetMaintenanceUrl(Uri requestUrl)
		{
			string fullyQualifiedUrl = string.Format("{0}/Maintenance", requestUrl.GetLeftPart(UriPartial.Authority));
			return fullyQualifiedUrl;
		}

		public string MaintenanceWarningMessage
		{
			get
			{
				return string.Format("The site is going to be down for maintenance at {0} UTC.", StartTimeUtc);
			}
		}

		public DateTime StartTimeUtc
		{
			get
			{
				return DateTime.UtcNow.AddHours(2);
			}
		}

		public double WarningLeadTime
		{
			get
			{
				return 24 * 60 * 60;
			}
		}

		public bool CanByPass(IPrincipal user)
		{
			return false;
		}
	}
}
