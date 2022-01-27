using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;



namespace iDeal.Models 
{

    public class Card
    {
        // [Key]
        // public int CardId {get; set; }

        public string Face {get; set; }

        public string Suit {get; set; }

        public int Value {get; set; }

        public Card(string face, string suit, int value)
        {
            this.Face  = face;
            this.Suit = suit;
            this.Value = value;
        }
    }
}