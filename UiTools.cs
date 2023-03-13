using static System.Console;

public static class UiTools
{
    static public void DrawBoxEmpty(int xStart, int xLength, int yStart, int yLength)
    {
        // Draw Corner
        SetCursorPosition(xStart, yStart);
        Write("┌");
        SetCursorPosition(xStart + xLength + 1, yStart);
        Write("┐");
        SetCursorPosition(xStart, yStart + yLength + 1);
        Write("└");
        SetCursorPosition(xStart + xLength + 1, yStart + yLength + 1);
        Write("┘");
        DrawLineH(xStart + 1, xLength, yStart, "-");
        DrawLineH(xStart + 1, xLength, yStart + yLength + 1, "-");

        DrawLineV(yStart + 1, yLength, xStart, "|");
        DrawLineV(yStart + 1, yLength, xStart + xLength + 1, "|");
    }

    static public void DrawBox(int xStart, int xLength, int yStart, int yLength, string symbol)
    {
        for (int y = 0; y < yLength + 2; y++)
        {
            for (int x = 0; x < xLength + 2; x++)
            {
                SetCursorPosition(xStart + x, yStart + y);
                Write(symbol);
            }
        }
    }

    static public void DrawLineH(int xStart, int xLength, int yStart, string symbol)
    {
        for (int i = 0; i < xLength; i++)
        {
            SetCursorPosition(xStart + i, yStart);
            Write(symbol);
        }
    }
    static public void DrawLineV(int yStart, int yLength, int xStart, string symbol)
    {
        for (int i = 0; i < yLength; i++)
        {
            SetCursorPosition(xStart, yStart + i);
            Write(symbol);
        }
    }
}