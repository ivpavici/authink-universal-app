using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ent = AuthinkDEMO.Model.Entities;

namespace AuthinkDEMO.Model.Queries
{
    public interface IPictureQueries
    {
        List<ent::Picture> GetAllPicturesForTask(int taskId);
    }
    public interface ITaskQueries
    {
        IReadOnlyList<ent::Task> GetAllTasksForTest(int testId);
        ent::Task GetSingle_byId(int taskid);
    }
    public interface ITestQueries
    {
        IReadOnlyList<ent::Test> GetAll();
    }
}
