using FixMath.NET;
using ZeroPhysics.AllPhysics.Physics3D.Facade;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public class PenetrationPhase
    {

        Physics3DFacade facade;

        public PenetrationPhase() { }

        public void Inject(Physics3DFacade facade)
        {
            this.facade = facade;
        }

        public void Tick(in FP64 time)
        {
            var idService = facade.IDService;
            var rbBoxes = facade.rbBoxes;
            var rbBoxInfos = idService.rbBoxIDInfos;
            var boxes = facade.boxes;
            var boxInfos = idService.rbBoxIDInfos;
            // - RB & RB
            for (int i = 0; i < rbBoxes.Length - 1; i++)
            {
                if (!rbBoxInfos[i]) continue;
                var rbBox1 = rbBoxes[i];
                for (int j = i + 1; j < rbBoxes.Length; j++)
                {
                    if (!rbBoxInfos[j]) continue;
                    var rbBox2 = rbBoxes[j];
                    if (Intersect3DUtils.HasCollision(rbBox1.Box, rbBox2.Box))
                    {
                        Intersect3DUtils.PenetrationCorrection(rbBox1.Box, FP64.Half, rbBox2.Box, FP64.Half);
                    }
                }
            }
            // - RB & SB
            for (int i = 0; i < rbBoxes.Length; i++)
            {
                if (!rbBoxInfos[i]) continue;
                var rbBox = rbBoxes[i];
                for (int j = 0; j < boxes.Length; j++)
                {
                    if (!boxInfos[j]) continue;
                    var box = boxes[j];
                    if (Intersect3DUtils.HasCollision(rbBox.Box, box))
                    {
                        Intersect3DUtils.PenetrationCorrection(rbBox.Box, 1, box, 0);
                    }
                }
            }
        }

    }

}