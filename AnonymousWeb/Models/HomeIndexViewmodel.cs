using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace AnonymousWeb.Models
{
	/// <summary>This class is what Home/Index returns to the web client.
	/// Alas; no sensitive information. Hacker vector stuff sorta.
	/// </summary>
	public class HomeIndexViewmodel
	{
		/// <summary>This class contains information about every file uploaded.
		/// Note that there shouldn't be any path information here.
		/// </summary>
		public class FileInformation
		{
			public string Filename { get; set; }
			public int Length { get; set; }
			
			public FileInformation() { }
			
			public FileInformation(
				string filename, 
				int length)
			{
				Set(filename, length);
			}

			private void Set(string filename, int length)
			{
				if (null == filename) { throw new ArgumentNullException("filename"); }
				if( filename.Contains( Path.DirectorySeparatorChar) ||
					filename.Contains(Path.VolumeSeparatorChar))
				{
					throw new ArgumentException("Filename contains invalid character.", "filename");
				}
				this.Filename = filename;
				this.Length = length;
			}
		}

		public ICollection<FileInformation> Files;

		/// <summary>This static constructor is the preferred as it allocates an empty list 
		/// ready to be used.
		/// </summary>
		/// <returns></returns>
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