using LehmaNauta.Common;
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
			public Guid Id { get; set; }
			public string Filename { get; set; }
			public int Length { get; set; }
			
			public FileInformation() { }
			
			public FileInformation(
				Guid id, 
				string filename, 
				int length)
			{
				Set(id, filename, length);
			}

			private void Set(Guid id, string filename, int length)
			{
				Assert.Argument.Called("filename")
					.IsNotNullAndDoesOnlyContainFilename( filename );

				this.Id = id;
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