using System.Collections.Generic;

using ent = AuthinkDEMO.Model.Entities;

namespace AuthinkDEMO.Model.Data
{
    public interface IDataProvider
    {
        IEnumerable<ent::Test> GetAll();
        IEnumerable<ent::Child> GetAll_children();
    }
}
