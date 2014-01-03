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
		private DocumentStore NewDocumentStore()
		{
			return new DocumentStore {Url = "http://localhost:8080/"};
		}

		private DocumentStore NewLehmaNautaStore()
		{
			return new DocumentStore {Url = "http://localhost:8080/", DefaultDatabase = "LehmaNautaFile"};
		}

		public Guid Create(FileInformation fileInfo)
		{
			using (var store = NewLehmaNautaStore())
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
			using (var store = NewLehmaNautaStore())
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

		public IList<FileInformation> GetAll()
		{
			using (var store = NewLehmaNautaStore())
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
			using (var store = NewDocumentStore())
			{
				store.Initialize();
				store.DatabaseCommands.EnsureDatabaseExists("LehmaNautaFile");
			}
		}

		public FileInformation Load(Guid id)
		{
			using (var store = new DocumentStore { Url = "http://localhost:8080/", DefaultDatabase = "LehmaNautaFile" })
			{
				store.Initialize();

				using (var session = store.OpenSession())
				{
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
