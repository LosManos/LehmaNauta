using System;
using System.IO;

namespace LehmaNautaLogic.Interface
{
	public interface IPhysicalfileService
	{
		IPathfile RepositoryPath { get; set; }

		void Create(Guid id, ISourcePathfile sourcePathfile);
		void Delete(Guid id);
		bool Exists(Guid id);
		FileStream Get(Guid id);
	}
}
