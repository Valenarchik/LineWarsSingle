using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class GameUI : Singleton<GameUI>
    {
        [SerializeField] private EnemyTurnPanel enemyTurnPanel;
        [SerializeField] private GameRefereeDrawer gameRefereeDrawer;
        
        private IExecutor currentExecutor;
        private List<TargetDrawer> currentDrawers = new();
        
        private static CommandsManager CommandsManager => CommandsManager.Instance;
        

        private void Start()
        {
            CommandsManager.ExecutorChanged += OnExecutorChanged;
            CommandsManager.FightNeedRedraw += ReDrawCurrentTargets;
            CommandsManager.BuyNeedRedraw += ReDrawBuyNodes;
            gameRefereeDrawer.DrawReferee(SingleGameRoot.Instance.GameReferee);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (CommandsManager != null)
            {
                CommandsManager.ExecutorChanged += OnExecutorChanged;
                CommandsManager.FightNeedRedraw += ReDrawCurrentTargets;
                CommandsManager.BuyNeedRedraw += ReDrawBuyNodes;
            }
        }

        public void ClearBuyNodes()
        {
            ReDrawBuyNodes(null);
        }
        
        private void ReDrawBuyNodes(BuyStateMessage buyStateMessage)
        {
            if(MonoGraph.Instance == null || MonoGraph.Instance.Nodes == null)
                return;
            foreach (var node in MonoGraph.Instance.Nodes)
            {
                if(node == null)
                    continue;
                var targetDrawer = node.GetComponent<NodeTargetDrawer>();
                if(targetDrawer == null)
                    continue;
                targetDrawer.ReDrawBuyInfo(false);
            }
            
            if (buyStateMessage == null || buyStateMessage.NodesToSpawnPreset == null)
                return;
            
            foreach (var node in buyStateMessage.NodesToSpawnPreset)
            {
                if(node == null)
                    continue;
                var targetDrawer = node.GetComponent<NodeTargetDrawer>();
                if(targetDrawer == null)
                    continue;
                targetDrawer.ReDrawBuyInfo(true);
            }
        }
        
        private void OnExecutorChanged(IExecutor before, IExecutor after)
        {
            currentExecutor = after;
            ReDrawAllAvailability(before, after);
        }

        private void ReDrawAllAvailability(IExecutor before, IExecutor after)
        {
            var unitsToReDraw = Player.LocalPlayer.MyUnits;
            if (after is null)
            {
                if (before is { CanDoAnyAction: true })
                    ReDrawAllAvailability(unitsToReDraw, true);
                else
                    ReDrawAllAvailability(unitsToReDraw, false);
            }
            else
            {
                ReDrawAllAvailability(unitsToReDraw, true);
            }
        }

        public void ReDrawAllAvailability(IEnumerable<Unit> units, bool isAvailable)
        {
            foreach (var unit in units)
            {
                var drawer = unit.GetComponent<UnitDrawer>();
                if (isAvailable && (!unit.CanDoAnyAction || (Unit) CommandsManager.Executor == unit))
                {
                    drawer.ReDrawAvailability(false);
                    continue;
                };
                drawer.ReDrawAvailability(isAvailable);
            }
        }

        public void SetEconomyTurn()
        {
            
        }
        
        public void SetEnemyTurn(bool isEnemyTurn)
        {
            enemyTurnPanel.SetTurn(isEnemyTurn);
        }

        private void ReDrawCurrentTargets(ExecutorMessage message)
        {
            foreach (var currentDrawer in currentDrawers)
            {
                if (currentDrawer == null) continue;
                currentDrawer.ReDrawCommads(CommandType.None);
            }

            if(message == null)
                return;

            currentDrawers = new List<TargetDrawer>();

            var dictionary = message.Data.GroupBy(info =>
                {
                    switch (info.Target)
                    {
                        case Node node:
                            return node;
                        case Unit unit:
                            return unit.Node;
                        default:
                            return null;
                    }
                }, targetInfo => targetInfo.CommandType,
                Tuple.Create);
            if (currentExecutor != null)
            {
                ReDrawTargetsIcons(dictionary);
            }
        }

        private void ReDrawTargetsIcons(IEnumerable<Tuple<Node, IEnumerable<CommandType>>> tuples)
        {
            foreach (var tuple in tuples)
            {
                var node = tuple.Item1;
                var commands = tuple.Item2;
                
                if (node == null) continue;
                var drawer = node.gameObject.GetComponent<NodeTargetDrawer>();
                if (drawer == null) continue;
                currentDrawers.Add(drawer);
                drawer.ReDrawCommads(commands);
            }
        }

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}