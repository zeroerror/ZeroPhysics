using FixMath.NET;
using ZeroPhysics.AllPhysics.Physics3D.Facade;

namespace ZeroPhysics.AllPhysics.Physics3D.Domain
{

    public class SpawnDomain
    {

        Physics3DFacade physicsFacade;

        public SpawnDomain() { }

        public void Inject(Physics3DFacade facade)
        {
            this.physicsFacade = facade;
        }

        public Box3D SpawnBox(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var factory = physicsFacade.Factory;
            var box = factory.SpawnBox3D(center, rotation, scale, size);

            var idService = physicsFacade.IDService;
            var id = idService.FetchID_Box();
            box.SetInstanceID(id);

            physicsFacade.boxes[id] = box;
            return box;
        }

        public RigidbodyBox3D SpawnRBBox(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size, in FP64 mass)
        {
            var factory = physicsFacade.Factory;
            var box = factory.SpawnBox3D(center, rotation, scale, size);

            RigidbodyBox3D rb = new RigidbodyBox3D(box);
            rb.SetMass(mass);

            var idService = physicsFacade.IDService;
            var id = idService.FetchID_RBBox();
            rb.SetInstanceID(id);

            physicsFacade.rbBoxes[id] = rb;
            return rb;
        }

        public Sphere3D SpawnSphere(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            Sphere3D sphere = new Sphere3D();
            sphere.SetCenter(center);
            sphere.SetRotation(rotation);
            sphere.SetScale(scale);
            sphere.SetRadius(size.x);
            sphere.UpdateScaledRadius();

            var idService = physicsFacade.IDService;
            var id = idService.FetchID_Sphere();
            sphere.SetInstanceID(id);

            physicsFacade.spheres[id] = sphere;
            return sphere;
        }


    }

}