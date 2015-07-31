using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GameLobby
{

    public class PlayerManagement
    {
        public event EventHandler<Guid> PlayerAddedToLobby;
        public event EventHandler<Guid> PlayerRemovedFromLobby;

        protected virtual void OnPlayerAddedToLobby(Guid e)
        {
            EventHandler<Guid> handler = PlayerAddedToLobby;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnPlayerRemovedFromLobby(Guid e)
        {
            EventHandler<Guid> handler = PlayerRemovedFromLobby;
            if (handler != null) handler(this, e);
        }

        readonly ConcurrentDictionary<Guid, BasePlayerGroup> _playerGroups = new ConcurrentDictionary<Guid, BasePlayerGroup>();

        /// <summary>
        /// View Flattened summary of players
        /// </summary>
        public List<BasePlayer> PlayerList
        {
            get
            {
                return _playerGroups.SelectMany(playerGroups => playerGroups.Value.Players).ToList();
            }
        }


        public void AddPlayers(BasePlayerGroup basePlayer)
        {
            _playerGroups.TryAdd(basePlayer.GroupId, basePlayer);
        }

        public void RemovePlayers(BasePlayer basePlayer)
        {
            var playerGroup = _playerGroups.First(x => x.Value.Players.Contains(basePlayer));
            playerGroup.Value.Players.Remove(basePlayer);
            _playerGroups[playerGroup.Key] = playerGroup.Value;
        }

        public void RemovePlayers(BasePlayerGroup basePlayerGroup)
        {
            if (!_playerGroups.ContainsKey(basePlayerGroup.GroupId)) return;
            BasePlayerGroup tempvar;
            _playerGroups.TryRemove(basePlayerGroup.GroupId, out tempvar);
        }
    }

    public class BasePlayerGroup
    {
        public DateTime DateTime
        {
            get { return new DateTime(Players.Sum(x => x.EnqueueTime.Ticks) / Players.Count); }
        }
        public Guid GroupId { get; private set; }

        public HashSet<BasePlayer> Players;

        public BasePreferences GamePreferences { get; private set; }

        public BasePlayerGroup(BasePlayer basePlayer)
        {
            GroupId = Guid.NewGuid();
            Players = new HashSet<BasePlayer>() { basePlayer };
        }

        public BasePlayerGroup(IEnumerable<BasePlayer> players)
        {
            GroupId = Guid.NewGuid();
            Players = new HashSet<BasePlayer>(players);
        }

        public virtual bool MatchMakingFit(BasePlayerGroup otherPlayerGroup)
        {
            return true;
        }
    }

    public class BasePlayer
    {

        public DateTime EnqueueTime { get; set; }
        public Guid PlayerId { get; private set; }

        //For determining ping times  (low lat games)
        public object Location { get; private set; }

        public BasePlayer()
        {
            PlayerId = Guid.NewGuid();
        }

        public BasePlayer(Guid playerId)
        {
            PlayerId = playerId;
        }
    }

    public static class BasePlayerGroupExtensions
    {
        public static BasePlayerGroup MergeBasePlayerGroups(this BasePlayerGroup firstBasePlayerGroup,
            BasePlayerGroup sencondBasePlayerGroup)
        {
            var listOfPlayers = firstBasePlayerGroup.Players.ToList();
            listOfPlayers.AddRange(sencondBasePlayerGroup.Players);
            return new BasePlayerGroup(listOfPlayers);
        }
    }
}
