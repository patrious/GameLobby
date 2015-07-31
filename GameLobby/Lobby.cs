using System;
using GameLobby.LobbyConfigs;

namespace GameLobby
{
    public class Lobby : PlayerManagement
    {
        public BaseLobbyConfig BaseLobbyConfig { get; private set; }
        public Guid GameLobbyId { get; private set; }

        public Lobby(BaseLobbyConfig baseLobbyConfig)
        {
            BaseLobbyConfig = baseLobbyConfig;
            GameLobbyId = Guid.NewGuid();
        }

        public void ChangeLobbyConfig(BaseLobbyConfig baseLobbyConfig)
        {
            BaseLobbyConfig = baseLobbyConfig;
        }
        
    }

}
