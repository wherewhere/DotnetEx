using System.Runtime.InteropServices.Resources;

namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Represents an operating system platform.
    /// </summary>
    public readonly struct OSPlatform : IEquatable<OSPlatform>
    {
        /// <summary>
        /// Gets an object that represents the FreeBSD operating system.
        /// </summary>
        /// <value>An object that represents the FreeBSD operating system.</value>
        public static OSPlatform FreeBSD { get; } = new OSPlatform("FREEBSD");

        /// <summary>
        /// Gets an object that represents the Linux operating system.
        /// </summary>
        /// <value>An object that represents the Linux operating system.</value>
        public static OSPlatform Linux { get; } = new OSPlatform("LINUX");

        /// <summary>
        /// Gets an object that represents the OSX operating system.
        /// </summary>
        /// <value>An object that represents the OSX operating system.</value>
        public static OSPlatform OSX { get; } = new OSPlatform("OSX");

        /// <summary>
        /// Gets an object that represents the Windows operating system.
        /// </summary>
        /// <value>An object that represents the Windows operating system.</value>
        public static OSPlatform Windows { get; } = new OSPlatform("WINDOWS");

        internal string Name { get; }

        private OSPlatform(string osPlatform)
        {
            if (osPlatform.Length == 0)
            {
                throw new ArgumentException(Strings.Argument_EmptyValue, nameof(osPlatform));
            }

            Name = osPlatform ?? throw new ArgumentNullException(nameof(osPlatform));
        }

        /// <summary>
        /// Creates a new OSPlatform instance.
        /// </summary>
        /// <remarks>If you plan to call this method frequently, please consider caching its result.</remarks>
        public static OSPlatform Create(string osPlatform)
        {
            return new OSPlatform(osPlatform);
        }

        /// <summary>
        /// Determines whether the current instance and the specified <see cref="OSPlatform"/> instance are equal.
        /// </summary>
        /// <param name="other">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance and <paramref name="other"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(OSPlatform other)
        {
            return Equals(other.Name);
        }

        internal bool Equals(string other)
        {
            return string.Equals(Name, other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the current <see cref="OSPlatform"/> instance is equal to the specified object.
        /// </summary>
        /// <param name="obj"><see langword="true"/> if <paramref name="obj"/> is a <see cref="OSPlatform"/> instance
        /// and its name is the same as the current object; otherwise, <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="OSPlatform"/> instance
        /// and its name is the same as the current object.</returns>
        public override bool Equals(object obj)
        {
            return obj is OSPlatform osPlatform && Equals(osPlatform);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Name == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

        /// <summary>
        /// Returns the string representation of this <see cref="OSPlatform"/> instance.
        /// </summary>
        /// <returns>A string that represents this <see cref="OSPlatform"/> instance.</returns>
        public override string ToString()
        {
            return Name ?? string.Empty;
        }

        /// <summary>
        /// Determines whether two <see cref="OSPlatform"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(OSPlatform left, OSPlatform right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="OSPlatform"/> instances are unequal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are unequal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(OSPlatform left, OSPlatform right)
        {
            return !(left == right);
        }
    }
}