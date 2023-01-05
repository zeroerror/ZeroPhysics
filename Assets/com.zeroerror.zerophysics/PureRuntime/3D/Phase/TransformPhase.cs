using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D
{

    public class TransformPhase
    {

        Physics3DFacade facade;

        public TransformPhase() { }

        public void Inject(Physics3DFacade facade)
        {
            this.facade = facade;
        }

        public void Tick(in FP64 time)
        {
            // --- Box
            var rbBoxes = facade.rb_boxes;
            var rbBoxInfos = facade.IDService.rbBoxIDInfos;
            for (int i = 0; i < rbBoxes.Length; i++)
            {
                if (!rbBoxInfos[i]) continue;
                var rb = rbBoxes[i];
                var box = rb.Box;
                var center = box.Center;
                var offset = rb.LinearV * time;
                center += offset;
                box.SetCenter(center);
            }
        }

    }

}