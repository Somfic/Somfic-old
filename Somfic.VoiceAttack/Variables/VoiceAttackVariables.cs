﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Somfic.VoiceAttack.Variables
{
    public class VoiceAttackVariables
    {
        private readonly dynamic _proxy;
        private readonly ILogger<VoiceAttackVariables> _log;

        internal VoiceAttackVariables(dynamic vaProxy, IServiceProvider services = null)
        {
            _proxy = vaProxy;
            _log = services?.GetService<ILogger<VoiceAttackVariables>>();
        }

        /// <summary>
        /// Set a variable
        /// </summary>
        /// <typeparam name="T">The type of variable</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void Set<T>(string name, T value)
        {
            var code = Type.GetTypeCode(typeof(T));

            switch (code)
            {
                case TypeCode.Boolean:
                    SetBoolean(name, (bool?)Convert.ChangeType(value, typeof(bool?)));
                    break;

                case TypeCode.DateTime:
                    SetDate(name, (DateTime?)Convert.ChangeType(value, typeof(DateTime?)));
                    break;

                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Double:
                    SetDecimal(name, (decimal?)Convert.ChangeType(value, typeof(decimal?)));
                    break;

                case TypeCode.Char:
                case TypeCode.String:
                    SetText(name, (string)Convert.ChangeType(value, typeof(string)));
                    break;

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                    SetShort(name, (short?)Convert.ChangeType(value, typeof(short?)));
                    break;

                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    SetInt(name, (int?)Convert.ChangeType(value, typeof(int?)));
                    break;

                default:
                    _log?.LogWarning("Could not set {name} variable because the type {type} is not supported", name, code.ToString());
                    break;
            }
        }

        /// <summary>
        /// Get a variable
        /// </summary>
        /// <typeparam name="T">The type of variable</typeparam>
        /// <param name="name">The name of the variable</param>
        public T Get<T>(string name)
        {
            var code = Type.GetTypeCode(typeof(T));

            switch (code)
            {
                case TypeCode.Boolean:
                    return (T)Convert.ChangeType(GetBoolean(name), typeof(T));

                case TypeCode.DateTime:
                    return (T)Convert.ChangeType(GetDate(name), typeof(T));

                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Double:
                    return (T)Convert.ChangeType(GetDecimal(name), typeof(T));

                case TypeCode.Char:
                case TypeCode.String:
                    return (T)Convert.ChangeType(GetText(name), typeof(T));

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                    return (T)Convert.ChangeType(GetShort(name), typeof(T));

                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return (T)Convert.ChangeType(GetInt(name), typeof(T));

                default:
                    _log?.LogWarning("Could not get {name} variable because the type {type} is not supported", name, code.ToString());
                    return default;
            }
        }

        private short? GetShort(string name)
        {
            return _proxy.GetSmall(name);
        }

        private int? GetInt(string name)
        {
            return _proxy.GetInt(name);
        }

        private string GetText(string name)
        {
            return _proxy.GetText(name);
        }

        private decimal? GetDecimal(string name)
        {
            return _proxy.GetDecimal(name);
        }

        private bool? GetBoolean(string name)
        {
            return _proxy.GetBoolean(name);
        }

        private DateTime? GetDate(string name)
        {
            return _proxy.GetDate(name);
        }

        private void SetShort(string name, short? value)
        {
            _log?.LogTrace("Setting {name} to {value}", $"{{SHORT:{name}}}", name, value);
            _proxy.SetSmall(name, value);
        }

        private void SetInt(string name, int? value)
        {
            _log?.LogTrace("Setting {name} to {value}", $"{{INT:{name}}}", name, value);
            _proxy.SetInt(name, value);
        }

        private void SetText(string name, string value)
        {
            _log?.LogTrace("Setting {name} to {value}", $"{{TXT:{name}}}", name, value);
            _proxy.SetText(name, value);
        }

        private void SetDecimal(string name, decimal? value)
        {
            _log?.LogTrace("Setting {name} to {value}", $"{{DEC:{name}}}", name, value);
            _proxy.SetDecimal(name, value);
        }

        private void SetBoolean(string name, bool? value)
        {
            _log?.LogTrace("Setting {name} to {value}", $"{{BOOL:{name}}}", name, value); 
            _proxy.SetBoolean(name, value);
        }

        private void SetDate(string name, DateTime? value)
        {
            _log?.LogTrace("Setting {name} to {value}", $"{{DATE:{name}}}", name, value);
            _proxy.SetDate(name, value);
        }
    }
}