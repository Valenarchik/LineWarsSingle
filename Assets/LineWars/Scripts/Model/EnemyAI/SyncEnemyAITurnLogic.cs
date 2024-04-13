using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        public class SyncEnemyAITurnLogic : BaseEnemyAITurnLogic
        {
            public SyncEnemyAITurnLogic(EnemyAI ai,
                GameEvaluator gameEvaluator,
                DepthDetailsData depthDetailsData) : base(ai, gameEvaluator, depthDetailsData)
            {
            }

            public override void Start()
            {
                ai.StartCoroutine(PlayTurn());
            }

            private IEnumerator PlayTurn()
            {
                var executorId = -1;
                yield return new WaitForSeconds(ai.firstCommandPause);
                while (true)
                {
                    var gameProjection = GameProjectionCreator.FromMono(
                    SingleGameRoot.Instance.AllPlayers.Values,
                    MonoGraph.Instance,
                    PhaseManager.Instance);
                    if (executorId != -1)
                    {
                        if (!SingleGameRoot.Instance.AllUnits.TryGetValue(executorId, out var unit) ||
                            !unit.CanDoAnyAction)
                        {
                            InvokeEnded();
                            yield break;
                        }
                    }
                    var possibleCommands =
                            CommandBlueprintCollector.CollectAllCommands(
                            gameProjection,
                            depthDetailsData.GetDepthDetails(1).AvailableCommands);

                    if (executorId != -1)
                    {
                        possibleCommands = possibleCommands
                            .Where(command => command.ExecutorId == executorId)
                            .ToList();
                    }
                    var allOutcomes = new List<PossibleOutcome>();

                    foreach (var command in possibleCommands)
                    {
                        var commandChain = new List<ICommandBlueprint>();
                        var possibleOutcome = MinMax(gameProjection, command, 1, -1, commandChain, true);
                        allOutcomes.Add(possibleOutcome);
                    }

                    var bestOutcome = allOutcomes.MaxItem((i1, i2) => i1.Score.CompareTo(i2.Score));

                    var bestCommand = bestOutcome.Commands.First();
                    executorId = bestCommand.ExecutorId;
                    var isDone = false;
                    PlayCommand(bestCommand, gameProjection, OnCommandExecuted);
                    while (!isDone)
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    yield return new WaitForSeconds(ai.commandPause);
                    void OnCommandExecuted()
                    {
                        isDone = true;
                    }
                }
            }
                
        }
    }
}
