using LehmaNautaLogic.Interface;

namespace LehmaNautaLogic.Implementation
{
	public class TargetPathfile : Pathfile, ITargetPathfile
	{
		public TargetPathfile() { }
		public TargetPathfile(string pathfile)
		:base(pathfile){ }

		public ITargetPathfile ToITargetPathfile()
		{
			return (ITargetPathfile)this;
		}

	}
}
