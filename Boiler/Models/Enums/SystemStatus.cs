namespace Boiler.Models.Enums
{
    /// <summary>
    /// TO track the status of the boiler
    /// </summary>
    internal enum SystemStatus
    {
        /// <summary>
        /// Lockout status
        /// </summary>
        LockOut,

        /// <summary>
        /// Ready to start the boiler
        /// </summary>
        Ready,

        /// <summary>
        /// Boiler being in PrePrudge State
        /// </summary>
        PrePrudge,

        /// <summary>
        /// Boiler being in Ignition State
        /// </summary>
        Ignition,

        /// <summary>
        /// Boiler being in Operational State
        /// </summary>
        Operational,
    }
}
