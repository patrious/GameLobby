using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobby
{
    /// <summary>
    /// Will Monitor all the GameLobby managers and hand it new connections and sort them into the proper lobbies
    /// May talk to other overseers? Handing off players and/or lobbies living on different machines.
    /// </summary>
    class LobbyOverseer
    {

        public void StartUp()
        {
            //Start up dequeue strategy
            //spin up number of manager nodes to config
        }

        public void CleanShutdown()
        {
            throw new NotImplementedException();
        }

    }
}
