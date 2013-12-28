using System;
using System.IO;

namespace LehmaNautaLogic.Interface
{
	public interface IPhysicalfileService
	{
		void Create(Guid id, ISourcePathfile sourcePathfile);
		FileStream Get(Guid id);
		IPathfile RepositoryPath { get; set; }
		bool Exists(Guid id);
		}
}
