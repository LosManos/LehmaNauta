using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LehmaNautaLogic.Test")]
namespace LehmaNautaLogic
{
	/// <summary>This class is for handling the very physical files.
	/// As far as files are physical
	/// </summary>
	public class PhysicalfileService
	{
		private const string Filename = "X";
		
		public string RepositoryPath { get; set; }

		public PhysicalfileService() { }

		public PhysicalfileService(string repositoryPath)
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
			var destPath = Path.Combine(RepositoryPath, id.ToString());
			var destPathfile = Path.Combine( destPath, Filename);
			Directory.CreateDirectory(destPath);
			File.Copy(sourcePathfile.Value, destPathfile);
		}

		/// <summary>This method gets a File.
		/// It also removes it at once.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public string Get(Guid id)
		{
			var pathfile = Path.Combine(RepositoryPath, id.ToString(), Filename);
			var ret = File.ReadAllText( pathfile );
			File.Delete(pathfile);
			return ret;
		}

		/// <summary>This method returns true if a File exists.
		/// It is internal but only for automatic tests to work easily;
		/// consider it private
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		internal bool Exists(Guid id)
		{
			return File.Exists(Path.Combine(RepositoryPath, id.ToString(), Filename));
		}
	}

	public interface IPathfile
	{
		string Value { get; set; }
	}

	public interface ISourcePathfile : IPathfile
	{

	}

	public class SourcePathfile : ISourcePathfile
	{
		public string Value { get; set; }
		public SourcePathfile(){}
		public SourcePathfile( string pathfile )
		{
			Set(pathfile);
		}
		private void Set(string pathfile)
		{
			this.Value = pathfile;
		}
	}
}
