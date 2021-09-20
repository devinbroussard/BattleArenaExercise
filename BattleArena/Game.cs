using System;
using System.IO;

namespace BattleArena
{
    public enum ItemType { DEFENSE, ATTACK, NONE }

    public struct Item
    {
        public string Name;
        public float StatBoost;
        public ItemType Type;
    }

    public enum Scene { STARTMENU, ENTERNAME, CHARACTERSELECTION, BATTLE, RESTARTMENU }

    class Game
    {
        //Initializing variables
        private bool _gameOver;
        private Scene _currentScene;
        private Player _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;
        private Item[] _wizardItems;
        private Item[] _knightItems;

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
            InitializeItems();
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
        
        /// <summary>
        /// Function called to save the game
        /// </summary>
        public void Save()
        {
            //streamwriter created that will save into vvvv
            StreamWriter writer = new StreamWriter("SaveData.txt");

            //Saves the current enemy
            writer.WriteLine(_currentEnemyIndex);

            //Save the characters' stats
            _player.Save(writer);
            _currentEnemy.Save(writer);

            //Closes the writer
            writer.Close();
        }

        public bool Load()
        {
            bool loadSuccessful = true;
            //Creating a new instance of the player
            _player = new Player();

            //If the file doesn't exist return false
            if (File.Exists("SavaData.txt"))
                loadSuccessful = false;

            //Creating a reader to read from the text file
            StreamReader reader = new StreamReader("SaveData.txt");

            //If the first line can't be converted to an integer then return false
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
                loadSuccessful = false;

            string job = reader.ReadLine();

            if (job == "Wizard")
                _player = new Player(_wizardItems);
            if (job == "Knight")
                _player = new Player(_knightItems);

            _player.Job = job;

            if (!_player.Load(reader))
                loadSuccessful = false;

            if (!_currentEnemy.Load(reader))
                loadSuccessful = false;

            //Set current enmy to the new current enemy index
            _enemies[_currentEnemyIndex] = _currentEnemy;

            _currentScene = Scene.BATTLE;

            //Closes reader
            reader.Close();

            return loadSuccessful;
        }

        public void InitializeItems()
        {
            //Wizard items
            Item wand = new Item { Name = "Wand", StatBoost = 20, Type = ItemType.ATTACK  };
            Item cloak = new Item { Name = "Cloak", StatBoost = 15, Type = ItemType.DEFENSE };

            //Knight items
            Item sword = new Item { Name = "Sword", StatBoost = 10, Type = ItemType.ATTACK };
            Item shield = new Item { Name = "Shield", StatBoost = 30, Type = ItemType.DEFENSE };

            //Initialize items arrays
            _wizardItems = new Item[] { wand, cloak };
            _knightItems = new Item[] { sword, shield };
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

        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

            while (inputReceived == -1)
            {
                //Print options
                Console.WriteLine(description);

                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i]}");
                }
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();
                
                //If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    //...decrement the input and check if it's  within the bounds of the array
                    inputReceived--;
                    if (inputReceived < 0 || inputReceived >= options.Length)
                    {
                        //Set input received to be the default value
                        inputReceived = -1;
                        //Display error message
                        Console.WriteLine("Invalid input.");
                        Console.ReadKey(true);
                    }
                }
                //If the player didn't type an int
                else
                {
                    inputReceived = -1;
                    Console.WriteLine("Invalid input.");
                    Console.ReadKey(true);
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
                case Scene.STARTMENU:
                    DisplayStartMenu();
                    break;
                case Scene.ENTERNAME:
                    GetPlayerName();
                    break;
                case Scene.CHARACTERSELECTION:
                    CharacterSelection();
                    break;
                case Scene.BATTLE:
                    Battle();
                    CheckBattleResults();
                    break;
                case Scene.RESTARTMENU:
                    DisplayRestartMenu();
                    break;
            }
        }

        void DisplayStartMenu()
        {
            int input = GetInput("Welcome to Battle Arena!", "New Game", "Load Game");

            if (input == 0)
            {
                _currentScene = Scene.ENTERNAME;
            }
            else if (input == 1)
            {
                if (Load())
                {
                    Console.Clear();
                    Console.WriteLine("Load successful!");
                    Console.ReadKey(true);
                    Console.Clear();
                    _currentScene = Scene.BATTLE;
                }
                else
                {
                    Console.WriteLine("Load failed.");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayRestartMenu()
        {
            int input = GetInput("Would you like to restart the game?", "Yes", "No");

            if (input == 0)
            {
                InitializeEnemies();
                _currentScene = 0;
            }
            else if (input == 1)
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

            if (input == 0)
                _currentScene++;
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int input = GetInput($"Okay, {_playerName}, select a class:", "Wizard", "Knight");

            if (input == 0)
                _player = new Player(_playerName + " The Wizard", 50, 25, 5, _wizardItems, "Wizard");
            else if (input == 1)
                _player = new Player(_playerName + " The Knight", 75, 20, 10, _knightItems, "Knight");

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


        public void DisplayEquipItemMenu()
        {
            //Get item inedex
            int input = GetInput("Select an item to equip.", _player.GetItemNames());

            //Equip item at given index
            if (!_player.TryEquipItem(input))
            {
                Console.WriteLine("You couldn't find that item in your bag.");
            }

            //Print feedback 
            Console.WriteLine($"You equipped {_player.CurrentItem.Name}!");
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            DisplayStats(_player);
            DisplayStats(_currentEnemy);

            int input = GetInput($"A {_currentEnemy.Name} stands in front of you! What will you do:", "Attack", "Equip Item", "Remove Current Item", "Save Game");

            if (input == 0)
            {
                float damage = _player.Attack(_currentEnemy);
                Console.WriteLine($"You dealt {damage} damage!");

                damage = _currentEnemy.Attack(_player);
                Console.WriteLine($"The {_currentEnemy.Name} dealt {damage} damage!");
            }
            else if (input == 1)
            {
                DisplayEquipItemMenu();
            }
            else if (input == 2)
            {
                if (!_player.TryRemoveCurrentItem())
                    Console.WriteLine("You don't have anything equipped.");
                else
                    Console.WriteLine("You placed the item in your bag.");
            }
            else if (input == 3)
            {
                Save();
                Console.WriteLine("Game saved successfully!");
            }

            Console.ReadKey(true);
            Console.Clear();
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
                _currentScene = Scene.RESTARTMENU;
            }

            return gameOver;
        }
    }
    
}
