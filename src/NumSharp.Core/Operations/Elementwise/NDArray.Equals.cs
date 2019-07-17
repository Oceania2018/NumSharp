﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using NumSharp.Generic;
using System.Linq;

namespace NumSharp
{
    public partial class NDArray
    {
        /// <summary>
        /// Determines if NDArray data is same
        /// </summary>
        /// <param name="obj">NDArray to compare</param>
        /// <returns>if reference is same</returns>
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case NDArray safeCastObj:
                    {
                        // We aren't using array_equal here because it assumes that both arrays have
                        // non-null Storage data and shapes.
                        var thatData = safeCastObj.Storage?.GetData();
                        var thisData = this.Storage?.GetData();
                        if ((thatData == null && thisData != null) ||(thatData != null && thisData == null))
                        {
                            return false;
                        }
                        if (thisData != null)
                        {
                            if (thatData != null)
                            {
                                // Compare array contents, which is clumsy since we don't know the element type
                                if (thisData.Length == thatData.Length)
                                {
                                    for(int i = 0; i < thisData.Length; i++)
                                    {
                                        if (!thisData.GetValue(i).Equals(thatData.GetValue(i)))
                                        {
                                            return false;
                                        }
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (thatData != null)
                            {
                                return false;
                            }
                        }

                        if (this.shape != null && safeCastObj.shape != null)
                        {
                            return this.shape.SequenceEqual(safeCastObj.shape);
                        }
                        else
                        {
                            return this.shape == safeCastObj.shape;
                        }
                    }
                case int val:
                    return Data<int>(0) == val;
                // Other object is not of Type NDArray, return false immediately.
                default:
                    return false;
            }

        }

        public static NDArray<bool> operator ==(NDArray np, object obj)
        {
            if (obj is NDArray np2)
            {
                return np.equal(np2);
            }
            var boolTensor = new NDArray(typeof(bool),np.shape);
            bool[] bools = boolTensor.Storage.GetData() as bool[];

            switch (np.Storage.GetData())
            {
                case int[] values :
                {
                    int value = Convert.ToInt32(obj);                 
                    for(int idx =0; idx < bools.Length;idx++)
                    {
                        if ( values[idx] == value )
                            bools[idx] = true;
                    }
                    break;
                }
                case Int64[] values :
                {
                    Int64 value = Convert.ToInt64(obj);                 
                    for(int idx =0; idx < bools.Length;idx++)
                    {
                        if ( values[idx] == value )
                            bools[idx] = true;
                    }
                    break;
                }
                case float[] values :
                {
                    float value = Convert.ToSingle(obj);                 
                    for(int idx =0; idx < bools.Length;idx++)
                    {
                        if ( values[idx] == value )
                            bools[idx] = true;
                    }
                    break;
                }
                case double[] values :
                {
                    double value = Convert.ToDouble(obj);                 
                    for(int idx =0; idx < bools.Length;idx++)
                    {
                        if ( values[idx] == value )
                            bools[idx] = true;
                    }
                    break;
                }
                case Complex[] values :
                {
                    Complex value = (Complex) obj;                 
                    for(int idx =0; idx < bools.Length;idx++)
                    {
                        if ( values[idx] == value )
                            bools[idx] = true;
                    }
                    break;
                }
                /*case Quaternion[] values :
                {
                    Quaternion value = (Quaternion) obj;                 
                    for(int idx =0; idx < bools.Length;idx++)
                    {
                        if ( values[idx] == value )
                            bools[idx] = true;
                    }
                    break;
                }*/
                default :
                {
                    throw new IncorrectTypeException();
                } 
            }

            return boolTensor.MakeGeneric<bool>();
        }

        /// NumPy signature: numpy.equal(x1, x2, /, out=None, *, where=True, casting='same_kind', order='K', dtype=None, subok=True[, signature, extobj]) = <ufunc 'equal'>
        /// <summary>
        /// Compare two NDArrays element wise
        /// </summary>
        /// <param name="np2">NDArray to compare with</param>
        /// <returns>NDArray with result of each element compare</returns>
        private NDArray<bool> equal(NDArray np2)
        {
            if (this.size != np2.size)
            {
                throw new ArgumentException("Different sized NDArray's in not yet supported by the equal operation", nameof(np2));
            }
            var boolTensor = new NDArray(typeof(bool), this.shape);
            bool[] bools = boolTensor.Storage.GetData() as bool[];

            var values1 = this.Storage.GetData();
            var values2 = np2.Storage.GetData();
            for (int idx = 0; idx < bools.Length; idx++)
            {
                var v1 = values1.GetValue(idx);
                var v2 = values2.GetValue(idx);
                if (v1.Equals(v2))
                    bools[idx] = true;
            }
            
            return boolTensor.MakeGeneric<bool>();
        }

        /// NumPy signature: numpy.array_equal(a1, a2)[source]
        /// <summary>
        /// Compares two NDArrays
        /// </summary>
        /// <param name="np2"></param>
        /// <returns>True if two arrays have the same shape and elements, False otherwise.</returns>
        public bool array_equal(NDArray np2)
        {
            if (!Enumerable.SequenceEqual(this.shape, np2.shape))
            {
                return false;
            }
            var values1 = this.Storage.GetData();
            var values2 = np2.Storage.GetData();
            for (int idx = 0; idx < values1.Length; idx++)
            {
                var v1 = values1.GetValue(idx);
                var v2 = values2.GetValue(idx);
                if (!v1.Equals(v2))
                    return false;
            }

            return true;
        }


    }
}
