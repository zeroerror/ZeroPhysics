using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D {

    public class VelocityPhase {

        Physics3DFacade physicsFacade;

        public VelocityPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time) {
            var rbBoxes = physicsFacade.boxRBs;
            var rbBoxIDInfos = physicsFacade.Service.IDService.boxRBIDInfos;
            for (int i = 0; i < rbBoxes.Length; i++) {
                if (!rbBoxIDInfos[i]) continue;
                var rb = rbBoxes[i];
                var linearV = rb.LinearV;
                var f = rb.TotalForce;
                var m = rb.Mass;
                var a = f / m;
                var offset = a * time;
                linearV += offset;
                rb.SetLinearV(linearV);
            }
        }

    }

}