using System;
namespace Infrasturcture
{
    public interface ISupportExceptionRecovery
    {
        void OnException(Exception exception);
    }
}
