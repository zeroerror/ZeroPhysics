using FixMath.NET;
using ZeroPhysics.Physics.Context;

namespace ZeroPhysics.Physics {

    public class TransformPhase {

        PhysicsContext physicsContext;

        public TransformPhase() { }

        public void Inject(PhysicsContext physicsContext) {
            this.physicsContext = physicsContext;
        }

        public void Tick(in FP64 time) {
            // --- Cube
            var rbCubees = physicsContext.rbs;
            var rbCubeInfos = physicsContext.Service.IDService.rbIDInfos;
            for (int i = 0; i < rbCubees.Length; i++) {
                if (!rbCubeInfos[i]) continue;
                var rb = rbCubees[i];
                var body = rb.Body;
                var trans = body.Trans;
                var center = trans.Center;
                var offset = rb.LinearV * time;
                center += offset;
                trans.SetCenter(center);
            }
        }

    }

}