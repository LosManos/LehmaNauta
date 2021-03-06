﻿using LehmaNautaLogic.Interface;

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

		public IPath GetDirectoryName()
		{
			return new Path(
				System.IO.Path.GetDirectoryName(Value)
			);
		}

		public IPathfile ToIPathfile()
		{
			return (IPathfile)this;
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
