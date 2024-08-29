namespace AuthoringAndMono
{
    using System;
    using ComponentsAndTags;
    using Unity.Entities;
    using Unity.Mathematics;
    using UnityEngine;
    using Random = Unity.Mathematics.Random;

    public class GraveyardMono : MonoBehaviour
    {
        public float2     fieldDimensions;
        public int        numberTombstonesToSpawn;
        public GameObject tombstonePrefab;
        public uint       randomSeed;
    }
    
    public class GraveyardBaker : Baker<GraveyardMono>
    {
        public override void Bake(GraveyardMono authoring)
        {
            var graveyardEntity = this.GetEntity(TransformUsageFlags.Dynamic);
            
            this.AddComponent(graveyardEntity, new GraveyardProperties
            {
                FieldDimensions         = authoring.fieldDimensions,
                NumberTombstonesToSpawn = authoring.numberTombstonesToSpawn,
                TombstonePrefab         = this.GetEntity(authoring.tombstonePrefab, TransformUsageFlags.Dynamic),
            });
            
            this.AddComponent(graveyardEntity, new GraveyardRandom
            {
                Value = Random.CreateFromIndex(authoring.randomSeed),
            });
        }
    }
}