using System;
using Castle.Core.Interceptor;

namespace Infrasturcture.ViewModelInterception
{
    public class RecoverableExceptionIntercepter : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (!typeof(ISupportExceptionRecovery).IsAssignableFrom(invocation.TargetType))
            {
                throw new NotSupportedException(string.Format("{0} should implement {1}",
                    invocation.TargetType.Name,
                    typeof(ISupportExceptionRecovery).Name));
            }

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                var recoverablePresenter = (ISupportExceptionRecovery)invocation.InvocationTarget;
                recoverablePresenter.OnException(ex);
            }

        }
    }
}
