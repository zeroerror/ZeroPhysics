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
                var boxRB1 = rb1.Box;
                rb1.SetBeHitDir(FPVector3.Zero);
                boxRB1.SetFirctionCoe_combined(FP64.Zero);
                for (int j = i + 1; j < boxRBs.Length; j++)
                {
                    if (!boxRBIDInfos[j]) continue;

                    var rb2 = boxRBs[j];
                    var boxRB2 = rb2.Box;
                    rb2.SetBeHitDir(FPVector3.Zero);
                    boxRB2.SetFirctionCoe_combined(FP64.Zero);

                    if (!Intersect3DUtils.HasCollision(rb1.Box, rb2.Box)) continue;

                    var mtv = Penetration3DUtils.PenetrationCorrection(rb1.Box, FP64.Half, rb2.Box, FP64.Half);
                    var beHitDir = mtv.normalized;
                    var firctionCoe1 = boxRB1.FrictionCoe;
                    var firctionCoe2 = boxRB2.FrictionCoe;
                    var firctionCoe_combined = firctionCoe1 < firctionCoe2 ? firctionCoe1 : firctionCoe2;

                    var v1 = Penetration3DUtils.GetBouncedV(rb1.LinearV, beHitDir, rb1.BounceCoefficient);
                    rb1.SetLinearV(v1);
                    rb1.SetBeHitDir(beHitDir);
                    boxRB1.SetFirctionCoe_combined(firctionCoe_combined);

                    var v2 = Penetration3DUtils.GetBouncedV(rb2.LinearV, -beHitDir, rb2.BounceCoefficient);
                    rb2.SetLinearV(v2);
                    rb2.SetBeHitDir(-beHitDir);
                    boxRB2.SetFirctionCoe_combined(firctionCoe_combined);
                }
            }

            // - RB & SB
            for (int i = 0; i < boxRBs.Length; i++)
            {
                if (!boxRBIDInfos[i]) continue;

                var rb = boxRBs[i];
                var rbBox = rb.Box;
                rb.SetBeHitDir(FPVector3.Zero);
                rbBox.SetFirctionCoe_combined(FP64.Zero);
                for (int j = 0; j < boxes.Length; j++)
                {
                    if (!boxInfos[j]) continue;

                    var box = boxes[j];
                    if (!Intersect3DUtils.HasCollision(rbBox, box)) continue;

                    var mtv = Penetration3DUtils.PenetrationCorrection(rbBox, 1, box, 0);
                    var beHitDir = mtv.normalized;
                    var v = Penetration3DUtils.GetBouncedV(rb.LinearV, mtv.normalized, rb.BounceCoefficient);
                    var firctionCoe1 = rbBox.FrictionCoe;
                    var firctionCoe2 = box.FrictionCoe;
                    var firctionCoe_combined = firctionCoe1 < firctionCoe2 ? firctionCoe1 : firctionCoe2;

                    rb.SetLinearV(v);
                    rb.SetBeHitDir(beHitDir);
                    rbBox.SetFirctionCoe_combined(firctionCoe_combined);
                }
            }
        }

    }

}