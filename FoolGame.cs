// ****************������� ����****************
/*
    * � ������ ����� ������ ����� (�������) ����� � ����� ������� ������� �� �����, ����� � ������ ���� �� ���� ������ ��� ��������� �� ������� �������;
    *��� �������� ������ ������ �� ������, ������� �����, �� ���� �� ������� �������;
    *�� ���� ��� ����� ������ ����� ������ ��� ����������� ������ ����� (��������, 3 ���������� ��� 2 ��������), 
     ��� ���� ����� �������� ����� ��� ����� ��� ���������, ���� ������ ����� ������ ���� �����, � ����� ���������� ���;
    *������������ ����� ��� ����� ���� �� �����, ������� ��������� � ������ ������: ��� ��, �������� �������, ��� � ��, �������� ����������;
    *������������ ����� ���� �� ����� ����� ��� ������ � ������� �������, �� �����;
    *������ ����� � ������ ����� ���� ����� ���������� 5 ��� 6 ���� (������� �� �������� ������ �� ����);
    *������ ����� ���������� ����� ���� ������� ������ ��� �� �����, ���� �������;
    *���� ������������ �� ����� ������ ���� �� ���� ����� - �� �������� ��� ����� ����, ������� ��, ������� �� ���� ������;
    *������������ ����� ����� ����� ����� ����������, ���� �� ������� �� ������. ��� ���� ��� ����� ����� ����������� �����, �� ������ �� ������� ����.
    *� ������, ���� ����� ���� ������ ��� ����� - ��� ����� ��������� � ����� (��������� � ��������� ������ "��������" �����);
    *����� ����� ��� ������ ����� ����������� ����� �� ����� �� ���������� � ������� �������, � ������� ������ ����� ������ ���������� �� 6 ����;
    *���� � ������ �� ������� ���� - ����� �������� � ����, ������� � ���� ���� �� ������ ������;
    *���� ����� ������� ����� ������ ������� � ��� ���� � ������ ��� �������� �����, �� ����� �����������;
    *���� ����� ������� ����� ������ ������� � ��� ���� � ������ ���� ������ ���: a) ���� ������ ��� ������ - �����, � �������� �������� �����, ����������� 
     �) ���� ������ ������ ���� ������� - �� �������� �� ����, ��������� ������ ���������� ������ �� �������, ���� �� ��������� ���� ����� � ������� 
     �� ����� - �� � ��������� "�������", �.�. �����������;
    
    ����������� ��� ������������ ����:
    *������ ��������� ������� ����������� ����, ��� ���� � �������������
     (���� � ������ ��� �����, ��� �������� ���� � �� ����� �� �������, �� ��� ����� �������� ������ ��� ��������);
    *������ ����������� ����� ���� ����, ���� ���� � ������ �� ����� ������ ����, �� ���� ������������ ���������� ��� ����� � �����;
    *���� ����� ������� ����� ������ ������� � ��� ���� � ������ ���� ������ ���, �� �� �������� �� ����, 
     � ��� �������� ����� �������� ����� ����� ����������� �� �������, ���� �� ��������� ���� ����� ��� �������-�������� � ������� �� �����. 
     ������� ���� ������� � ����� ��������� "��������" �.�. ������������.
*/


//TODO: ���� � ����

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

        private static List<Card> packCards = new List<Card>(36); // ����� � ������
        private static List<Card> cardsOnTable = new List<Card>();
        private static CardSuit royalSuit; // �������� �����

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
            else { computer.PlayerOrder = 1; player.PlayerOrder = 0; }
            Console.WriteLine("�������� �����: " + royalSuit.ToString());
            while (true)
            {
                if (computer.PlayerOrder == 0)
                {
                    Console.WriteLine("������� " + computer.Name + '!');
                    Attack(computer, player);
                    ShowCardsOnTable();
                    Defence(computer, player);
                    Console.WriteLine("���� �����: ");
                    player.DisplayCards();
                    Console.WriteLine();
                    while (true)
                    {
                        //��������� ����� ����������
                        if (packCards.Count() > 0 && computer.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            computer.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        //������
                        if (packCards.Count() > 0 && player.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            player.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        if (packCards.Count() == 0) break;
                        if (computer.playerCards.Count() >= 6 || player.playerCards.Count() >= 6) break;
                    }
                    if (computer.playerCards.Count() == 0) { Console.WriteLine(computer.Name + " �������!"); return; }
                    if (player.playerCards.Count() == 0) { Console.WriteLine(player.Name + " �������!"); return; }
                    Console.WriteLine("������� " + player.Name + '!');
                    Console.WriteLine("���� �� �������� ����������� �������, �� ��������� ������ ����� ������ �����!");
                    Attack(player, computer);
                    ShowCardsOnTable();
                    Defence(player, computer);
                    Console.WriteLine("���� �����: ");
                    player.DisplayCards();
                    Console.WriteLine();
                    while (true)
                    {
                        //������
                        if (packCards.Count() > 0 && player.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            player.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        //��������� ����� ����������
                        if (packCards.Count() > 0 && computer.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            computer.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        if (packCards.Count() == 0) break;
                        if (computer.playerCards.Count() >= 6 || player.playerCards.Count() >= 6) break;
                    }
                    if (player.playerCards.Count() == 0) { Console.WriteLine(player.Name + " �������!"); return; }
                    if (computer.playerCards.Count() == 0) { Console.WriteLine(computer.Name + " �������!"); return; }
                }
                if (player.PlayerOrder == 0)
                {
                    Console.WriteLine("������� " + player.Name + '!');
                    Console.WriteLine("���� �� �������� ����������� �������, �� ��������� ������ ����� ������ �����!");
                    Attack(player, computer);
                    ShowCardsOnTable();
                    Defence(player, computer);
                    Console.WriteLine("���� �����: ");
                    player.DisplayCards();
                    Console.WriteLine();
                    while (true)
                    {
                        //������
                        if (packCards.Count() > 0 && player.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            player.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        //��������� ����� ����������
                        if (packCards.Count() > 0 && computer.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            computer.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        if (packCards.Count() == 0) break;
                        if (computer.playerCards.Count() >= 6 || player.playerCards.Count() >= 6) break;
                    }
                    if (player.playerCards.Count() == 0) { Console.WriteLine(player.Name + " �������!"); return; }
                    if (computer.playerCards.Count() == 0) { Console.WriteLine(computer.Name + " �������!"); return; }
                    Console.WriteLine("������� " + computer.Name + '!');
                    Attack(computer, player);
                    ShowCardsOnTable();
                    Defence(computer, player);
                    Console.WriteLine("���� �����: ");
                    player.DisplayCards();
                    Console.WriteLine();
                    while (true)
                    {
                        //��������� ����� ����������
                        if (packCards.Count() > 0 && computer.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            computer.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        //������
                        if (packCards.Count() > 0 && player.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            player.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        if (packCards.Count() == 0) break;
                        if (computer.playerCards.Count() >= 6 || player.playerCards.Count() >= 6) break;
                    }
                    if (computer.playerCards.Count() == 0) { Console.WriteLine(computer.Name + " �������!"); return; }
                    if (player.playerCards.Count() == 0) { Console.WriteLine(player.Name + " �������!"); return; }
                }
            }

        }
        //�����, ������� ���������� ������
        private static void ShowCardsOnTable()
        {
            Console.WriteLine("����� �� �����: ");
            int i = 0;
            foreach (Card card in cardsOnTable)
            {
                i++;
                Console.WriteLine(i + ". Suit: " + card.Suit + "; Power: " + card.Power);
            }
        }
        //�����, ��� ������ ������ �������� �-��, ���������� ����� ������
        private static void Attack(Player attacker, Player defender)
        {
            if (attacker is Human)
            {
                Console.WriteLine("�������� ����� ��� �����:");
                int i = 0;
                Console.WriteLine("0. ��������� ���.");
                //�������� ���� ��� ��������� ���� (������������ ������ ��������� ������� ��� ������ ���������� ���� - 
                //��� ����� ���� ������ ������ �����
                Console.WriteLine("� ���������� " + defender.playerCards.Count() + " ����");
                while (attacker.playerCards.Count > 0 && cardsOnTable.Count != defender.playerCards.Count())
                {
                    attacker.DisplayCards();
                    Console.WriteLine();
                    string line = Console.ReadLine();
                    int crd = int.Parse(line);
                    if (crd == 0) if (i > 0) break; else Console.WriteLine("�������� ���� �� ���� �����");
                    i++;
                    //��������� ����� �� ���� � ������� �� � ������
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
                    //��������� ������ ��� ����� ������
                    case 0:
                        //�������� ����� ������� �����, ����� �� �� ��������
                        attacker.playerCards = attacker.playerCards.OrderBy(c => c.Power).ToList();
                        cardsOnTable.Add(attacker.playerCards[0]);
                        attacker.playerCards.RemoveAt(0);
                        break;
                    //��������� ������ ��� ����������� ������� ������ �����, ���� ��� ��������
                    case 1:
                        //�������� ����� ������� ������ ����� 
                        int min = 15;
                        for (int i = 6; i < 14; i++)
                        {
                            if (attacker.playerCards.Where(c => c.Power == i).Count() > 1 && min > i) min = i;
                        }
                        if (min == 15) goto case 0; //������ ���� ���
                        //� ����������� ������ ������� ���� ��� �������
                        int num = attacker.playerCards.Where(c => c.Power == min).Count();
                        for (int i = 0; i < num && i < defender.playerCards.Count; i++)
                        {
                            cardsOnTable.Add(attacker.playerCards.Where(c => c.Power == min).ToList()[0]);
                            attacker.playerCards.Remove(attacker.playerCards.Where(c => c.Power == min).ToList()[0]);
                        }
                        break;
                }
            }
        }

        //�������, ������ - ����������
        private static void Defence(Player attacker, Player defender)
        {
            if (defender is Human)
            {
                Console.WriteLine("�� ������ ������� ����� (������� 1), ���� ������ �� (0).");
                defender.DisplayCards();
                Console.WriteLine();
                string line = Console.ReadLine();
                int num = int.Parse(line);
                if (num == 1)
                {
                    defender.playerCards.AddRange(cardsOnTable);
                    cardsOnTable = new List<Card>();
                    return;
                }
                List<Card> cards_to_delete = new List<Card>(); //����� ������, �������� �� ��� ����� �� �����. ���� ����� �� ������ ������ ����� �� �����, �� ��� ������� � ������
                //�������� �� ������ ����� �� ����� � ���� �
                foreach (Card card in cardsOnTable)
                {
                    defender.DisplayCards();
                    Console.WriteLine();
                    Console.WriteLine("�������� ����� �������, ��� " + card.Suit + " " + card.Power + ". ���� �� �� ������ ������ � ������� 0");
                    line = Console.ReadLine();
                    num = int.Parse(line);
                    if (num == 0) break;
                    if (defender.playerCards[num - 1].Power > card.Power && card.Suit != royalSuit && defender.playerCards[num - 1].Suit == card.Suit)
                    { cards_to_delete.Add(defender.playerCards[num - 1]); Console.WriteLine("����� ����"); defender.playerCards.RemoveAt(num - 1); }//����� �� ����� �� ��������, ���� �� ��������
                    else if (defender.playerCards[num - 1].Power > card.Power && card.Suit == royalSuit && defender.playerCards[num - 1].Suit == royalSuit)
                    { cards_to_delete.Add(defender.playerCards[num - 1]); Console.WriteLine("����� ����"); defender.playerCards.RemoveAt(num - 1); }//����� �� ����� ��������, ���� ��������
                    else if (defender.playerCards[num - 1].Suit == royalSuit && card.Suit != royalSuit)
                    { cards_to_delete.Add(defender.playerCards[num - 1]); Console.WriteLine("����� ����"); defender.playerCards.RemoveAt(num - 1); }//����� �� ����� �� ��������, ���� ��������
                    else
                    {
                        Console.WriteLine("����� �� ����");
                    }
                }
                //� ������ 2 � 3 ��������� ����������� �����
                Random random = new Random();
                num = random.Next(0, 3);
                List<Card> thrown_cards = new List<Card>(); //���������� �����
                if (num != 0 && attacker.playerCards
                    .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power).Contains(c.Power)).Count() > 0)
                //������������ ����� ��� ����� ���� �� �����, ������� ��������� � ������ ������: ��� ��, �������� �������, ��� � ��, �������� ����������;
                {
                    num = random.Next(1, attacker.playerCards
                        .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power).Contains(c.Power)).Count() + 1);
                    //��������� ���������� ���� ��� ������������ (������� 1)
                    if (num > 5) num = 5; //����������� ����� �������� 5 ����
                    for (int i = 0; i < num && i < defender.playerCards.Count(); i++)
                    {
                        thrown_cards.Add(attacker.playerCards
                            .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power).Contains(c.Power)).ToList()[i]);
                        //����������� �����
                    }
                }
                if (thrown_cards.Count > 0)
                {
                    Console.WriteLine("��������� �����:");
                    int i = 0;
                    thrown_cards.ForEach(c => { i++; Console.WriteLine(i + ". " + c.Suit + ' ' + c.Power); });

                    foreach (Card card in thrown_cards)
                    {
                        defender.DisplayCards();
                        Console.WriteLine();
                        Console.WriteLine("�������� ����� �������, ��� " + card.Suit + " " + card.Power + ". ���� �� �� ������ ������ � ������� 0");
                        line = Console.ReadLine();
                        num = int.Parse(line);
                        if (num == 0) break;
                        if (defender.playerCards[num - 1].Power > card.Power && card.Suit != royalSuit && defender.playerCards[num - 1].Suit == card.Suit)
                        { cards_to_delete.Add(card); Console.WriteLine("����� ����"); defender.playerCards.RemoveAt(num - 1); }//����� �� ����� �� ��������, ���� �� ��������
                        else if (defender.playerCards[num - 1].Power > card.Power && card.Suit == royalSuit && defender.playerCards[num - 1].Suit == royalSuit)
                        { cards_to_delete.Add(card); Console.WriteLine("����� ����"); defender.playerCards.RemoveAt(num - 1); }//����� �� ����� ��������, ���� ��������
                        else if (defender.playerCards[num - 1].Suit == royalSuit && card.Suit != royalSuit)
                        { cards_to_delete.Add(card); Console.WriteLine("����� ����"); defender.playerCards.RemoveAt(num - 1); }//����� �� ����� �� ��������, ���� ��������
                        else Console.WriteLine("����� �� ����");
                    }
                }

                if (cards_to_delete.Count == cardsOnTable.Count + thrown_cards.Count()) cardsOnTable.Clear();//���� ��� ����� ����, �� ������� ����
                else //�����, ��������� ������ ��� �����
                {
                    defender.playerCards.AddRange(cardsOnTable);
                    defender.playerCards.AddRange(cards_to_delete);
                    defender.playerCards.AddRange(thrown_cards);
                    cardsOnTable.Clear();
                }

            }

            if (defender is Computer)
            {
                Random random = new Random();
                int num = random.Next(0, 3);
                //� ������������ 1 � 3 ��������� ������� �������� ����� ����
                if (num == 0 && cardsOnTable.Where(c => c.Suit == royalSuit).Count() > 0)
                {
                    defender.playerCards.AddRange(cardsOnTable);
                    cardsOnTable.Clear();
                    return;
                }
                List<Card> cards_to_delete = new List<Card>(); //����� �., �������� �� ��� ����� �� �����. ���� �. �� ������ ������ ����� �� �����, �� ��� ������� � �.
                //�������� �� ������ ����� �� ����� � ���� �
                foreach (Card card in cardsOnTable)
                {
                    Card c_card; //�������� �����, ������� ����� ����
                    if (defender.playerCards.Where(c => c.Suit == card.Suit && c.Power > card.Power).Count() > 0)
                    {//����� ���������� �����, ����� �������� �����, ��� �� ����� - �������� ����������� ����������
                        c_card = defender.playerCards.Where(c => c.Suit == card.Suit && c.Power > card.Power).OrderBy(c => c.Power).ToList()[0];
                        Console.WriteLine("����� ����. ������������ ����� " + c_card.Suit + ' ' + c_card.Power);
                        defender.playerCards.Remove(c_card);
                        cards_to_delete.Add(c_card);
                    }
                    else if (defender.playerCards.Where(c => c.Suit == royalSuit && card.Suit != royalSuit).Count() > 0)
                    {//���� ����������� ������� ����� ������ �����
                        c_card = defender.playerCards.Where(c => c.Suit == royalSuit && card.Suit != royalSuit).OrderBy(c => c.Power).ToList()[0];
                        Console.WriteLine("����� ����. ������������ ����� " + c_card.Suit + ' ' + c_card.Power);
                        defender.playerCards.Remove(c_card);
                        cards_to_delete.Add(c_card);
                    }
                    else Console.WriteLine("����� �� ����");
                }

                List<Card> thrown_cards = new List<Card>(); //���������� �����
                Console.WriteLine("1. ���������� �����\n2. �� �����������");
                string line = Console.ReadLine();
                num = int.Parse(line);
                if (num != 2 && attacker.playerCards
                    .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power).Contains(c.Power)).Count() > 0)
                //������������ ����� ��� ����� ���� �� �����, ������� ��������� � ������ ������: ��� ��, �������� �������, ��� � ��, �������� ����������;
                {
                    num = 1;
                    for (int i = 0; i < 5 && i < defender.playerCards.Count(); i++)
                    {
                        int j = 0;
                        Console.WriteLine("���������� ����� ���� ��: ");
                        attacker.playerCards
                            .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power)
                            .Contains(c.Power))
                            .ToList()
                            .ForEach(c => { j++; Console.Write(j + ". " + c.Suit + ' ' + c.Power + ' '); });
                        Console.WriteLine("\n���� �� �� ������ ������������ �����, ������� 0");
                        line = Console.ReadLine();
                        num = int.Parse(line);
                        if (num == 0) break;
                        thrown_cards.Add(attacker.playerCards
                            .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power)
                            .Contains(c.Power))
                            .ToList()[num - 1]);

                        //����������� �����
                    }
                }
                if (thrown_cards.Count > 0)
                {
                    Console.WriteLine("��������� �����:");
                    int i = 0;
                    thrown_cards.ForEach(c => { i++; Console.WriteLine(i + ". " + c.Suit + ' ' + c.Power); });
                    foreach (Card card in thrown_cards)
                    {
                        Card c_card; //�������� �����, ������� ����� ����
                        if (defender.playerCards.Where(c => c.Suit == card.Suit && c.Power > card.Power).Count() > 0)
                        {//����� ���������� �����, ����� �������� �����, ��� �� ����� - �������� ����������� ����������
                            c_card = defender.playerCards.Where(c => c.Suit == card.Suit && c.Power > card.Power).OrderBy(c => c.Power).ToList()[0];
                            Console.WriteLine("����� " + card.Suit + ' ' + card.Power + "����. ������������ ����� " + c_card.Suit + ' ' + c_card.Power);
                            defender.playerCards.Remove(c_card);
                            cards_to_delete.Add(c_card);
                        }
                        else if (defender.playerCards.Where(c => c.Suit == royalSuit && card.Suit != royalSuit).Count() > 0)
                        {//���� ����������� ������� ����� ������ �����
                            c_card = defender.playerCards.Where(c => c.Suit == royalSuit && card.Suit != royalSuit).OrderBy(c => c.Power).ToList()[0];
                            Console.WriteLine("����� " + card.Suit + ' ' + card.Power + "����. ������������ ����� " + c_card.Suit + ' ' + c_card.Power);
                            defender.playerCards.Remove(c_card);
                            cards_to_delete.Add(c_card);
                        }
                        else Console.WriteLine("����� " + card.Suit + ' ' + card.Power + "�� ����");
                    }
                    if (cards_to_delete.Count == cardsOnTable.Count + thrown_cards.Count()) cardsOnTable.Clear();//���� ��� ����� ����, �� ������� ����
                    else //�����, ��������� ������ ��� �����
                    {
                        defender.playerCards.AddRange(cardsOnTable);
                        defender.playerCards.AddRange(cards_to_delete);
                        defender.playerCards.AddRange(thrown_cards);
                        cardsOnTable.Clear();
                    }

                }

            }
        }

        public static void DisplayPackCards()
        {
            for (int i = 0; i < packCards.Count; i++)
            {
                Console.WriteLine("Suit: " + packCards[i].Suit + "; Power: " + packCards[i].Power);
            }

        }

        public abstract class Player
        {
            //���, �����, ������� ������
            public string Name { get; private set; }

            public abstract void DisplayCards();

            public List<Card> playerCards = new List<Card>(6);

            public int PlayerOrder { get; set; }

            public Player(string name)
            {
                Name = name;
            }
            // ��� ����������� ����, ��� ����� ������
            public int MinRoyalCard()
            {
                int min = 15;
                foreach (Card card in playerCards)
                {
                    if (card.Suit == royalSuit && card.Power < min)
                    {
                        min = card.Power;
                    }
                }
                return min;
            }

        }

        public class Human : Player
        {
            //�������� ������ ��� ����� ��� ������
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
            public override void DisplayCards()
            {
                playerCards.ForEach(c => Console.WriteLine(c.Suit + " " + c.Power));
            }

            public Computer(string name) : base(name)
            {
            }
        }

    }
}