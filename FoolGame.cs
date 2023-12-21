


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
        //private static List<Card> playerCards = new List<Card>(6);
        //private static List<Card> computerCards = new List<Card>(6);
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


        }

        

        public static void DisplayPackCards()
        {
            for (int i = 0; i < packCards.Count; i++)
            {
                Console.WriteLine("Suit: " + packCards[i].Suit + "; Power: " + packCards[i].Power);
            }

        }

        public class Player
        {
            public string Name { get; private set; }

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

            public void DisplayCards()
            {
                foreach (Card card in playerCards)
                {
                    Console.WriteLine(card.Suit + " " + card.Power);
                }
            }

            public Human(string name) : base(name)
            {
            }
        }

        public class Computer : Player
        {

            public Computer(string name) : base(name)
            {
            }
        }

    }
}