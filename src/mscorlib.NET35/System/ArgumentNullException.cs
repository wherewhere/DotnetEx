using System.Resources;
using System.Runtime.Serialization;

namespace System
{
    // The ArgumentException is thrown when an argument
    // is null when it shouldn't be.
    /// <summary>
    /// The exception that is thrown when a null reference (Nothing in Visual Basic) is passed
    /// to a method that does not accept it as a valid argument.
    /// </summary>
    [Serializable]
    public class ArgumentNullExceptionEx : ArgumentNullException
    {
        // Creates a new ArgumentNullException with its message
        // string set to a default message explaining an argument was null.
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullException"/> class.
        /// </summary>
        public ArgumentNullExceptionEx()
             : base(Strings.ArgumentNull_Generic)
        {
            // Use E_POINTER - COM used that for null pointers.  Description is "invalid pointer"
            HResult = HResults.E_POINTER;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullException"/> class with
        /// the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        public ArgumentNullExceptionEx(string paramName)
            : base(Strings.ArgumentNull_Generic, paramName)
        {
            HResult = HResults.E_POINTER;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullException"/> class with a
        /// specified error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ArgumentNullExceptionEx(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = HResults.E_POINTER;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ArgumentNullException"/> class with a
        /// specified error message and the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        public ArgumentNullExceptionEx(string paramName, string message)
            : base(message, paramName)
        {
            HResult = HResults.E_POINTER;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        protected ArgumentNullExceptionEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        public static void ThrowIfNull(object argument, string paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The pointer argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        public static unsafe void ThrowIfNull(void* argument, string paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The pointer argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        internal static unsafe void ThrowIfNull(IntPtr argument, string paramName = null)
        {
            if (argument == IntPtr.Zero)
            {
                Throw(paramName);
            }
        }

        internal static void Throw(string paramName) =>
            throw new ArgumentNullException(paramName);
    }
}
