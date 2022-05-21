using System.Resources;
using System.Runtime.Serialization;

namespace System
{
    // The ArgumentException is thrown when an argument
    // is null when it shouldn't be.
    [Serializable]
    public class ArgumentNullExceptionEx : ArgumentNullException
    {
        // Creates a new ArgumentNullException with its message
        // string set to a default message explaining an argument was null.
        public ArgumentNullExceptionEx()
             : base(Strings.ArgumentNull_Generic)
        {
            // Use E_POINTER - COM used that for null pointers.  Description is "invalid pointer"
            HResult = HResults.E_POINTER;
        }

        public ArgumentNullExceptionEx(string? paramName)
            : base(Strings.ArgumentNull_Generic, paramName)
        {
            HResult = HResults.E_POINTER;
        }

        public ArgumentNullExceptionEx(string? message, Exception? innerException)
            : base(message, innerException)
        {
            HResult = HResults.E_POINTER;
        }

        public ArgumentNullExceptionEx(string? paramName, string? message)
            : base(message, paramName)
        {
            HResult = HResults.E_POINTER;
        }

        protected ArgumentNullExceptionEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        public static void ThrowIfNull(object? argument, string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The pointer argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        [CLSCompliant(false)]
        public static unsafe void ThrowIfNull(void* argument, string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The pointer argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        internal static unsafe void ThrowIfNull(IntPtr argument, string? paramName = null)
        {
            if (argument == IntPtr.Zero)
            {
                Throw(paramName);
            }
        }

        internal static void Throw(string? paramName) =>
            throw new ArgumentNullException(paramName);
    }
}
