using System.Collections.Generic;

namespace LineWars.Model
{
    public class ConvertBaseUnitActionToBlueprints<TNode, TEdge, TUnit> 
        : IBaseUnitActionVisitor<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit> 
    {
        public IGraphForGame<TNode, TEdge, TUnit> Graph {  get; private set; }
        public List<ICommandBlueprint> BlueprintList { get; private set; }


        public ConvertBaseUnitActionToBlueprints(IGraphForGame<TNode, TEdge, TUnit> graph,
            List<ICommandBlueprint> list)
        {
            Graph = graph;
            BlueprintList = list;
        }

        public void Visit(BuildAction<TNode, TEdge, TUnit> action)
        {
            foreach(var edge in action.Executor.Node.Edges)
            {
                if(action.CanUpRoad(edge))
                {
                    var command = new BuildCommandBlueprint(action.Executor.Id, edge.Id);
                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(BlockAction<TNode, TEdge, TUnit> action)
        {
            if(action.CanBlock())
            {
                var command = new BlockCommandBlueprint(action.Executor.Id);

                BlueprintList.Add(command);
            }
        }

        public void Visit(MoveAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                
                if(action.CanMoveTo(node))
                {

                    var command = new MoveCommandBlueprint(action.Executor.Id, node.Id);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(HealAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (action.CanHeal(unit))
                {
                    var unitId = action.Executor.Id;
                    var command = new HealCommandBlueprint(unitId, unit.Id);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit> action)
        {
            ProcessAttackAction(action, (uint)action.Executor.MaxActionPoints);
        }

        public void Visit(ArtilleryAttackAction<TNode, TEdge, TUnit> action)
        {
            ProcessAttackAction(action, (uint)action.Executor.MaxActionPoints);
        }

        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit> action)
        {
            ProcessAttackAction(action, 2);
        }

        public void Visit(RLBlockAction<TNode, TEdge, TUnit> action)
        {
            if(action.CanBlock())
            {
                var unitId = action.Executor.Id;
                var command = new RlBlockCommandBlueprint(unitId);
                BlueprintList.Add(command);
            }
        }
        
        public void Visit(RamAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if(action.CanRam(node))
                {
                    var unitId = action.Executor.Id;
                    var nodeId = node.Id;
                    var command = new RamCommandBlueprint(unitId, nodeId);

                    BlueprintList.Add(command);
                }
            }
        }
        
        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit> action)
        {
            var unitId = action.Executor.Id;

            foreach (var node in Graph.Nodes)
            {
                if(action.CanSacrifice(node))
                {
                    var nodeId = node.Id;
                    var command = new PerunActionCommandBlueprint(unitId, nodeId);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if(!node.LeftIsFree && action.IsAvailable(node.LeftUnit))
                {
                    ProcessUnit(node.LeftUnit);  
                    return;
                }
                if(!node.RightIsFree && action.IsAvailable(node.RightUnit))
                {
                    ProcessUnit(node.RightUnit);
                    return;
                }
            }

            void ProcessUnit(TUnit unit)
            {
                var command = new SwingCommandBlueprint(action.Executor.Id, unit.Id);
                BlueprintList.Add(command);
            }
        }
        public void Visit(ShotUnitAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                foreach (var nodeTarget in Graph.Nodes)
                {
                    if (action.IsAvailable(unit, nodeTarget))
                        BlueprintList.Add(new ShotUnitBlueprint(action.Executor.Id, unit.Id, nodeTarget.Id));
                }
            }    
        }

        private void ProcessAttackAction(AttackAction<TNode, TEdge, TUnit> action, uint range)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, range))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (action.CanAttackFrom(action.Executor.Node, unit))
                {
                    var unitId = action.Executor.Id;
                    var command = new AttackCommandBlueprint(unitId, unit.Id, AttackCommandBlueprint.Target.Unit);

                    BlueprintList.Add(command);
                }
            }
        } 

        public void Visit(StunAttackAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (!action.IsAvailable(unit)) return;

                var executorId = action.Executor.Id;
                var targetId = unit.Id;
                BlueprintList.Add(new TargetUnitUniversalCommandBlueprint<
                    MonoStunAttackAction,
                    StunAttackAction<Node, Edge, Unit>,
                    StunAttackAction<NodeProjection, EdgeProjection, UnitProjection>>
                    (executorId, targetId));

            }
        }

        public void Visit(HealingAttackAction<TNode, TEdge, TUnit> action)
        {
            foreach(var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (!action.IsAvailable(unit)) return;

                var executorId = action.Executor.Id;
                var targetId = unit.Id;
                BlueprintList.Add(new TargetUnitUniversalCommandBlueprint<
                    MonoHealingAttackAction,
                    HealingAttackAction<Node, Edge, Unit>,
                    HealingAttackAction<NodeProjection, EdgeProjection, UnitProjection>>
                    (executorId, targetId));

            }
        }

        public void Visit(TargetPowerBasedAttackAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (!action.IsAvailable(unit)) return;

                var executorId = action.Executor.Id;
                var targetId = unit.Id;
                BlueprintList.Add(new TargetUnitUniversalCommandBlueprint<
                    MonoTargetPowerBasedAttackAction,
                    TargetPowerBasedAttackAction<Node, Edge, Unit>,
                    TargetPowerBasedAttackAction<NodeProjection, EdgeProjection, UnitProjection>>
                    (executorId, targetId));

            }
        }

        public void Visit(UpArmorAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (!action.IsAvailable(unit)) return;

                var executorId = action.Executor.Id;
                var targetId = unit.Id;
                BlueprintList.Add(new TargetUnitUniversalCommandBlueprint<
                    MonoUpArmorAction,
                    UpArmorAction<Node, Edge, Unit>,
                    UpArmorAction<NodeProjection, EdgeProjection, UnitProjection>>
                    (executorId, targetId));
            }
        }

        public void Visit(PowerBasedHealAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (!action.IsAvailable(unit)) return;

                var executorId = action.Executor.Id;
                var targetId = unit.Id;
                BlueprintList.Add(new TargetUnitUniversalCommandBlueprint<
                    MonoPowerBasedHealAction,
                    PowerBasedHealAction<Node, Edge, Unit>,
                    PowerBasedHealAction<NodeProjection, EdgeProjection, UnitProjection>>
                    (executorId, targetId));
            }
        }

        public void Visit(RLBuildAction<TNode, TEdge, TUnit> action)
        {
            // TODO 
        }

        public void Visit(HealYourselfAction<TNode, TEdge, TUnit> action)
        {
            // TODO 
        }

        public void Visit(ArmorBasedAttackAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }

        public void Visit(ConsumeUnitAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }

        public void Visit(FogEraseAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }

        public void Visit(ArsonAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }

        public void Visit(SpawningUnitAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }

        public void Visit(UpActionPointsAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }

        public void Visit(JumpAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }

        public void Visit(HealSacrificeAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }

        public void Visit(VenomousSpitAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree && node.LeftUnit.Size == UnitSize.Little)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (!action.IsAvailable(unit)) return;

                var executorId = action.Executor.Id;
                var targetId = unit.Id;
                BlueprintList.Add(new TargetUnitUniversalCommandBlueprint<
                    MonoVenomousSpitAction,
                    VenomousSpitAction<Node, Edge, Unit>,
                    VenomousSpitAction<NodeProjection, EdgeProjection, UnitProjection>>
                    (executorId, targetId));
            }
        }

        public void Visit(LichKingMeleeAttack<TNode, TEdge, TUnit> action)
        {
            // TODO
        }
    }

    public static class ConvertUnitActionToBlueprints
    {
        public static ConvertBaseUnitActionToBlueprints<TNode, TEdge, TUnit> Create<TNode, TEdge, TUnit>(
            IGraphForGame<TNode, TEdge, TUnit> graph,
            List<ICommandBlueprint> list)
            where TNode : class, INodeForGame<TNode, TEdge, TUnit>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
            where TUnit : class, IUnit<TNode, TEdge, TUnit>
        {
            return new ConvertBaseUnitActionToBlueprints<TNode, TEdge, TUnit>(graph, list);
        }
    }
}
