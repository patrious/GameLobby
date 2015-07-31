using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GameLobby
{
    //TODO: Preferences for timeout time for searching, how strict things need to be. etc...

    //TODO: Must impliment comparison of players to lobbies by each game. How to inject this?


    
    /// <summary>
    /// Find 2 players for a head to head match up, if grouped then match automatically
    /// </summary>
    class VersusPlayerProcessor : BasePlayerProcessor
    {
        private BasePlayerGroup _basePlayer;
        public BasePlayerGroup MainPlayerGroup {
            get
            {
                if (_basePlayer != null) return _basePlayer;

                _basePlayer = _workingQueue.First();
                _workingQueue.Remove(_basePlayer);
                return _basePlayer;
            }
            set { _basePlayer = value; }
        }

        readonly SortedSet<BasePlayerGroup> _workingQueue = new SortedSet<BasePlayerGroup>(new PlayerGroupComparer());
        public override void DequeuePlayersForProcessing()
        {
            FillWorkingQueue();

            //Compare all current players skill levels and match as many as possible with provided criteria in under 10 seconds. 
            //Constant stream? Constantly adding more players and matching.
            while (true)
            {
                bool matches;

                if (MainPlayerGroup.Players.Count == 2)
                {
                    OnPlayerGroupLobbyMatchHandler(MainPlayerGroup);
                    matches = true;
                    goto EndOfLoop;
                }

                var match = _workingQueue.FirstOrDefault(MainPlayerGroup.MatchMakingFit);

                if (match == null)
                {
                    matches = false;
                    goto EndOfLoop;
                }

                matches = true;
                //say this is the 3rd attempt? should we then loosen the contraints... should we add more players to the list
                var matchedPlayers = MainPlayerGroup.MergeBasePlayerGroups(match);


                //Notify that a match has been found
                OnPlayerGroupLobbyMatchHandler(matchedPlayers);

                //if no matches, or < configurable amount, add more
            EndOfLoop:
                if (matches == false || _workingQueue.Count < 50)
                {
                    FillWorkingQueue();
                }
            }
        }

        private void FillWorkingQueue()
        {
            //ToDO: used configurable amount here instead of 100
            _playerQueue.Take(100 - _workingQueue.Count).ToList().ForEach(x => _workingQueue.Add(x));
        }

        private class PlayerGroupComparer : IComparer<BasePlayerGroup>
        {
            public int Compare(BasePlayerGroup x, BasePlayerGroup y)
            {
                return x.DateTime.CompareTo(y.DateTime);
            }
        }
    }

    /// <summary>
    /// Find space for BasePlayerGroup in a running Game
    /// </summary>
    class DropInTeamPlayerProcessor : BasePlayerProcessor
    {
        public override void DequeuePlayersForProcessing()
        {
            //Find space in server with specifications matching player group preferences.

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Find Lobby that has space for player group
    /// </summary>
    class SetTeamPlayerProcessor : BasePlayerProcessor
    {
        public override void DequeuePlayersForProcessing()
        {
            throw new NotImplementedException();
        }
    }
}
