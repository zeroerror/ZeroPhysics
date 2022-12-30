using FixMath.NET;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public class ForcePhase
    {

        PhysicsWorld3D world;

        public ForcePhase() { }

        public void Inject(PhysicsWorld3D world)
        {
            this.world = world;
        }

        public void Tick(in FP64 time)
        {
       
        }

    }

}