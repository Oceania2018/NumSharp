﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NumSharp
{
    public static partial class np
    {
        /// <summary>
        ///     Return a new float array of given shape, filled with zeros.
        /// </summary>
        /// <param name="shapes">Shape of the new array,</param>
        /// <returns>Array of zeros with the given shape, dtype.</returns>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.zeros.html</remarks>
        public static NDArray zeros(params int[] shapes)
        {
            return zeros(shapes, null); //theres a fallback from null.
        }

        /// <summary>
        ///     Return a new float array of given shape, filled with zeros.
        /// </summary>
        /// <param name="shapes">Shape of the new array,</param>
        /// <returns>Array of zeros with the given shape, type <typeparamref name="T"/>.</returns>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.zeros.html</remarks>
        public static NDArray zeros<T>(params int[] shapes)
        {
            return zeros(shapes, typeof(T));
        }

        /// <summary>
        ///     Return a new float array of given shape, filled with zeros.
        /// </summary>
        /// <param name="shape">Shape of the new array,</param>
        /// <param name="dtype">The desired data-type for the array, e.g., <see cref="uint8"/>. Default is <see cref="float64"/> / <see cref="double"/>.</param>
        /// <returns>Array of zeros with the given shape, dtype.</returns>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.zeros.html</remarks>
        public static NDArray zeros(Shape shape, Type dtype = null)
        {
            return new NDArray(dtype ?? np.float64, shape);
        }
    }
}
