namespace Boiler.View
{
    internal class UserInput
    {
        /// <summary>
        /// user to enter the enum.
        /// </summary>
        /// <param name="prompt">The message displayed to the user.</param>
        /// <returns>The entered number if valid; otherwise, null.</returns>
        public static T? ReadEnum<T>(string prompt)
    where T : struct, Enum
        {
            Console.Write(prompt);

            if (int.TryParse(Console.ReadLine(), out int number) &&
                Enum.IsDefined(typeof(T), number))
            {
                return (T)Enum.ToObject(typeof(T), number);
            }

            ConsolePrinter.WriteLine("Invalid choice. Please select a valid option.");
            return null;
        }
    }
}
