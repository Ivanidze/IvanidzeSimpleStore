using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using NHibernate;

namespace DataModel.Repositories
{
    public class WorkerRepository:BaseRepository<Worker>, IWorkerRepository
    {
        
    }
}
