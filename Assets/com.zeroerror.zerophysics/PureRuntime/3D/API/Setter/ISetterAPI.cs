using FixMath.NET;

namespace ZeroPhysics.Physics3D.API
{

    public interface ISetterAPI
    {

        Cube SpawnCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);
        Rigidbody3D SpawnRBCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);
        Sphere3D SpawnSphere(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);

    }

}