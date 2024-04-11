using System;
using JetBrains.Annotations;
using LineWars.Controllers;

namespace LineWars.Model
{
    public abstract class ExecutorAction<T> : IExecutorAction<T>
        where T : class, IExecutor
    {
        public T Executor { get; }
        public IntModifier ActionModifier { get; set; }
        public abstract CommandType CommandType { get; set; }
        public event Action ActionCompleted;

        protected ExecutorAction([NotNull] T executor)
        {
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public virtual void OnReplenish()
        {
        }

        protected void Complete()
        {
            ActionCompleted?.Invoke();
        }

        protected void CompleteAndAutoModify()
        {
            Executor.CurrentActionPoints = GetActionPointsAfterModify();
            Complete();
        }

        public int GetActionPointsAfterModify()
        {
            return ActionModifier.Modify(Executor.CurrentActionPoints);
        }

        public int GetActionPointsCost()
        {
            return Executor.CurrentActionPoints - GetActionPointsAfterModify();
        }

        public bool ActionPointsCondition()
        {
            return ExecutorAction.ActionPointsCondition(ActionModifier, Executor.CurrentActionPoints);
        }
    }

    public static class ExecutorAction
    {
        public static bool ActionPointsCondition(IntModifier modifier, int actionPoints)
        {
            return actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
        }
    }
}