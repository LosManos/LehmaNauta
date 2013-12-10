using LehmaNautaLogic.Inferface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LehmaNautaLogic.Implementation
{
	public class SourcePathfile : ISourcePathfile
	{
		public string Value { get; set; }
		public SourcePathfile() { }
		public SourcePathfile(string pathfile)
		{
			Set(pathfile);
		}
		private void Set(string pathfile)
		{
			this.Value = pathfile;
		}
	}
}
