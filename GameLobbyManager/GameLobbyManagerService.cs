using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GameLobby
{
    public partial class GameLobbyManagerService : ServiceBase
    {
        public GameLobbyManagerService()
        {
            InitializeComponent();
        }

        private LobbyOverseer overseer;
        private WebService webService;

        protected override void OnStart(string[] args)
        {
            overseer = new LobbyOverseer();
            webService = new WebService();
            webService.Start();
        }

        protected override void OnStop()
        {
            webService.Stop();
            overseer.CleanShutdown();
        }
    }


    /// <summary>
    /// Used to communicate with the outside world
    /// </summary>
    internal class WebService
    {
        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
