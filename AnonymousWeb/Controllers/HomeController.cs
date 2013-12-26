using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LNLImp = LehmaNautaLogicImplementation;
using LNLInt = LehmaNautaLogic.Interface;

namespace AnonymousWeb.Controllers
{
	public class HomeController : Controller
	{
		private const string RepositoryPathKey = "RepositoryPath";
		private const string UploadPath = "~/Uploads";

		public HttpServerUtilityBase _httpContextServer;

		public HomeController() 
			:this( new HttpServerUtilityWrapper( System.Web.HttpContext.Current.Server))
		{
			//_httpContextServer = 
			//	( null == this.HttpContext ) ?
			//	null :
			//	this.HttpContext.Server;
		}

		/// <summary>This constructor is used for automatic testing.
		/// </summary>
		/// <param name="server"></param>
		public HomeController(HttpServerUtilityBase server)
		{
			//TODO:Make this method friend and make it reachable from testing code.
			//	Why?
			_httpContextServer = server;
		}

		public ActionResult Index()
		{
			//	Code copied with pride from http://towardsnext.wordpress.com/2009/04/17/file-upload-in-aspnet-mvc/
			var model = UploadFiles( _httpContextServer, Request.Files);

			var owner = Guid.NewGuid();

			SendFilesToRepository(owner, model.Files);

			return View(model);
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

		private void SendFilesToRepository(Guid owner, ICollection<Models.HomeIndexViewmodel.FileInformation> files)
		{
			var ln = new LNLImp.Factory( ConfigurationManager.AppSettings[RepositoryPathKey]);
			var blobService = ln.CreateBlobService();

			foreach (var file in files)
			{
				blobService.Create(owner.ToString(),
					new LehmaNautaLogic.Implementation.SourcePathfile(
						Path.Combine(_httpContextServer.MapPath(UploadPath), file.Filename))
				);
			}
		}


		/// <summary>This method copies the files from the http stream
		/// into a folder.
		/// </summary>
		/// <param name="httpContextServer"></param>
		/// <param name="requestFiles"></param>
		/// <returns></returns>
		private static Models.HomeIndexViewmodel UploadFiles(
			HttpServerUtilityBase httpContextServer, 
			HttpFileCollectionBase requestFiles
		)
		{
			var ret = Models.HomeIndexViewmodel.Create();

			//	We use a for loop here instead of a foreach because I haven't
			//	got mocking to work with foreach yet.
			for (var i = 0; i < requestFiles.Count; ++i)
			{
				var inputTagName = requestFiles[i];
				HttpPostedFileBase file = requestFiles[i];

				//	If we have a file with a size at all. I don't know why but this seems like a
				//	necessary test.
				if (file.ContentLength > 0)
				{
					//	Create the full path to where we store the file.
					//TODO:	Move "Upload" folder to be a setting either in web config
					//	or as a public property, at least readable. If it remains here it is too "secret".
					var filePath = Path.Combine(
						httpContextServer.MapPath(UploadPath),
						Path.GetFileName(file.FileName));

					//	Save the file on disc.
					file.SaveAs(filePath);

					//	Create something to return.
					ret.Files.Add(
						new Models.HomeIndexViewmodel.FileInformation(
							Path.GetFileName( file.FileName ), 
							file.ContentLength));
				}
			}

			return ret;
		}
	}
}