using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace iDeal.Models
{
    public class Deck
    {

        public List<Card> Cards {get; set; } = new List<Card>();

        public Deck()
        {
        
        }
        public Card GetOneCard()
        {
            Card topCard = this.Cards[0];
            this.Cards.RemoveAt(0);
            return topCard;
        }



        public void Reset()
        {
            List<string> faces = new List<string> () {"A", "2", "3", "4", "5","6","7","8","9","10","J","Q","K"};
            List<string> suits = new List<string> () {"♥", "♦", "♠", "♣"};
            foreach(string suit in suits)
            {
                for(var i = 1; i<14; i++)
                {
                    var value = 0;
            foreach (string face in faces)
            {
                if(faces[i-1] == "A")
                {
                    value = 11;
                }
                else if(faces[i-1] == "2")
                {
                    value = 2;
                }
                else if(faces[i-1] == "3")
                {
                    value = 3;
                }
                else if(faces[i-1] == "4")
                {
                    value = 4;
                }
                else if(faces[i-1] == "5")
                {
                    value = 5;
                }
                else if(faces[i-1] == "6")
                {
                    value = 6;
                }
                else if(faces[i-1] == "7")
                {
                    value = 7;
                }
                else if(faces[i-1] == "8")
                {
                    value = 8;
                }
                else if(faces[i-1] == "9")
                {
                    value = 9;
                }
                else if(faces[i-1] == "10" || faces[i-1] == "J" || faces[i-1] == "Q" || faces[i-1] == "K")
                {
                    value = 10;
                }
            }
                    Card newCard = new Card(faces[i-1], suit, value);
                    Cards.Add(newCard);
                    // Console.WriteLine($" Card: {newCard.Suit}, {newCard.Face}, {newCard.Value}");
                }
            }
        }
        public void Shuffle()
        {
            var shuffledDeck = new List<Card> ();
            var random = new Random();
            while (this.Cards.Count != 0)
            {
                var index = random.Next(0, this.Cards.Count);
                shuffledDeck.Add(this.Cards[index]);
                this.Cards.RemoveAt(index);
            }
            // Console.WriteLine("My Shuffled Deck:");
            // shuffledDeck.ForEach(card => Console.WriteLine($" Card: {card.Suit}, {card.Face}, {card.Value}"));
            this.Cards = shuffledDeck;
        }
    }
}

