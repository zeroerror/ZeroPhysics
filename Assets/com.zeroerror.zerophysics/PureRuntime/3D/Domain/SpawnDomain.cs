using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D.Domain
{

    public class SpawnDomain
    {

        Physics3DFacade physicsFacade;

        public SpawnDomain() { }

        public void Inject(Physics3DFacade physicsFacade)
        {
            this.physicsFacade = physicsFacade;
        }

        public Rigidbody3D SpawnRBCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size, in FP64 mass)
        {
            var factory = physicsFacade.Factory;
            var cube = SpawnCube(center, rotation, scale, size);
            Rigidbody3D rb = new Rigidbody3D(cube);
            rb.SetMass(mass);

            var idService = physicsFacade.Service.IDService;
            var id = idService.FetchID_RB();
            rb.SetRBID(id);
            UnityEngine.Debug.Log($"Spawn RB--{id} Cube--{cube.BodyID}");

            physicsFacade.rbs[id] = rb;
            return rb;
        }

        public Cube SpawnCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var factory = physicsFacade.Factory;
            var cube = factory.SpawnCube(center, rotation, scale, size);

            var idService = physicsFacade.Service.IDService;
            var id = idService.FetchID_Cube();
            cube.SetBodyID(id);
            physicsFacade.cubes[id] = cube;
            UnityEngine.Debug.Log($"Spawn Cube--{id}");
            return cube;
        }

        public Sphere3D SpawnSphere(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            Sphere3D sphere = new Sphere3D();
            sphere.SetCenter(center);
            sphere.SetRotation(rotation);
            sphere.SetScale(scale);
            sphere.SetRadius(size.x);
            sphere.UpdateScaledRadius();

            var idService = physicsFacade.Service.IDService;
            var id = idService.FetchID_Sphere();
            sphere.SetInstanceID(id);

            physicsFacade.spheres[id] = sphere;
            return sphere;
        }


    }

}