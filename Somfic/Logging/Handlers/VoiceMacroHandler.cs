using System.Drawing;
using vmAPI;

namespace Somfic.Logging.Handlers
{
    /// <summary>
    /// A logging handler for VoiceMacro.
    /// Uses VoicMacro's <c>vmCommand.AddLogEntry()</c> method .
    /// </summary>
    public class VoiceMacroHandler : LoggerHandler
    {
        private dynamic proxy;
        private readonly string displayName;
        private readonly string id;

        /// <summary>
        /// Creates a new VoiceMacroHandler with a display name.
        /// </summary>
        /// <param name="displayName">The name used while logging.</param>
        /// <param name="id">The id of the plugin.</param>
        public VoiceMacroHandler(string displayName, string id)
        {
            this.displayName = displayName;
            this.id = id;
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
            Color color;

            switch (message.Severity)
            {
                case Severity.Error:
                    color = Color.Red;
                    break;
                case Severity.Warning:
                    color = Color.Orange;
                    break;
                case Severity.Success:
                    color = Color.GreenYellow;
                    break;
                case Severity.Info:
                    color = Color.CornflowerBlue;
                    break;
                case Severity.Debug:
                    color = Color.Gray;
                    break;
                case Severity.Special:
                    color = Color.Pink;
                    break;
                default:
                    color =  Color.Black;
                    break;
            }

            vmCommand.AddLogEntry(message.Content,color,$"{displayName} - {message.Content}", id);
        }
    }
}