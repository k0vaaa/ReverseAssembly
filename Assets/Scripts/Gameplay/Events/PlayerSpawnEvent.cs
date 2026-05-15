using Core.Events;
using UnityEngine;

namespace Gameplay.Events
{
    public class PlayerSpawnEvent : IEvent
    {
        public Transform PlayerTransform;
        public Camera Camera;
    }
}