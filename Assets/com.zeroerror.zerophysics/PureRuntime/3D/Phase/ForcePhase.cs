using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public class ForcePhase {

        Physics3DFacade physicsFacade;

        public ForcePhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 dt, in FPVector3 gravity) {
            var boxRBs = physicsFacade.boxRBs;
            var boxes = physicsFacade.boxes;

            var service = physicsFacade.Service;
            var idService = service.IDService;
            var collisionService = service.CollisionService;

            var boxRBIDInfos = idService.boxRBIDInfos;
            var boxInfos = idService.boxIDInfos;

            // - RB & SB
            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

                FPVector3 outForce = FPVector3.Zero;
                var rb = boxRBs[i];

                // ====== 重力计算
                ApplyGravity(gravity, rb, ref outForce);
                rb.SetOutForce(outForce);
            }
        }

        void ApplyGravity(in FPVector3 gravity, Box3DRigidbody rb, ref FPVector3 outForce) {
            var rbBox = rb.Box;
            var mass = rb.Mass;
            outForce += gravity * mass;
        }

    }

}