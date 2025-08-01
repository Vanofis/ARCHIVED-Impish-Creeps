using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using GC.Gameplay.Units.Movement;
using GC.Gameplay.SplineFramework;

namespace GC.Gameplay.Units.Spawn
{
    [BurstCompile]
    public readonly partial struct UnitSpawnerAspect : IAspect
    {
        private readonly Entity entity;

        private readonly RefRW<UnitSpawnTimeComponent> spawnTime;

        [BurstCompile]
        public void UpdateSpawnTimer(float deltaTime)
        {
            spawnTime.ValueRW.spawnTimer += deltaTime;

            if (spawnTime.ValueRO.spawnTimer < spawnTime.ValueRO.spawnInterval)
                return;

            spawnTime.ValueRW.spawnTimer = 0;
            spawnTime.ValueRW.canSpawn = true;
        }

        public bool IsAbleToSpawn()
            => spawnTime.ValueRO.canSpawn;

        public void SetAbilityToSpawn(bool b)
            => spawnTime.ValueRW.canSpawn = b;

        [BurstCompile]
        public void Spawn(EntityCommandBuffer entityCommandBuffer, SplineContainer splineContainer, DynamicBuffer<UnitDeckElement> unitBuffer, int unitIndex)
        {
            if (!spawnTime.ValueRW.canSpawn)
                return;

            var unitDeckElement = unitBuffer[unitIndex];

            Entity spawnedEntity = entityCommandBuffer.Instantiate(unitDeckElement.unit);

            entityCommandBuffer.SetComponent(spawnedEntity, new LocalTransform
            {
                Position = splineContainer.GetSplineByIndex(0).SplineSegments[0].StartPoint,
                Rotation = quaternion.identity,
                Scale = 1,
            });

            spawnTime.ValueRW.canSpawn = false;
        }
    }
}
