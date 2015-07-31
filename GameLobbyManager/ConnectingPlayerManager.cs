using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using GameLobby;

namespace GameLobby
{
    internal class ConnectingPlayerManager : AsyncPlayerProcessor
    {
        private Func<IPlayerProcessor> _selectLeastBusyProcessor;
        public Func<IPlayerProcessor> SelectLeastBusyProcessor
        {
            get { return _selectLeastBusyProcessor ?? (_selectLeastBusyProcessor = () => PlayerProcessors.First()); }
            set { _selectLeastBusyProcessor = value; }
        }

        private IPlayerProcessorSource PlayerProcessorSource { get; set; }
        public List<BasePlayerProcessor> PlayerProcessors { get { return PlayerProcessorSource.GetProcessorList().ToList(); } }



        public ConnectingPlayerManager(IPlayerProcessorSource playerProcessorSource, Func<IPlayerProcessor> processorSelectionAlgorithm)
        {
            PlayerProcessorSource = playerProcessorSource;
            SelectLeastBusyProcessor = processorSelectionAlgorithm;


            //Start up the Processor
            PlayerAsyncProcessor().Start();

        }

        public void NewPlayerGroupConnection(BasePlayerGroup basePlayerGroup)
        {
            AddPlayerToProcessing(basePlayerGroup);
        }

        protected override void ProcessPlayerGroup(BasePlayerGroup basePlayerGroup)
        {
            var processor = SelectLeastBusyProcessor();
            processor.EnqueNewPlayers(basePlayerGroup);
        }
    }

}

