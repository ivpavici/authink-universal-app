using System.Collections.Generic;

using ent = AuthinkDEMO.Model.Entities;

namespace AuthinkDEMO.Model.Data.Private
{
    public static class StorageData
    {
        private static IEnumerable<ent::Child> Data { get; set; }

        public static void Setup(IEnumerable<ent::Child> childData)
        {
            Data = childData;
        }
        
        public static IEnumerable<ent::Child> GetChildData()
        {
            return Data;
        }
    }
}
