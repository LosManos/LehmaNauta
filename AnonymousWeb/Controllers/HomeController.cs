using LehmaNautaLogic.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
		internal HomeController(HttpServerUtilityBase server)
		{
			_httpContextServer = server;
		}

		#endregion	//	Constructors.

		#region Actions.

		public ActionResult Index()
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

			var retModel = StoreFiles();

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

		public FileStreamResult Download(string id)
		{
			//	#	Setup.
			var uid = Guid.Parse(id);
			var downloadPathForFile = 
				new LehmaNautaLogic.Implementation.TargetPath(
					Path.Combine(Common.Settings.DownloadPath, uid.ToString()
				));
			var ln = new LNLImp.Factory(Common.Settings.RepositoryPath);
			var blobService = ln.CreateBlobService();
			//	##	Clean the download directory for old stuff.
			blobService.DeleteOldFiles(
				new LehmaNautaLogic.Implementation.Path(
					Common.Settings.DownloadPath
				)
			);

			//	#	Get data.
			var model = GetFileInformationFromRepository(blobService, uid, downloadPathForFile);

			//	#	Return the file as a stream. If it exists that is.
			//	Code below is inspired with love from:
			//	http://stackoverflow.com/questions/1375486/how-to-create-file-and-return-it-via-fileresult-in-asp-net-mvc
			var fileinfo = new FileInfo(
				Path.Combine(downloadPathForFile.Value, model.Files.Single().Filename)
			);
			if (fileinfo.Exists && fileinfo.Length >= 1 )
			{
				return File(fileinfo.OpenRead(), MimeMapping.GetMimeMapping( model.Files.Single().Filename), model.Files.Single().Filename);
			}
			else
			{
				return null;
			}
		}

		#endregion	//	Actions.

		#region Private methods.

		/// <summary>This method creates file information in the database.
		/// 
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="files"></param>
		private void CreateFileInformationInRepository(Guid owner, ICollection<Models.HomeIndexViewmodel.FileInformation> files)
		{
			var ln = new LNLImp.Factory( Common.Settings.RepositoryPath);
			var blobService = ln.CreateBlobService();

			foreach (var file in files)
			{
				var id = blobService.Create(owner.ToString(),
					new LehmaNautaLogic.Implementation.SourcePathfile(
						Path.Combine(
						//_httpContextServer.MapPath(Common.Settings.UploadPath)
						Common.Settings.UploadPath, 
						file.Filename))
				);
				Debug.Assert(Guid.Empty != id);
				file.Id = id;
			}
		}

		/// <summary>This method retrieves the file information from the database
		/// and copies the file to the folder in the path.
		/// The file is then \MyDownloadpath\xxx\yyy
		/// where xxx is the UID for the blob
		/// and yyy is the name of the file.
		/// </summary>
		/// <param name="blobService"></param>
		/// <param name="fileinformationId"></param>
		/// <param name="downloadPath"></param>
		/// <returns></returns>
		private static Models.HomeIndexViewmodel GetFileInformationFromRepository(
			LNLInt.IBlobService blobService, 
			Guid fileinformationId, 
			LNLInt.ITargetPath downloadPath )
		{
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
						fileinformation.Id, 
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
					var filePath = Path.Combine(
						Common.Settings.UploadPath, 
						Path.GetFileName(file.FileName));

					//	Save the file on disc.
					file.SaveAs(filePath);

					//	Create something to return.
					ret.Files.Add(
						new Models.HomeIndexViewmodel.FileInformation(
							Guid.Empty,	//	We still don't have an Id for the file.
							Path.GetFileName( file.FileName ), 
							file.ContentLength));
				}
			}

			return ret;
		}

		#endregion	//	Private methods.
	}
}