using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ent = AuthinkDEMO.Model.Entities;
using AuthinkDEMO.Model.Data;

namespace AuthinkDEMO.Model.Queries.Impl
{
    public class PictureQueries : IPictureQueries
    {
        public PictureQueries
        (
            ITaskQueries taskProvider
        )
        {
            this.taskprovider = taskProvider;
        }

        private readonly ITaskQueries taskprovider;

        public List<ent::Picture> GetAllPicturesForTask(int taskId)
        {
            return
                taskprovider.GetSingle_byId(taskId)
                            .Pictures
                            .ToList();
        }
    }
    public class TaskQueries : ITaskQueries
    {
        public TaskQueries
        (
            IDataProvider dataProvider
        )
        {
            this.dataProvider = dataProvider;
        }

        private readonly IDataProvider dataProvider;

        public IReadOnlyList<ent::Task> GetAllTasksForTest(int testId)
        {
            var s = dataProvider.GetAll();
            return
                dataProvider.GetAll()
                            .Single(test => test.Id == testId)
                            .Tasks
                            .ToList();
        }
        public ent::Task GetSingle_byId(int taskid)
        {
            return
                dataProvider.GetAll()
                            .SingleOrDefault(test => test.Tasks.Any(task => task.Id == taskid))
                            .Tasks
                            .Single(task => task.Id == taskid);
        }
    }
    public class TestQueries : ITestQueries
    {
        public TestQueries
        (
            IDataProvider dataProvider
        )
        {
            this.dataProvider = dataProvider;
        }

        private readonly IDataProvider dataProvider;

        public IReadOnlyList<ent::Test> GetAll()
        {
            return
                dataProvider.GetAll().ToList();
        }
    }
}
