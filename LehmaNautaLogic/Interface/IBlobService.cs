using System;
using System.Collections.Generic;

namespace LehmaNautaLogic.Interface
{
	public interface IBlobService
	{
		Guid Create(string owner, ISourcePathfile sourcePathfile);

		/// <summary>This method deletes all files that are not in the database
		/// in the folder specified.
		/// </summary>
		/// <param name="path"></param>
		void DeleteOldFiles(IPath path);
	
		DTO.FileInformation Get(Guid id, ITargetPath targetPath);
	}
}
