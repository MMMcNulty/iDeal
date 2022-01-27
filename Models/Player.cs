using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace iDeal.Models
{
    public class Player
    {
        // [Key]
        // public int PlayerId {get; set; }

        public string PlayerType {get; set; }

        public string PlayerName {get; set; }

        public int HandValue {get; set; }

        public List<Card> Hand {get; set; }

        public int Draw(Card topCard)
        {
            Hand.Add(topCard);
            HandValue += topCard.Value;
            System.Console.WriteLine($"You drew a {topCard.Face} of {topCard.Suit}. Your current hand value is {HandValue}.");
            return HandValue;
        }

        public int DealerDraw(Card topCard)
        {
            Hand.Add(topCard);
            HandValue += topCard.Value;
            return HandValue;
        }
    }
}