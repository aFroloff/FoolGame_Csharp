


namespace Games
{
    public class FoolGame
    {
        public enum CardSuit { Spades, Clubs, Hearts, Diamonds }
        public class Card
        {
            public int Power { get; private set; }
            public CardSuit Suit { get; private set; }

            public Card(CardSuit suit, int power)
            {
                Power = power;
                Suit = suit;
            }
            public Card(int suit, int power)
            {
                Power = power;
                if (suit == 0) Suit = CardSuit.Spades;
                if (suit == 1) Suit = CardSuit.Clubs;
                if (suit == 2) Suit = CardSuit.Hearts;
                if (suit == 3) Suit = CardSuit.Diamonds;
            }
        }

        private static  List<Card> packCards = new List<Card>(36); // карты в колоде
        private static List<Card> cardsOnTable = new List<Card>();
        private static CardSuit royalSuit; // козырная масть

        public static void StartGame()
        {
            Computer computer = new Computer("YourPC");
            Human player = new Human("You");
            // Inicialize cards
            for (int i = 0; i < 36; i++)
            {
                packCards.Add(new Card(i / 9, 6 + i % 9));
            }

            // Give a cards for a player
            Random random = new Random();
            while (player.playerCards.Count() < 6)
            {
                Card randomCard = packCards[random.Next(0, packCards.Count)];
                player.playerCards.Add(randomCard);
                packCards.Remove(randomCard);
            }

            // Give a cards for a computer
            while (computer.playerCards.Count < 6)
            {
                Card randomCard = packCards[random.Next(0, packCards.Count)];
                computer.playerCards.Add(randomCard);
                packCards.Remove(randomCard);
            }

            // set royal suit
            int randomValue = random.Next(0, 3 + 1);
            if (randomValue == 0) royalSuit = CardSuit.Spades;
            if (randomValue == 1) royalSuit = CardSuit.Clubs;
            if (randomValue == 2) royalSuit = CardSuit.Hearts;
            if (randomValue == 3) royalSuit = CardSuit.Diamonds;

            //setting first player
            if (player.MinRoyalCard() > computer.MinRoyalCard()) { computer.PlayerOrder = 0; player.PlayerOrder = 1; }
            else {  computer.PlayerOrder = 1; player.PlayerOrder = 0; }

            Attack(player, computer);
            ShowCardsOnTable();
        }

        private static void ShowCardsOnTable()
        {
            foreach (Card card in cardsOnTable)
            {
                Console.WriteLine("Suit: " + card.Suit + "; Power: " + card.Power);
            }
        }

        private static void Attack(Player attacker, Player p2)
        {
            if (attacker is Human)
            {
                Console.WriteLine("Выберете карты для атаки:");
                int i = 0;
                Console.WriteLine("0. Завершить атаку.");
                //выбираем одну или несколько карт (пользователь должен соблюдать правило при выборе нескольких карт - 
                //они могут быть только одной масти
                while (attacker.playerCards.Count > 0)
                {
                    attacker.DisplayCards();
                    Console.WriteLine();
                    string line = Console.ReadLine();
                    int crd = int.Parse(line);
                    if (crd == 0) if (i > 0) break; else Console.WriteLine("Выберете хотя бы одну карту");
                    i++;
                    //добавляем карты на стол и убираем их у игрока
                    if (crd > 0) cardsOnTable.Add(attacker.playerCards[crd - 1]);
                    if (crd > 0) attacker.playerCards.RemoveAt(crd - 1);
                }
            }
            if (attacker is Computer)
            {
                Random random = new Random();
                int p = random.Next(0, 2);
                switch (p)
                {
                    //компьютер выбрал ход одной картой
                    case 0:

                        break;
                    //компьютер выбрал ход несколькими картами одной масти, если это возможно
                    case 1:

                        break;
                }
            }
        }

        public static void DisplayPackCards()
        {
            Console.WriteLine("Карты на столе: ");
            for (int i = 0; i < packCards.Count; i++)
            {
                Console.WriteLine("Suit: " + packCards[i].Suit + "; Power: " + packCards[i].Power);
            }

        }

        public abstract class Player
        {
            public string Name { get; private set; }

            public abstract void DisplayCards();

            public List<Card> playerCards = new List<Card>(6);

            public int PlayerOrder { get; set; }

            public Player(string name)
            {
                Name = name;
            }

            public int MinRoyalCard()
            {
                int min = 15;
                foreach (Card card in playerCards)
                {
                    if(card.Suit == royalSuit && card.Power > min)
                    {
                        min = card.Power;
                    }
                }
                return min;
            }

        }

        public class Human : Player
        {

            public override void DisplayCards()
            {
                int i = 0;
                foreach (Card card in playerCards)
                {
                    i++;
                    Console.Write(i + ". " + card.Suit + " " + card.Power + "   ");
                }
            }

            public Human(string name) : base(name)
            {
            }
        }

        public class Computer : Player
        {
            public override void DisplayCards() { }

            public Computer(string name) : base(name)
            {
            }
        }

    }
}