﻿using System;
using System.Collections.Generic;
using System.Text;
using NumSharp.Utilities;

namespace NumSharp.Backends
{
    public abstract partial class Storage
    {
        public void SetValue(ValueType value, params int[] indices)
        {
            var offset = _shape.GetOffset(indices);
            SetAtIndex(value, offset);
        }

        public void SetValue(object value, params int[] indices)
        {
            throw new NotImplementedException();
        }

        public virtual void SetValue<T>(T value, params int[] indices) where T : unmanaged
        {
            throw new NotImplementedException();
        }

        public void SetAtIndex(object value, int index)
        {
            throw new NotImplementedException();
        }

        public virtual void SetAtIndex(ValueType value, int index)
        {
            throw new NotImplementedException();
        }

        public void SetBoolean(bool value, params int[] indices)
        {
            throw new NotImplementedException();
        }

        public void SetByte(byte value, params int[] indices)
        {
            throw new NotImplementedException();
        }

        public void SetInt32(int value, params int[] indices)
        {
            throw new NotImplementedException();
        }

        public void SetInt64(long value, params int[] indices)
        {
            throw new NotImplementedException();
        }

        public void SetSingle(float value, params int[] indices)
        {
            throw new NotImplementedException();
        }

        public void SetDouble(double value, params int[] indices)
        {
            throw new NotImplementedException();
        }
    }
}
