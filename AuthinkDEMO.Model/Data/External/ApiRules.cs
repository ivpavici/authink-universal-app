namespace AuthinkDEMO.Model.Data.External.ApiRules
{
    public static class ApiDataSource
    {   
#if (DEBUG || LITE)
        public static string BaseUrl = "http://authink.dump.hr/";
#elif RELEASE
        public static string BaseUrl = "http://authink.dump.hr/";
#endif
        public static string UserLogin      = "api/login/user";
        public static string UsersChildren  = "api/users/children/";
        public static string TaskStatistics = "api/statistics/task";

        public const string LoginToken        = "doVt4I-aovZtPnjXz-D1Fi";
        public const string ChildrenDataToken = "MI4-GPHwyr-phAadk5S-e9S";
        public const string StatisticsToken   = "tbosh5qhso-Q6gnMN2jf3";
    }
}
