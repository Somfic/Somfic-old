﻿using System.Threading.Tasks;

namespace Somfic.VoiceAttack.Log
{
    public class VoiceAttackLog
    {
        private readonly dynamic _proxy;

        internal VoiceAttackLog(dynamic proxy)
        {
            _proxy = proxy;
        }

        /// <summary>
        /// Writes to the VoiceAttack log
        /// </summary>
        /// <param name="content">The log message</param>
        /// <param name="color">The log color</param>
        public Task Write(string content, VoiceAttackColor color)
        {
            _proxy.WriteToLog(content, color.ToString());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Clears the VoiceAttack log
        /// </summary>
        public Task Clear()
        {
            _proxy.ClearLog();
            return Task.CompletedTask;
        }
    }
}