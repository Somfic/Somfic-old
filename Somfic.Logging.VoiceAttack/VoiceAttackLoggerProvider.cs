using Microsoft.Extensions.Logging;

namespace Somfic.Logging.VoiceAttack
{
    public class VoiceAttackLoggerProvider : ILoggerProvider
    {
        private readonly dynamic _vaProxy;

        public VoiceAttackLoggerProvider(dynamic vaProxy)
        {
            _vaProxy = vaProxy;
        }

        public void Dispose()
        {

        }

        public ILogger CreateLogger(string categoryName)
        {
            return new VoiceAttackLogger(_vaProxy);
        }
    }
}