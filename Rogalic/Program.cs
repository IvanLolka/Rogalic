using System;

class Program
{
    static void Main()
    {
        Game game = new Game();
        game.Start();
    }
}

class Player
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public string WeaponName { get; set; }
    public int Score { get; set; }

    public Player()
    {
        Health = 100;
        Damage = 10;
        WeaponName = "Начальное оружие";
        Score = 0;
    }

    public void ShowStats()
    {
        Console.WriteLine($"Здоровье: {Health}, Урон: {Damage}, Оружие: {WeaponName}, Счет: {Score}");
    }
}

class Enemy
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public string Name { get; set; }

    public Enemy(int difficulty, string[] enemyNames)
    {
        Random random = new Random();

        if (difficulty == 1)
        {
            Health = random.Next(20, 30);
        }
        else
        {
            Health = (int)(30 * Math.Pow(1.4, difficulty - 1));
        }

        Damage = random.Next(5 * difficulty, 10 * difficulty);
        Name = enemyNames[random.Next(enemyNames.Length)];
    }
}

class Weapon
{
    public int Damage { get; set; }
    public string Name { get; set; }

    public Weapon(int difficulty)
    {
        Random random = new Random();
        Damage = random.Next(5 * difficulty, 15 * difficulty);
        Name = GenerateWeaponName();
    }

    private string GenerateWeaponName()
    {
        string[] weaponNames = { "Меч", "Топор", "Кинжал", "Посох", "Лук" };
        Random random = new Random();
        return weaponNames[random.Next(weaponNames.Length)];
    }
}

class Game
{
    private Player player;
    private int roomCount;
    private string[] enemyNames = { "Огр", "Вампир", "Гоблин", "Скелет", "Ведьма" };

    public void Start()
    {
        Console.WriteLine("Добро пожаловать в игру 'Рогалик'!");
        player = new Player();
        roomCount = 1;

        while (true)
        {
            Console.WriteLine($"\n--- Комната {roomCount} ---");
            player.ShowStats();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Идти дальше");
            Console.WriteLine("2. Сражаться с монстром");

            string choice;
            do
            {
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2");

            switch (choice)
            {
                case "1":
                    ExploreRoom();
                    break;
                case "2":
                    FightMonster();
                    break;
            }
        }
    }

    private void ExploreRoom()
    {
        if (roomCount > player.Score / 5) // Проверка на пропуск комнаты
        {
            Console.WriteLine("Вы пропустили комнату! Нет счета за пропуск комнаты.");
        }
        else
        {
            Console.WriteLine("Вы идете дальше по лабиринту...");
            roomCount++;
        }
    }

    private void FightMonster()
    {
        Weapon newWeapon = new Weapon(roomCount);
        Enemy enemy = new Enemy(roomCount, enemyNames);
        Console.WriteLine($"Вы встретили {enemy.Name} с оружием {newWeapon.Name}! Здоровье {enemy.Name}: {enemy.Health}, Урон {enemy.Name}: {enemy.Damage}");

        while (player.Health > 0 && enemy.Health > 0)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Атаковать");
            Console.WriteLine("2. Увернуться");

            string choice;
            do
            {
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2");

            switch (choice)
            {
                case "1":
                    AttackEnemy(enemy);
                    break;
                case "2":
                    DodgeAttack(enemy);
                    break;
            }

            if (enemy.Health > 0)
            {
                AttackPlayer(enemy);
            }

            player.ShowStats();
        }

        if (player.Health <= 0)
        {
            Console.WriteLine("Вы проиграли. Игра окончена.");
            ShowFinalScore();
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine($"Вы победили {enemy.Name} c оружием {newWeapon.Name} и получили его!");
            player.Score += roomCount * 10; // Счет зависит от комнаты и монстра в ней
            Console.WriteLine($"Комната {roomCount + 1} ждет вас.");
            Console.WriteLine($"Новое оружие: {newWeapon.Name}, Урон: {newWeapon.Damage}");
            Console.WriteLine("Хотите взять новое оружие? (да/нет)");

            string weaponChoice;
            do
            {
                weaponChoice = Console.ReadLine().ToLower();
            } while (weaponChoice != "да" && weaponChoice != "нет");

            if (weaponChoice == "да")
            {
                player.Damage = newWeapon.Damage;
                player.WeaponName = newWeapon.Name;
                Console.WriteLine("Вы взяли новое оружие!");
            }

            roomCount++; // Переход в следующую комнату
        }
    }

    private void AttackEnemy(Enemy enemy)
    {
        Random random = new Random();
        int playerAttack = random.Next(player.Damage / 2, player.Damage + 1);
        enemy.Health -= playerAttack;
        Console.WriteLine($"Вы атаковали {enemy.Name} и нанесли {playerAttack} урона! Здоровье {enemy.Name}: {enemy.Health}");
    }

    private void DodgeAttack(Enemy enemy)
    {
        Random random = new Random();
        int dodgeChance = random.Next(1, 11);

        if (dodgeChance <= 3)
        {
            Console.WriteLine($"Вы увернулись от атаки {enemy.Name}!");
        }
        else
        {
            int enemyAttack = random.Next(enemy.Damage / 2, enemy.Damage + 1);
            player.Health -= enemyAttack;
            if (player.Health > 0)
                Console.WriteLine($"Вы не увернулись и получили {enemyAttack} урона от {enemy.Name}! Здоровье игрока: {player.Health}");
            else
                Console.WriteLine($"Вы не увернулись и получили {enemyAttack} урона от {enemy.Name}! Вы проиграли. Игра окончена.");
        }
    }

    private void AttackPlayer(Enemy enemy)
    {
        Random random = new Random();
        int enemyAttack = random.Next(enemy.Damage / 2, enemy.Damage + 1);
        player.Health -= enemyAttack;
        if (player.Health > 0)
            Console.WriteLine($"{enemy.Name} атаковал вас и нанес {enemyAttack} урона! Здоровье игрока: {player.Health}");
        else
            Console.WriteLine($"{enemy.Name} атаковал вас и нанес {enemyAttack} урона! Вы проиграли. Игра окончена.");
    }

    private void ShowFinalScore()
    {
        Console.WriteLine($"Ваш счет: {player.Score}");
        Console.ReadLine();
    }
}

