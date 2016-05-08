namespace AuthinkDEMO.Model.Data.External.ApiEntities
{
    public class User
    {
        public User
        (
            string username,
            string password
        )
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
    }
    public class Statistics
    {
        public Statistics
        (
            int    taskId,
            int    sucessfullClicksCount,
            int    errorClicksCount,
            string timeRun
        )
        {
            this.TaskId                = taskId;
            this.SucessfullClicksCount = sucessfullClicksCount;
            this.ErrorClicksCount      = errorClicksCount;
            this.TimeRun               = timeRun;
        }

        public int TaskId                { get; private set; }
        public int SucessfullClicksCount { get; private set; }
        public int ErrorClicksCount      { get; private set; }
        public string TimeRun            { get; private set; }
    }
}
