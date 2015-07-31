using System;
using System.Collections.Generic;
using System.Linq;
using GameLobby;
using GameLobby.LobbyConfigs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLobbyTests
{
    [TestClass]
    public class LobbyTests
    {
        #region HelperMethods
        private Lobby CreateXPersonLobby(int numberOfPlayers = 4, int numberOfGroups = 1)
        {
            var lobby = new Lobby(new LobbyConfig());
            for (var i = 0; i < numberOfGroups; i++)
            {
                lobby.AddPlayers(CreateXPlayerGroup(numberOfPlayers));
            }
            return lobby;
        }

        private BasePlayerGroup CreateXPlayerGroup(int numberOfPlayers)
        {
            var playerList = new List<BasePlayer>();
            for (var i = 0; i < numberOfPlayers; i++)
            {
                playerList.Add(new BasePlayer());
            }
            return new BasePlayerGroup(playerList);
        }

        #endregion

        [TestMethod]
        public void NewLobbyTest()
        {
            var lobby = new Lobby(new LobbyConfig());
            var playerList = lobby.PlayerList;
            Assert.AreEqual(0, playerList.Count);
        }

        [TestMethod]
        public void AddPlayerLobbyTest()
        {
            var lobby = new Lobby(new LobbyConfig());
            var player = new BasePlayer();
            lobby.AddPlayers(new BasePlayerGroup(player));
            Assert.IsTrue(lobby.PlayerList.Contains(player));
        }
        [TestMethod]
        public void AddPlayerAndGroupLobbyTest()
        {
            var lobby = new Lobby(new LobbyConfig());
            var player = new BasePlayer();
            lobby.AddPlayers(new BasePlayerGroup(player));
            var group = CreateXPlayerGroup(2);
            lobby.AddPlayers(group);
            Assert.AreEqual(3, lobby.PlayerList.Count);
        }

        [TestMethod]
        public void AddPlayerGroupLobbyTest()
        {
            var lobby = CreateXPersonLobby();


            Assert.AreEqual(4, lobby.PlayerList.Count);
        }

        [TestMethod]
        public void RemovePlayerFromLobbyTest()
        {
            const int numberOfInitialPlayers = 4;
            var lobby = CreateXPersonLobby(numberOfInitialPlayers);
            var firstPlayer = lobby.PlayerList.First();
            lobby.RemovePlayers(firstPlayer);
            Assert.AreEqual(numberOfInitialPlayers - 1, lobby.PlayerList.Count);

        }

        [TestMethod]
        public void RemovePlayerGroupFromLobbyTest()
        {
            var lobby = new Lobby(new LobbyConfig());
            var group1 = CreateXPlayerGroup(4);
            var group2 = CreateXPlayerGroup(4);

            lobby.AddPlayers(group1);
            lobby.AddPlayers(group2);

            lobby.RemovePlayers(group1);
        }
    }
}
