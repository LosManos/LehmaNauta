using System;
using System.Threading;
using LehmaNautaLogic.DTO;
using LNLI = LehmaNautaLogicImplementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LehmaNautaLogic.Test
{
	[TestClass]
	public class FileInfoServiceTest
	{
		[TestInitialize]
		public void Init()
		{
			IFileInformationService logic = new LNLI.FileInformationService();
			logic.EnsureDatabaseExists();
		}

		[TestMethod]
		public void Create()
		{
			var before = DateTime.Now;
			IFileInformationService testee = new LNLI.FileInformationService();
			var fiIeID = testee.Create(new DTO.FileInformation().Set("IT.myfilename", "IT.myowner"));
			Assert.AreNotEqual(Guid.Empty, fiIeID);

			var fileinfo = testee.Load(fiIeID);
			Assert.AreEqual(fiIeID, fileinfo.Id);
			Assert.AreEqual("IT.myfilename", fileinfo.Filename);
			Assert.AreEqual( "IT.myowner", fileinfo.Owner);
			Assert.IsTrue(before <= fileinfo.Created);
		}

		[TestMethod]
		public void Delete()
		{
			IFileInformationService testee = new LNLI.FileInformationService();
			var id = Guid.NewGuid();
			var preCount = testee.IT_GetAll().Count;
			var newFileinfo = new FileInformation().Set("IT.a", "IT.b");
			newFileinfo.Id = Guid.NewGuid();
			testee.Create(newFileinfo);
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.
			var postCount = testee.IT_GetAll().Count;
			Assert.AreEqual(preCount + 1, postCount);
			testee.Delete(newFileinfo.Id);
			var postPostCount = testee.IT_GetAll().Count;
			Assert.AreEqual(preCount, postPostCount);
		}

		[TestMethod]
		public void DeleteOld()
		{
			var testee = new LNLI.FileInformationService();
			var preCount = testee.IT_GetAll().Count;
			var pastFileinfo = new FileInformation().Set("IT.a", "IT.b");
			pastFileinfo.Created = DateTime.Now.AddDays(-1).AddHours(-1);
			testee.Create(pastFileinfo);
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.
			var postCount = testee.IT_GetAll().Count;
			Assert.IsTrue(preCount < postCount);
			testee.DeleteOld();
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.
			var postPostCount = testee.IT_GetAll().Count;
			Assert.IsTrue(preCount <= postPostCount);
		}

		[TestMethod]
		public void GetAll()
		{
			var testee = new LNLI.FileInformationService();
			var allPreTestDocs = testee.IT_GetAll();
			var fileId = testee.Create(new DTO.FileInformation().Set("IT.a", "IT.b"));
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.
			var allPostTestDocs = testee.IT_GetAll();
			Assert.IsTrue(allPreTestDocs.Count + 1 == allPostTestDocs.Count);
		}
	}
}
