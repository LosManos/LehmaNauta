using System;
using LNLInt = LehmaNautaLogic.Interface;
using LehmaNautaLogic.DTO;

namespace LehmaNautaLogicImplementation
{
	internal class BlobService : LNLInt.IBlobService
	{
		private readonly LNLInt.IPathfile _repositoryPathfile;

		public BlobService(LNLInt.IPathfile repositoryPathfile)
		{
			_repositoryPathfile = repositoryPathfile;
		}

		/// <summary>This method creates a FilInformation record and a file in the file system.
		/// The record is identified by the key (a guid) returned.
		/// Through this one can also find the file.
		/// Sideffect:
		/// Finally the original file is deleted.
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="sourcePathfile"></param>
		/// <returns></returns>
		public Guid Create(string owner, LNLInt.ISourcePathfile sourcePathfile)
		{
			LNLInt.IFileInformationService fis = new FileInformationService();
			var id = fis.Create(new FileInformation().Set(sourcePathfile.Value, owner));

			LNLInt.IPhysicalfileService pfs = new PhysicalfileService(_repositoryPathfile);
			pfs.Create(id, sourcePathfile);

			System.IO.File.Delete(sourcePathfile.Value);

			return id;
		}

		public bool Get(Guid id, LNLInt.ITargetPathfile targetPathfile)
		{
			LNLInt.IPhysicalfileService pfs = new PhysicalfileService(_repositoryPathfile);
			if (pfs.Exists(id))
			{
				var filecontents = pfs.Get(id);
				System.IO.File.WriteAllText(targetPathfile.Value, filecontents);
				return true;
			}
			return false;
		}

	}
}
