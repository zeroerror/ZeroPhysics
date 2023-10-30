using ZeroPhysics.Physics.Domain;

namespace ZeroPhysics.Physics.Context
{

    public class AllPhysicsDomain
    {

        public SpawnDomain SpawnDomain { get; private set; }
        public DataDomain DataDomain { get; private set; }

        public AllPhysicsDomain()
        {
            SpawnDomain = new SpawnDomain();
            DataDomain = new DataDomain();
        }

        public void Inject(PhysicsContext physicsContext)
        {
            SpawnDomain.Inject(physicsContext);
            DataDomain.Inject(physicsContext);
        }

    }

}