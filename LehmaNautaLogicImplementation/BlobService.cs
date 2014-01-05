using System;
using LNLInt = LehmaNautaLogic.Interface;
using System.IO;
using System.Linq;
using LehmaNautaLogic.DTO;
using LehmaNauta.Common;
using LehmaNautaLogic.Implementation;

namespace LehmaNautaLogicImplementation
{
	/// <summary>This class contains the methods needed to upload and download files/blobs.
	/// By the time of writing it contains 3 public methods.
	/// Create(...) for creating/uploading a new blob.
	/// Get(...) for getting/downloading an existing blob.
	/// and the stray DeleteOldFiles that should be called by every Get.
	/// This latter method should be incorporated into Get as we develop further as only 2 (upload/download)
	/// should be necessary.
	/// </summary>
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

		/// <summary>This method deletes old files from the Download folder.
		/// This method is a bit strange; normally this service only handles
		/// the database and the repository. But this method is used for manipulating
		/// the download dir. Why?
		///		Because we don't want anyone to be able to get all Ids - the use of
		///		blobservice should be like a black hole.
		/// </summary>
		/// <param name="path"></param>
		public void DeleteOldFiles(LNLInt.IPath path)
		{
			//TODO:	Unit test this method (BlobService.DeleteOldFiles).		

			//	Get all files in the database. These are the only valid files.
			//	Anything but these should be deleted.
			var fis = new FileInformationService().ToIFileInformationService();
			var allowedDirectoryNames = fis.GetAll().Select( fi => fi.Id.ToString());

			//	Get all directories on disc.
			var existingDirectories = Directory.EnumerateDirectories(path.Value);
			var directoriesToDelete = existingDirectories
				.Where(ed => false == allowedDirectoryNames.Contains(ed));

			//	Use this code to delete files.
			//var allowedFileNames = allowedFiles.Select( fi => fi.Filename);
			////	Get all files that reside on disc.
			//var existingFileNames = Directory.EnumerateFiles( path.Value );
			////	Find all physical files that are not in the fileinformations.
			//var filesToDelete = existingFileNames
			//	.Where( efn => false == allowedFileNames.Contains( efn));
			//filesToDelete.ToList().ForEach( ftd =>
			//	{
			//		//try{
			//			File.Delete( System.IO.Path.Combine( path.Value, ftd));
			//		//}catch( Exception exc){
			//		//	Only catch file delete exceptions, like IO one(s).
			//		//}
			//	}
			//Remove .gitignore and readme.md from files to be deleted
			//	while devloping. How?


			//	TODO:	Then remove them. Catch files-locked-fails.
			directoriesToDelete.ToList().ForEach( d =>
				{
					try{
						Directory.Delete(d, recursive: true);
					}catch( IOException){
						//TODO: Log.
					}
				}
			);
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

			var fis = new FileInformationService().ToIFileInformationService();
			var fileinformation = fis.Get(id);

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
			if (null != fileinformation)
			{
				fis.Delete(fileinformation.Id);	//	Delete the fileinformation from the database.
			}
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
