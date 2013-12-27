namespace LehmaNautaLogic.Interface
{
	public interface IPathfile
	{
		string Value { get; set; }

		IPath GetDirectoryName();
		IPathfile ToIPathfile();
	}

}
