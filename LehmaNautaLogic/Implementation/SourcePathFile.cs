using LehmaNautaLogic.Interface;

namespace LehmaNautaLogic.Implementation
{
	public class SourcePathfile : Pathfile, ISourcePathfile
	{
		public SourcePathfile() { }
		public SourcePathfile(string pathfile) 
		:base(pathfile){ }
	}
}
