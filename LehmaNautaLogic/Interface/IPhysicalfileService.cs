using System;

namespace LehmaNautaLogic.Interface
{
	public interface IPhysicalfileService
	{
		void Create(Guid id, ISourcePathfile sourcePathfile);
		string GetAndDelete(Guid id);
		IPathfile RepositoryPath { get; set; }
		bool Exists(Guid id);
		}
}
