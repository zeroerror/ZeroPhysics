using FixMath.NET;

namespace ZeroPhysics.Physics.API
{

    public interface ISetterAPI
    {

        Box SpawnCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);
        Rigidbody SpawnRBCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);
        Sphere SpawnSphere(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size);

    }

}