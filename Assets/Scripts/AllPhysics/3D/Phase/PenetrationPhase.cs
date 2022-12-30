using FixMath.NET;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public class PenetrationPhase
    {

        PhysicsWorld3D world;

        public PenetrationPhase() { }

        public void Inject(PhysicsWorld3D world)
        {
            this.world = world;
        }

        public void Tick(in FP64 time)
        {
            var allRbBoxes = world.allRigidbodyBoxes;
            var rbBoxCount = world.rbBoxCount;
            for (int i = 0; i < rbBoxCount - 1; i++)
            {
                var rb1 = allRbBoxes[i];
                for (int j = i + 1; j < rbBoxCount; j++)
                {
                    var rb2 = allRbBoxes[j];
                    Intersect3DUtils.PenetrationCorrection(rb1.Box, FP64.ToFP64(0.5f), rb2.Box, FP64.ToFP64(0.5f));
                }
            }
        }

    }

}