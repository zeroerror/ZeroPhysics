using FixMath.NET;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public class VelocityPhase
    {

        PhysicsWorld3D world;

        public VelocityPhase() { }

        public void Inject(PhysicsWorld3D world)
        {
            this.world = world;
        }

        public void Tick(in FP64 time)
        {
            var allRbBoxes = world.allRigidbodyBoxes;
            var rbBoxCount = world.rbBoxCount;
            for (int i = 0; i < rbBoxCount; i++)
            {
                var rb = allRbBoxes[i];
                var linearV = rb.LinearV;
                var gravity = rb.Gravity;
                var offset = gravity * FPVector3.Down * time;
                linearV += offset;
                rb.SetLinearV(linearV);
            }
        }

    }

}