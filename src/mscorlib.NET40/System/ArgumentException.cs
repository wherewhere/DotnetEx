using System.Diagnostics.CodeAnalysis;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// The exception that is thrown when one of the arguments provided to a method is not valid.
    /// </summary>
    public static class ArgumentExceptionEx
    {
        /// <summary>
        /// The extension for <see cref="ArgumentException"/> class.
        /// </summary>
        extension(ArgumentException)
        {
            /// <summary>
            /// Throws an exception if <paramref name="argument"/> is null or empty.
            /// </summary>
            /// <param name="argument">The string argument to validate as non-null and non-empty.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
            /// <exception cref="ArgumentNullException"><paramref name="argument"/> is null.</exception>
            /// <exception cref="ArgumentException"><paramref name="argument"/> is empty.</exception>
            public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
            {
                if (IsNullOrEmpty(argument))
                {
                    ArgumentException.ThrowNullOrEmptyException(argument, paramName);
                }
                [MethodImpl((MethodImplOptions)0x100)]
                static bool IsNullOrEmpty([NotNullWhen(false)] string? value) => string.IsNullOrEmpty(value);
            }

            /// <summary>
            /// Throws an exception if <paramref name="argument"/> is null, empty, or consists only of white-space characters.
            /// </summary>
            /// <param name="argument">The string argument to validate.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
            /// <exception cref="ArgumentNullException"><paramref name="argument"/> is null.</exception>
            /// <exception cref="ArgumentException"><paramref name="argument"/> is empty or consists only of white-space characters.</exception>
            public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
            {
                if (IsNullOrWhiteSpace(argument))
                {
                    ArgumentException.ThrowNullOrWhiteSpaceException(argument, paramName);
                }
                [MethodImpl((MethodImplOptions)0x100)]
                static bool IsNullOrWhiteSpace([NotNullWhen(false)] string? value) => string.IsNullOrWhiteSpace(value);
            }

            [DoesNotReturn]
            private static void ThrowNullOrEmptyException(string? argument, string? paramName)
            {
                ArgumentNullException.ThrowIfNull(argument, paramName);
                throw new ArgumentException(Strings.Argument_EmptyString, paramName);
            }

            [DoesNotReturn]
            private static void ThrowNullOrWhiteSpaceException(string? argument, string? paramName)
            {
                ArgumentNullException.ThrowIfNull(argument, paramName);
                throw new ArgumentException(Strings.Argument_EmptyOrWhiteSpaceString, paramName);
            }
        }
    }
}
