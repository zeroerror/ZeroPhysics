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
            var rbBoxes = facade.rb_boxes;
            var rbBoxInfos = facade.IDService.rbBoxIDInfos;
            for (int i = 0; i < rbBoxes.Length; i++)
            {
                if (!rbBoxInfos[i]) continue;
                
                // - Force Calculation
                FPVector3 f = FPVector3.Zero;
                f += gravity;
                // - Set
                var rb = rbBoxes[i];
                rb.SetForce(f);
            }
        }

    }

}