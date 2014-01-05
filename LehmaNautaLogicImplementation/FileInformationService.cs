using System;
using System.Collections.Generic;
using System.Linq;
using LNLDTO = LehmaNautaLogic.DTO;
using Raven.Client.Document;
using Raven.Client.Extensions;
using LehmaNautaLogic;
using LehmaNautaLogic.DTO;
using LehmaNautaLogic.Interface;

namespace LehmaNautaLogicImplementation
{
	public class FileInformationService : IFileInformationService
	{
		private DocumentStore GetDocumentStore()
		{
			//TODO:Make this settable through config file.
			return new DocumentStore {Url = "http://localhost:8080/"};
		}

		private DocumentStore GetLehmaNautaStore()
		{
			//TODO:Make this settable through config file.
			return new DocumentStore { Url = "http://localhost:8080/", DefaultDatabase = "LehmaNautaFile" };
		}

		/// <summary>This method creates a new record in the database.
		/// </summary>
		/// <param name="fileInfo"></param>
		/// <returns></returns>
		public Guid Create(FileInformation fileInfo)
		{
			using (var store = GetLehmaNautaStore())
			{
				store.Initialize();

				using (var session = store.OpenSession())
				{
					fileInfo.EnsureID();
					session.Store(fileInfo);

					session.SaveChanges();
					return fileInfo.Id;
				}
			}
		}

		public void Delete(Guid id)
		{
			using (var store = GetLehmaNautaStore())
			{
				store.Initialize();
				using (var session = store.OpenSession())
				{
					var fileinfo = session.Load<FileInformation>(id);
					session.Delete( fileinfo);
					session.SaveChanges();
				}
			}
		}

		/// <summary>This method deletes all records older than a day.
		/// </summary>
		public void DeleteOld()
		{
			var now = DateTime.Now;
			foreach (var fileinfo in GetAll())
			{
				if (fileinfo.Created <= now.AddDays(-1))
				{
					Delete(fileinfo.Id);
				}
			}
		}

		/// <summary>This method retrieves all FileInformation records.
		/// It is prefered to have this method not exist as we don't want anyone
		/// to be able to browse the contents of the database. But I guess it has to
		/// exist. For now.
		/// </summary>
		/// <returns></returns>
		public IList<FileInformation> GetAll()
		{
			using (var store = GetLehmaNautaStore())
			{
				store.Initialize();

				using (var session = store.OpenSession())
				{
					var fileinfos = session.Query<FileInformation>();
					return fileinfos.ToList();
				}
			}
		}

		public void EnsureDatabaseExists()
		{
			using (var store = GetDocumentStore())
			{
				store.Initialize();
				store.DatabaseCommands.EnsureDatabaseExists("LehmaNautaFile");
			}
		}

		/// <summary>This method gets a record from the database by its PK.
		/// A side effect is that it deletes all records passed its termination datetime.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public FileInformation Get(Guid id)
		{
			using (var store = new DocumentStore { Url = "http://localhost:8080/", DefaultDatabase = "LehmaNautaFile" })
			{
				store.Initialize();

				using (var session = store.OpenSession())
				{
					DeleteOld();

					var ret = session.Load<FileInformation>(id);
					return ret;
				}
			}
		}

		internal IFileInformationService ToIFileInformationService()
		{
			return (IFileInformationService)this;
		}
	}
}
