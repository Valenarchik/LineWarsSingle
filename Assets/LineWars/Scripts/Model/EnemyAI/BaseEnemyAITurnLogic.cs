using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        public abstract class BaseEnemyAITurnLogic
        {
            protected readonly EnemyAI ai;
            protected readonly GameEvaluator gameEvaluator;
            protected readonly DepthDetailsData depthDetailsData;
            private List<PhaseType> skippedPhases = new List<PhaseType>()
        {
            PhaseType.Buy,
            PhaseType.Payday
        };
            public BaseEnemyAITurnLogic(
                EnemyAI ai,
                GameEvaluator gameEvaluator,
                DepthDetailsData depthDetailsData)
            {
                this.ai = ai;
                this.gameEvaluator = gameEvaluator;
                this.depthDetailsData = depthDetailsData;
            }

            public event Action Ended;

            public abstract void Start();

            protected void InvokeEnded()
            {
                Ended?.Invoke();
            }

            protected PossibleOutcome MinMax(GameProjection gameProjection,
                ICommandBlueprint blueprint,
                int depth,
                int currentExecutorId,
                List<ICommandBlueprint> firstCommandChain,
                bool isSavingCommands, int alpha = int.MinValue, int beta = int.MaxValue)
            {
                if (currentExecutorId != -1 && blueprint.ExecutorId != currentExecutorId)
                    throw new ArgumentException();
                currentExecutorId = blueprint.ExecutorId;
                var newGame = GameProjectionCreator.FromProjection(gameProjection);
                var thisCommand = blueprint.GenerateCommand(newGame);
                thisCommand.Execute();

                if (isSavingCommands)
                {
                    var newCommandChain = new List<ICommandBlueprint>(firstCommandChain);
                    newCommandChain.Add(blueprint);
                    firstCommandChain = newCommandChain;
                }

                var thisPlayerProjection = newGame.OriginalToProjectionPlayers[ai];
                if (newGame.UnitsIndexList.Count == 0)
                    return new(gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);

                if (IsTurnOver(newGame, currentExecutorId))
                {
                    depth++;
                    currentExecutorId = -1;
                    isSavingCommands = false;
                    if (!newGame.IsUnitPhaseAvailable())
                        newGame.CycleTurn();
                    else
                        newGame.CyclePlayers();
                }
                var safeguard = 0;
                while (true)
                {
                    if (skippedPhases.Contains(newGame.CurrentPhase))
                    {
                        newGame.CycleTurn();
                    }
                    else
                    {
                        break;
                    }
                    safeguard++;
                    if (safeguard > 100)
                    {
                        return new(gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);
                    }
                }

                if (depth > ai.Depth || newGame.CurrentPhase == PhaseType.Buy)
                {
                    return new(gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);
                }

                var availableCommands = depthDetailsData.GetDepthDetails(depth).AvailableCommands;
                var possibleCommands = CommandBlueprintCollector.CollectAllCommands(newGame, availableCommands)
                .Where(newBlueprint => currentExecutorId == -1 || newBlueprint.ExecutorId == currentExecutorId);

                if (thisPlayerProjection != newGame.CurrentPlayer)
                {
                    var currentValue = new PossibleOutcome(int.MaxValue, null);
                    foreach (var command in possibleCommands)
                    {
                        var newValue = MinMax(newGame,
                            command,
                            depth,
                            currentExecutorId,
                            firstCommandChain,
                            isSavingCommands,
                            alpha, beta);
                        if (newValue.Score < currentValue.Score)
                            currentValue = newValue;
                        if (newValue.Score <= alpha)
                        {
                            return currentValue;
                        }
                        if (newValue.Score < beta)
                            beta = newValue.Score;
                    }
                    return currentValue;
                }
                else
                {
                    var currentValue = new PossibleOutcome(int.MinValue, null);
                    foreach (var command in possibleCommands)
                    {
                        var newValue = MinMax(newGame,
                            command,
                            depth,
                            currentExecutorId,
                            firstCommandChain,
                            isSavingCommands,
                            alpha, beta);
                        if (newValue.Score > currentValue.Score)
                            currentValue = newValue;
                        if (newValue.Score >= beta)
                        {
                            return currentValue;
                        }
                        if (newValue.Score > alpha)
                            alpha = newValue.Score;
                    }
                    return currentValue;
                }
            }

            private bool IsTurnOver(GameProjection game, int currentExecutorId)
            {
                if (game.CurrentPhase != PhaseType.Buy && currentExecutorId != -1)
                {
                    if (!game.UnitsIndexList.ContainsKey(currentExecutorId))
                        return true;
                    var currentExecutor = game.UnitsIndexList[currentExecutorId];
                    return currentExecutor.CurrentActionPoints <= 0;
                }
                return true;
            }

            protected IEnumerator PlayOutcome(PossibleOutcome outcome, GameProjection gameProjection)
            {
                yield return new WaitForSeconds(ai.firstCommandPause);
                foreach (var blueprint in outcome.Commands)
                {
                    var command = blueprint.GenerateMonoCommand(gameProjection);
                    
                    if (command is IActionCommand actionCommand)
                    {
                        Debug.Log(actionCommand.Action);
                        var isCompleted = false;
                        actionCommand.Action.ActionCompleted += OnActionCompleted;
                        if(actionCommand.Action is IExecutorAction<Unit> unitAction)
                        {
                            unitAction.Executor.Died.AddListener(OnUnitActionCompleted);
                        }

                        UnitsController.ExecuteCommand(command);
                        while (!isCompleted)
                        {
                            yield return new WaitForFixedUpdate();
                        }

                        yield return new WaitForSeconds(ai.commandPause);
                        void OnActionCompleted()
                        {
                            if (isCompleted) return;
                            if(actionCommand.Action is IExecutorAction<Unit> uAction)
                                uAction.Executor.Died.RemoveListener(OnUnitActionCompleted);
                            actionCommand.Action.ActionCompleted -= OnActionCompleted;
                            isCompleted = true;
                        }
                        void OnUnitActionCompleted(Unit unit)
                        {
                            OnActionCompleted();
                        }
                    }
                    else
                    {
                        UnitsController.ExecuteCommand(command);
                        yield return new WaitForSeconds(ai.commandPause);
                    }
                    
                }

                InvokeEnded();
            }

            
        }
    }
}
