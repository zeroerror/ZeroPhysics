using FixMath.NET;

namespace ZeroPhysics.Physics.Context {

    public class PhysicsFactory {

        PhysicsContext physicsContext;

        public PhysicsFactory() { }

        public void Inject(PhysicsContext physicsContext) {
            this.physicsContext = physicsContext;
        }

        public Box SpawnCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size) {
            Box cube = new Box(center, rotation, scale, size);
            cube.SetCenter(center);
            cube.SetRotation(rotation);
            cube.SetScale(scale);
            return cube;
        }

    }

}