using static System.Console;
using System.Threading.Tasks;

SystemInit();
MainMenu();
QuitApp();

// === METHODS
void MainMenu()
{
    Clear();
    WriteLine("Main Menu");
    ReadKey();
    Game.GameStart();
}

void SystemInit()
{
    Console.CursorVisible = false;
    frameRate = 1 / 8;    // ! Check why can't write 0,0001 numbers
}

void QuitApp()
{
    Clear();
    SetCursorPosition(0, 0);
    WriteLine("|=== APP TERMINATED ===|");
    WriteLine("Press a button to exit...");
    ReadKey();
}