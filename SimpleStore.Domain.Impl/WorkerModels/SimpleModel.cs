using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleStore.Domain.Model.WorkerModels;
using uNhAddIns.Adapters;

namespace SimpleStore.Domain.Impl.WorkerModels
{
    [PersistenceConversational(
        MethodsIncludeMode = MethodsIncludeMode.Implicit,
        DefaultEndMode = EndMode.Continue)]
    public class SimpleModel : ISimpleModel
    {


        public SimpleModel()
        {

        }


        [PersistenceConversation(ConversationEndMode = EndMode.End)]
        public IList<Worker> GetAllArtists()
        {
            return new List<Worker>();
        }
    }
}
