namespace Systems
{
    using ComponentsAndTags;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;

    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { state.RequireForUpdate<GraveyardProperties>(); }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
            var graveyard       = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);

            var     entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            var     tombstoneOffset     = new float3(0, -2f, 1);
            var     builder             = new BlobBuilder(Allocator.Temp);
            ref var spawnPoints         = ref builder.ConstructRoot<ZombieSpawnPointBlob>();
            var     arrayBuilder        = builder.Allocate(ref spawnPoints.Value, graveyard.NumberTombstonesToSpawn);

            for (var i = 0; i < graveyard.NumberTombstonesToSpawn; i++)
            {
                var newTombStone = entityCommandBuffer.Instantiate(graveyard.TombstonePrefab);
                var newTombStoneTransform = graveyard.GetRandomTombstoneTransform();
                entityCommandBuffer.SetComponent(newTombStone, newTombStoneTransform);
                
                var newZombieSpawnPoint = newTombStoneTransform.Position + tombstoneOffset;
                arrayBuilder[i] = newZombieSpawnPoint;
            }
            
            var blobAsset = builder.CreateBlobAssetReference<ZombieSpawnPointBlob>(Allocator.Persistent);
            entityCommandBuffer.SetComponent(graveyardEntity, new ZombieSpawnPoints{Value = blobAsset});
            builder.Dispose();
            
            entityCommandBuffer.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
           
        }
    }
}