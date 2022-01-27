using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace iDeal.Models
{
    public class NewGameView
    {
        public int BetValueForm {get; set; }

        public int RemainingBalance {get; set; }


        public List<Card> DealerCards {get; set; } = new List<Card>();

        public List<Card> PlayerCards {get; set; } = new List<Card>();

        public int PlayerHandValue {get; set; }
    }
}