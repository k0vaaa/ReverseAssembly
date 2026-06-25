using Core.StateMachines;
using Reflex.Core;
using Reflex.Injectors;

namespace Gameplay.StateMachines
{
    public class StateMachineInjector
    {
        public static void InjectStates(StateMachine stateMachine, Container container)
        {
            foreach (var stateMachineState in stateMachine.States)
            {
                AttributeInjector.Inject(stateMachineState.Value, container);
            }
        }
    }
}