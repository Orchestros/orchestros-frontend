using UnityEngine;

namespace Managers
{
    public class ObjectManager : MonoBehaviourWithState
    {
        public void AddObject(GameObject sourceObject)
        {
            if (IsActive) return;

            OnActivate();

            var newObject = Instantiate(sourceObject, sourceObject.transform.position,
                sourceObject.transform.rotation);

            newObject.layer = 2; // invisible to raytracing in layer 2 

            var c = newObject.AddComponent<ObjectAdder>();
            c.OnCompleted = OnDeactivate;
        }

    }
}