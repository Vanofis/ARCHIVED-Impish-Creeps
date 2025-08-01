using UnityEngine;
using Unity.Entities;

namespace GC.Gameplay.Status
{
    public class IsDisposingAuthoring : MonoBehaviour { }

    public class IsDisposingBaker : Baker<IsDisposingAuthoring>
    {
        public override void Bake(IsDisposingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new IsDisposingComponent());
        }
    }
}
