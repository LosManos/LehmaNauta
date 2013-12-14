using System;

namespace LehmaNautaLogic.Interface
{
	public interface IBlobService
	{
		Guid Create(string owner, ISourcePathfile sourcePathfile);
		bool Get(Guid id, ITargetPathfile targetPathfile);
	}
}
