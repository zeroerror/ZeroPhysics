using FixMath.NET;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public class PhysicsWorld3D
    {

        public RigidbodyBox3D[] allRigidbodyBoxes;
        public int rbBoxCount;

        public Box3D[] allBoxes;
        public int boxCount;

        public Sphere3D[] allSpheres;
        public int sphereCount;

        FP64 gravity;
        public FP64 Gravity => gravity;
        public void SetGravity(FP64 v) => gravity = v;

        // ====== Phase
        ForcePhase forcePhase;
        VelocityPhase velocityPhase;
        TransformPhase transformPhase;
        PenetrationPhase penetrationPhase;

        public PhysicsWorld3D(FP64 gravity)
        {
            this.gravity = gravity;
            
            allBoxes = new Box3D[1000];
            allRigidbodyBoxes = new RigidbodyBox3D[1000];
            allSpheres = new Sphere3D[1000];

            forcePhase = new ForcePhase();
            velocityPhase = new VelocityPhase();
            transformPhase = new TransformPhase();
            penetrationPhase = new PenetrationPhase();

            forcePhase.Inject(this);
            velocityPhase.Inject(this);
            transformPhase.Inject(this);
            penetrationPhase.Inject(this);
        }

        public void Tick(FP64 time)
        {
            forcePhase.Tick(time);
            velocityPhase.Tick(time);
            transformPhase.Tick(time);
            penetrationPhase.Tick(time);
        }

        public Box3D SpawnBox(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            Box3D box = new Box3D();
            box.SetCenter(center);
            box.SetRotation(rotation);
            box.SetScale(scale);

            box.SetWidth(size.x);
            box.SetHeight(size.y);
            box.SetLength(size.z);
            allBoxes[boxCount++] = box;
            return box;
        }

        public RigidbodyBox3D SpawnRigidbodyAndBox(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var box = SpawnBox(center, rotation, scale, size);
            RigidbodyBox3D rb = new RigidbodyBox3D(box);
            rb.SetGravity(gravity);
            allRigidbodyBoxes[rbBoxCount++] = rb;
            return rb;
        }

        public RigidbodyBox3D SpawnRigidbody(Box3D box)
        {
            RigidbodyBox3D rb = new RigidbodyBox3D(box);
            rb.SetGravity(gravity);
            allRigidbodyBoxes[rbBoxCount++] = rb;
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
            allSpheres[rbBoxCount++] = sphere;
            return sphere;
        }

    }

}
