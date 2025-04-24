using System.Numerics;

namespace System
{
    /// <summary>
    /// Represents a pseudo-random number generator, which is an algorithm that produces a sequence of numbers
    /// that meet certain statistical requirements for randomness.
    /// </summary>
    public static class RandomEx
    {
        /// <summary>
        /// Creates an array populated with items chosen at random from the provided set of choices.
        /// </summary>
        /// <param name="random">The random number generator to use.</param>
        /// <param name="choices">The items to use to populate the array.</param>
        /// <param name="length">The length of array to return.</param>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <returns>An array populated with random items.</returns>
        /// <exception cref="ArgumentException"><paramref name="choices" /> is empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="choices" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> is not zero or a positive number.</exception>
        /// <remarks>
        /// The method uses <see cref="Random.Next(int)" /> to select items randomly from <paramref name="choices" />
        /// by index. This is used to populate a newly-created array.
        /// </remarks>
        public static T[] GetItems<T>(this Random random, T[] choices, int length)
        {
            ArgumentNullException.ThrowIfNull(choices);
            T[] destination = new T[length];

            // Simple fallback: get each item individually, generating a new random Int32 for each
            // item. This is slower than the above, but it works for all types and sizes of choices.
            for (int i = 0; i < length; i++)
            {
                destination[i] = choices[random.Next(choices.Length)];
            }

            return destination;
        }
    }
}
