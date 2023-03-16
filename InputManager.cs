using static Game;
using static EntityManager;
using static ShootManager;



public static class InputManager
{
    // Player Input Status
    public static bool isMoving = false;
    public static bool inputReady = true;
    public static bool recoveredFromJump = true;

    // === Input Handler Loops
    public static void InputHandler()  // Detect and Handle Player Input
    {
        Task.Run(async () =>
                {
                    Direction inputDirection = new Direction();

                    while (isRunning)
                    {
                        if (inputTimer < inputDelay) inputTimer++;
                        if (inputTimer >= inputDelay) inputReady = true;
                        // ! Find a way to detect just one input at time (prevent hold spam) | Clean the buffer
                        if (Console.KeyAvailable && !isMoving && recoveredFromJump)
                        {
                            ConsoleKeyInfo input = Console.ReadKey();
                            switch (input.Key)
                            {
                                case ConsoleKey.W:
                                    if (inputReady)
                                    {
                                        inputDirection = new Direction { x = 0, y = -verticalRange };
                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            // Check Collision
                                            if (players[i].posY > mapStartY)
                                            {
                                                players[i].MoveEntity(inputDirection);
                                                UpdateMoveState();
                                            }
                                        }
                                    }
                                    break;
                                case ConsoleKey.S:
                                    if (inputReady)
                                    {
                                        inputDirection = new Direction { x = 0, y = +verticalRange };
                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            // Check Collision
                                            if (players[i].posY < mapStartY + mapLenghtY - 2)
                                            {
                                                players[i].MoveEntity(inputDirection);
                                                UpdateMoveState();
                                            }
                                        }
                                    }
                                    break;
                                case ConsoleKey.A:
                                    if (inputReady)
                                    {
                                        inputDirection = new Direction { x = -horizontalRange, y = 0 };
                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            // Check Collision
                                            if (players[i].posX > 1)
                                            {
                                                players[i].MoveEntity(inputDirection);
                                                UpdateMoveState();
                                            }
                                        }

                                        // ! TEMP TEST | shoulb me inside entityHandler ?
                                        for (int i = 0; i < cars.Count; i++)
                                        {
                                            cars[i].MoveEntity(cars[i].direction);
                                        }
                                    }
                                    break;
                                case ConsoleKey.D:
                                    if (inputReady)
                                    {
                                        inputDirection = new Direction { x = horizontalRange, y = 0 };
                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            // Check Collision
                                            if (players[i].posX < mapLenghtX + mapStartX - charLength)
                                            {
                                                players[i].MoveEntity(inputDirection);
                                                UpdateMoveState();
                                            }
                                        }
                                    }
                                    break;
                                case ConsoleKey.Spacebar:
                                    if (players.Count > 0)
                                    {
                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            if (players[i].posY > mapStartY && players[i].gun.canShoot)
                                            {
                                                players[i].gun.Shoot(players[i].posX + 2, players[i].posY - 1, players[i], true);
                                            }
                                        }
                                    }
                                    break;
                                case ConsoleKey.Escape:
                                    isRunning = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        // Clear the input buffer if input is not being processed within the switch cases
                        else if (Console.KeyAvailable)
                        {
                            Console.ReadKey(true); // Read the key to remove it from the input buffer
                        }
                        await Task.Delay(inputRefresh);
                    }
                });
    }

    static void UpdateMoveState()
    {
        inputReady = false;
        isMoving = true;
        recoveredFromJump = false;
        inputTimer = 0;
        moveCounter++;
        JumpAnimation();
    }
    static void JumpAnimation()
    {
        Task.Run(async () =>
               {
                   Thread.Sleep(120);
                   isMoving = false;
                   Thread.Sleep(60);    // Extra Pause to movement immediatly after the animation end
                   recoveredFromJump = true;
                   await Task.Delay(1);
                   return;
               });
    }
}