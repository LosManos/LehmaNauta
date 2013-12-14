using LNL = LehmaNautaLogic;
using LNLInt = LehmaNautaLogic.Interface;

namespace LehmaNautaLogicImplementation
{
	/// <summary>This is a Very simple factory class.
	/// </summary>
	public static class Factory
	{
		private const string RepositoryPath = @"..\..\TI.Repofolder";	//	TODO:	Make settable proper.

		public static LNLInt.IBlobService CreateBlobService()
		{
			return new BlobService(new LNL.Implementation.TargetPathfile(RepositoryPath));
		}
	}
}
