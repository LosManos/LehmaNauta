using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using LehmaNautaLogic;
using LehmaNautaLogic.Interface;
using LehmaNautaLogic.Implementation;
using LehmaNauta.Common;

[assembly: InternalsVisibleTo("LehmaNautaLogic.Test")]
namespace LehmaNautaLogicImplementation
{
	/// <summary>This class is for handling the very physical files.
	/// As far as files are physical
	/// </summary>
	public class PhysicalfileService : IPhysicalfileService
	{
		private const string Filename = "X";
		
		public IPathfile RepositoryPath { get; set; }

		public PhysicalfileService() { }

		public PhysicalfileService(IPathfile repositoryPath)
		{
			this.RepositoryPath = repositoryPath;
		}

		/// <summary>This method creates a new File in the repository.
		/// At first glance it might seem to have a bad name or its parameters
		/// in the wrong order but no.
		/// It Creates a File with the Unique-Id from the Source-Pathfile.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="sourcePathfile"></param>
		public void Create( Guid id, ISourcePathfile sourcePathfile)
		{
			var destPath = System.IO.Path.Combine(RepositoryPath.Value, id.ToString());
			var destPathfile = System.IO.Path.Combine(destPath, Filename);
			Directory.CreateDirectory(destPath);
			File.Copy(sourcePathfile.Value, destPathfile);
		}

		/// <summary>This method gets a File.
		/// It also removes it at once.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public FileStream Get(Guid id)
		{
			//TODO:	Returning a string being the full file is not good.
			//	Have it return a stream or copy to a folder instead.
			var path = System.IO.Path.Combine(RepositoryPath.Value, id.ToString());
			var pathfile = System.IO.Path.Combine(path, Filename);

			var stream = new FileStream(pathfile, FileMode.Open);

			//DeleteFile(new Pathfile(System.IO.Path.Combine(RepositoryPath.Va.ToString(), Filename)));
			return stream;
		}

		/// <summary>This method returns true if a File exists.
		/// It is public but only for automatic tests to work easily;
		/// consider it private
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Exists(Guid id)
		{
			return File.Exists(System.IO.Path.Combine(RepositoryPath.Value, id.ToString(), Filename));
		}

		/// <summary>This method deletes the file and its path
		/// all the way to the repository root.
		/// The file is stored a typically LN.Repository/xxx/X
		/// where xxx is the GUID identifying the file
		/// and X is just X as we call them X and nothing else.
		/// Security through obscurity.
		/// </summary>
		/// <param name="pathfile"></param>
		private void DeleteFile(IPathfile pathfile)
		{
			Assert.Argument.Called("pathfile").IsNotNull(pathfile);

			File.Delete(pathfile.Value);	//	Deletes the file.
			Directory.Delete(pathfile.GetDirectoryName().Value);
		}
	}

}
