using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleStore.Domain.Model
{
    public interface IWareGroupModel
    {
        void SaveWareGroup(WareGroup wareGroup);
        void UpdateWareGroup(WareGroup wareGroup);
        bool IsValid(WareGroup wareGroup);
        void CancelWareGroup(WareGroup wareGroup);
        void DeleteWareGroup(WareGroup wareGroup);
        WareGroup CreateWareGroup();
        IEnumerable<WareGroup> GetWareGroupChildren(WareGroup wareGroup);
        IList<WareGroup> GetAllWareGroups();
        IList<WareGroup> GetAllRootWareGroups();
    }
}
