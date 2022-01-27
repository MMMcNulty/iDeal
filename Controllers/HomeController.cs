using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using iDeal.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace iDeal.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        public User getUserFromSession()
        {
            var user = new User();
                user.UserId = HttpContext.Session.GetInt32("userId") ?? -1;
                user.FirstName = HttpContext.Session.GetString("firstName");
                user.LastName = HttpContext.Session.GetString("lastName");
                user.Email = HttpContext.Session.GetString("email");
                user.ChipValue = HttpContext.Session.GetInt32("chipValue") ?? 0;
                return user;
        }



        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User sessionUser = this.getUserFromSession();
            if(sessionUser.UserId != -1)
            {
                return View(sessionUser);
            }
            else
            {
                return RedirectToAction("Index", "LogReg");
            }
        }


        [HttpGet("gameboard")]
        public IActionResult GameBoard()
        {
            User sessionUser = this.getUserFromSession();
            if(sessionUser.UserId != -1)
            {
                
                GameView ActiveGameView = new GameView();

                Deck GameDeck = HttpContext.Session.GetObjectFromJson<Deck>("GameDeck");
                ActiveGameView.PlayerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("PlayerCards");
                ActiveGameView.DealerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("DealerCards");
                
                ActiveGameView.RemainingBalance = sessionUser.ChipValue;
                ActiveGameView.BetValue = HttpContext.Session.GetInt32("BetValue") ?? 0;
                sessionUser.ChipValue = HttpContext.Session.GetInt32("chipValue") ?? 0;

                ActiveGameView.PlayerHandValue = this.GetHandValue(ActiveGameView.PlayerCards);
                ActiveGameView.DealerHandValue = this.GetHandValue(ActiveGameView.DealerCards);

                ActiveGameView.DealerCards.RemoveAt(0);

                if(ActiveGameView.PlayerHandValue < 22)
                {
                    return View(ActiveGameView);
                }
                else
                {
                    return RedirectToAction("GameOver", ActiveGameView);
                }
            }
            else
            {
                return RedirectToAction("Index", "LogReg");
            }

        }

        [HttpGet("NewGame")]

        public IActionResult NewGame()
        {
            User sessionUser = this.getUserFromSession();
            
            NewGameView ActiveGameView = new NewGameView();
            Deck NewDeck = new Deck();
            NewDeck.Reset();
            NewDeck.Shuffle();

            HttpContext.Session.SetInt32("BetValue", 0);
            sessionUser.ChipValue = HttpContext.Session.GetInt32("chipValue") ?? 0;

            ActiveGameView.DealerCards.Add(NewDeck.GetOneCard());
            ActiveGameView.PlayerCards.Add(NewDeck.GetOneCard());
            ActiveGameView.DealerCards.Add(NewDeck.GetOneCard());
            ActiveGameView.PlayerCards.Add(NewDeck.GetOneCard());
            ActiveGameView.RemainingBalance = sessionUser.ChipValue;

            HttpContext.Session.SetObjectAsJson("GameDeck", NewDeck);
            HttpContext.Session.SetObjectAsJson("DealerCards", ActiveGameView.DealerCards);
            HttpContext.Session.SetObjectAsJson("PlayerCards", ActiveGameView.PlayerCards);

            if(GetHandValue(ActiveGameView.PlayerCards) == 21)
            {
                return RedirectToAction("Winner");
            }
            else
            {
                return RedirectToAction("GameBoard", ActiveGameView);
            }

        }

        [HttpGet("Hit")]

        public IActionResult Hit()
        {
            Deck GameDeck = HttpContext.Session.GetObjectFromJson<Deck>("GameDeck");
            Console.WriteLine($"Number of cards in deck before hit: {GameDeck.Cards.Count}");
            List<Card> PlayerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("PlayerCards");
            PlayerCards.Add(GameDeck.GetOneCard());
            HttpContext.Session.SetObjectAsJson("GameDeck", GameDeck);
            HttpContext.Session.SetObjectAsJson("PlayerCards", PlayerCards);
            Console.WriteLine($"Number of cards in deck: {GameDeck.Cards.Count}");

            if(GetHandValue(PlayerCards) == 21)
            {
                return RedirectToAction("Winner");
            }

            else
            {
                return RedirectToAction("GameBoard");
            }
            
        }

        [HttpGet("stand")]

        public IActionResult Stand()
        {

            Deck GameDeck = HttpContext.Session.GetObjectFromJson<Deck>("GameDeck");
            Console.WriteLine($"Number of cards in deck before Dealer Hits: {GameDeck.Cards.Count}");
            List<Card> DealerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("DealerCards");
            List<Card> PlayerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("PlayerCards");

            while(GetHandValue(DealerCards) <16) 
            {
                DealerCards.Add(GameDeck.GetOneCard());
            }

            HttpContext.Session.SetObjectAsJson("GameDeck", GameDeck);
            HttpContext.Session.SetObjectAsJson("DealerCards", DealerCards);
            Console.WriteLine($"Number of cards in deck: {GameDeck.Cards.Count}");

            if(GetHandValue(DealerCards) > 21 || GetHandValue(PlayerCards) >= GetHandValue(DealerCards))
            {
                return RedirectToAction("Winner");
            }
            else
            {
                return RedirectToAction("GameOver");
            }
            
        }

        [HttpPost("placeBet")]

        public IActionResult SubmitBet(GameView BetValueForm)
        {
            HttpContext.Session.SetInt32("BetValue", BetValueForm.BetValue);
            return RedirectToAction("GameBoard");
        }

        [HttpGet("Winner")]

        public IActionResult Winner()
        {
            GameView winnerGameView = new GameView();
            List<Card> DealerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("DealerCards");
            List<Card> PlayerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("PlayerCards");
            winnerGameView.PlayerHandValue = GetHandValue(PlayerCards);
            winnerGameView.DealerHandValue = GetHandValue(DealerCards);
            bool PlayerWin = true;
            AddChipValue();
            RecordGame(winnerGameView.PlayerHandValue, winnerGameView.DealerHandValue, PlayerWin);
            return View(winnerGameView);
        }

        [HttpGet("GameOver")]

        public IActionResult GameOver()
        {
            GameView LoserGameView = new GameView();
            List<Card> DealerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("DealerCards");
            List<Card> PlayerCards = HttpContext.Session.GetObjectFromJson<List<Card>>("PlayerCards");
            LoserGameView.PlayerHandValue = GetHandValue(PlayerCards);
            LoserGameView.DealerHandValue = GetHandValue(DealerCards);
            bool PlayerWin = false;
            RemoveChipValue();
            RecordGame(LoserGameView.PlayerHandValue, LoserGameView.DealerHandValue, PlayerWin);
            return View(LoserGameView);
        }

        [HttpGet("PlayerStats")]

        public IActionResult PlayerStats()
        {
            User sessionUser = this.getUserFromSession();
            StatsView NewStatsView = new StatsView();
            var busts = _context.Games.Where(x => x.UserId == sessionUser.UserId && x.PlayerHandValue < 21).Count();
            NewStatsView.NumberOfGamesPlayed = _context.Games.Where(x => x.UserId == sessionUser.UserId).Count();
            NewStatsView.NumberOfTotalWins = _context.Games.Where(x => x.UserId == sessionUser.UserId && x.PlayerWin == true).Count();
            NewStatsView.WinPercentage = ((double)NewStatsView.NumberOfTotalWins/(double)NewStatsView.NumberOfGamesPlayed) * 100;
            Console.WriteLine($"You've played {NewStatsView.NumberOfGamesPlayed} and won {NewStatsView.NumberOfTotalWins}");
            NewStatsView.BustPercentage = busts/(double)NewStatsView.NumberOfGamesPlayed * 100;
            Console.WriteLine($"You've played {NewStatsView.NumberOfGamesPlayed} and busted {busts}");
            return View(NewStatsView);
        }

        private int GetHandValue(List<Card> Hand)
        {
            var handValue = 0;
            var aceCounter = 0;
            foreach(Card card in Hand)
            {
                handValue = handValue + card.Value;
                if(card.Face == "A")
                {
                    aceCounter++;
                }
            }
            while(handValue > 21 && aceCounter != 0)
            {
                handValue -= 10;
                aceCounter--;
            }
            return handValue;
        }

        private IActionResult AddChipValue()
        {
            User sessionUser = this.getUserFromSession();
            int betValue = HttpContext.Session.GetInt32("BetValue") ?? 0;
            User inDb = _context.User.FirstOrDefault(user => user.UserId == sessionUser.UserId);
            inDb.ChipValue = inDb.ChipValue + betValue;
            inDb.UpdatedAt = DateTime.Now;
            HttpContext.Session.SetInt32("chipValue", inDb.ChipValue);

            _context.SaveChanges();

            Console.WriteLine($"Player one chip value has increased by {betValue} for a total of {inDb.ChipValue}");

            return RedirectToAction("Gameboard");
        }

        private IActionResult RemoveChipValue()
        {
            User sessionUser = this.getUserFromSession();
            int betValue = HttpContext.Session.GetInt32("BetValue") ?? 0;
            User inDb = _context.User.FirstOrDefault(user => user.UserId == sessionUser.UserId);
            inDb.ChipValue = inDb.ChipValue - betValue;
            inDb.UpdatedAt = DateTime.Now;
            HttpContext.Session.SetInt32("chipValue", inDb.ChipValue);

            _context.SaveChanges();

            Console.WriteLine($"Player one chip value has decreased by {betValue} for a total of {inDb.ChipValue}");

            return RedirectToAction("Gameboard");
        }
        
        public void RecordGame(int PlayerHandvalue, int DealerHandValue, bool PlayerWin)
        {
            var sessionUser = this.getUserFromSession();

            Games GameForDb =  new Games();
            GameForDb.UserId = sessionUser.UserId;
            GameForDb.PlayerHandValue = PlayerHandvalue;
            GameForDb.DealerHandValue = DealerHandValue;
            GameForDb.PlayerWin = PlayerWin;
            _context.Add(GameForDb);
            _context.SaveChanges();

        }

    }
}