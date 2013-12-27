using System;

namespace LehmaNautaLogic.Interface
{
	public interface IBlobService
	{
		Guid Create(string owner, ISourcePathfile sourcePathfile);
		DTO.FileInformation Get(Guid id, ITargetPath targetPath);
	}
}
