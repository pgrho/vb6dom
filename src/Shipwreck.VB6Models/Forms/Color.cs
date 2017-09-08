using System;
using System.Runtime.InteropServices;

namespace Shipwreck.VB6Models.Forms
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Color : IEquatable<Color>
    {
        [FieldOffset(3)]
        public byte A;

        [FieldOffset(2)]
        public byte R;

        [FieldOffset(1)]
        public byte G;

        [FieldOffset(0)]
        public byte B;

        public static unsafe bool operator ==(Color left, Color right)
            => *(int*)&left == *(int*)&right;

        public static unsafe bool operator !=(Color left, Color right)
            => *(int*)&left != *(int*)&right;

        public override bool Equals(object obj)
            => obj is Color c && this == c;

        public override unsafe int GetHashCode()
            => A << 24 ^ R << 16 ^ G << 8 ^ B;

        public bool Equals(Color other)
            => this == other;
    }
}