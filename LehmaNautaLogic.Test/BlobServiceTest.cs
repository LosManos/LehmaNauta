using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LNL = LehmaNautaLogic;
using LNLInt = LehmaNautaLogic.Interface;
using LNLImp = LehmaNautaLogicImplementation;
using System.IO;

namespace LehmaNautaLogic.Test
{
	[TestClass]
	public class BlobServiceTest
	{
		private const string RepoFolder  = @"..\..\IT.Repository";
		private const string FileFolder = @"..\..\IT.Folder";
		private const string WorkFolder = @"..\..\IT.WorkFolder";

		[TestMethod]
		public void Create()
		{
			//	Given.
			const string Filename = "Create.txt";
			File.Copy( 
				Path.Combine( FileFolder, Filename), 
				Path.Combine( WorkFolder, Filename),
				true
			);
			var owner = "IT." + Guid.NewGuid().ToString();
			var blobService = new LNLImp.Factory(RepoFolder).CreateBlobService();

			//	Do.
			blobService.Create(owner, new LNL.Implementation.SourcePathfile(
				Path.Combine( WorkFolder, Filename)));

			//	Assert.
			Assert.IsFalse(File.Exists(
				Path.Combine( WorkFolder, Filename)
			));
			//	There really isn't any way to prove the file has been stored somewhere
			//	without knowing and duplicaing the inner workings of LNL.
			//	We have to trust another integration test to store and retrieve said file.
		}

		[TestMethod]
		public void Get()
		{
			//	Given.
			const string Filename1 = "Get.txt";
			const string Filename2 = "Get.result.txt";
			var sourcePathfile = new LNL.Implementation.SourcePathfile( 
				Path.Combine(WorkFolder, Filename1)
			);
			File.Copy(Path.Combine(FileFolder, Filename1), sourcePathfile.Value);
			Assert.IsTrue( File.Exists( sourcePathfile.Value));
			var targetPathfile = new LNL.Implementation.TargetPathfile(
				Path.Combine(WorkFolder, Filename2)
			);
			File.Delete(targetPathfile.Value);
			Assert.IsFalse( File.Exists( targetPathfile.Value));

			var owner = "IT." + Guid.NewGuid().ToString();
			var blobService = new LNLImp.Factory(RepoFolder).CreateBlobService();
			var id = blobService.Create(owner, sourcePathfile);
			System.IO.File.Delete(targetPathfile.Value);
			Assert.IsFalse(System.IO.File.Exists(targetPathfile.Value));

			//	Do.
			var res = blobService.Get(id, targetPathfile);

			//	Assert.
			Assert.IsTrue(res);
			Assert.IsTrue(System.IO.File.Exists(targetPathfile.Value));
		}
	}
}
