using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{

    class Game
    {
        //Initializing variables
        private bool _gameOver;
        private int _currentScene;
        private Entity _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        { 
            Start();

            while (!_gameOver)
                Update();

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            _gameOver = false;
            _currentScene = 0;

            InitializeEnemies();
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            Console.WriteLine("Goodbye!");
            Console.ReadKey(true);
        }

        public void InitializeEnemies()
        {
            Entity skeleton = new Entity("Skeleton", 10, 5, 0);
            Entity buffAlien = new Entity("Buff Alien", 15, 30, 10);
            Entity strangeMan = new Entity("Strange Man", 20, 15, 5);
            _enemies = new Entity[] { skeleton, buffAlien, strangeMan };

            _currentEnemyIndex = 0;
            _currentEnemy = _enemies[_currentEnemyIndex];

        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, string option1, string option2)
        {
            string input = "";
            int inputReceived = 0;

            while (inputReceived != 1 && inputReceived != 2)
            {//Print options
                Console.WriteLine(description);
                Console.WriteLine("1. " + option1);
                Console.WriteLine("2. " + option2);
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If player selected the first option...
                if (input == "1" || input == option1)
                {
                    //Set input received to be the first option
                    inputReceived = 1;
                }
                //Otherwise if the player selected the second option...
                else if (input == "2" || input == option2)
                {
                    //Set input received to be the second option
                    inputReceived = 2;
                }
                //If neither are true...
                else
                {
                    //...display error message
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
                }

                Console.Clear();
            }
            return inputReceived;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            switch (_currentScene)
            {
                case 0:
                    GetPlayerName();
                    break;
                case 1:
                    CharacterSelection();
                    break;
                case 2:
                    Battle();
                    CheckBattleResults();
                    break;
                case 3:
                    DisplayMainMenu();
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayMainMenu()
        {
            int input = GetInput("Would you like to restart the game?", "Yes", "No");

            if (input == 1)
            {
                InitializeEnemies();
                _currentScene = 0;
            }
            else if (input == 2)
                _gameOver = true;

        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.WriteLine("Hello, welcome to BattleArena, where you will fight enemies to the death!");
            Console.WriteLine("Please enter your name:");
            Console.Write("> ");
            _playerName = Console.ReadLine();

            Console.Clear();
            int input = GetInput($"Hmm... Are you sure {_playerName} is your name?", "Yes", "No");

            if (input == 1)
                _currentScene++;
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int input = GetInput($"Okay, {_playerName}, select a class:", "Wizard", "Knight");

            if (input == 1)
                _player = new Entity(_playerName + " The Wizard", 50, 25, 5);
            else if (input == 2)
                _player = new Entity(_playerName + " The Knight", 75, 20, 10);

            _currentScene++;
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Entity entity)
        {
            Console.WriteLine($"Name: {entity.Name}");
            Console.WriteLine($"Health: {entity.Health}");
            Console.WriteLine($"Attack Power: {entity.AttackPower}");
            Console.WriteLine($"Defense Power: {entity.DefensePower}\n");
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
                DisplayStats(_player);
                DisplayStats(_currentEnemy);

                int input = GetInput($"A {_currentEnemy.Name} stands in front of you! What will you do:", "Attack", "Dodge");

                if (input == 1)
                {
                    float damage = _player.Attack(_currentEnemy);
                    Console.WriteLine($"You dealt {damage} damage!");

                    damage = _currentEnemy.Attack(_player);
                    Console.WriteLine($"The {_currentEnemy.Name} dealt {damage} damage!");
                    Console.ReadKey(true);
                    Console.Clear();
                }
                else if (input == 2)
                {
                    Console.WriteLine("You dodged the enemy's attack!");
                    Console.ReadKey(true);
                    Console.Clear();
                }
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            if (_player.Health == 0)
            {
                Console.WriteLine("You died!");
                Console.WriteLine("Game over!");
                Console.ReadKey(true);
                Console.Clear();

                _currentScene++;
            }

            else if (_currentEnemy.Health == 0)
            {
                Console.WriteLine($"You slayed the {_currentEnemy.Name}");
                Console.ReadKey(true);
                Console.Clear();
                _currentEnemyIndex++;

                if (TryEndGame())
                    return;

                _currentEnemy = _enemies[_currentEnemyIndex];
            }
        }

        bool TryEndGame()
        {
            bool gameOver = _currentEnemyIndex >= _enemies.Length;

            if (gameOver)
            {
                Console.WriteLine("Congradulations, you have reached the end of the game.");
                Console.ReadKey(true);
                Console.Clear();
                _currentScene = 3;
            }

            return gameOver;
        }

    }
}
