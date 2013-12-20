using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnonymousWeb.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			//	Code copied with pride from http://towardsnext.wordpress.com/2009/04/17/file-upload-in-aspnet-mvc/
			var ret = UploadFiles(Request.Files);
			return View(ret);
		}

		private Models.HomeIndexViewmodel UploadFiles(HttpFileCollectionBase requestFiles)
		{
			var ret = Models.HomeIndexViewmodel.Create();
			foreach (string inputTagName in requestFiles)
			{
				HttpPostedFileBase file = Request.Files[inputTagName];
				if (file.ContentLength > 0)
				{
					var filePath = Path.Combine(HttpContext.Server.MapPath("~/Uploads"),
						Path.GetFileName(file.FileName));
					file.SaveAs(filePath);
				}
				ret.Files.Add(
					new Models.HomeIndexViewmodel.FileInformation(file.FileName, file.ContentLength));
			}
			return ret;
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}