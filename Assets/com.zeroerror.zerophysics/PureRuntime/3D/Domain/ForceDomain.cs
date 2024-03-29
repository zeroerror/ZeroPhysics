using FixMath.NET;
using ZeroPhysics.Physics.Context;

namespace ZeroPhysics.Physics.Domain {

    public class ForceDomain {

        PhysicsContext physicsContext;

        public ForceDomain() { }

        public void Inject(PhysicsContext physicsContext) {
            this.physicsContext = physicsContext;
        }

        public void Tick(in FP64 dt, in FPVector3 gravity) {
            var rbs = physicsContext.rbs;
            var service = physicsContext.Service;
            var idService = service.IDService;
            var rbInfos = idService.rbIDInfos;

            for (int i = 0; i < rbs.Length; i++) {
                if (!rbInfos[i]) continue;

                FPVector3 outForce = FPVector3.Zero;
                var rb = rbs[i];
                ApplyGravity(gravity, rb, ref outForce);
                rb.SetOutForce(outForce);
                rb.SetDirtyOutForce(outForce);
            }
        }

        void ApplyGravity(in FPVector3 gravity, Rigidbody rb, ref FPVector3 outForce) {
            var rbCube = rb.Body;
            var mass = rb.Mass;
            outForce += gravity * mass;
        }

    }

}