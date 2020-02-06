namespace Somfic.Logging.Handlers
{
    /// <summary>
    /// A logging handler for VoiceAttack.
    /// Uses VoiceAttack's <c>proxy.WriteToLog()</c> method .
    /// </summary>
    public class VoiceAttackHandler : LoggerHandler
    {
        private dynamic proxy;
        private readonly string displayName;

        /// <summary>
        /// Creates a new VoiceAttackHandler with a display name.
        /// </summary>
        /// <param name="displayName">The name used while logging.</param>
        public VoiceAttackHandler(string displayName)
        {
            this.displayName = displayName;
        }

        /// <summary>
        /// Update the VoiceAttack proxy object.
        /// </summary>
        /// <param name="proxy">The VoiceAttack proxy dynamic.</param>
        public void UpdateProxy(dynamic proxy)
        {
            this.proxy = proxy;
        }

        public override void WriteLog(LogMessage message)
        {
            string color;

            switch (message.Severity)
            {
                case Severity.Error:
                    color = "red";
                    break;
                case Severity.Warning:
                    color = "orange";
                    break;
                case Severity.Success:
                    color = "green";
                    break;
                case Severity.Info:
                    color = "blue";
                    break;
                case Severity.Debug:
                    color = "blank";
                    break;
                case Severity.Special:
                    color = "pink";
                    break;
                default:
                    color = "black";
                    break;
            }

            proxy.WriteToLog($"{displayName} - {message.Content}", color);
        }
    }
}
