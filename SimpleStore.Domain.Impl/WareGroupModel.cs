using System;
using System.Linq;
using System.Collections.Generic;
using Infrasturcture;
using SimpleStore.Data;
using SimpleStore.Domain.Model;
using uNhAddIns.Adapters;

namespace SimpleStore.Domain.Impl
{

    [PersistenceConversational(
        MethodsIncludeMode = MethodsIncludeMode.Implicit)]
    public class WareGroupModel:IWareGroupModel
    {
        private readonly IDao<WareGroup> _wareGroupDao;
        private readonly IEntityFactory _entityFactory;
        private readonly IEntityValidator _validator;


        public WareGroupModel(IDao<WareGroup> workerDao, IEntityFactory entityFactory, IEntityValidator validator)
        {
            _wareGroupDao = workerDao;
            _entityFactory = entityFactory;
            _validator = validator;
        }
        [PersistenceConversation(ConversationEndMode = EndMode.CommitAndContinue)]
        public void SaveWareGroup(WareGroup wareGroup)
        {
            if (IsValid(wareGroup))
               _wareGroupDao.Save(wareGroup);
        }
        [PersistenceConversation(ConversationEndMode = EndMode.CommitAndContinue)]
        public void UpdateWareGroup(WareGroup wareGroup)
        {
            if (IsValid(wareGroup))
               _wareGroupDao.Update(wareGroup);
        }

        [PersistenceConversation(Exclude = true)]
        public bool IsValid(WareGroup wareGroup)
        {
            return _validator.IsValid(wareGroup);
        }

        public void CancelWareGroup(WareGroup wareGroup)
        {
            _wareGroupDao.Refresh(wareGroup);
        }
        [PersistenceConversation(ConversationEndMode = EndMode.CommitAndContinue)]
        public void DeleteWareGroup(WareGroup wareGroup)
        {
            _wareGroupDao.Delete(wareGroup);
        }

        public WareGroup CreateWareGroup()
        {
            var wareGroup = _entityFactory.Create<WareGroup>();
            return wareGroup;
        }

        public IEnumerable<WareGroup> GetWareGroupChildren(WareGroup wareGroup)
        {
            return wareGroup.Children;
        }
        
        public IList<WareGroup> GetAllWareGroups()
        {
            return _wareGroupDao.RetrieveAll().ToList();
        }
        
        public IList<WareGroup> GetAllRootWareGroups()
        {
            return _wareGroupDao.Retrieve(wg => wg.Parent == null).ToList();
        }
        
    }
}
