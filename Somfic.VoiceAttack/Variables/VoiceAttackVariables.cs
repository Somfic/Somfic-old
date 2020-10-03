using System;

namespace Somfic.VoiceAttack.Variables
{
    //todo: there must be a cleaner way than this... perhaps with a factory?

    public class VoiceAttackVariables
    {
        private readonly dynamic _proxy;

        internal VoiceAttackVariables(dynamic proxy)
        {
            _proxy = proxy;
        }

        /// <summary>
        /// Set a variable
        /// </summary>
        /// <typeparam name="T">The type of variable</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void Set<T>(string name, T value)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    SetBoolean(name, (dynamic)value);
                    break;

                case TypeCode.DateTime:
                    SetDate(name, (dynamic)value);
                    break;

                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Double:
                    SetDecimal(name, (dynamic)value);
                    break;

                case TypeCode.Char:
                case TypeCode.String:
                    SetText(name, value.ToString());
                    break;

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                    SetShort(name, (dynamic)value);
                    break;

                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    SetInt(name, (dynamic)value);
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
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    return (dynamic)GetBoolean(name);

                case TypeCode.DateTime:
                    return (dynamic)GetDate(name);

                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Double:
                    return (dynamic)GetDecimal(name);

                case TypeCode.Char:
                case TypeCode.String:
                    return (dynamic)GetText(name);

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                    return (dynamic)GetShort(name);

                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return (dynamic)GetInt(name);

                default:
                    return (dynamic) null;
            }
        }

        private short? GetShort(string name) => _proxy.GetSmall(name);
        private int? GetInt(string name) => _proxy.GetInt(name);
        private string GetText(string name) => _proxy.GetText(name);
        private decimal? GetDecimal(string name) => _proxy.GetDecimal(name);
        private bool? GetBoolean(string name) => _proxy.GetBoolean(name);
        private DateTime? GetDate(string name) => _proxy.GetDate(name); 
        
        private void SetShort(string name, short? value) => _proxy.GetSmall(name, value);
        private void SetInt(string name, int? value) => _proxy.GetInt(name, value);
        private void SetText(string name, string value) => _proxy.SetText(name, value);
        private void SetDecimal(string name, decimal? value) => _proxy.GetDecimal(name, value);
        private void SetBoolean(string name, bool? value) => _proxy.GetBoolean(name, value);
        private void SetDate(string name, DateTime? value) => _proxy.GetDate(name,value);
    }
}