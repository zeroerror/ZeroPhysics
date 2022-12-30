using FixMath.NET;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public class TransformPhase
    {

        PhysicsWorld3D world;

        public TransformPhase() { }

        public void Inject(PhysicsWorld3D world)
        {
            this.world = world;
        }

        public void Tick(in FP64 time)
        {
            // --- Box
            var allRbBoxses = world.allRigidbodyBoxes;
            var rbBoxCount = world.rbBoxCount;
            for (int i = 0; i < rbBoxCount; i++)
            {
                var rb = allRbBoxses[i];
                var box = rb.Box;
                var center = box.Center;
                var offset = rb.LinearV * time;
                center += offset;
                box.SetCenter(center);
            }
        }

    }

}