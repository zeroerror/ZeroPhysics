using FixMath.NET;

namespace ZeroPhysics.Physics3D.Facade {

    public class Physics3DFactory {

        Physics3DFacade physicsFacade;

        public Physics3DFactory() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public Cube SpawnCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size) {
            Cube cube = new Cube(center, rotation, scale, size);
            cube.SetCenter(center);
            cube.SetRotation(rotation);
            cube.SetScale(scale);
            return cube;
        }

    }

}