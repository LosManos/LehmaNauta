using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnonymousWeb;
using AnonymousWeb.Controllers;
using Moq;
using System.Web;
using System.IO;

namespace AnonymousWeb.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Home_Index_WithNoArgument()
		{
			//	#	Arrange.
			//	##	Arrange folders and files. At least names of.
			var Filename = "HomeIndex.txt";	//	Presently the only file we juggle in this test.
			var UploadPath = //	Where we mock HomeController.Index is storing the files it is sent.
					Path.Combine(
						System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), 
						@"..\..\Uploads"
					);
			var UploadPathfile = Path.Combine( UploadPath, Filename);
			var UtPath = @"..\..\UT.Folder";	//	Where we find our files to work with.
			var UtPathfilename = Path.Combine( UtPath, Filename);

			//	##	Arrange mocking of HttpContextServer.
			var mHttpContextServer = new Mock<HttpServerUtilityBase>();
			mHttpContextServer.Setup(s => s.MapPath("~/Uploads"))
				.Returns( UploadPath );
			HomeController controller = new HomeController(
				mHttpContextServer.Object
			);
			
			//	##	Arrange mocking of HttpPostedFilebase.
			//	Copied with pride from: 
			//	http://blog.csainty.com/2009/01/aspnet-mvc-unit-test-file-upload-with.html
			//	I have't been ableto mock GetEnumerator to make it work with foreach.
			var mFile = new Mock<HttpPostedFileBase>();
			mFile.Setup(f => f.FileName).Returns(Filename);	//	This is the file name we get from the web client/user.
			mFile.Setup(f => f.ContentLength).Returns(42);	//	This is the length of the file we get from the web client/user. It shouldn't be hard coded to 42 but instead read from the file we use when testing but hard coding a value solves the problem for now.
			mFile.Setup(f => f.InputStream).Returns(
				new FileStream (UtPathfilename, FileMode.Open, FileAccess.Read)
				);	//	Here is another mocking trick; we use a physical file for returning a stream.
			
			//	Mock file.SaveAs. Since we only call it once we don't bother about the parameter.
			//	SaveAs saves a file. We have to do that manually here since we mock.
			//	Can we use the value of the parameter to SaveAs (presently It.IsAny<string>
			//	as a target for Uploadfile?
			mFile.Setup(f => f.SaveAs(It.IsAny<string>())).Callback(() =>
				{
					File.Copy(UtPathfilename, UploadPathfile, true);
				}
			);

			//	##	Arrange mocking of ControllerContext.
			//	This is used by the controller to handle the file(s) that come(s) with the request.
			var mCC = new Mock<ControllerContext>();
			mCC.Setup(d => d.HttpContext.Request.Files.Count).Returns(1);
			mCC.Setup(d => d.HttpContext.Request.Files[0]).Returns(mFile.Object);
				
			controller.ControllerContext = mCC.Object;

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Model, typeof(Models.HomeIndexViewmodel));
			var model = (Models.HomeIndexViewmodel)result.Model;
			Assert.AreEqual(1, model.Files.Count);
			Assert.AreEqual(Filename, model.Files.Single().Filename);
			Assert.AreEqual(42, model.Files.Single().Length);
			mFile.Verify( f => f.SaveAs(It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void About()
		{
			// Arrange
			var mHttpContextServer = new Mock<HttpServerUtilityBase>();
			HomeController controller = new HomeController(
				mHttpContextServer.Object
			);

			// Act
			ViewResult result = controller.About() as ViewResult;

			// Assert
			Assert.AreEqual("Your application description page.", result.ViewBag.Message);
		}

		[TestMethod]
		public void Contact()
		{
			// Arrange
			var mHttpContextServer = new Mock<HttpServerUtilityBase>();
			HomeController controller = new HomeController(
				mHttpContextServer.Object
			);

			// Act
			ViewResult result = controller.Contact() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
