﻿using System;
using NumSharp;

namespace NumSharp.Backends
{
    public class StorageOfInt32 : Storage
    {
        public StorageOfInt32()
        {
            DType = NPTypeCode.Int32;
        }

        public StorageOfInt32(int x)
            => Init(new[] { x }, NumSharp.Shape.Scalar);

        public StorageOfInt32(int[] x, Shape? shape = null)
            => Init(x, shape);

        public override void Allocate(Shape shape)
            => Init(new int[shape.Size], shape);

        unsafe void Init(int[] x, Shape? shape = null)
        {
            DType = NPTypeCode.Int32;
            Shape = shape ?? new Shape(x.Length);

            fixed (int* ptr = &x[0])
                address = ptr;
        }

        public override TensorEngine Engine => BackendFactory.GetEngine(DType);
    }
}