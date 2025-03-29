using UnityEngine;
using Command.Utilities;
using Command.Sound;
using System.Collections.Generic;
using Command.Input;
using Command.Player;
using Command.UI;
using Command.Events;
using Command.Battle;
using Command.Actions;
using UnityEngine.UI;
using Command.Commands;

namespace Command.Main
{

    /**  This script demonstrates implementation of the Service Locator Pattern.
    *  If you're interested in learning about Service Locator Pattern, 
    *  you can find a dedicated course on Outscal's website.
    *  Link: https://outscal.com/courses
    **/

    public class GameService : GenericMonoSingleton<GameService>
    {
        // Services:
        public EventService EventService { get; private set; }
        public SoundService SoundService { get; private set; }
        public ActionService ActionService { get; private set; }
        public InputService InputService { get; private set; }
        public BattleService BattleService { get; private set; }
        public PlayerService PlayerService { get; private set; }
        public CommandInvoker CommandInvoker { get; private set; }

        [SerializeField] private UIService uiService;
        public UIService UIService => uiService;

        // Scriptable Objects:
        [SerializeField] private SoundScriptableObject soundScriptableObject;
        [SerializeField] private List<BattleScriptableObject> battleScriptableObjects;

        // Scene References:
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource bgMusicSource;

        private void Start()
        {
            SoundService = new SoundService(soundScriptableObject, sfxSource, bgMusicSource);
            EventService = new EventService();
            ActionService = new ActionService();
            InputService = new InputService();
            BattleService = new BattleService(battleScriptableObjects);
            PlayerService = new PlayerService();
            uiService.Init(battleScriptableObjects.Count);
            CommandInvoker = new CommandInvoker();
        }

        private void Update() => InputService.UpdateInputService();

        public void ProcessUnitCommand(ICommand commandToProcess) => PlayerService.ProcessUnitCommand(commandToProcess as UnitCommand);
    }
}