using static System.Console;
using System.Threading.Tasks;

string logoGfx = @"
       ▄████████    ▄████████  ▄██████▄     ▄██████▄     ▄██████▄     ▄████████    ▄████████ 
      ███    ███   ███    ███ ███    ███   ███    ███   ███    ███   ███    ███   ███    ███ 
      ███    █▀    ███    ███ ███    ███   ███    █▀    ███    █▀    ███    █▀    ███    ███ 
     ▄███▄▄▄      ▄███▄▄▄▄██▀ ███    ███  ▄███         ▄███         ▄███▄▄▄      ▄███▄▄▄▄██▀ 
    ▀▀███▀▀▀     ▀▀███▀▀▀▀▀   ███    ███ ▀▀███ ████▄  ▀▀███ ████▄  ▀▀███▀▀▀     ▀▀███▀▀▀▀▀   
      ███        ▀███████████ ███    ███   ███    ███   ███    ███   ███    █▄  ▀███████████ 
      ███          ███    ███ ███    ███   ███    ███   ███    ███   ███    ███   ███    ███ 
      ███          ███    ███  ▀██████▀    ████████▀    ████████▀    ██████████   ███    ███ 
                   ███    ███                                                     ███    ███ 

        ";



SystemInit();
MainMenu();
QuitApp();

// === METHODS
void MainMenu()
{
    // Logo Init
    // TODO check logo length + screen length and calculate the offset from the center
    Clear();
    Console.Title = "▓▒░ FROGGER SHOOTER ░▒▓ V.03";
    SetCursorPosition(0, 2);
    WriteLine(logoGfx);
    SetCursorPosition(35, 16);
    WriteLine("Press any key to |PLAY|");
    ReadKey();
    Game.GameStart();
}

void SystemInit()
{
    Console.CursorVisible = false;
}

void QuitApp()
{
    Clear();
    SetCursorPosition(0, 0);
    WriteLine("|=== APP TERMINATED ===|");
    WriteLine("Press a button to exit...");
    ReadKey();
}