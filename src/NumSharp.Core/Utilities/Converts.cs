﻿using System;
using NumSharp.Backends;

namespace NumSharp.Utilities
{
    /// <summary>
    ///     Provides various methods related to <see cref="System.Convert"/>.
    /// </summary>
    public static class Converts
    {
        /// <summary>
        ///     Finds the conversion method from <see cref="Convert"/> based on <typeparamref name="TIn"/> and <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">The type that is expected as input and to be converted from</typeparam>
        /// <typeparam name="TOut">The type we expect to convert to.</typeparam>
        public static Func<TIn, TOut> FindConverter<TIn, TOut>()
        {
#if _REGEN
            #region Compute
            //#n is input, #10n is output
		    switch (InfoOf<TIn>.NPTypeCode)
		    {
			    %foreach supported_dtypes,supported_dtypes_lowercase%
			    case NPTypeCode.#1:
			    {
				    switch (InfoOf<TOut>.NPTypeCode)
		            {
			            %foreach supported_dtypes,supported_dtypes_lowercase%
			            case NPTypeCode.#101:
			            {
				            Func<#2, #102> ret = Convert.To#101;
                            return (Func<TIn, TOut>) (object) ret;
			            }
			            %
			            default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
		            }
			    }
			    %
			    default:
                {
                    var tout = typeof(TOut);
                    return @in => (TOut)Convert.ChangeType(@in, tout);
                }
		    }
            #endregion
#else

            #region Compute

            //#n is input, #10n is output
            switch (InfoOf<TIn>.NPTypeCode)
            {
                case NPTypeCode.Boolean:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<bool, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<bool, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<bool, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<bool, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<bool, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<bool, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<bool, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<bool, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<bool, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<bool, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<bool, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<bool, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.Byte:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<byte, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<byte, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<byte, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<byte, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<byte, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<byte, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<byte, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<byte, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<byte, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<byte, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<byte, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<byte, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.Int16:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<short, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<short, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<short, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<short, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<short, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<short, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<short, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<short, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<short, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<short, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<short, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<short, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.UInt16:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<ushort, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<ushort, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<ushort, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<ushort, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<ushort, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<ushort, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<ushort, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<ushort, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<ushort, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<ushort, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<ushort, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<ushort, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.Int32:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<int, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<int, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<int, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<int, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<int, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<int, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<int, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<int, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<int, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<int, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<int, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<int, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.UInt32:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<uint, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<uint, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<uint, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<uint, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<uint, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<uint, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<uint, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<uint, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<uint, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<uint, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<uint, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<uint, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.Int64:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<long, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<long, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<long, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<long, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<long, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<long, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<long, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<long, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<long, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<long, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<long, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<long, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.UInt64:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<ulong, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<ulong, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<ulong, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<ulong, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<ulong, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<ulong, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<ulong, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<ulong, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<ulong, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<ulong, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<ulong, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<ulong, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.Char:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<char, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<char, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<char, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<char, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<char, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<char, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<char, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<char, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<char, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<char, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<char, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<char, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.Double:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<double, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<double, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<double, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<double, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<double, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<double, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<double, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<double, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<double, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<double, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<double, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<double, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.Single:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<float, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<float, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<float, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<float, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<float, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<float, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<float, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<float, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<float, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<float, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<float, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<float, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                case NPTypeCode.Decimal:
                {
                    switch (InfoOf<TOut>.NPTypeCode)
                    {
                        case NPTypeCode.Boolean:
                        {
                            Func<decimal, bool> ret = Convert.ToBoolean;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Byte:
                        {
                            Func<decimal, byte> ret = Convert.ToByte;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int16:
                        {
                            Func<decimal, short> ret = Convert.ToInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt16:
                        {
                            Func<decimal, ushort> ret = Convert.ToUInt16;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int32:
                        {
                            Func<decimal, int> ret = Convert.ToInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt32:
                        {
                            Func<decimal, uint> ret = Convert.ToUInt32;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Int64:
                        {
                            Func<decimal, long> ret = Convert.ToInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.UInt64:
                        {
                            Func<decimal, ulong> ret = Convert.ToUInt64;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Char:
                        {
                            Func<decimal, char> ret = Convert.ToChar;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Double:
                        {
                            Func<decimal, double> ret = Convert.ToDouble;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Single:
                        {
                            Func<decimal, float> ret = Convert.ToSingle;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        case NPTypeCode.Decimal:
                        {
                            Func<decimal, decimal> ret = Convert.ToDecimal;
                            return (Func<TIn, TOut>)(object)ret;
                        }

                        default:
                        {
                            var tout = typeof(TOut);
                            return @in => (TOut)Convert.ChangeType(@in, tout);
                        }
                    }
                }

                default:
                {
                    var tout = typeof(TOut);
                    return @in => (TOut)Convert.ChangeType(@in, tout);
                }
            }

            #endregion

#endif
        }
    }
}
