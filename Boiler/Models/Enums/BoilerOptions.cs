namespace Boiler.Models.Enums
{
    /// <summary>
    /// Options to choose for boiler
    /// </summary>
    internal enum BoilerOptions
    {
        /// <summary>
        /// To toggle the switch
        /// </summary>
        ToggleSwitch = 1,

        /// <summary>
        /// To Start the Boiler
        /// </summary>
        StartBoiler,

        /// <summary>
        /// To Stop the Boiler
        /// </summary>
        StopBoiler,

        /// <summary>
        /// To Stimulate Error
        /// </summary>
        SimulateError,

        /// <summary>
        /// To reset the lock
        /// </summary>
        ResetLock,

        /// <summary>
        /// To Display the Log
        /// </summary>
        DisplayLog,

        /// <summary>
        /// To Exit the application
        /// </summary>
        Exit,
    }
}
