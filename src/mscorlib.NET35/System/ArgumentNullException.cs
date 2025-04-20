using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System
{
    // The ArgumentException is thrown when an argument
    // is null when it shouldn't be.
    /// <summary>
    /// The exception that is thrown when a null reference (Nothing in Visual Basic) is passed
    /// to a method that does not accept it as a valid argument.
    /// </summary>
    public static class ArgumentNullExceptionEx
    {
        /// <summary>
        /// The extension for <see cref="ArgumentNullException"/> class.
        /// </summary>
        extension(ArgumentNullException)
        {
            /// <summary>
            /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.
            /// </summary>
            /// <param name="argument">The reference type argument to validate as non-null.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
            public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
            {
                if (argument is null)
                {
                    Throw(paramName);
                }
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.
            /// </summary>
            /// <param name="argument">The pointer argument to validate as non-null.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
            public static unsafe void ThrowIfNull([NotNull] void* argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
            {
                if (argument is null)
                {
                    Throw(paramName);
                }
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.
            /// </summary>
            /// <param name="argument">The pointer argument to validate as non-null.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
            internal static unsafe void ThrowIfNull(nint argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
            {
                if (argument == 0)
                {
                    Throw(paramName);
                }
            }

            [DoesNotReturn]
            internal static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
        }
    }
}
