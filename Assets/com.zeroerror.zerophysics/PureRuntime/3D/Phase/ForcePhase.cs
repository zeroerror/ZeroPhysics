using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D {

    public class ForcePhase {

        Physics3DFacade physicsFacade;

        public ForcePhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time, in FPVector3 gravity) {
            var boxRBs = physicsFacade.boxRBs;
            var rbBoxInfos = physicsFacade.Service.IDService.boxRBIDInfos;
            var collisionService = physicsFacade.Service.CollisionService;
            for (int i = 0; i < boxRBs.Length; i++) {
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