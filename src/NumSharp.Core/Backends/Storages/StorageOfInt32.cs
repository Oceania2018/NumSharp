﻿using System;
using NumSharp.Backends.Unmanaged;

namespace NumSharp.Backends
{
    public class StorageOfInt32 : Storage
    {
        int[] data;

        public override unsafe void* Address
        {
            get
            {
                if (_address != null)
                    return _address;

                fixed (int* ptr = &data[0])
                    return ptr;
            }
            set => base.Address = value;
        }

        public StorageOfInt32()
        {
            _typecode = NPTypeCode.Int32;
        }

        public StorageOfInt32(int x)
            => Init(new[] { x }, NumSharp.Shape.Scalar);

        public StorageOfInt32(int[] x, Shape? shape = null)
            => Init(x, shape);

        public override void Allocate(Shape shape)
            => Init(new int[shape.Size], shape);

        unsafe void Init(int[] x, Shape? shape = null)
        {
            _typecode = NPTypeCode.Int32;
            Shape = shape ?? new Shape(x.Length);
            data = x;
            _internalArray = ArraySlice.FromArray(data);
            _address = _internalArray.Address;
        }

        public override ValueType GetAtIndex(int index)
            => data == null ? _internalArray.GetIndex<int>(index) : data[index];
    }
}
