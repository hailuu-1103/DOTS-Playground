namespace ComponentsAndTags
{
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;

    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> transform;
        private          LocalTransform        Transform => this.transform.ValueRO;

        private readonly RefRO<GraveyardProperties> graveyardProperties;
        private readonly RefRW<GraveyardRandom>     graveyardRandom;

        public int    NumberTombstonesToSpawn => this.graveyardProperties.ValueRO.NumberTombstonesToSpawn;
        public Entity TombstonePrefab         => this.graveyardProperties.ValueRO.TombstonePrefab;

        public LocalTransform GetRandomTombstoneTransform()
        {
            return new()
            {
                Position = this.GetRandomPosition(),
                Rotation = this.GetRandomRotation(),
                Scale    = this.GetRandomScale(0.5f),
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;
            do
            {
                randomPosition = this.graveyardRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(this.Transform.Position, randomPosition) <= BRAIN_SAFETY_RADIUS_SQ);

            return randomPosition;
        }

        private quaternion GetRandomRotation() => quaternion.RotateY(this.graveyardRandom.ValueRW.Value.NextFloat(-0.25f, 0.25f));

        private float GetRandomScale(float min) => this.graveyardRandom.ValueRW.Value.NextFloat(min, 1f);

        private float3 MinCorner => this.Transform.Position - HalfDimensions;
        private float3 MaxCorner => this.Transform.Position + HalfDimensions;

        private float3 HalfDimensions => new()
        {
            x = this.graveyardProperties.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = this.graveyardProperties.ValueRO.FieldDimensions.y * 0.5f,
        };

        private const float BRAIN_SAFETY_RADIUS_SQ = 100;
    }
}