using System.Diagnostics.CodeAnalysis;
using LineWars.Model;
using UnityEngine;


namespace LineWars
{
    public class UnitsController : MonoBehaviour
    {
        public static UnitsController Instance { get; private set; }
        [SerializeField] private bool needLog = true;  
        private int currentCommandIndex;
        
        private void Awake()
        {
            Instance = this;
        }

        public static void ExecuteCommand([NotNull]ICommand command, bool dontCheckExecute = true)
        {
            if (Instance != null)
                Instance._ExecuteCommand(command, dontCheckExecute);
        }

        private void _ExecuteCommand([NotNull] ICommand command, bool dontCheckExecute = true)
        {
            if (dontCheckExecute)
            {
                Execute();
            }
            else
            {
                if (command.CanExecute())
                {
                    Execute();
                }
                else
                {
                    Debug.LogError($"Cant execute command: {command.GetLog()}");
                }
            }

            void Execute()
            {
                currentCommandIndex++;
                if (needLog)
                {
                    Debug.Log($"<color=yellow>COMMAND {currentCommandIndex}</color> {command.GetLog()}");
                }
                command.Execute();
            }
        }
    }
}