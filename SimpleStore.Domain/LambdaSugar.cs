using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleStore.Domain
{
    public static class LambdaSugar
    {
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o);
        }
        public static TResult Return<TInput, TResult>(this TInput o,
  Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class
        {
            if (o == null) return failureValue;
            return evaluator(o);
        }

    }
}
