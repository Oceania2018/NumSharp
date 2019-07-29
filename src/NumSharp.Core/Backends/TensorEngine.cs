﻿using System;
using System.Collections.Generic;
using System.Text;
using NumSharp.Backends;
using NumSharp.Backends.Unmanaged;
using NumSharp.Generic;

namespace NumSharp
{
    public abstract class TensorEngine
    {
        #region Allocation

        /// <summary>
        ///     Get storage for given <paramref name="dtype"/>.
        /// </summary>
        public abstract UnmanagedStorage GetStorage(Type dtype);

        /// <summary>
        ///     Get storage for given <paramref name="typeCode"/>.
        /// </summary>
        public abstract UnmanagedStorage GetStorage(NPTypeCode typeCode);

        #endregion

        #region Math

        #region Reduction

        public abstract NDArray ReduceAdd(in NDArray arr, int? axis_, bool keepdims = false, NPTypeCode? typeCode = null);
        public abstract NDArray ReduceMean(in NDArray arr, int? axis_, bool keepdims = false, NPTypeCode? typeCode = null);

        #endregion

        public abstract NDArray Add(in NDArray lhs, in NDArray rhs);
        public abstract NDArray Dot(in NDArray x, in NDArray y);
        public abstract NDArray Divide(in NDArray lhs, in NDArray rhs);
        public abstract NDArray Log(in NDArray nd, Type dtype = null);
        public abstract NDArray Log(in NDArray nd, NPTypeCode typeCode);
        public abstract NDArray Matmul(in NDArray lhs, in NDArray rhs);
        public abstract NDArray Mean(in NDArray nd, int? axis = null, NPTypeCode? typeCode = null, bool keepdims = false);
        public abstract NDArray Mean(in NDArray nd, int axis, Type dtype, bool keepdims = false);
        public abstract NDArray Multiply(NDArray lhs, NDArray rhs);
        public abstract NDArray Power(in NDArray lhs, in ValueType rhs, Type type = null);
        public abstract NDArray Power(in NDArray lhs, in ValueType rhs, NPTypeCode typeCode);
        public abstract NDArray Subtract(in NDArray lhs, in NDArray rhs);
        public abstract NDArray Sum(in NDArray nd, int? axis = null, NPTypeCode? typeCode = null, bool keepdims = false);
        public abstract NDArray Sum(in NDArray nd, int axis, Type dtype, bool keepdims = false);
        public abstract NDArray Negate(in NDArray nd);

        #endregion

        #region Logic

        public abstract NDArray<bool> Compare(in NDArray lhs, in NDArray rhs);
        public abstract bool All(NDArray nd);
        public abstract NDArray<bool> All(NDArray nd, int axis);
        public abstract bool AllClose(NDArray a, NDArray b, double rtol = 1.0E-5, double atol = 1.0E-8, bool equal_nan = false);
        public abstract NDArray<bool> IsClose(NDArray a, NDArray b, double rtol = 1.0E-5, double atol = 1.0E-8, bool equal_nan = false);
        public abstract NDArray<bool> IsFinite(NDArray a);
        public abstract NDArray<bool> IsNan(NDArray a);

        #endregion

        #region Array Manipulation

        public abstract NDArray CreateNDArray(Shape shape, Type dtype = null, Array buffer = null, char order = 'C');
        public abstract NDArray CreateNDArray(Shape shape, Type dtype = null, IArraySlice buffer = null, char order = 'C');
        public abstract NDArray Transpose(NDArray nd, int[] axes = null);
        public abstract NDArray Cast(NDArray x, Type dtype, bool copy);
        public abstract NDArray Cast(NDArray x, NPTypeCode dtype, bool copy);

        #endregion

        #region Sorting, searching, counting

        public abstract NDArray ArgMax(NDArray nd, int axis = -1);

        #endregion

    }
}
