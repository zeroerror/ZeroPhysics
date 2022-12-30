using ZeroPhysics.AllPhysics.Physics3D.Domain;

namespace ZeroPhysics.AllPhysics.Physics3D.Facade
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

        public void Inject(Physics3DFacade facade)
        {
            SpawnDomain.Inject(facade);
            DataDomain.Inject(facade);
        }

    }

}