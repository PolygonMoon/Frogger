using static System.Console;
using System.Threading.Tasks;

// = GameStatus
int charPosX;
int charPosY;
bool isMoving = true;

bool isRunning = true;
int frame = 0;
int frameInput = 0;
float frameRate;
// = Map Setup
byte mapStartX = 4;
byte mapStartY = 6;
byte mapLenghtX = 40;
byte mapLenghtY = 20;
// = Char GFX Setup
string charGfxTopA =    @"\/°°\/"; // * Pivot is on first left char
string charGfxBottomA = @"\\__//";

string charGfxTopB =    @"|/°°\|"; // * Pivot is on first left char
string charGfxBottomB = @"/\__/\";

// === SYSTEM INIT
Console.CursorVisible = false;
frameRate = 1 / 8;    // ! Check why can't write 0,0001 numbers

Clear();
// MainMenu()

// = GAME INIT
charPosX = mapStartX + mapLenghtX / 2;
charPosY = mapStartY + mapLenghtY - 1;

Clear();
// DrawBox() // Draw Map Border
// DrawBox() // Draw UI Border

// === MAIN FLOW

// = Asynch Input Manager
Task.Run(async () =>
        {
            while (isRunning)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.W:
                            // ! ADD if (inside map size)
                            charPosY -= 1;
                            break;
                        case ConsoleKey.S:
                            charPosY += 1;
                            break;
                        case ConsoleKey.A:
                            charPosX -= 1;
                            break;
                        case ConsoleKey.D:
                            charPosX += 1;
                            break;
                        case ConsoleKey.Escape:
                            isRunning = false;
                            break;
                        default:
                            break;
                    }
                    frameInput++;
                    isMoving = !isMoving;
                    await Task.Delay(20);
                }
            }
        });
// = Asynch Enemy Movement // ? Use foreach array do move?
// === Main Flow
while (isRunning)
{
    frame++;
    charRender();
    // CollisionCheck
}

QuitApp();

// === METHODS
void charRender()
{
    Clear();
    SetCursorPosition(0, 0);
    Write($"FRAME: {frame}");
    SetCursorPosition(16, 0);
    Write($"Frame Refresh Rate: {frameRate}");

    // Draw player // ! Call RenderChar() from input handler
    // TODO Use IdleRender, on jump render JumpRender, wait 0.2 then render IdleRender
    if (isMoving)
    {
        SetCursorPosition(charPosX, charPosY);
        Write(charGfxTopA);
        SetCursorPosition(charPosX, charPosY + 1);
        Write(charGfxBottomA);
    }
    else
    {
        SetCursorPosition(charPosX, charPosY);
        Write(charGfxTopB);
        SetCursorPosition(charPosX, charPosY + 1);
        Write(charGfxBottomB);
    }

    // Draw input frame iteraction count
    SetCursorPosition(0, 1);
    Write($"Input Pass: {frameInput}");
    Thread.Sleep(8);
}

void QuitApp()
{
    Clear();
    SetCursorPosition(0, 0);
    WriteLine("|=== APP TERMINATED ===|");
    WriteLine("Press a button to exit...");
    ReadKey();
}