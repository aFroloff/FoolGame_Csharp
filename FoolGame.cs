// ****************Правила игры****************
/*
    * В первой сдаче первым ходит (атакует) игрок с самым младшим козырем на руках, далее в рамках игры за этим столом ход переходит по часовой стрелке;
    *ход делается всегда налево от игрока, который ходит, то есть по часовой стрелке;
    *за один раз можно ходить одной картой или несколькими одного ранга (например, 3 шестерками или 2 королями), 
     при этом можно положить сразу все карты или дождаться, пока другой игрок побъет одну карту, а потом подбросить еще;
    *подбрасывать можно все карты того же ранга, которые участвуют в данном заходе: как те, которыми атакуют, так и те, которыми отбиваются;
    *подбрасывать карты того же ранга могут все игроки в порядке очереди, по кругу;
    *первый отбой в первой атаке игры может составлять 5 или 6 карт (зависит от настроек заявки на игру);
    *побить карту противника можно либо старшей картой той же масти, либо козырем;
    *если защищающийся не может побить хотя бы одну карту - он забирает все карты себе, включая те, которые он смог побить;
    *защищающийся может сразу взять карты атакующего, даже не пытаясь их побить. При этом ему также можно подкидывать карты, он обязан их забрать себе.
    *в случае, если игрок смог отбить все карты - эти карты переходят в отбой (убираются в отдельную стопку "рубашкой" вверх);
    *после отбоя все игроки берут недостающие карты по кругу от атакующего в порядке очереди, у каждого игрока опять должно получиться по 6 карт;
    *если в колоде не хватает карт - игрок остается с теми, которые у него есть на данный момент;
    *если игрок отбился всеми своими картами и при этом в колоде еще остались карты, он берет недостающие;
    *если игрок отбился всеми своими картами и при этом в колоде карт больше нет: a) если играют два играка - игрок, у которого остались карты, проигрывает 
     б) если играют больше двух игроков - он выбывает из игры, остальные игроки продолжают играть до момента, пока не останется один игрок с картами 
     на руках - он и считается "дураком", т.е. проигравшим;
    
    Ограничения при подкидывании карт:
    *нельзя атаковать большим количеством карт, чем есть у отбивающегося
     (если у игрока три карты, ему положили одну и он решил ее принять, то ему можно добавить только две вдогонку);
    *нельзя подкидывать более пяти карт, даже если у игрока на руках больше карт, то есть максимальное количество для отбоя – шесть;
    *если игрок отбился всеми своими картами и при этом в колоде карт больше нет, то он выбывает из игры, 
     а его напарник будет отбивать атаки обоих противников по очереди, пока не останется один игрок или команда-соперник с картами на руках. 
     Команда этих игроков и будет считаться "дураками" т.е. проигравшими.
*/


//TODO: логи в файл

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

        private static List<Card> packCards = new List<Card>(36); // карты в колоде
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
            else { computer.PlayerOrder = 1; player.PlayerOrder = 0; }
            Console.WriteLine("Козырная масть: " + royalSuit.ToString());
            while (true)
            {
                if (computer.PlayerOrder == 0)
                {
                    Console.WriteLine("Атакует " + computer.Name + '!');
                    Attack(computer, player);
                    ShowCardsOnTable();
                    Defence(computer, player);
                    Console.WriteLine("Ваши карты: ");
                    player.DisplayCards();
                    Console.WriteLine();
                    while (true)
                    {
                        //добавляем карты компьютеру
                        if (packCards.Count() > 0 && computer.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            computer.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        //игроку
                        if (packCards.Count() > 0 && player.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            player.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        if (packCards.Count() == 0) break;
                        if (computer.playerCards.Count() >= 6 || player.playerCards.Count() >= 6) break;
                    }
                    if (computer.playerCards.Count() == 0) { Console.WriteLine(computer.Name + " выиграл!"); return; }
                    if (player.playerCards.Count() == 0) { Console.WriteLine(player.Name + " выиграл!"); return; }
                    Console.WriteLine("Атакует " + player.Name + '!');
                    Console.WriteLine("Если вы атакуете несколькими картами, то выбирайте только карты одного ранга!");
                    Attack(player, computer);
                    ShowCardsOnTable();
                    Defence(player, computer);
                    Console.WriteLine("Ваши карты: ");
                    player.DisplayCards();
                    Console.WriteLine();
                    while (true)
                    {
                        //игроку
                        if (packCards.Count() > 0 && player.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            player.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        //добавляем карты компьютеру
                        if (packCards.Count() > 0 && computer.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            computer.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        if (packCards.Count() == 0) break;
                        if (computer.playerCards.Count() >= 6 || player.playerCards.Count() >= 6) break;
                    }
                    if (player.playerCards.Count() == 0) { Console.WriteLine(player.Name + " выиграл!"); return; }
                    if (computer.playerCards.Count() == 0) { Console.WriteLine(computer.Name + " выиграл!"); return; }
                }
                if (player.PlayerOrder == 0)
                {
                    Console.WriteLine("Атакует " + player.Name + '!');
                    Console.WriteLine("Если вы атакуете несколькими картами, то выбирайте только карты одного ранга!");
                    Attack(player, computer);
                    ShowCardsOnTable();
                    Defence(player, computer);
                    Console.WriteLine("Ваши карты: ");
                    player.DisplayCards();
                    Console.WriteLine();
                    while (true)
                    {
                        //игроку
                        if (packCards.Count() > 0 && player.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            player.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        //добавляем карты компьютеру
                        if (packCards.Count() > 0 && computer.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            computer.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        if (packCards.Count() == 0) break;
                        if (computer.playerCards.Count() >= 6 || player.playerCards.Count() >= 6) break;
                    }
                    if (player.playerCards.Count() == 0) { Console.WriteLine(player.Name + " выиграл!"); return; }
                    if (computer.playerCards.Count() == 0) { Console.WriteLine(computer.Name + " выиграл!"); return; }
                    Console.WriteLine("Атакует " + computer.Name + '!');
                    Attack(computer, player);
                    ShowCardsOnTable();
                    Defence(computer, player);
                    Console.WriteLine("Ваши карты: ");
                    player.DisplayCards();
                    Console.WriteLine();
                    while (true)
                    {
                        //добавляем карты компьютеру
                        if (packCards.Count() > 0 && computer.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            computer.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        //игроку
                        if (packCards.Count() > 0 && player.playerCards.Count() < 6)
                        {
                            Card randomCard = packCards[random.Next(0, packCards.Count)];
                            player.playerCards.Add(randomCard);
                            packCards.Remove(randomCard);
                        }
                        if (packCards.Count() == 0) break;
                        if (computer.playerCards.Count() >= 6 || player.playerCards.Count() >= 6) break;
                    }
                    if (computer.playerCards.Count() == 0) { Console.WriteLine(computer.Name + " выиграл!"); return; }
                    if (player.playerCards.Count() == 0) { Console.WriteLine(player.Name + " выиграл!"); return; }
                }
            }

        }
        //карты, которые необходимо отбить
        private static void ShowCardsOnTable()
        {
            Console.WriteLine("Карты на столе: ");
            int i = 0;
            foreach (Card card in cardsOnTable)
            {
                i++;
                Console.WriteLine(i + ". Suit: " + card.Suit + "; Power: " + card.Power);
            }
        }
        //атака, ход делает первый аргумент ф-ии, защищаться будет второй
        private static void Attack(Player attacker, Player defender)
        {
            if (attacker is Human)
            {
                Console.WriteLine("Выберете карты для атаки:");
                int i = 0;
                Console.WriteLine("0. Завершить ход.");
                //выбираем одну или несколько карт (пользователь должен соблюдать правило при выборе нескольких карт - 
                //они могут быть только одного ранга
                Console.WriteLine("У противника " + defender.playerCards.Count() + " карт");
                while (attacker.playerCards.Count > 0 && cardsOnTable.Count != defender.playerCards.Count())
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
                        //выбираем самую младшую карту, чтобы от неё отбиться
                        attacker.playerCards = attacker.playerCards.OrderBy(c => c.Power).ToList();
                        cardsOnTable.Add(attacker.playerCards[0]);
                        attacker.playerCards.RemoveAt(0);
                        break;
                    //компьютер выбрал ход несколькими картами одного ранга, если это возможно
                    case 1:
                        //выбираем самые младшие парные карты 
                        int min = 15;
                        for (int i = 6; i < 14; i++)
                        {
                            if (attacker.playerCards.Where(c => c.Power == i).Count() > 1 && min > i) min = i;
                        }
                        if (min == 15) goto case 0; //парных карт нет
                        //у защищаегося должно хватать карт для обороны
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

        //оборона, второй - защищается
        private static void Defence(Player attacker, Player defender)
        {
            if (defender is Human)
            {
                Console.WriteLine("Вы можете забрать карты (нажмите 1), либо отбить их (0).");
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
                List<Card> cards_to_delete = new List<Card>(); //карты игрока, которыми он бил карты на столе. если игрок не сможет побить карты на столе, то они отходят к игроку
                //проходим по каждой карте на столе и бьём её
                foreach (Card card in cardsOnTable)
                {
                    defender.DisplayCards();
                    Console.WriteLine();
                    Console.WriteLine("Выберете карту сильнее, чем " + card.Suit + " " + card.Power + ". Если вы не можете побить её нажмите 0");
                    line = Console.ReadLine();
                    num = int.Parse(line);
                    if (num == 0) break;
                    if (defender.playerCards[num - 1].Power > card.Power && card.Suit != royalSuit && defender.playerCards[num - 1].Suit == card.Suit)
                    { cards_to_delete.Add(defender.playerCards[num - 1]); Console.WriteLine("Карта бита"); defender.playerCards.RemoveAt(num - 1); }//карта на столе не козырная, бьём не козырной
                    else if (defender.playerCards[num - 1].Power > card.Power && card.Suit == royalSuit && defender.playerCards[num - 1].Suit == royalSuit)
                    { cards_to_delete.Add(defender.playerCards[num - 1]); Console.WriteLine("Карта бита"); defender.playerCards.RemoveAt(num - 1); }//карта на столе козырная, бьём козырной
                    else if (defender.playerCards[num - 1].Suit == royalSuit && card.Suit != royalSuit)
                    { cards_to_delete.Add(defender.playerCards[num - 1]); Console.WriteLine("Карта бита"); defender.playerCards.RemoveAt(num - 1); }//карта на столе не козырная, бьём козырной
                    else
                    {
                        Console.WriteLine("Карта не бита");
                    }
                }
                //с шансом 2 к 3 компьютер подкидывает карты
                Random random = new Random();
                num = random.Next(0, 3);
                List<Card> thrown_cards = new List<Card>(); //подкинутые карты
                if (num != 0 && attacker.playerCards
                    .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power).Contains(c.Power)).Count() > 0)
                //подбрасывать можно все карты того же ранга, которые участвуют в данном заходе: как те, которыми атакуют, так и те, которыми отбиваются;
                {
                    num = random.Next(1, attacker.playerCards
                        .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power).Contains(c.Power)).Count() + 1);
                    //случайное количество карт для подкидывания (минимум 1)
                    if (num > 5) num = 5; //подкидывать можно максимум 5 карт
                    for (int i = 0; i < num && i < defender.playerCards.Count(); i++)
                    {
                        thrown_cards.Add(attacker.playerCards
                            .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power).Contains(c.Power)).ToList()[i]);
                        //подкидываем карту
                    }
                }
                if (thrown_cards.Count > 0)
                {
                    Console.WriteLine("Подкинуты карты:");
                    int i = 0;
                    thrown_cards.ForEach(c => { i++; Console.WriteLine(i + ". " + c.Suit + ' ' + c.Power); });

                    foreach (Card card in thrown_cards)
                    {
                        defender.DisplayCards();
                        Console.WriteLine();
                        Console.WriteLine("Выберете карту сильнее, чем " + card.Suit + " " + card.Power + ". Если вы не можете побить её нажмите 0");
                        line = Console.ReadLine();
                        num = int.Parse(line);
                        if (num == 0) break;
                        if (defender.playerCards[num - 1].Power > card.Power && card.Suit != royalSuit && defender.playerCards[num - 1].Suit == card.Suit)
                        { cards_to_delete.Add(card); Console.WriteLine("Карта бита"); defender.playerCards.RemoveAt(num - 1); }//карта на столе не козырная, бьём не козырной
                        else if (defender.playerCards[num - 1].Power > card.Power && card.Suit == royalSuit && defender.playerCards[num - 1].Suit == royalSuit)
                        { cards_to_delete.Add(card); Console.WriteLine("Карта бита"); defender.playerCards.RemoveAt(num - 1); }//карта на столе козырная, бьём козырной
                        else if (defender.playerCards[num - 1].Suit == royalSuit && card.Suit != royalSuit)
                        { cards_to_delete.Add(card); Console.WriteLine("Карта бита"); defender.playerCards.RemoveAt(num - 1); }//карта на столе не козырная, бьём козырной
                        else Console.WriteLine("Карта не бита");
                    }
                }

                if (cards_to_delete.Count == cardsOnTable.Count + thrown_cards.Count()) cardsOnTable.Clear();//если все карты биты, то очищаем стол
                else //иначе, добавляем игроку все карты
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
                //с вероятностью 1 к 3 компьютер заберет козырные карты себе
                if (num == 0 && cardsOnTable.Where(c => c.Suit == royalSuit).Count() > 0)
                {
                    defender.playerCards.AddRange(cardsOnTable);
                    cardsOnTable.Clear();
                    return;
                }
                List<Card> cards_to_delete = new List<Card>(); //карты к., которыми он бил карты на столе. если к. не сможет побить карты на столе, то они отходят к к.
                //проходим по каждой карте на столе и бьём её
                foreach (Card card in cardsOnTable)
                {
                    Card c_card; //выбираем карту, которой будем бить
                    if (defender.playerCards.Where(c => c.Suit == card.Suit && c.Power > card.Power).Count() > 0)
                    {//карты одинаковой масти, карта большего ранга, чем на столе - выбираем минимальную подходящую
                        c_card = defender.playerCards.Where(c => c.Suit == card.Suit && c.Power > card.Power).OrderBy(c => c.Power).ToList()[0];
                        Console.WriteLine("Карта бита. Использована карта " + c_card.Suit + ' ' + c_card.Power);
                        defender.playerCards.Remove(c_card);
                        cards_to_delete.Add(c_card);
                    }
                    else if (defender.playerCards.Where(c => c.Suit == royalSuit && card.Suit != royalSuit).Count() > 0)
                    {//бьём минимальным козырем карту другой масти
                        c_card = defender.playerCards.Where(c => c.Suit == royalSuit && card.Suit != royalSuit).OrderBy(c => c.Power).ToList()[0];
                        Console.WriteLine("Карта бита. Использована карта " + c_card.Suit + ' ' + c_card.Power);
                        defender.playerCards.Remove(c_card);
                        cards_to_delete.Add(c_card);
                    }
                    else Console.WriteLine("Карта не бита");
                }

                List<Card> thrown_cards = new List<Card>(); //подкинутые карты
                Console.WriteLine("1. Подбросить карты\n2. Не подкидывать");
                string line = Console.ReadLine();
                num = int.Parse(line);
                if (num != 2 && attacker.playerCards
                    .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power).Contains(c.Power)).Count() > 0)
                //подбрасывать можно все карты того же ранга, которые участвуют в данном заходе: как те, которыми атакуют, так и те, которыми отбиваются;
                {
                    num = 1;
                    for (int i = 0; i < 5 && i < defender.playerCards.Count(); i++)
                    {
                        int j = 0;
                        Console.WriteLine("Подбросить можно одну из: ");
                        attacker.playerCards
                            .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power)
                            .Contains(c.Power))
                            .ToList()
                            .ForEach(c => { j++; Console.Write(j + ". " + c.Suit + ' ' + c.Power + ' '); });
                        Console.WriteLine("\nЕсли вы не хотите подбрасывать карту, нажмите 0");
                        line = Console.ReadLine();
                        num = int.Parse(line);
                        if (num == 0) break;
                        thrown_cards.Add(attacker.playerCards
                            .Where(c => c.Power == cardsOnTable[0].Power || cards_to_delete.ConvertAll(c => c.Power)
                            .Contains(c.Power))
                            .ToList()[num - 1]);

                        //подкидываем карту
                    }
                }
                if (thrown_cards.Count > 0)
                {
                    Console.WriteLine("Подкинуты карты:");
                    int i = 0;
                    thrown_cards.ForEach(c => { i++; Console.WriteLine(i + ". " + c.Suit + ' ' + c.Power); });
                    foreach (Card card in thrown_cards)
                    {
                        Card c_card; //выбираем карту, которой будем бить
                        if (defender.playerCards.Where(c => c.Suit == card.Suit && c.Power > card.Power).Count() > 0)
                        {//карты одинаковой масти, карта большего ранга, чем на столе - выбираем минимальную подходящую
                            c_card = defender.playerCards.Where(c => c.Suit == card.Suit && c.Power > card.Power).OrderBy(c => c.Power).ToList()[0];
                            Console.WriteLine("Карта " + card.Suit + ' ' + card.Power + "бита. Использована карта " + c_card.Suit + ' ' + c_card.Power);
                            defender.playerCards.Remove(c_card);
                            cards_to_delete.Add(c_card);
                        }
                        else if (defender.playerCards.Where(c => c.Suit == royalSuit && card.Suit != royalSuit).Count() > 0)
                        {//бьём минимальным козырем карту другой масти
                            c_card = defender.playerCards.Where(c => c.Suit == royalSuit && card.Suit != royalSuit).OrderBy(c => c.Power).ToList()[0];
                            Console.WriteLine("Карта " + card.Suit + ' ' + card.Power + "бита. Использована карта " + c_card.Suit + ' ' + c_card.Power);
                            defender.playerCards.Remove(c_card);
                            cards_to_delete.Add(c_card);
                        }
                        else Console.WriteLine("Карта " + card.Suit + ' ' + card.Power + "не бита");
                    }
                    if (cards_to_delete.Count == cardsOnTable.Count + thrown_cards.Count()) cardsOnTable.Clear();//если все карты биты, то очищаем стол
                    else //иначе, добавляем игроку все карты
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
            //имя, карты, очередь игрока
            public string Name { get; private set; }

            public abstract void DisplayCards();

            public List<Card> playerCards = new List<Card>(6);

            public int PlayerOrder { get; set; }

            public Player(string name)
            {
                Name = name;
            }
            // для определения того, кто ходит первым
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
            //показать игроку его карты для выбора
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