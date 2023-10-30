using FixMath.NET;
using ZeroPhysics.Physics.Context;

namespace ZeroPhysics.Physics.Domain {

    public class SpawnDomain {

        PhysicsContext physicsContext;

        public SpawnDomain() { }

        public void Inject(PhysicsContext physicsContext) {
            this.physicsContext = physicsContext;
        }

        public Rigidbody SpawnRBCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size, in FP64 mass) {
            var factory = physicsContext.Factory;
            var cube = factory.SpawnCube(center, rotation, scale, size);
            Rigidbody rb = new Rigidbody(cube);
            rb.SetMass(mass);
            var idService = physicsContext.Service.IDService;
            var id = idService.FetchID_RB();
            rb.SetRBID(id);
            cube.SetRB(rb);

            Logger.Log($"Spawn RBCube--{id}");

            physicsContext.rbs[id] = rb;
            return rb;
        }

        public Box SpawnCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size) {
            var factory = physicsContext.Factory;
            var cube = factory.SpawnCube(center, rotation, scale, size);

            var idService = physicsContext.Service.IDService;
            var id = idService.FetchID_Cube();
            cube.SetBodyID(id);
            physicsContext.cubes[id] = cube;
            Logger.Log($"Spawn Cube--{id}");
            return cube;
        }

        public Sphere SpawnSphere(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size) {
            Sphere sphere = new Sphere();
            sphere.SetCenter(center);
            sphere.SetRotation(rotation);
            sphere.SetScale(scale);
            sphere.SetRadius(size.x);
            sphere.UpdateScaledRadius();

            var idService = physicsContext.Service.IDService;
            var id = idService.FetchID_Sphere();
            sphere.SetInstanceID(id);

            physicsContext.spheres[id] = sphere;
            return sphere;
        }


    }

}