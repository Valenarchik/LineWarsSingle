using System;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class MonoUnitAction: 
        MonoBehaviour,
        IUnitAction<Node, Edge, Unit>
    {
        public abstract CommandType CommandType { get; }
        public abstract int InitializePriority { get; }
        public abstract Unit Executor { get; }
        public abstract event Action ActionCompleted;
        
        public abstract void OnReplenish();
        public abstract int GetActionPointsCost();
        
        public abstract void Accept(IMonoUnitActionVisitor visitor);
        public abstract TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor);
    }

    [RequireComponent(typeof(Unit))]
    public abstract class MonoUnitAction<TAction> : 
        MonoUnitAction,
        IUnitAction<Node, Edge, Unit>,
        ISerializationCallbackReceiver
        where TAction : UnitAction<Node, Edge, Unit>
    {
        [SerializeField, ReadOnlyInspector] private Unit executor;
        [SerializeField] private int initializePriority;
        [SerializeField] protected IntModifier actionModifier;
        [SerializeField] protected Optional<CommandType> overrideCommandType;
        
        private TAction action;
        public override event Action ActionCompleted;
        
        public override Unit Executor => executor;
        public TAction Action => action;
        public override int InitializePriority => initializePriority;
        public override CommandType CommandType => Action.CommandType;



        protected virtual bool NeedAutoComplete => true;
        protected virtual bool NeedRecalculateVisibilityAfterCompleteAction => true;

        private void Awake()
        {
            executor = GetComponent<Unit>();
            InitializeInnerAction();
        }

        private void InitializeInnerAction()
        {
            action = GetAction();
            action.ActionModifier = actionModifier;
            if(NeedAutoComplete)
                action.ActionCompleted += () => ActionCompleted?.Invoke();
        }

        protected abstract TAction GetAction();

        protected void Complete()
        {
            ActionCompleted?.Invoke();
            if (NeedRecalculateVisibilityAfterCompleteAction)
                Player.LocalPlayer.RecalculateVisibility();
        }

        public override void OnReplenish() => Action.OnReplenish();
        public override int GetActionPointsCost() => Action.GetActionPointsCost();
        
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (Executor == null)
                return;
            InitializeInnerAction();
        }
        
        private void OnValidate()
        {
            executor = GetComponent<Unit>();
        }
        
    }
}