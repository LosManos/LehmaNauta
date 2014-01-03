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

		/// <summary>This method deletes the file from the repository.
		/// </summary>
		/// <param name="id"></param>
		public void Delete(Guid id)
		{
			File.Delete(
				System.IO.Path.Combine(RepositoryPath.Value, id.ToString(), Filename)
			);
			Directory.Delete(
				System.IO.Path.Combine(RepositoryPath.Value, id.ToString())
			);
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

		/// <summary>This method gets a File.
		/// It also removes it at once.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public FileStream Get(Guid id)
		{
			var path = System.IO.Path.Combine(RepositoryPath.Value, id.ToString());
			var pathfile = System.IO.Path.Combine(path, Filename);

			var stream = new FileStream(pathfile, FileMode.Open);

			return stream;
		}

		public IPhysicalfileService ToIPhysicalfileService()
		{
			return (IPhysicalfileService)this;
		}

	}

}
