using LehmaNautaLogic.Interface;
namespace LehmaNautaLogic.Implementation
{
	public class Path : IPath
	{
		public string Value { get; set; }
		
		public Path() { }
		public Path(string pathfile)
		{
			Set(pathfile);
		}

		public IPath ToIPath()
		{
			return (IPath)this;
		}

		public override string ToString()
		{
			return this.Value;
		}

		private void Set(string pathfile)
		{
			this.Value = pathfile;
		}

	}
}
