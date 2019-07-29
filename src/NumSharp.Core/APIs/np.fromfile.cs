﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using NumSharp.Backends;
using NumSharp.Backends.Unmanaged;

namespace NumSharp
{
    public static partial class np
    {
        /// <summary>
        ///     Construct an array from data in a text or binary file.
        ///     A highly efficient way of reading binary data with a known data-type, as well as parsing simply formatted text files. Data written using the tofile method can be read using this function.
        /// </summary>
        /// <param name="file">filename.</param>
        /// <param name="dtype">Data type of the returned array. For binary files, it is used to determine the size and byte-order of the items in the file.</param>
        /// <returns></returns>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.fromfile.html</remarks>
        public static NDArray fromfile(string file, NPTypeCode dtype)
        {
            return fromfile(file, dtype.AsType());
        }

        /// <summary>
        ///     Construct an array from data in a text or binary file.
        ///     A highly efficient way of reading binary data with a known data-type, as well as parsing simply formatted text files. Data written using the tofile method can be read using this function.
        /// </summary>
        /// <param name="file">filename.</param>
        /// <param name="dtype">Data type of the returned array. For binary files, it is used to determine the size and byte-order of the items in the file.</param>
        /// <returns></returns>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.fromfile.html</remarks>
        public static NDArray fromfile(string file, Type dtype)
        {
            unsafe
            {
                var bytes = File.ReadAllBytes(file);
                switch (dtype.GetTypeCode())
                {
#if _REGEN
	                %foreach supported_dtypes,supported_dtypes_lowercase%
	                case NPTypeCode.#1:
	                {
                        return new NDArray(new ArraySlice<#2>(UnmanagedMemoryBlock<#2>.FromBuffer(bytes, false)));
	                }
	                %
	                default:
		                throw new NotSupportedException();
#else
	                case NPTypeCode.Boolean:
	                {
                        return new NDArray(new ArraySlice<bool>(UnmanagedMemoryBlock<bool>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.Byte:
	                {
                        return new NDArray(new ArraySlice<byte>(UnmanagedMemoryBlock<byte>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.Int16:
	                {
                        return new NDArray(new ArraySlice<short>(UnmanagedMemoryBlock<short>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.UInt16:
	                {
                        return new NDArray(new ArraySlice<ushort>(UnmanagedMemoryBlock<ushort>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.Int32:
	                {
                        return new NDArray(new ArraySlice<int>(UnmanagedMemoryBlock<int>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.UInt32:
	                {
                        return new NDArray(new ArraySlice<uint>(UnmanagedMemoryBlock<uint>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.Int64:
	                {
                        return new NDArray(new ArraySlice<long>(UnmanagedMemoryBlock<long>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.UInt64:
	                {
                        return new NDArray(new ArraySlice<ulong>(UnmanagedMemoryBlock<ulong>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.Char:
	                {
                        return new NDArray(new ArraySlice<char>(UnmanagedMemoryBlock<char>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.Double:
	                {
                        return new NDArray(new ArraySlice<double>(UnmanagedMemoryBlock<double>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.Single:
	                {
                        return new NDArray(new ArraySlice<float>(UnmanagedMemoryBlock<float>.FromBuffer(bytes, false)));
	                }
	                case NPTypeCode.Decimal:
	                {
                        return new NDArray(new ArraySlice<decimal>(UnmanagedMemoryBlock<decimal>.FromBuffer(bytes, false)));
	                }
	                default:
		                throw new NotSupportedException();
#endif
                }
            }
        }
    }
}
