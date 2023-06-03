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
//TestCode();
MainMenu();
QuitApp();


// === TEST CODE
// void TestCode()
// {
//     WriteLine("=== Starting Test Code Space");
//     WriteLine("Test 2d array length reading");
//     char[,] testArray = {
//     {'x', 'x', 'x'},
//     {'x', 'x', 'x'}
// };
//     int x = testArray.GetLength(1);
//     int y = testArray.GetLength(0);
//     WriteLine(x);
//     WriteLine(y);

//     WriteLine("=== Test Code Space Ended | Press a key to continue . . .");
//     ReadKey();
// }

// === METHODS
void SystemInit()
{
    Console.CursorVisible = false;
}

void MainMenu()
{
    // ! MAIN MENU TEMP PLACEHOLDER
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

void QuitApp()
{
    Clear();
    SetCursorPosition(0, 0);
    WriteLine("|=== APP TERMINATED ===|");
    WriteLine("Press a button to exit...");
    ReadKey();
}