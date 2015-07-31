using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobby
{
    internal interface IPlayerProcessor
    {
        void EnqueNewPlayers(BasePlayerGroup basePlayerGroup);
        void DequeuePlayersForProcessing();
    }

    abstract class BasePlayerProcessor : IPlayerProcessor
    {
        public event EventHandler<BasePlayerGroup> PlayerGroupLobbyMatchHandler;

        protected virtual void OnPlayerGroupLobbyMatchHandler(BasePlayerGroup basePlayerGroup)
        {
            EventHandler<BasePlayerGroup> handler = PlayerGroupLobbyMatchHandler;
            if (handler != null) handler(this, basePlayerGroup);
        }

        protected readonly Queue<BasePlayerGroup> _playerQueue = new Queue<BasePlayerGroup>();

        protected BasePlayerProcessor()
        {
            new Task(DequeuePlayersForProcessing).Start();
        }

        public void EnqueNewPlayers(BasePlayerGroup basePlayerGroup)
        {
            var dateTime = DateTime.UtcNow;
            basePlayerGroup.Players.ToList().ForEach(x => x.EnqueueTime = dateTime);
            _playerQueue.Enqueue(basePlayerGroup);
        }

        public abstract void DequeuePlayersForProcessing();
    }
}
