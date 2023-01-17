using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D.API
{

    public class SetterAPI : ISetterAPI
    {

        Physics3DFacade physicsFacade;

        public SetterAPI() { }

        public void Inject(Physics3DFacade physicsFacade)
        {
            this.physicsFacade = physicsFacade;
        }

        Rigidbody3D ISetterAPI.SpawnRBCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var domain = physicsFacade.Domain.SpawnDomain;
            return domain.SpawnRBCube(center, rotation, scale, size, 1);
        }

        Cube ISetterAPI.SpawnCube(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var domain = physicsFacade.Domain.SpawnDomain;
            return domain.SpawnCube(center, rotation, scale, size);
        }

        Sphere3D ISetterAPI.SpawnSphere(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            var domain = physicsFacade.Domain.SpawnDomain;
            return domain.SpawnSphere(center, rotation, scale, size);
        }
    }

}