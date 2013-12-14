using LehmaNautaLogic.Interface;
namespace LehmaNautaLogic.Implementation
{
	public class Pathfile : IPathfile
	{
		public string Value { get; set; }
		public Pathfile() { }
		public Pathfile(string pathfile)
		{
			Set(pathfile);
		}
		private void Set(string pathfile)
		{
			this.Value = pathfile;
		}

		public override string ToString()
		{
			return this.Value;
		}
	}
}
