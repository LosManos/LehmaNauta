using System;
using LNLInt = LehmaNautaLogic.Interface;
using LehmaNautaLogic.DTO;
using System.IO;
using LehmaNauta.Common;
using LehmaNautaLogic.Implementation;

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
			var id = fis.Create(new FileInformation().Set(
				System.IO.Path.GetFileName( sourcePathfile.Value), 
				owner));

			LNLInt.IPhysicalfileService pfs = new PhysicalfileService(_repositoryPathfile);
			pfs.Create(id, sourcePathfile);

			System.IO.File.Delete(sourcePathfile.Value);

			return id;
		}

		/// <summary>This method retrieves the Fileinformation for the file/blob for the id
		/// and copies the very file to the TargetPath.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="targetPath"></param>
		/// <returns></returns>
		public LehmaNautaLogic.DTO.FileInformation Get(Guid id, LNLInt.ITargetPath targetPath)
		{
			Assert.Argument.Called("targetPath").IsNotNull(targetPath);

			LNLInt.IFileInformationService fis = new FileInformationService();
			var fileinformation = fis.Load(id);

			LNLInt.IPhysicalfileService pfs = new PhysicalfileService(_repositoryPathfile);
			if (pfs.Exists(id))
			{
				using (var filestream = pfs.Get(id))
				{

					CreateDownloadDirectoryAndCopyFile(fileinformation, filestream, targetPath);
				}
				pfs.Delete(id);	//	Delete the file from the repository.
				fis.Delete(fileinformation.Id);	//	Delete the fileinformation from the database.
				return fileinformation;

			}
			fis.Delete(fileinformation.Id);	//	Delete the fileinformation from the database.
			return null;
		}

		/// <summary>This method creates the file to download from by copying it
		/// from the repository.
		/// </summary>
		/// <param name="fileinformation"></param>
		/// <param name="filestream"></param>
		/// <param name="targetPath"></param>
		private void CreateDownloadDirectoryAndCopyFile(
			FileInformation fileinformation, 
			FileStream filestream, 
			LNLInt.ITargetPath targetPath )
		{
			Assert.Argument.Called("targetPath").IsNotNull(targetPath);

			//	Decide path and filename to let the user download.
			var targetPathfile = new TargetPathfile( 
				System.IO.Path.Combine( targetPath.Value, fileinformation.Filename)
			).ToITargetPathfile();

			//	Now create directory and the very file.
			Directory.CreateDirectory(targetPath.Value);
			using (var fs = new FileStream(targetPathfile.Value, FileMode.CreateNew))
			{
				filestream.CopyTo(fs);
			}
		}

	}
}
