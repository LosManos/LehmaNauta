using System;

namespace LehmaNautaLogic.Inferface
{
	public interface IPhysicalfileService
	{
		void Create(Guid id, ISourcePathfile sourcePathfile);
		string Get(Guid id);
		string RepositoryPath { get; set; }
		bool Exists(Guid id);
		}
}
