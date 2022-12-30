#pragma warning disable CA1416 // Validate platform compatibility

using BotwRegistryToolkit.Runtime;
using BotwRegistryToolkit.Runtime.Models;
using System.Media;

if (args.Length < 2) {
    return;
}

// Hide window
WindowHelper.SetWindowMode(WindowMode.Hide);

try {
    await CommandsModel.GetCommand(args);
}
catch (Exception ex) {
    WindowHelper.SetWindowMode(WindowMode.Show);
    Console.ForegroundColor = ConsoleColor.DarkRed;
    SystemSounds.Exclamation.Play();
    Console.WriteLine(ex);
    Console.ResetColor();

    Console.WriteLine("\nPress enter to continue. . .");
    Console.ReadLine();
}