# What is MaintenanceActionFilter

MaintenanceActionFilter is a asp.net MVC/Web Api action filter library for maintenance message handling.

# Nuget
```xml
Install-Package MaintenanceActionFilter
```
# Getting started with MaintenanceActionFilter 

  * Reference MaintenanceActionFilter.dll and SessionMessage.dll (https://github.com/zsu/SessionMessage)
  * Create class MaintenanceSettingProvider that implement interface IMaintenanceSettingProvider
```xml
  * StartTimeUtc: UTC time to start maintenance; Set to default datetime value will disable maintenance detection.
  * EndTimeUtc: UTC time to end maintenance.
  * WarningLeadTime: Time in seconds before the start time to display the maintenance warning message. Set to 0 to disable warning message.
  * MaintenanceWarningMessage: Warning message to display.
  * GetMaintenanceUrl: Maintenance page url.
  * CanByPass: Return true to bypass maintenance detection in certain circumstance.
```
  * Register it in filterconfig.cs:
```js
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
```js
		[MvcMaintenanceActionFilter(Disabled = true)]
```
  * Setup SessionMessage: Please refer to https://github.com/zsu/SessionMessage

# Screenshots
![MaintenanceActionFilter](screenshots/maintenancemessage.jpg?raw=true "modaldialog")

# License
All source code is licensed under MIT license - http://www.opensource.org/licenses/mit-license.php
