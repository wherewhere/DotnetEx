#if COMP_NETSTANDARD1_0
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.CompilerServices.Unsafe))]
#else
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Contains generic, low-level functionality for manipulating managed and unmanaged pointers.
    /// </summary>
    public static class Unsafe
    {
        /// <summary>
        /// Converts a managed pointer into an unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="value">The managed pointer to convert.</param>
        /// <returns>An unmanaged pointer corresponding to the original source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void* AsPointer<T>(ref readonly T value) where T : unmanaged
        {
            fixed (T* ptr = &value)
            {
                return ptr;
            }
        }

        /// <summary>
        /// Returns the size of a value of the given type parameter.
        /// </summary>
        /// <typeparam name="T">The type whose size is to be retrieved.</typeparam>
        /// <returns>The size, in bytes, of a value of type <typeparamref name="T"/>.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe int SizeOf<T>() where T : unmanaged
        {
            return sizeof(T);
        }

        /// <summary>
        /// Casts the given object to the specified type.
        /// </summary>
        /// <typeparam name="T">The type which the object will be cast to.</typeparam>
        /// <param name="o">The object to cast.</param>
        /// <returns>The original object, cast to the given type.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        [return: NotNullIfNotNull(nameof(o))]
        public static T? As<T>(object? o) where T : class?
        {
            return o as T;
        }

        /// <summary>
        /// Reinterprets the given managed pointer as a new managed pointer to a value of type <typeparamref name="TTo"/>.
        /// </summary>
        /// <typeparam name="TFrom">The type of managed pointer to reinterpret.</typeparam>
        /// <typeparam name="TTo">The desired type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to reinterpret.</param>
        /// <returns>A managed pointer to a value of type <typeparamref name="TTo"/>.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref TTo As<TFrom, TTo>(ref TFrom source)
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            fixed (TFrom* ptr = &source)
            {
                return ref *(TTo*)ptr;
            }
        }

        /// <summary>
        /// Adds an element offset to the given unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">The type whose size will be used as a scale factor for <paramref name="elementOffset"/>.</typeparam>
        /// <param name="source">The unmanaged pointer to add the offset to.</param>
        /// <param name="elementOffset">The offset to add.</param>
        /// <returns>A new unmanaged pointer that reflects the addition of the specified offset to the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void* Add<T>(void* source, int elementOffset) where T : unmanaged
        {
            return (T*)source + elementOffset;
        }

        /// <summary>
        /// Adds an offset to the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to add the offset to.</param>
        /// <param name="elementOffset">The offset to add.</param>
        /// <returns>A new managed pointer that reflects the addition of the specified offset to the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T Add<T>(ref T source, int elementOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(ptr + elementOffset);
            }
        }

        /// <summary>
        /// Adds an offset to the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to add the offset to.</param>
        /// <param name="elementOffset">The offset to add.</param>
        /// <returns>A new managed pointer that reflects the addition of the specified offset to the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T Add<T>(ref T source, nint elementOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(ptr + elementOffset);
            }
        }

        /// <summary>
        /// Adds an offset to the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to add the offset to.</param>
        /// <param name="elementOffset">The offset to add.</param>
        /// <returns>A new managed pointer that reflects the addition of the specified offset to the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T Add<T>(ref T source, nuint elementOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(ptr + elementOffset);
            }
        }

        /// <summary>
        /// Adds a byte offset to the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to add the offset to.</param>
        /// <param name="byteOffset">The offset to add.</param>
        /// <returns>A new managed pointer that reflects the addition of the specified byte offset to the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T AddByteOffset<T>(ref T source, nuint byteOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(T*)((byte*)ptr + byteOffset);
            }
        }

        /// <summary>
        /// Determines whether the specified references point to the same location.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointers.</typeparam>
        /// <param name="left">The first managed pointer to compare.</param>
        /// <param name="right">The second managed pointer to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> point to the same location; otherwise, <see langword="false"/>.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe bool AreSame<T>([AllowNull] ref readonly T left, [AllowNull] ref readonly T right) where T : unmanaged
        {
            fixed (T* leftPtr = &left)
            fixed (T* rightPtr = &right)
            {
                return leftPtr == rightPtr;
            }
        }

        /// <summary>
        /// Reinterprets the given value of type <typeparamref name="TFrom" /> as a value of type <typeparamref name="TTo" />.
        /// </summary>
        /// <returns>A value of type <typeparamref name="TTo"/>.</returns>
        /// <exception cref="NotSupportedException">The sizes of <typeparamref name="TFrom" /> and <typeparamref name="TTo" /> are not the same
        /// or the type parameters are not <see langword="struct"/>s.</exception>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe TTo BitCast<TFrom, TTo>(TFrom source)
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            if (sizeof(TFrom) != sizeof(TTo))
            {
                throw new NotSupportedException();
            }
            return ReadUnaligned<TTo>(ref As<TFrom, byte>(ref source));
        }

        /// <summary>
        /// Copies a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to copy.</typeparam>
        /// <param name="destination">The location to copy to.</param>
        /// <param name="source">A reference to the value to copy.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void Copy<T>(void* destination, ref T source) where T : unmanaged
        {
            *(T*)destination = source;
        }

        /// <summary>
        /// Copies a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to copy.</typeparam>
        /// <param name="destination">The location to copy to.</param>
        /// <param name="source">A pointer to the value to copy.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void Copy<T>(ref T destination, void* source) where T : unmanaged
        {
            destination = *(T*)source;
        }

        /// <summary>
        /// Copies bytes from the source address to the destination address.
        /// </summary>
        /// <param name="destination">The unmanaged pointer corresponding to the destination address to copy to.</param>
        /// <param name="source">The unmanaged pointer corresponding to the source address to copy from.</param>
        /// <param name="byteCount">The number of bytes to copy.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void CopyBlock(void* destination, void* source, uint byteCount)
        {
            byte* dest = (byte*)destination;
            byte* src = (byte*)source;
            for (uint i = 0; i < byteCount; i++)
            {
                dest[i] = src[i];
            }
        }

        /// <summary>
        /// Copies bytes from the source address to the destination address.
        /// </summary>
        /// <param name="destination">The managed pointer corresponding to the destination address to copy to.</param>
        /// <param name="source">The managed pointer corresponding to the source address to copy from.</param>
        /// <param name="byteCount">The number of bytes to copy.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void CopyBlock(ref byte destination, ref readonly byte source, uint byteCount)
        {
            fixed (byte* dest = &destination)
            fixed (byte* src = &source)
            {
                for (uint i = 0; i < byteCount; i++)
                {
                    dest[i] = src[i];
                }
            }
        }

        /// <summary>
        /// Copies bytes from the source address to the destination address without assuming architecture dependent alignment of the addresses.
        /// </summary>
        /// <param name="destination">The unmanaged pointer corresponding to the destination address to copy to.</param>
        /// <param name="source">The unmanaged pointer corresponding to the source address to copy from.</param>
        /// <param name="byteCount">The number of bytes to copy.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
        {
            byte* dest = (byte*)destination;
            byte* src = (byte*)source;
            for (uint i = 0; i < byteCount; i++)
            {
                dest[i] = src[i];
            }
        }

        /// <summary>
        /// Copies bytes from the source address to the destination address without assuming architecture dependent alignment of the addresses.
        /// </summary>
        /// <param name="destination">The managed pointer corresponding to the destination address to copy to.</param>
        /// <param name="source">The managed pointer corresponding to the source address to copy from.</param>
        /// <param name="byteCount">The number of bytes to copy.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void CopyBlockUnaligned(ref byte destination, ref readonly byte source, uint byteCount)
        {
            fixed (byte* dest = &destination)
            fixed (byte* src = &source)
            {
                for (uint i = 0; i < byteCount; i++)
                {
                    dest[i] = src[i];
                }
            }
        }

        /// <summary>
        /// Returns a value that indicates whether a specified managed pointer is greater than another specified managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="left">The first managed pointer to compare.</param>
        /// <param name="right">The second managed pointer to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>This check is conceptually similar to "(void*)(&amp;left) &gt; (void*)(&amp;right)".</remarks>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe bool IsAddressGreaterThan<T>([AllowNull] ref readonly T left, [AllowNull] ref readonly T right) where T : unmanaged
        {
            fixed (T* leftPtr = &left)
            fixed (T* rightPtr = &right)
            {
                return leftPtr > rightPtr;
            }
        }

        /// <summary>
        /// Returns a value that indicates whether a specified managed pointer is less than another specified managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="left">The first managed pointer to compare.</param>
        /// <param name="right">The second managed pointer to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>This check is conceptually similar to "(void*)(&amp;left) &lt; (void*)(&amp;right)".</remarks>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe bool IsAddressLessThan<T>([AllowNull] ref readonly T left, [AllowNull] ref readonly T right) where T : unmanaged
        {
            fixed (T* leftPtr = &left)
            fixed (T* rightPtr = &right)
            {
                return leftPtr < rightPtr;
            }
        }

        /// <summary>
        /// Initializes a block of memory at the given location with a given initial value.
        /// </summary>
        /// <param name="startAddress">The unmanaged pointer referencing the start of the memory block to initialize.</param>
        /// <param name="value">The value to initialize all bytes of the memory block to.</param>
        /// <param name="byteCount">The number of bytes to initialize.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void InitBlock(void* startAddress, byte value, uint byteCount)
        {
            byte* ptr = (byte*)startAddress;
            for (uint i = 0; i < byteCount; i++)
            {
                ptr[i] = value;
            }
        }

        /// <summary>
        /// Initializes a block of memory at the given location with a given initial value.
        /// </summary>
        /// <param name="startAddress">The managed pointer referencing the start of the memory block to initialize.</param>
        /// <param name="value">The value to initialize all bytes of the memory block to.</param>
        /// <param name="byteCount">The number of bytes to initialize.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void InitBlock(ref byte startAddress, byte value, uint byteCount)
        {
            fixed (byte* ptr = &startAddress)
            {
                for (uint i = 0; i < byteCount; i++)
                {
                    ptr[i] = value;
                }
            }
        }

        /// <summary>
        /// Initializes a block of memory at the given location with a given initial value without assuming architecture dependent alignment of the address.
        /// </summary>
        /// <param name="startAddress">The unmanaged pointer referencing the start of the memory block to initialize.</param>
        /// <param name="value">The value to initialize all bytes of the memory block to.</param>
        /// <param name="byteCount">The number of bytes to initialize.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void InitBlockUnaligned(void* startAddress, byte value, uint byteCount)
        {
            byte* ptr = (byte*)startAddress;
            for (uint i = 0; i < byteCount; i++)
            {
                ptr[i] = value;
            }
        }

        /// <summary>
        /// Initializes a block of memory at the given location with a given initial value without assuming architecture dependent alignment of the address.
        /// </summary>
        /// <param name="startAddress">The managed pointer referencing the start of the memory block to initialize.</param>
        /// <param name="value">The value to initialize all bytes of the memory block to.</param>
        /// <param name="byteCount">The number of bytes to initialize.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
        {
            fixed (byte* ptr = &startAddress)
            {
                for (uint i = 0; i < byteCount; i++)
                {
                    ptr[i] = value;
                }
            }
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location without assuming architecture dependent alignment of the source address.
        /// </summary>
        /// <typeparam name="T">The type of the value to read.</typeparam>
        /// <param name="source">An unmanaged pointer containing the address to read from.</param>
        /// <returns>A value of type <typeparamref name="T"/> read from the given location.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe T ReadUnaligned<T>(void* source) where T : unmanaged
        {
            return *(T*)source;
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location without assuming architecture dependent alignment of the source address.
        /// </summary>
        /// <typeparam name="T">The type of the value to read.</typeparam>
        /// <param name="source">A managed pointer containing the address to read from.</param>
        /// <returns>A value of type <typeparamref name="T"/> read from the given location.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe T ReadUnaligned<T>(scoped ref readonly byte source) where T : unmanaged
        {
            fixed (byte* ptr = &source)
            {
                return *(T*)ptr;
            }
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location without assuming architecture dependent alignment of the destination address.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="destination">A managed pointer containing the address to write to.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void WriteUnaligned<T>(void* destination, T value) where T : unmanaged
        {
            *(T*)destination = value;
        }

        /// <summary>
        /// Writes a value of type T to the given location without assuming architecture dependent alignment of the destination address.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="destination">A managed pointer containing the address to write to.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void WriteUnaligned<T>(ref byte destination, T value) where T : unmanaged
        {
            fixed (byte* ptr = &destination)
            {
                *(T*)ptr = value;
            }
        }

        /// <summary>
        /// Adds a byte offset to the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to add the offset to.</param>
        /// <param name="byteOffset">The offset to add.</param>
        /// <returns>A new managed pointer that reflects the addition of the specified byte offset to the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T AddByteOffset<T>(ref T source, nint byteOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(T*)((byte*)ptr + byteOffset);
            }
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        /// <typeparam name="T">The type of the value to read.</typeparam>
        /// <param name="source">An unmanaged pointer containing the address to read from.</param>
        /// <returns>A value of type <typeparamref name="T"/> read from the given location.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe T Read<T>(void* source) where T : unmanaged
        {
            return *(T*)source;
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="destination">The location to write to.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void Write<T>(void* destination, T value) where T : unmanaged
        {
            *(T*)destination = value;
        }

        /// <summary>
        /// Reinterprets the given location as a reference to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The unmanaged pointer to convert.</param>
        /// <returns>A managed pointer to a value of type <typeparamref name="T"/>.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T AsRef<T>(void* source) where T : unmanaged
        {
            return ref *(T*)source;
        }

        /// <summary>
        /// Reinterprets the given location as a reference to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The read-only reference to reinterpret.</param>
        /// <returns>A mutable reference to a value of type <typeparamref name="T"/>.</returns>
        /// <remarks>The lifetime of the reference will not be validated when using this API.</remarks>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T AsRef<T>(scoped ref readonly T source) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *ptr;
            }
        }

        /// <summary>
        /// Determines the byte offset from origin to target from the given managed pointers.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointers.</typeparam>
        /// <param name="origin">The managed pointer to the origin.</param>
        /// <param name="target">The managed pointer to the target.</param>
        /// <returns>The byte offset from origin to target, that is, <paramref name="target"/> - <paramref name="origin"/>.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe nint ByteOffset<T>([AllowNull] ref readonly T origin, [AllowNull] ref readonly T target) where T : unmanaged
        {
            fixed (T* originPtr = &origin)
            fixed (T* targetPtr = &target)
            {
                return (nint)(targetPtr - originPtr);
            }
        }

        /// <summary>
        /// Returns a null managed pointer to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <returns>A null managed pointer to a value of type <typeparamref name="T"/>.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T NullRef<T>() where T : unmanaged
        {
            return ref *(T*)0;
        }

        /// <summary>
        /// Determines if a given managed pointer to a value of type <typeparamref name="T"/> is a null reference.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to check.</param>
        /// <returns><see langword="true"/> if <paramref name="source"/> is a null reference; otherwise, <see langword="false"/>.</returns>
        /// <remarks>This check is conceptually similar to "(void*)(&amp;source) == nullptr".</remarks>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe bool IsNullRef<T>(ref readonly T source) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ptr == null;
            }
        }

        /// <summary>
        /// Bypasses definite assignment rules for a given reference.
        /// </summary>
        /// <typeparam name="T">The type of the reference.</typeparam>
        /// <param name="value">The reference whose initialization should be skipped.</param>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void SkipInit<T>(out T value) where T : unmanaged
        {
            fixed (T* ptr = &value) { }
        }

        /// <summary>
        /// Subtracts an offset from the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to subtract the offset from.</param>
        /// <param name="elementOffset">The offset to subtract.</param>
        /// <returns>A new managed pointer that reflects the subtraction of the specified offset from the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T Subtract<T>(ref T source, int elementOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(ptr - elementOffset);
            }
        }

        /// <summary>
        /// Subtracts an element offset from the given unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">The type whose size will be used as a scale factor for <paramref name="elementOffset"/>.</typeparam>
        /// <param name="source">The unmanaged pointer to subtract the offset from.</param>
        /// <param name="elementOffset">The offset to subtract.</param>
        /// <returns>A new unmanaged pointer that reflects the subtraction of the specified offset from the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe void* Subtract<T>(void* source, int elementOffset) where T : unmanaged
        {
            return (T*)source - elementOffset;
        }

        /// <summary>
        /// Subtracts an offset from the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to subtract the offset from.</param>
        /// <param name="elementOffset">The offset to subtract.</param>
        /// <returns>A new managed pointer that reflects the subtraction of the specified offset from the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T Subtract<T>(ref T source, nint elementOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(ptr - elementOffset);
            }
        }

        /// <summary>
        /// Subtracts an offset from the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to subtract the offset from.</param>
        /// <param name="elementOffset">The offset to subtract.</param>
        /// <returns>A new managed pointer that reflects the subtraction of the specified offset from the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T Subtract<T>(ref T source, nuint elementOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(ptr - elementOffset);
            }
        }

        /// <summary>
        /// Subtracts a byte offset from the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to subtract the offset from.</param>
        /// <param name="byteOffset">The offset to subtract.</param>
        /// <returns>A new managed pointer that reflects the subtraction of the specified byte offset from the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T SubtractByteOffset<T>(ref T source, nint byteOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(T*)((byte*)ptr - byteOffset);
            }
        }

        /// <summary>
        /// Subtracts a byte offset from the given managed pointer.
        /// </summary>
        /// <typeparam name="T">The elemental type of the managed pointer.</typeparam>
        /// <param name="source">The managed pointer to subtract the offset from.</param>
        /// <param name="byteOffset">The offset to subtract.</param>
        /// <returns>A new managed pointer that reflects the subtraction of the specified byte offset from the source pointer.</returns>
        [MethodImpl((MethodImplOptions)0x100)]
        public static unsafe ref T SubtractByteOffset<T>(ref T source, nuint byteOffset) where T : unmanaged
        {
            fixed (T* ptr = &source)
            {
                return ref *(T*)((byte*)ptr - byteOffset);
            }
        }
    }
}
#endif