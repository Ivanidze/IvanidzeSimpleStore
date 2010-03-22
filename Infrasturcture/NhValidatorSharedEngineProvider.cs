using NHibernate.Validator.Engine;

namespace Infrasturcture
{
    /// <summary>
    /// класс для провайдера валидатора хибернейта
    /// </summary>
    public class NHVSharedEngineProvider:ISharedEngineProvider
    {
        private readonly ValidatorEngine _validatorEngine;
        public NHVSharedEngineProvider(ValidatorEngine validatorEngine)
        {
            _validatorEngine = validatorEngine;
        }
        public  ValidatorEngine GetEngine()
        {
            return _validatorEngine;
        }
    }
}
