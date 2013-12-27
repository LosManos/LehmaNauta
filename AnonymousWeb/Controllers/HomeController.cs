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
		private const string DownloadPath = "~/Downloads";

		public HttpServerUtilityBase _httpContextServer;

		#region Constructors.
		
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

		#endregion	//	Constructors.

		public ActionResult Index( string id = null)
		{
			///	This function stores the file(s) in the request in the respository and database.
			Func<Models.HomeIndexViewmodel> StoreFiles = delegate()
			{
				//	Code copied with pride from http://towardsnext.wordpress.com/2009/04/17/file-upload-in-aspnet-mvc/
				var model = UploadFiles(_httpContextServer, Request.Files);

				var owner = Guid.NewGuid();

				CreateFileInformationInRepository(owner, model.Files);

				return model;
			};

			///	This function retrieves a file from the respository and database.
			Func<Guid, Models.HomeIndexViewmodel> RetrieveFiles = delegate(Guid uid)
			{
				var downloadPathForFile =
					new LehmaNautaLogic.Implementation.TargetPath(
						_httpContextServer.MapPath( Path.Combine( DownloadPath, uid.ToString() )
					));
				var model = GetFileInformationFromRepository(uid, downloadPathForFile);
				return model;
			};

			var retModel = (null == id) ?
				StoreFiles() :
				RetrieveFiles(Guid.Parse(id) );

			return View(retModel);
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

		/// <summary>This method creates file information in the database.
		/// 
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="files"></param>
		private void CreateFileInformationInRepository(Guid owner, ICollection<Models.HomeIndexViewmodel.FileInformation> files)
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

		/// <summary>This method retrieves the file information from the database
		/// and copies the file to the folder in the path.
		/// The file is then \MyDownloadpath\xxx\yyy
		/// where xxx is the UID for the blob
		/// and yyy is the name of the file.
		/// </summary>
		/// <param name="fileinformationId"></param>
		/// <param name="downloadPath"></param>
		/// <returns></returns>
		private static Models.HomeIndexViewmodel GetFileInformationFromRepository(
			Guid fileinformationId, 
			LNLInt.ITargetPath downloadPath )
		{
			var ln = new LNLImp.Factory(ConfigurationManager.AppSettings[RepositoryPathKey]);
			var blobService = ln.CreateBlobService();

			var ret = Models.HomeIndexViewmodel.Create();

			var fileinformation= blobService.Get(
				fileinformationId,
				downloadPath
			);

			if (null == fileinformation)
			{
				//	The blob/file couldn't be fetched.
			}
			else
			{
				ret.Files.Add(
					new Models.HomeIndexViewmodel.FileInformation(
						fileinformation.Filename,
						0	//TODO:	Implement to get real size of file.
					)
				);
			}

			return ret;
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