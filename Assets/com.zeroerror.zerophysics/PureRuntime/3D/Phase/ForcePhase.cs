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
            var rbs = physicsFacade.rbs;
            var service = physicsFacade.Service;
            var idService = service.IDService;
            var rbInfos = idService.rbIDInfos;

            for (int i = 0; i < rbs.Length; i++) {
                if (!rbInfos[i]) continue;

                FPVector3 outForce = FPVector3.Zero;
                var rb = rbs[i];
                ApplyGravity(gravity, rb, ref outForce);
                rb.SetOutForce(outForce);
            }
        }

        void ApplyGravity(in FPVector3 gravity, Rigidbody3D rb, ref FPVector3 outForce) {
            var rbCube = rb.Body;
            var mass = rb.Mass;
            outForce += gravity * mass;
        }

    }

}