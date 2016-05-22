﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThatAuthentication;
using Bets.Data.Models;
using Bets.Helpers;
using Bets.Data;

namespace Bets.Controllers
{
	[AuthorizeX(Role.Admin)]
    public class AdminController : BaseController
    {
		public AdminController() : base(Module.Admin)
		{
		}

		protected override void SetIndexData()
		{
			base.SetIndexData();

			var teamRepo = new TeamRepository();
			ViewBag.Teams = teamRepo.GetTeamList(true).ToList();
			ViewBag.MatchID = new MatchRepository(teamRepo.Context, teamRepo.UserID).GetActiveMatchList().ToList();
			ViewBag.PlayerID = new PlayerRepository(teamRepo.Context, teamRepo.UserID).GetPlayerGoalsForCurrentRound();
			ViewBag.RoundID = new RoundRepository(teamRepo.Context, teamRepo.UserID).GetRecentRounds();
		}

		[HttpPost]
		public ActionResult AddMatches(List<MatchModel> matches)
		{
			return Json(new MatchRepository().AddMatches(matches));
		}

		[HttpPost]
		public ActionResult AddMatchResult(MatchModel match)
		{
			return Json(new MatchRepository().AddMatchResult(match).Success);
		}

		[HttpPost]
		public ActionResult AddGoalscorerForRound(GoalscorerBetModel model)
		{
			return Json(new PlayerRepository().AddGoalscorerForRound(model).Success);
		}

		[HttpPost]
		public ActionResult AddRound(string name)
		{
			return Json(new RoundRepository().AddRound(name).Success);
		}

		[HttpPost]
		public ActionResult RemoveTeam(int id)
		{
			return Json(new TeamRepository().RemoveTeam(id).Success);
		}

		[HttpPost]
		public ActionResult RemovePlayer(int id)
		{
			return Json(new PlayerRepository().RemovePlayer(id).Success);
		}

		[HttpPost]
		public ActionResult CloseCurrentRound(string name)
		{
			return Json(new RoundRepository().CloseCurrentRound().Success);
		}
    }
}
