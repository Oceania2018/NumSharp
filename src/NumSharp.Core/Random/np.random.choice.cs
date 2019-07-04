﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NumSharp
{
    public partial class NumPyRandom
    {
        /// <summary>
        /// Generates a random sample from a given 1-D array
        /// </summary>
        /// <param name="arr">If an ndarray, a random sample is generated from its elements. If an int, the random sample is generated as if a were np.arange(a)</param>
        /// <param name="shape">Output shape. If the given shape is, e.g., (m, n, k), then m * n * k samples are drawn. Default is None, in which case a single value is returned.</param>
        /// <param name="replace">Whether the sample is with or without replacement</param>
        /// <param name="probabilities">The probabilities associated with each entry in a. If not given the sample assumes a uniform distribution over all entries in a.</param>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.random.choice.html</remarks>
        public NDArray choice(NDArray arr, Shape shape = null, bool replace = true, double[] probabilities = null)
        {
            int arrSize = arr.len;
            NDArray idx = np.random.choice(arrSize, shape, probabilities: probabilities);
            return arr[idx];
        }

        /// <summary>
        ///  Generates a random sample from a given 1-D array
        /// </summary>
        /// <param name="a">If an ndarray, a random sample is generated from its elements. If an int, the random sample is generated as if a were np.arange(a)</param>
        /// <param name="shape">Output shape. If the given shape is, e.g., (m, n, k), then m * n * k samples are drawn. Default is None, in which case a single value is returned.</param>
        /// <param name="replace">Whether the sample is with or without replacement</param>
        /// <param name="probabilities">The probabilities associated with each entry in a. If not given the sample assumes a uniform distribution over all entries in a.</param>
        /// <remarks>https://docs.scipy.org/doc/numpy/reference/generated/numpy.random.choice.html</remarks>
        public NDArray choice(int a, Shape shape = null, bool replace = true, double[] probabilities = null)
        {
            if (shape is null)
            {
                shape = 1;
            }
            NDArray arr = np.arange(a);
            NDArray idx = null;
            //Debug.WriteLine($"arr: {arr}");
            if (probabilities is null)
            {
                idx = np.random.randint(0, arr.len, shape);
            }
            else
            {
                NDArray cdf = np.cumsum(probabilities);
                cdf /= cdf[cdf.len - 1];
                NDArray uniformSamples = np.random.uniform(0, 1, shape);
                idx = np.searchsorted(cdf, uniformSamples);
            }
            return idx;
        }
    }
}
