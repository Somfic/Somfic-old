using System;

namespace Somfic.VoiceAttack.Versions
{
    public class VoiceAttackVersions
    {
        private readonly dynamic _proxy;

        internal VoiceAttackVersions(dynamic proxy)
        {
            _proxy = proxy;
        }

        /// <summary>
        /// The Proxy interface version
        /// </summary>
        public Version Proxy => _proxy.ProxyVersion;

        /// <summary>
        /// The VoiceAttack version
        /// </summary>
        public Version VoiceAttack =>  _proxy.VAVersion;

        /// <summary>
        /// Whether this is a stable VoiceAttack release
        /// </summary>
        public bool IsStableRelease => _proxy.IsRelease;

        /// <summary>
        /// Whether this is the trial version
        /// </summary>
        public bool IsTrailVersion => _proxy.IsTrial;
    }
}