using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D
{

    public class ForcePhase
    {

        Physics3DFacade facade;

        public ForcePhase() { }

        public void Inject(Physics3DFacade facade)
        {
            this.facade = facade;
        }

        public void Tick(in FP64 time, in FPVector3 gravity)
        {
            var boxRBs = facade.boxRBs;
            var rbBoxInfos = facade.IDService.boxRBIDInfos;
            for (int i = 0; i < boxRBs.Length; i++)
            {
                if (!rbBoxInfos[i]) continue;

                var rb = boxRBs[i];
                var rbBox = rb.Box;
                var mass = rb.Mass;
                FPVector3 totalForce = FPVector3.Zero;

                // === Gravity
                totalForce += gravity * mass;

                // - Set
                rb.SetTotalForce(totalForce);
            }
        }

    }

}