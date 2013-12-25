using System.Diagnostics;
using LNL = LehmaNautaLogic;
using LNLInt = LehmaNautaLogic.Interface;

namespace LehmaNautaLogicImplementation
{
	/// <summary>This is a Very simple factory class.
	/// </summary>
	public class Factory
	{
		private readonly string _repositoryPath;
		//@"..\..\TI.Repofolder";	//	TODO:	Make settable proper.

		public Factory( string repositoryPath )
		{
			Debug.Assert(false == string.IsNullOrEmpty( repositoryPath ), "Null value not allowed.");
			_repositoryPath = repositoryPath;
		}

		public LNLInt.IBlobService CreateBlobService()
		{
			return new BlobService(new LNL.Implementation.TargetPathfile(_repositoryPath));
		}
	}
}
