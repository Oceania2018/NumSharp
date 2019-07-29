﻿using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumSharp.Backends;
using NumSharp.Backends.Unmanaged;

namespace NumSharp.UnitTest.Backends.Unmanaged.Math
{
    [TestClass]
    public class np_multiply_tests
    {
        [TestMethod]
        public void Multiply()
        {
            var left = np.zeros(new Shape(5, 5)) + 5d;
            var right = np.ones(new Shape(5, 5));
            var ret = left * right;
            ret.GetData<double>().All(d => d == 5).Should().BeTrue();

            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }
#if _REGEN
        %a = except(supported_dtypes, "NDArray", "Boolean")
        %foreach a
        [DataRow(NPTypeCode.Boolean, NPTypeCode.#1)]
#else
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Boolean)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Byte)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Int16)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.UInt16)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Int32)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.UInt32)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Int64)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.UInt64)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Char)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Double)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Single)]
        [DataRow(NPTypeCode.Boolean, NPTypeCode.Decimal)]
#endif
        [DataTestMethod]
        public void MultiplyAllPossabilitiesBoolean(NPTypeCode ltc, NPTypeCode rtc)
        {
            var left = np.ones(new Shape(5, 5), rtc);
            var right = np.ones(new Shape(5, 5), ltc);
            var ret = left * right;

            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(1);
                Console.WriteLine(val);
            }
        }

        [TestMethod]
        public void MultiplyUpcast()
        {
            var left = (np.zeros(new Shape(5, 5)) + 5d).astype(NPTypeCode.Single);
            var right = np.ones(new Shape(5, 5)).astype(NPTypeCode.Int32);
            np._FindCommonArrayType(left.dtype, right.dtype).Should().Be(NPTypeCode.Double);
            var ret = left * right;

            ret.GetTypeCode.Should().Be(NPTypeCode.Double);
            ret.GetData<double>().All(d => d == 5).Should().BeTrue();

            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }

        [TestMethod]
        public void MultiplyDowncast()
        {
            var left = (np.zeros(new Shape(5, 5)) + 5d).astype(NPTypeCode.Single);
            var right = (np.ones(new Shape(5, 5)) + 1d).astype(NPTypeCode.Int32);
            np._FindCommonArrayType(left.dtype, right.dtype).Should().Be(NPTypeCode.Double);
            var ret = left * right;

            ret.GetTypeCode.Should().Be(NPTypeCode.Double);
            ret.GetData<double>().All(d => d == 10).Should().BeTrue();

            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }

        [TestMethod]
        public void Multiply_Two_Scalars()
        {
            var left = NDArray.Scalar(2d) * NDArray.Scalar(5d);
            left.GetDouble(0).Should().Be(10);
        }

        [TestMethod]
        public void Multiply_ND_3_1_vs_1_3()
        {
            var left = np.arange(3).reshape((3, 1)).astype(np.float64);
            var right = np.arange(3).reshape((1, 3)).astype(np.float64);
            var ret = left * right;
            ret.size.Should().Be(9);
            ret.dtype.Should().Be<double>();
            ret.GetDouble(0, 0).Should().Be(0);
            ret.GetDouble(1, 1).Should().Be(1);
            ret.GetDouble(2, 2).Should().Be(4);

            ret.GetDouble(1, 0).Should().Be(0);
            ret.GetDouble(2, 0).Should().Be(0);

            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }

        [TestMethod]
        public void Multiply_ND_2_1_3_3_vs_1_3()
        {
            var left = np.arange(2 * 3 * 3).reshape((2, 1, 3, 3)).astype(np.float64);
            var right = np.arange(3).reshape((1, 3)).astype(np.float64);
            var ret = left * right;

            for (int i = 0; i < ret.size; i++) 
                Console.WriteLine(ret.GetAtIndex(i));

            ret.size.Should().Be(18);
            ret.dtype.Should().Be<double>();
            ret.GetDouble(0, 0, 0, 0).Should().Be(0);
            ret.GetDouble(0, 0, 1, 1).Should().Be(4);
            ret.GetDouble(0, 0, 2, 2).Should().Be(16);

            ret.GetDouble(1, 0, 0, 0).Should().Be(0);
            ret.GetDouble(1, 0, 1, 1).Should().Be(13);
            ret.GetDouble(1, 0, 2, 2).Should().Be(34);
            ret.GetDouble(1, 0, 0, 2).Should().Be(22);
        }

        [TestMethod]
        public void Multiply_ND_2_3_3()
        {
            var left = np.arange(6).reshape((2, 3, 1)).astype(np.float64);
            var right = np.arange(6).reshape((2, 1, 3)).astype(np.float64);
            var ret = left * right;
            ret.size.Should().Be(18);
            ret.dtype.Should().Be<double>();
            ret.GetDouble(0, 0, 0).Should().Be(0);
            ret.GetDouble(0, 1, 1).Should().Be(1);
            ret.GetDouble(0, 2, 2).Should().Be(4);

            ret.GetDouble(1, 0, 0).Should().Be(9);
            ret.GetDouble(1, 1, 1).Should().Be(16);
            ret.GetDouble(1, 2, 2).Should().Be(25);

            ret.GetDouble(1, 0, 1).Should().Be(12);
            ret.GetDouble(1, 0, 2).Should().Be(15);
            ret.GetDouble(1, 1, 2).Should().Be(20);

            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }

        [TestMethod]
        public void Multiply_RightScalar()
        {
            var left = np.zeros(new Shape(5, 5), np.float64) + 5d;
            var right = NDArray.Scalar(2d);
            var ret = left * right;
            ret.Cast<double>().All(d => d == 10).Should().BeTrue();

            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }

        [TestMethod]
        public void Multiply_LeftScalar()
        {
            var left = NDArray.Scalar(2d);
            var right = np.zeros(new Shape(5, 5), np.float64) + 5d;
            var ret = left * right;
            ret.Cast<double>().All(d => d == 10).Should().BeTrue();

            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }

        [TestMethod]
        public void Multiply_Rising()
        {
            var left = np.zeros(new Shape(5, 5), np.float64);
            for (int i = 0; i < 25; i++)
            {
                left.SetAtIndex<double>(i, i);
            }

            var right = np.zeros(new Shape(5, 5), np.float64);
            for (int i = 0; i < 25; i++)
            {
                right.SetAtIndex<double>(i, i);
            }

            var ret = left * right;

            ret.Array.As<ArraySlice<double>>().Should().BeInAscendingOrder();

            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }

        [TestMethod]
        public void Multiply_RightScalar_Rising()
        {
            var left = np.zeros(new Shape(5, 5), np.float64);
            for (int i = 0; i < 25; i++) 
                left.SetAtIndex<double>(i, i);

            var right = NDArray.Scalar(2d);
            var ret = left * right;
            ret.Should().BeInAscendingOrder();
            ret.GetDouble(0).Should().Be(0);
            ret.GetAtIndex<double>(24).Should().Be(48);
            for (int i = 0; i < ret.size; i++)
            {
                Console.WriteLine(ret.GetAtIndex(i));
            }
        }

        [TestMethod]
        public void Multiply_LeftScalar_Rising()
        {
            var left = NDArray.Scalar(2d);
            var right = np.zeros(new Shape(5, 5), np.float64);
            for (int i = 0; i < 25; i++) right.SetAtIndex<double>(i, i);

            var ret = left * right;
            ret.Should().BeInAscendingOrder();

            for (int i = 0; i < ret.size; i++) Console.WriteLine(ret.GetAtIndex(i));
        }


#if _REGEN
        %a = except(supported_dtypes, "NDArray")
        %b = [true,"1","1","1","1","1u","1L","1UL","1","1d","1f","1m"]
        %foreach forevery(a,a,true), forevery(b,b,true)%
        [TestMethod]
        public void Multiply_#1_To_#2()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.#1) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.#2) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        %
#else

        [TestMethod]
        public void Multiply_Boolean_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Boolean_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Boolean) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Byte_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Byte) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int16_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt16_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt16) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int32_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt32_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt32) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Int64_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Int64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_UInt64_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.UInt64) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_Double()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Double) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Char_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Char) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_Int16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_UInt16()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt16) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_Int32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_UInt32()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt32) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_Int64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Int64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_UInt64()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.UInt64) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_Char()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Char) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_Single()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Single) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Double_To_Decimal()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Double) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Decimal) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Single_To_Boolean()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Single) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Boolean) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
        [TestMethod]
        public void Multiply_Single_To_Byte()
        {
            var left = np.zeros(new Shape(5, 5), NPTypeCode.Single) + 3;
            var right = np.ones(new Shape(5, 5), NPTypeCode.Byte) + 1;
            var ret = left * right;
            
            for (int i = 0; i < ret.size; i++)
            {
                var val = ret.GetAtIndex(i);
                Convert.ToInt32(val).Should().Be(6);
                Console.WriteLine(val);
            }
        }
#endif
    }
}
