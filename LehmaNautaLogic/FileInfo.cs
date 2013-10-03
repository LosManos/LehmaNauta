using System;
using System.Collections.Generic;
using System.Linq;
using LehmaNautaLogic.DTO;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace LehmaNautaLogic
{
	public class FileInfoService
	{
		private DocumentStore NewDocumentStore()
		{
			return new DocumentStore {Url = "http://localhost:8080/"};
		}

		private DocumentStore NewLehmaNautaStore()
		{
			return new DocumentStore {Url = "http://localhost:8080/", DefaultDatabase = "LehmaNautaFile"};
		}

		public Guid Create(DTO.FileInfo fileInfo)
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
					var fileinfo = session.Load<DTO.FileInfo>(id);
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

		//TODO: Remove. Let GetAll be limited to testing assembly.
		public IList<DTO.FileInfo> IT_GetAll()
		{
			return GetAll();
		}

		public void EnsureDatabaseExists()
		{
			using (var store = NewDocumentStore())
			{
				store.Initialize();
				store.DatabaseCommands.EnsureDatabaseExists("LehmaNautaFile");
			}
		}

		public DTO.FileInfo Load(Guid id)
		{
			using (var store = new DocumentStore { Url = "http://localhost:8080/", DefaultDatabase = "LehmaNautaFile" })
			{
				store.Initialize();

				using (var session = store.OpenSession())
				{
					var ret = session.Load<DTO.FileInfo>(id);
					return ret;
				}
			}
		}

		private IList<DTO.FileInfo> GetAll()
		{
			using (var store = NewLehmaNautaStore())
			{
				store.Initialize();

				using (var session = store.OpenSession())
				{
					var fileinfos = session.Query<DTO.FileInfo>();
					return fileinfos.ToList();
				}
			}
		}
	}
}
