namespace ComponentsAndTags
{
    using Unity.Entities;
    using Unity.Mathematics;

    public struct ZombieSpawnPoints : IComponentData
    {
        public BlobAssetReference<ZombieSpawnPointBlob> Value;
    }

    public struct ZombieSpawnPointBlob
    {
        public BlobArray<float3> Value;
    }
}