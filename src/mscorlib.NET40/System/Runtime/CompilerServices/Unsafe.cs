namespace System.Runtime.CompilerServices
{
    internal static class Unsafe
    {
        public unsafe static ref T Add<T>(ref T source, nint elementOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(ptr + elementOffset);
            }
        }

        public unsafe static ref T AddByteOffset<T>(ref T source, nint byteOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(T*)((byte*)ptr + byteOffset);
            }
        }
    }
}
