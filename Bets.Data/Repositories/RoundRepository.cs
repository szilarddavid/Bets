﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bets.Data.Models;

namespace Bets.Data
{
	public class RoundRepository : Repository<Round, ModelIDName>
	{
		public RoundRepository() : base() { }
		public RoundRepository(BetsDataContext context, int userID) : base(context, userID) {}

		public IQueryable<ModelIDName> GetClosedRounds(bool forAdmin = false)
		{
			var entities = this.GetAll();

			if (!forAdmin)
				entities = entities.Where(e => e.Closed);

			return entities.OrderByDescending(e => e.RoundID).Select(e => new ModelIDName
			{
				ID = e.RoundID,
				Name = e.Name
			});
		}

        public bool IsTheRoundExpired(int roundID)
        {
            bool isexpired =
           (
               from rounds in this.Context.Rounds
               where rounds.RoundID == roundID
               select rounds.Closed
           ).SingleOrDefault();

            return isexpired;
        }

        public string GetRoundName(int roundID)
        {
            string roundname =
               (
                   from rounds in this.Context.Rounds
                   where rounds.RoundID == roundID
                   select rounds.Name
               ).SingleOrDefault();

            return roundname;
        }

		public IEnumerable<DropdownOption> GetRecentRounds()
		{
			return this.GetAll().OrderByDescending(e => e.RoundID).Take(2).Select(e => new DropdownOption
			{
				Text = e.Name,
				Value = e.RoundID.ToString()
			}).ToList();
		}

		public ActionStatus AddRound(string name)
		{
			return this.CallStoredProcedure
			(
				DBActionType.Insert,
				() =>
				{
					return new SPResult { Result = this.Context.AddRound(name) };
				}
			);
		}

		public ActionStatus CloseCurrentRound()
		{
			return this.CallStoredProcedure
			(
				DBActionType.Insert,
				() =>
				{
					return new SPResult { Result = this.Context.CloseRound(null) };
				}
			);
		}
	}
}
