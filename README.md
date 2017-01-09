#What is MaintenanceActionFilter

MaintenanceActionFilter is a asp.net MVC/Web Api action filter library for maintenance message handling.

#Getting started with MaintenanceActionFilter 

  * Reference MaintenanceActionFilter.dll and SessionMessage.dll(https://github.com/zsu/SessionMessage)
  * Create class MaintenanceSettingProvider that implement interface IMaintenanceSettingProvider
  * Register it in filterconfig.cs:
```xml
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new MvcMaintenanceActionFilterAttribute(new MaintenanceSettingProvider()));
		}
		public static void RegisterWebApiGlobalFilters()
		{
			System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new WebApiMaintenanceActionFilterAttribute(new MaintenanceSettingProvider()));
		}
```
  * You can disable the filter on individual action method:
```xml
		[MvcMaintenanceActionFilter(Disabled = true)]
```
  * Setup SessionMessage: Please refer to https://github.com/zsu/SessionMessage

#License
All source code is licensed under MIT license - http://www.opensource.org/licenses/mit-license.php
