using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D {

    public class TransformPhase {

        Physics3DFacade physicsFacade;

        public TransformPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time) {
            // --- Cube
            var rbCubees = physicsFacade.rbs;
            var rbCubeInfos = physicsFacade.Service.IDService.rbIDInfos;
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