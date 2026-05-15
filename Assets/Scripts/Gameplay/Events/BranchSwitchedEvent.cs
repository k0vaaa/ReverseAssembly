using Core.Events;

namespace Gameplay.Events
{
    public enum WorldBranch { Main, Alpha }

    public class BranchSwitchedEvent : IEvent
    {
        public WorldBranch NewBranch;

        public BranchSwitchedEvent(WorldBranch newBranch)
        {
            NewBranch = newBranch;
        }
    }
}