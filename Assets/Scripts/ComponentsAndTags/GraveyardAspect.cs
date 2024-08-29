namespace ComponentsAndTags
{
    using Unity.Entities;
    using Unity.Transforms;

    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<LocalTransform>      localTransform;
        private readonly RefRO<GraveyardProperties> graveyardProperties;
    }
}