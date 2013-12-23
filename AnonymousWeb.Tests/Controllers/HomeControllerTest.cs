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
		public void Index()
		{
			// Arrange
			var mHttpContextServer = new Mock<HttpServerUtilityBase>();
			mHttpContextServer.Setup(s => s.MapPath("~/Uploads")).Returns(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location));
			HomeController controller = new HomeController(
				mHttpContextServer.Object
				);
			
			//	Copied with pride from: 
			//	http://blog.csainty.com/2009/01/aspnet-mvc-unit-test-file-upload-with.html
			//	I have't been ableto mock GetEnumerator to make it work with foreach..
			var mFile = new Mock<HttpPostedFileBase>();
			mFile.Setup(f => f.FileName).Returns("MyFilename");
			mFile.Setup(f => f.ContentLength).Returns(42);
			mFile.Setup(f => f.InputStream).Returns(
				new FileStream(@"..\..\UT.Folder\HomeIndex.txt", FileMode.Open, FileAccess.Read));

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
			Assert.AreEqual("MyFilename", model.Files.Single().PathFile);
			Assert.AreEqual(42, model.Files.Single().Length);
			mFile.Verify( f => f.SaveAs(
				Path.Combine( (System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location)),
				@"MyFilename")
			), Times.Once());
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
