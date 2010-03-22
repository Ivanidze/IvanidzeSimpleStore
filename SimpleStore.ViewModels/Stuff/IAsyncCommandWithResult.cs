using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SimpleStore.ViewModels.Stuff
{
    public interface IAsyncCommandWithResult<TParameter,TResult>:ICommand
    {
        Action<TParameter,TResult> Completed{ get; set;}
        Action<TParameter> Preview{ get; set;}
        bool BlockInteraction{ get; set;}
        TResult ExecuteSync(TParameter obj);
    }
}
