using System;
using System.IO;

namespace LehmaNautaLogic.Interface
{
	public interface IPhysicalfileService
	{
		IPathfile RepositoryPath { get; set; }

		void Create(Guid id, ISourcePathfile sourcePathfile);
		FileStream Get(Guid id);
		void Delete(Guid id);
		bool Exists(Guid id);
		}
}
