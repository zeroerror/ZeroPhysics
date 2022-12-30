using FixMath.NET;

namespace ZeroPhysics.AllPhysics.Physics3D.API
{

    public interface ISetterAPI
    {

        Box3D SpawnBox(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);
        RigidbodyBox3D SpawnRBBox(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);
        public Sphere3D SpawnSphere(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);

    }

}