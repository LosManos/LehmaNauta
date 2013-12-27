using LehmaNautaLogic.Interface;
namespace LehmaNautaLogic.Implementation
{
	public class TargetPath : ITargetPath
	{
		public string Value { get; set; }
		public TargetPath() {  }
		public TargetPath(string path)
		{
			Set(path);
		}
		private void Set(string path)
		{
			this.Value = path;
		}

		public override string ToString()
		{
			return this.Value;
		}
	}
}
