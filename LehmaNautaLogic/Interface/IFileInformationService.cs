using System;
using System.Collections.Generic;

namespace LehmaNautaLogic.Interface
{
	public interface IFileInformationService
	{
		Guid Create(DTO.FileInformation fileInfo);
		void Delete(Guid id);
		void DeleteOld();
		void EnsureDatabaseExists();
		IList<DTO.FileInformation> GetAll();
		DTO.FileInformation Get(Guid id);
	}
}
