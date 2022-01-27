using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace iDeal.Models
{
    public class StatsView
    {
        public double WinPercentage {get; set; }

        public double BustPercentage {get; set; }

        public int? NumberOfGamesPlayed {get; set; }

        public int? NumberOfTotalWins {get; set; }


    }
}