using System;
using System.Threading;
using LehmaNautaLogic.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LNL = LehmaNautaLogic;
using LNLInt = LehmaNautaLogic.Interface;
using LNLImp = LehmaNautaLogicImplementation;

namespace LehmaNautaLogic.Test
{
	[TestClass]
	public class FileInfoServiceTest
	{
		[TestInitialize]
		public void Init()
		{
			LNLInt.IFileInformationService logic = new LNLImp.FileInformationService();
			logic.EnsureDatabaseExists();
		}

		[TestMethod]
		public void Create()
		{
			var before = DateTime.Now;
			LNLInt.IFileInformationService testee = new LNLImp.FileInformationService();
			var fiIeID = testee.Create(new DTO.FileInformation().Set("IT.myfilename", "IT.myowner"));
			Assert.AreNotEqual(Guid.Empty, fiIeID);

			var fileinfo = testee.Get(fiIeID);
			Assert.AreEqual(fiIeID, fileinfo.Id);
			Assert.AreEqual("IT.myfilename", fileinfo.Filename);
			Assert.AreEqual( "IT.myowner", fileinfo.Owner);
			Assert.IsTrue(before <= fileinfo.Created);
		}

		[TestMethod]
		public void Delete()
		{
			LNLInt.IFileInformationService testee = new LNLImp.FileInformationService();
			var id = Guid.NewGuid();
			var preCount = testee.GetAll().Count;
			var newFileinfo = new FileInformation().Set("IT.a", "IT.b");
			newFileinfo.Id = Guid.NewGuid();
			testee.Create(newFileinfo);
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.
			var postCount = testee.GetAll().Count;
			Assert.AreEqual(preCount + 1, postCount);
			testee.Delete(newFileinfo.Id);
			var postPostCount = testee.GetAll().Count;
			Assert.AreEqual(preCount, postPostCount);
		}

		[TestMethod]
		public void DeleteOld()
		{
			//	Given.
			LNLInt.IFileInformationService testee = new LNLImp.FileInformationService();
			testee.DeleteOld();
			var preCount = testee.GetAll().Count;
			var pastFileinfo = new FileInformation().Set("IT.a", "IT.b");
			pastFileinfo.Created = DateTime.Now.AddDays(-1).AddHours(-1);
			testee.Create(pastFileinfo);
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.
			var postCount = testee.GetAll().Count;
			Assert.IsTrue(preCount < postCount);
			testee.DeleteOld();
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.

			//	Do.
			var postPostCount = testee.GetAll().Count;
	
			//	Assert.
			Assert.IsTrue(preCount <= postPostCount);
		}

		[TestMethod]
		public void GetAll()
		{
			LNLInt.IFileInformationService testee = new LNLImp.FileInformationService();
			var allPreTestDocs = testee.GetAll();
			var fileId = testee.Create(new DTO.FileInformation().Set("IT.a", "IT.b"));
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.
			var allPostTestDocs = testee.GetAll();
			Assert.IsTrue(allPreTestDocs.Count + 1 == allPostTestDocs.Count);
		}
	}
}
