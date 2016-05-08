namespace AuthinkDEMO.Model.Data
{
    public interface ILoginService
    {
        bool Login(string username, string password);
    }
}
