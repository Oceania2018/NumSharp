﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NumSharp.Backends;
using NumSharp.Backends.Unmanaged;
using NumSharp.Generic;
using NumSharp.Utilities;

namespace NumSharp
{
    public partial class NDArray
    {
        /// <summary>
        ///     Get and set element wise data
        ///     Low performance
        ///     Use generic Data{T} and SetData{T}(value, shape) method for better performance
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        [DebuggerHidden]
        public NDArray this[params int[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetData(indices);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Storage.SetData(value, indices);
        }

        /// <summary>
        ///     Used to perform filtering by whats true and whats false.
        /// </summary>
        /// <param name="mindices"></param>
        /// <returns></returns>
        /// <remarks>https://docs.scipy.org/doc/numpy-1.17.0/user/basics.indexing.html</remarks>
        /// <exception cref="IndexOutOfRangeException">When one of the indices exceeds limits.</exception>
        /// <exception cref="ArgumentException">indices must be of Int type (byte, u/short, u/int, u/long).</exception>
        public NDArray this[params NDArray[] mindices] => _extract_indices(mindices, false);

        private NDArray _extract_indices(NDArray[] mindices, bool isCollapsed)
        {
            if (mindices == null)
                throw new ArgumentNullException(nameof(mindices));

            if (mindices.Length == 1 && (mindices[0].ndim >= this.ndim || isCollapsed))
            {
                //element-wise if ndims are equal or src is flat
                return _index_elemntwise(mindices[0]);

                //otherwise returns like GetNDArrays() but specific indexes so we falldown
            }

            if (mindices.Length > ndim)
                throw new ArgumentException($"There are more mindices ({mindices.Length}) than dimensions ({ndim}) in current array.");

            //handle broadcasting
            var indicesShape = mindices[0].Shape.Clean();
            for (int i = 1; i < mindices.Length; i++)
            {
                if (indicesShape != mindices[i].Shape)
                {
                    mindices = DefaultEngine.Broadcast(mindices);
                    break;
                }
            }

            //case 1 multidim eq to ndim
            if (mindices.Length == ndim)
            {
                //this is multidims, collapse them into singledim
                var collapsed = new NDArray(NPTypeCode.Int32, indicesShape, false);
                var iter = new NDCoordinatesIncrementor(ref indicesShape);
                var ndims = mindices.Length;
                var collapsedIndex = new int[ndims];
                var individualIndex = iter.Index;
                Func<int[], int> getOffset = Shape.GetOffset;
                do
                {
                    for (int i = 0; i < ndims; i++)
                    {
                        collapsedIndex[i] = (int)mindices[i].GetValue(individualIndex);
                    }

                    collapsed[individualIndex] = getOffset(collapsedIndex);
                } while (iter.Next() != null);

                return _extract_indices(new NDArray[]{ collapsed }, true);
            }



            //case 2 return is not scalar collection but axis iteration
            {
                var flat_mindices = new NDArray[mindices.Length];
                for (var i = 0; i < mindices.Length; i++) flat_mindices[i] = mindices[i];

                //this is multidims, collapse them into singledim
                var collapsed = new NDArray(NPTypeCode.Int32, indicesShape, false);

                var (retShape, _) = Shape.GetSubshape(new int[collapsed.ndim]);
                var dims = collapsed.shape.Concat(retShape.dimensions).ToArray();
                var ret = new NDArray(typecode, dims); //retshape is already clean

                //this is multidims, collapse them into singledim
                var iter = new NDCoordinatesIncrementor(ref indicesShape);
                var ndims = mindices.Length;
                var collapsedIndex = new int[ndims];
                var individualIndex = iter.Index;

                do
                {
                    for (int i = 0; i < ndims; i++)
                    {
                        collapsedIndex[i] = (int)mindices[i].GetValue(individualIndex);
                    }

                    ret[individualIndex] = this[collapsedIndex];
                } while (iter.Next() != null);

                return ret;
            }

        }

        public NDArray this[string slice]
        {
            get => this[Slice.ParseSlices(slice)];
            set => Storage.GetView(Slice.ParseSlices(slice)).SetData(value);
        }

        public NDArray this[params Slice[] slices]
        {
            get => new NDArray(Storage.GetView(slices));
            set => Storage.GetView(slices).SetData(value);
        }

        //TODO! masking with boolean array
        // example:
        //
        // a = np.arange(12);
        // b=a[a > 6];
        //
        //public NDArray this[NDArray<bool> booleanArray]
        //{
        //    get
        //    {
        //        if (!Enumerable.SequenceEqual(shape, booleanArray.shape))
        //        {
        //            throw new IncorrectShapeException();
        //        }

        //        var boolDotNetArray = booleanArray.Data<bool>();

        //        switch (dtype.Name)
        //        {
        //            case "Int32":
        //                {
        //                    var nd = new List<int>();

        //                    for (int idx = 0; idx < boolDotNetArray.Length; idx++)
        //                    {
        //                        if (boolDotNetArray[idx])
        //                        {
        //                            nd.Add(Data<int>(booleanArray.Storage.Shape.GetDimIndexOutShape(idx)));
        //                        }
        //                    }

        //                    return new NDArray(nd.ToArray(), nd.Count);
        //                }
        //            case "Double":
        //                {
        //                    var nd = new List<double>();

        //                    for (int idx = 0; idx < boolDotNetArray.Length; idx++)
        //                    {
        //                        if (boolDotNetArray[idx])
        //                        {
        //                            nd.Add(Data<double>(booleanArray.Storage.Shape.GetDimIndexOutShape(idx)));
        //                        }
        //                    }

        //                    return new NDArray(nd.ToArray(), nd.Count);
        //                }
        //        }

        //        throw new NotImplementedException("");

        //    }
        //    set
        //    {
        //        if (!Enumerable.SequenceEqual(shape, booleanArray.shape))
        //        {
        //            throw new IncorrectShapeException();
        //        }

        //        object scalarObj = value.Storage.GetData().GetValue(0);

        //        bool[] boolDotNetArray = booleanArray.Storage.GetData() as bool[];

        //        int elementsAmount = booleanArray.size;

        //        for (int idx = 0; idx < elementsAmount; idx++)
        //        {
        //            if (boolDotNetArray[idx])
        //            {
        //                int[] indexes = booleanArray.Storage.Shape.GetDimIndexOutShape(idx);
        //                Array.SetValue(scalarObj, Storage.Shape.GetOffset(slice, indexes));
        //            }
        //        }

        //    }
        //}

        /// <summary>
        /// Get n-th dimension data
        /// </summary>
        /// <param name="indices">indexes</param>
        /// <returns>NDArray</returns>
        private NDArray GetData(params int[] indices)
        {
            // TODO: we can achive even better performance by distinguishing between stepped and non-stepped slices ...
            // ... non-stepped slices could still use the other solution below if GetSubshape supports slicing
            var this_shape = Storage.Shape;
            if (this_shape.IsSliced)
            {
                // in this case we can not get a slice of contiguous memory, so we slice
                return new NDArray(Storage.GetView(indices.Select(Slice.Index).ToArray()));
            }

            var (shape, offset) = Storage.Shape.GetSubshape(indices);
            return new NDArray(new UnmanagedStorage(Storage.InternalArray.Slice(offset, shape.Size), shape));
        }

        [MethodImpl((MethodImplOptions)512)]
        private NDArray _index_elemntwise(NDArray indices)
        {
            //verify the indices dtype is Int.
            var grp = indices.typecode.GetGroup();
            if (grp != 1 && grp != 2 && indices.typecode != NPTypeCode.Byte)
                throw new ArgumentException("indices must be of Int type.", nameof(indices));

            var ret = new NDArray(dtype, indices.Shape.Clean(), false);

            // ReSharper disable once LocalVariableHidesMember
            var size = this.size;
            var src = this.flat;

            if (this.ndim == 1)
            {
                var dst = ret.flat;
#if _REGEN
                #region Compute
		        switch (typecode)
		        {
			        %foreach supported_dtypes,supported_dtypes_lowercase%
			        case NPTypeCode.#1:
			        {
				        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex()) {
                            index[0] = nextIndex();
                            dst.Set#1(src.Get#1(index), indexset);
                            indexset[0]++;
                        }
                        break;
			        }
			        %
			        default:
				        throw new NotSupportedException();
		        }
                #endregion
#else

                #region Compute

                switch (typecode)
                {
                    case NPTypeCode.Boolean:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetBoolean(src.GetBoolean(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.Byte:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetByte(src.GetByte(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.Int16:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetInt16(src.GetInt16(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.UInt16:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetUInt16(src.GetUInt16(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.Int32:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetInt32(src.GetInt32(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.UInt32:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetUInt32(src.GetUInt32(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.Int64:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetInt64(src.GetInt64(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.UInt64:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetUInt64(src.GetUInt64(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.Char:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetChar(src.GetChar(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.Double:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetDouble(src.GetDouble(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.Single:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetSingle(src.GetSingle(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    case NPTypeCode.Decimal:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = new int[1];
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            index[0] = nextIndex();
                            dst.SetDecimal(src.GetDecimal(index), indexset);
                            indexset[0]++;
                        }

                        break;
                    }

                    default:
                        throw new NotSupportedException();
                }

                #endregion

#endif
            }
            else
            {
                var retIncr = new NDCoordinatesIncrementor(ret.shape);
#if _REGEN
                #region Compute
		        switch (typecode)
		        {
			        %foreach supported_dtypes,supported_dtypes_lowercase%
			        case NPTypeCode.#1:
			        {
				        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = incr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex()) {
                            ret.Set#1(src.Get#1(nextIndex()), index);
                            indexset[0]++;
                            incr.Next();
                        }
                        break;
			        }
			        %
			        default:
				        throw new NotSupportedException();
		        }
                #endregion
#else

                #region Compute

                switch (typecode)
                {
                    case NPTypeCode.Boolean:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetBoolean(src.GetBoolean(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.Byte:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetByte(src.GetByte(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.Int16:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetInt16(src.GetInt16(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.UInt16:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetUInt16(src.GetUInt16(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.Int32:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetInt32(src.GetInt32(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.UInt32:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetUInt32(src.GetUInt32(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.Int64:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetInt64(src.GetInt64(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.UInt64:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetUInt64(src.GetUInt64(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.Char:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetChar(src.GetChar(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.Double:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetDouble(src.GetDouble(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.Single:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetSingle(src.GetSingle(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    case NPTypeCode.Decimal:
                    {
                        var data = indices.AsIterator<int>(); //iterator handles cast internal if required
                        var hasNextIndex = data.HasNext;
                        var nextIndex = data.MoveNext;
                        var _nextIndexClosure = nextIndex;
                        //handle cases of negative index
                        nextIndex = () =>
                        {
                            var nextIndexValue = _nextIndexClosure();
                            if (nextIndexValue >= size)
                                throw new IndexOutOfRangeException($"index {nextIndexValue} out of bounds 0<=index<{size}");

                            return nextIndexValue < 0 ? size + nextIndexValue : nextIndexValue;
                        };

                        var index = retIncr.Index;
                        var indexset = new int[1];
                        while (hasNextIndex())
                        {
                            ret.SetDecimal(src.GetDecimal(nextIndex()), index);
                            indexset[0]++;
                            retIncr.Next();
                        }

                        break;
                    }

                    default:
                        throw new NotSupportedException();
                }

                #endregion

#endif
            }

            if (indices.ndim != ret.ndim)
                ret.Storage.SetShapeUnsafe(indices.Shape);

            return ret;
        }
    }
}
