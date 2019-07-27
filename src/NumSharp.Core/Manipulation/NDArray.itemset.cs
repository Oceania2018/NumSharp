﻿using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace NumSharp
{
    public partial class NDArray
    {
        /// <summary>
        ///     Insert scalar into an array (scalar is cast to array’s dtype, if possible)
        /// </summary>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.ndarray.itemset.html</remarks>
        public void itemset(ref Shape shape, ValueType val) 
        {
            SetData(val, shape.dimensions);
        }

        /// <summary>
        ///     Insert scalar into an array (scalar is cast to array’s dtype, if possible)
        /// </summary>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.ndarray.itemset.html</remarks>
        public void itemset(Shape shape, ValueType val) 
        {
            SetData(val, shape.dimensions);
        }

        /// <summary>
        ///     Insert scalar into an array (scalar is cast to array’s dtype, if possible)
        /// </summary>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.ndarray.itemset.html</remarks>
        public void itemset(int[] shape, ValueType val)
        {
            SetData(val, shape);
        }
    }
}
