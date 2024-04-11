using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class RLBuildAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IRLBuildAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public IBuildingFactory Factory { get; private set; }
        public IEnumerable<BuildingType> PossibleBuildings { get; }
        
        public override CommandType CommandType { get; set; } = CommandType.Build;
        public RLBuildAction(
            TUnit executor,
            IEnumerable<BuildingType> possibleTypes,
            IBuildingFactory factory) : base(executor)
        {
            Factory = factory;
            PossibleBuildings = possibleTypes.ToHashSet();
        }

        public bool CanBuild(TNode node, BuildingType buildingType)
        {
            return ActionPointsCondition()
                   && node.Building == null
                   && PossibleBuildings.Contains(buildingType);
        }

        public void Build(TNode node, BuildingType buildingType)
        {
            node.Building = Factory.Create(buildingType);
        }


        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) =>
            visitor.Visit(this);
    }
}