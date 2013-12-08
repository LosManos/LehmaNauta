using System;
using System.Threading;
using LehmaNautaLogic.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LehmaNautaLogic.Test
{
	[TestClass]
	public class FileInfoTest
	{
		[TestInitialize]
		public void Init()
		{
			var logic = new LehmaNautaLogic.FileInfoService();
			logic.EnsureDatabaseExists();
		}

		[TestMethod]
		public void Create()
		{
			var before = DateTime.Now;
			var testee = new LehmaNautaLogic.FileInfoService();
			var fiIeID = testee.Create(new DTO.FileInfo().Set("IT.myfilename", "IT.myowner"));
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
			var testee = new LehmaNautaLogic.FileInfoService();
			var id = Guid.NewGuid();
			var preCount = testee.IT_GetAll().Count;
			var newFileinfo = new FileInfo().Set("IT.a", "IT.b");
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
			var testee = new LehmaNautaLogic.FileInfoService();
			var preCount = testee.IT_GetAll().Count;
			var pastFileinfo = new FileInfo().Set("IT.a", "IT.b");
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
			var testee = new LehmaNautaLogic.FileInfoService();
			var allPreTestDocs = testee.IT_GetAll();
			var fileId = testee.Create(new DTO.FileInfo().Set("IT.a", "IT.b"));
			Thread.Sleep(500);	//	HACK: Shouldn't have to wait.
			var allPostTestDocs = testee.IT_GetAll();
			Assert.IsTrue(allPreTestDocs.Count + 1 == allPostTestDocs.Count);
		}
	}
}
