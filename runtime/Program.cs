#pragma warning disable CA1416 // Validate platform compatibility

using BotwRegistryToolkit.Runtime;
using BotwRegistryToolkit.Runtime.Models;
using System.Media;

if (args.Length < 2) {
    return;
}

// Hide window
WindowHelper.SetWindowMode(WindowMode.Hide);

bool error = false;
try {
    await Parallel.ForEachAsync(CommandsModel.GetCommands(args), async (command, cancellationToken) => {
        try {
            await Task.Run(command, cancellationToken);
        }
        catch (Exception ex) {
            HandleException(ex);
        }
    });
}
catch (Exception ex) {
    HandleException(ex);
}

void HandleException(Exception ex)
{
    error = true;

    WindowHelper.SetWindowMode(WindowMode.Show);
    Console.ForegroundColor = ConsoleColor.DarkRed;
    SystemSounds.Exclamation.Play();
    Console.WriteLine(ex);
    Console.ResetColor();
}

if (error) {
    Console.WriteLine("\nPress enter to continue. . .");
    Console.ReadLine();
}