﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LNL = LehmaNautaLogic;
using LNLInt = LehmaNautaLogic.Inferface;
using LNLImp = LehmaNautaLogicImplementation;

namespace LehmaNautaLogic.Test
{
	[TestClass]
	public class FileServiceTest
	{
		private const string RepositoryPath = @"..\..\TI.RepoFolder";

		[TestMethod]
		public void FullRobin()
		{
			const string SourcePathfile = @"..\..\Incoming\Fullrobin.txt";
			CreateSourceFile(SourcePathfile);

			LNLInt.IPhysicalfileService testee = new LNLImp.PhysicalfileService(RepositoryPath);
			var id = Guid.NewGuid();

			testee.Create(id, new LNL.Implementation.SourcePathfile(SourcePathfile));
	
			var exists = testee.Exists(id);
			Assert.IsTrue(exists);
			
			var res = testee.Get(id);
			Assert.IsNotNull(res);	//TODO:What is Get really?
			
			exists = testee.Exists(id);
			Assert.IsFalse(exists);
		}

		private void CreateSourceFile( string pathFile ){
			System.IO.File.WriteAllText( pathFile, "räksmörgås" );
		}
	}
}
