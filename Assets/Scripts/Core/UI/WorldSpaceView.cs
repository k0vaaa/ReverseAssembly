using Core.Extensions;
using UnityEngine;

namespace Core.UI
{
    public class WorldSpaceView : View
    {
        public void Place(Transform pos)
        {
            gameObject.transform.position = pos.position.Add(y:1f);
            if (Camera.main != null)
            {
                gameObject.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - pos.position);
            }
        }
    }
}