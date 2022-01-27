using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace iDeal.Models
{
    public class Games
    {
        [Key]
        public int GameId {get; set; }

        public int UserId {get; set; }

        public int PlayerHandValue {get; set; }

        public int DealerHandValue {get; set; }

        public bool PlayerWin {get; set; }

        public DateTime CreateAt {get; set; } = DateTime.Now;
        public DateTime UpdatedAt {get; set; } = DateTime.Now;

    }
}