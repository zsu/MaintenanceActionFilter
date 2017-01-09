using MaintenanceActionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace MvcExample.Controllers
{
	public class MaintenanceController : Controller
	{
		[MvcMaintenanceActionFilter(Disabled = true)]
		public ActionResult Index()
		{
			return View();
		}
	}
}