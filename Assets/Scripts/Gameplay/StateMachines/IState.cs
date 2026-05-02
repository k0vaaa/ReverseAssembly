namespace Gameplay.StateMachines
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }
}