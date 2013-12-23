using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LNLI = LehmaNautaLogicImplementation;

namespace AnonymousWeb.Controllers
{
	public class HomeController : Controller
	{
		public HttpServerUtilityBase _httpContextServer;

		public HomeController() 
		{
			_httpContextServer = this.HttpContext.Server;
		}

		/// <summary>This constructor is used for automatic testing.
		/// </summary>
		/// <param name="server"></param>
		public HomeController(HttpServerUtilityBase server)
		{
			//TODO:Make this method friend and make it reachable from testing code.
			_httpContextServer = server;
		}

		public ActionResult Index()
		{
			//	Code copied with pride from http://towardsnext.wordpress.com/2009/04/17/file-upload-in-aspnet-mvc/
			var ret = UploadFiles(Request.Files);

//			var lnl = new LNLI.Factory();

			return View(ret);
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

		/// <summary>This method copies the files from the http stream
		/// into a folder.
		/// </summary>
		/// <param name="requestFiles"></param>
		/// <returns></returns>
		private Models.HomeIndexViewmodel UploadFiles(HttpFileCollectionBase requestFiles)
		{
			var ret = Models.HomeIndexViewmodel.Create();

			//	We use a for loop here instead of a foreach because I haven't
			//	got mocking to work with foreach yet.
			for (var i = 0; i < Request.Files.Count; ++i)
			{
				var inputTagName = Request.Files[i];
				HttpPostedFileBase file = Request.Files[i];
				if (file.ContentLength > 0)
				{
					var filePath = Path.Combine(_httpContextServer.MapPath("~/Uploads"),
						Path.GetFileName(file.FileName));
					file.SaveAs(filePath);
				}
				ret.Files.Add(
					new Models.HomeIndexViewmodel.FileInformation(file.FileName, file.ContentLength));
			}

			return ret;
		}
	}
}