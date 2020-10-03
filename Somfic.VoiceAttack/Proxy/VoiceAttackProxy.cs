﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Somfic.VoiceAttack.Audio;
using Somfic.VoiceAttack.Log;
using Somfic.VoiceAttack.Options;
using Somfic.VoiceAttack.Paths;
using Somfic.VoiceAttack.Proxy.Abstractions;
using Somfic.VoiceAttack.Variables;
using Somfic.VoiceAttack.Versions;

namespace Somfic.VoiceAttack.Proxy
{
    public class VoiceAttackProxy : IVoiceAttackProxy
    {
        private readonly dynamic _vaProxy;

        public VoiceAttackProxy(dynamic vaProxy)
        {
            _vaProxy = vaProxy;

            Variables = new VoiceAttackVariables(vaProxy);
            Versions = new VoiceAttackVersions(vaProxy);
            Log = new VoiceAttackLog(vaProxy);
            Paths = new VoiceAttackPaths(vaProxy);
            Options = new VoiceAttackOptions(vaProxy);
            Speech = new VoiceAttackSpeech(vaProxy);
        }

        /// <inheritdoc />
        public string Context => _vaProxy.Context;

        /// <inheritdoc />
        public bool HasStopped => _vaProxy.Stopped;

        /// <inheritdoc />
        public IReadOnlyCollection<string> Profiles => _vaProxy.ProfileNames();

        /// <inheritdoc />
        public IntPtr Handle => _vaProxy.MainWindowHandle;

        /// <inheritdoc />
        public VoiceAttackVariables Variables { get; }

        /// <inheritdoc />
        public VoiceAttackVersions Versions { get; }

        /// <inheritdoc />
        public VoiceAttackLog Log { get; }

        /// <inheritdoc />
        public VoiceAttackPaths Paths { get; }

        /// <inheritdoc />
        public VoiceAttackOptions Options { get; }

        /// <inheritdoc />
        public VoiceAttackSpeech Speech { get; }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<string>> GeneratePhrases(string query, bool trimSpaces = false, bool lowercase = false)
        {
            return Task.FromResult(_vaProxy.ExtractPhrases(query, trimSpaces, lowercase));
        }

        /// <inheritdoc />
        public Task Opacity(int percentage)
        {
            _vaProxy.SetOpacity(percentage);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task ResetStopFlag()
        {
            _vaProxy.ResetStopFlag();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Close()
        {
            _vaProxy.Close();
            return Task.CompletedTask;
        }
    }
}