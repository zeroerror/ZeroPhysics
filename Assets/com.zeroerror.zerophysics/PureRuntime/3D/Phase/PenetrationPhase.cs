using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D
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
            var boxRBs = facade.boxRBs;
            var boxRBIDInfos = idService.boxRBIDInfos;
            var boxes = facade.boxes;
            var boxInfos = idService.boxIDInfos;

            // - RB & RB
            for (int i = 0; i < boxRBs.Length - 1; i++)
            {
                if (!boxRBIDInfos[i]) continue;
                var rb1 = boxRBs[i];
                for (int j = i + 1; j < boxRBs.Length; j++)
                {
                    if (!boxRBIDInfos[j]) continue;
                    var rb2 = boxRBs[j];
                    if (Intersect3DUtils.HasCollision(rb1.Box, rb2.Box))
                    {
                        var mtv = Penetration3DUtils.PenetrationCorrection(rb1.Box, FP64.Half, rb2.Box, FP64.Half);
                        var v1 = Penetration3DUtils.GetBouncedV(rb1.LinearV, mtv.normalized, rb1.BounceCoefficient);
                        rb1.SetLinearV(v1);
                        var v2 = Penetration3DUtils.GetBouncedV(rb2.LinearV, -mtv.normalized, rb2.BounceCoefficient);
                        rb2.SetLinearV(v2);
                        ;
                    }
                }
            }

            // - RB & SB
            for (int i = 0; i < boxRBs.Length; i++)
            {
                if (!boxRBIDInfos[i]) continue;
                var rb = boxRBs[i];
                var rbBox = rb.Box;
                for (int j = 0; j < boxes.Length; j++)
                {
                    if (!boxInfos[j]) continue;
                    var box = boxes[j];
                    if (Intersect3DUtils.HasCollision(rbBox, box))
                    {
                        var mtv = Penetration3DUtils.PenetrationCorrection(rbBox, 1, box, 0);
                        var v = Penetration3DUtils.GetBouncedV(rb.LinearV, mtv.normalized, rb.BounceCoefficient);
                        rb.SetLinearV(v);
                    }
                }
            }
        }

    }

}