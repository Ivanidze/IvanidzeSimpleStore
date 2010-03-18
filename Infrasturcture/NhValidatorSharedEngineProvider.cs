using NHibernate.Validator.Engine;

namespace Infrasturcture
{
    /// <summary>
    /// класс для провайдера валидатора хибернейта
    /// </summary>
    public class NhValidatorSharedEngineProvider:ISharedEngineProvider
    {
        private readonly ValidatorEngine _validatorEngine;
        public NhValidatorSharedEngineProvider(ValidatorEngine validatorEngine)
        {
            _validatorEngine = validatorEngine;
        }
        public  ValidatorEngine GetEngine()
        {
            return _validatorEngine;
        }
    }
}
