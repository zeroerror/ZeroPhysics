using FixMath.NET;
using ZeroPhysics.Physics.Context;

namespace ZeroPhysics.Physics.API
{

    public class SetterAPI : ISetterAPI
    {

        PhysicsContext physicsContext;

        public SetterAPI() { }

        public void Inject(PhysicsContext physicsContext)
        {
            this.physicsContext = physicsContext;
        }

        Rigidbody ISetterAPI.SpawnRBCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var domain = physicsContext.Domain.SpawnDomain;
            return domain.SpawnRBCube(center, rotation, scale, size, 1);
        }

        Box ISetterAPI.SpawnCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var domain = physicsContext.Domain.SpawnDomain;
            return domain.SpawnCube(center, rotation, scale, size);
        }

        Sphere ISetterAPI.SpawnSphere(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var domain = physicsContext.Domain.SpawnDomain;
            return domain.SpawnSphere(center, rotation, scale, size);
        }
    }

}