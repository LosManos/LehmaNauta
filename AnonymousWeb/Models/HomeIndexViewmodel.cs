using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnonymousWeb.Models
{
	public class HomeIndexViewmodel
	{
		public class FileInformation
		{
			public string PathFile { get; set; }
			public int Length { get; set; }
			
			public FileInformation() { }
			
			public FileInformation(
				string pathfile, 
				int length)
			{
				Set(pathfile, length);
			}

			private void Set(string pathfile, int length)
			{
				this.PathFile = pathfile;
				this.Length = length;
			}
		}

		public ICollection<FileInformation> Files;

		public static HomeIndexViewmodel Create()
		{
			var ret = new HomeIndexViewmodel
			{
				Files = new List<FileInformation>()
			};
			return ret;
		}
	}	
}