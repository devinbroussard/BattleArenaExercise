using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
//test commit

    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>
    struct Character
    {
        public string name;
        public float health;
        public float attackPower;
        public float defensePower;
    }

    class Game
    {
        //Initializing variables
        bool gameOver = false;
        int currentScene = 0;
        Character player;
        Character[] enemies;
        private int currentEnemyIndex = 0;
        private Character currentEnemy;
        Character strangeMan;
        Character alien;
        Character skeleton;
        Character unclePhil;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!gameOver)
                Update();

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            //Initializing enemies' stats
            skeleton.name = "Skeleton";
            skeleton.health = 10;
            skeleton.attackPower = 5;
            skeleton.defensePower = 0;

            alien.name = "Buff Alien";
            alien.health = 15;
            alien.attackPower = 30;
            alien.defensePower = 10;

            strangeMan.name = "Strange Man";
            strangeMan.health = 20;
            strangeMan.attackPower = 15;
            strangeMan.defensePower = 5;

            enemies = new Character[] { skeleton, alien, strangeMan };

            ResetCurrentEnemy();

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
        /// 
        /// </summary>
        void ResetCurrentEnemy()
        {
            currentEnemyIndex = 0;
            currentEnemy = enemies[currentEnemyIndex];
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
            switch (currentScene)
            {
                case 0:
                    GetPlayerName();
                    break;
                case 1:
                    CharacterSelection();
                    break;
                case 2:
                    Battle(ref currentEnemy);
                    CheckBattleResults(ref currentEnemy);
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
                ResetCurrentEnemy();
                currentScene = 0;
            }

            if (input == 2)
                gameOver = true;

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
            player.name = Console.ReadLine();

            Console.Clear();
            int input = GetInput($"Hmm...Are you sure {player.name} is your name?", "Yes", "No");

            if (input == 1)
                currentScene++;
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int input = GetInput($"Okay, {player.name}, select a class:", "Wizard", "Knight");

            if (input == 1)
            {
                player.health = 50;
                player.attackPower = 25;
                player.defensePower = 5;

                player.name += " The Wizard";
            }
            if (input == 2)
            {
                player.health = 75;
                player.attackPower = 15;
                player.defensePower = 10;

                player.name += " The Knight";
            }


            currentScene++;
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Character character)
        {
            Console.WriteLine($"Name: {character.name}");
            Console.WriteLine($"Health: {character.health}");
            Console.WriteLine($"Attack Power: {character.attackPower}");
            Console.WriteLine($"Defense Power: {character.defensePower}\n");
        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            float damage = attackPower - defensePower;
            if (damage < 0) damage = 0;

            return damage;
        }

        /// <summary>
        /// Deals damage to a character based on an attacker's attack power
        /// </summary>
        /// <param name="attacker">The character that initiated the attack</param>
        /// <param name="defender">The character that is being attacked</param>
        /// <returns>The amount of damage done to the defender</returns>
        public float Attack(ref Character attacker, ref Character defender)
        {
            float damage = CalculateDamage(attacker.attackPower, defender.defensePower);

            defender.health -= damage;
            if (defender.health < 0) defender.health = 0;

            return damage;

        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle(ref Character enemy)
        {
            while (player.health > 0 && enemy.health > 0)
            {
                DisplayStats(player);
                DisplayStats(enemy);

                int input = GetInput($"A {enemy.name} stands in front of you! What will you do:", "Attack", "Dodge");

                if (input == 1)
                {
                    float playerDamage = Attack(ref player, ref enemy);
                    Console.WriteLine($"You dealt {playerDamage} damage!");

                    float enemyDamage = Attack(ref enemy, ref player);
                    Console.WriteLine($"The {enemy.name} dealt {enemyDamage} damage!");
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
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults(ref Character enemy)
        {
            if (player.health == 0)
            {
                Console.WriteLine("You died!");
                Console.WriteLine("Game over!");
                Console.ReadKey(true);

                currentScene = 4;
            }

            else if (currentEnemyIndex >= enemies.Length)
            {
                Console.WriteLine("Congradulations, you have reached the end of the game.");
                Console.ReadKey(true);
                Console.Clear();

                currentScene = 3;
            }

            else if (enemy.health == 0)
            {
                Console.WriteLine($"You slayed the {enemy.name}");
                Console.ReadKey(true);
                Console.Clear();
                currentEnemyIndex++;

                if (TryEndGame())
                    return;

                currentEnemy = enemies[currentEnemyIndex];
            }
        }

        bool TryEndGame()
        {
            bool gameOver = currentEnemyIndex >= enemies.Length;

            if (gameOver)
                currentScene = 3;

            return gameOver;
        }

    }
}
