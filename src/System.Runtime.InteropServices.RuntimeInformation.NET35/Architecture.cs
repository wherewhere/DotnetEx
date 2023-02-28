namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Indicates the processor architecture.
    /// </summary>
    public enum Architecture
    {
        /// <summary>
        /// An Intel-based 32-bit processor architecture.
        /// </summary>
        X86,

        /// <summary>
        /// An Intel-based 64-bit processor architecture.
        /// </summary>
        X64,

        /// <summary>
        /// A 32-bit ARM processor architecture.
        /// </summary>
        Arm,

        /// <summary>
        /// A 64-bit ARM processor architecture.
        /// </summary>
        Arm64,

        /// <summary>
        /// The WebAssembly platform.
        /// </summary>
        Wasm,

        /// <summary>
        /// The S390x platform architecture.
        /// </summary>
        S390x,

        /// <summary>
        /// A LoongArch64 processor architecture.
        /// </summary>
        LoongArch64,

        /// <summary>
        /// A 32-bit ARMv6 processor architecture.
        /// </summary>
        Armv6,

        /// <summary>
        /// A PowerPC 64-bit (little-endian) processor architecture.
        /// </summary>
        Ppc64le
    }
}