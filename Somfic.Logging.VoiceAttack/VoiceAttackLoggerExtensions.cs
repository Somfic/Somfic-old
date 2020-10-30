using Microsoft.Extensions.Logging;

namespace Somfic.Logging.VoiceAttack
{
    public static class VoiceAttackLoggerExtensions
    {
        /// <summary>
        /// Adds VoiceAttack logging to the <seealso cref="ILoggingBuilder"/>
        /// </summary>
        /// <param name="vaProxy">VoiceAttack's proxy</param>
        public static ILoggingBuilder AddVoiceAttack(this ILoggingBuilder builder, dynamic vaProxy)
        {
            builder.AddProvider(new VoiceAttackLoggerProvider(vaProxy));

            return builder;
        }
    }
}