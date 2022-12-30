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
            var rb_boxes = facade.rb_boxes;
            var rbBoxInfos = idService.rbBoxIDInfos;
            var boxes = facade.boxes;
            var boxInfos = idService.boxIDInfos;

            // - RB & RB
            for (int i = 0; i < rb_boxes.Length - 1; i++)
            {
                if (!rbBoxInfos[i]) continue;
                var rbBox1 = rb_boxes[i];
                for (int j = i + 1; j < rb_boxes.Length; j++)
                {
                    if (!rbBoxInfos[j]) continue;
                    var rbBox2 = rb_boxes[j];
                    if (Intersect3DUtils.HasCollision(rbBox1.Box, rbBox2.Box))
                    {
                        var mtv = Penetration3DUtils.PenetrationCorrection(rbBox1.Box, FP64.Half, rbBox2.Box, FP64.Half);
                        var v1 = Penetration3DUtils.GetErasedVector3(rbBox1.LinearV, mtv.normalized);
                        rbBox1.SetLinearV(v1);
                        var v2 = Penetration3DUtils.GetErasedVector3(rbBox2.LinearV, -mtv.normalized);
                        rbBox2.SetLinearV(v2);
                    }
                }
            }

            // - RB & SB
            for (int i = 0; i < rb_boxes.Length; i++)
            {
                if (!rbBoxInfos[i]) continue;
                var rb = rb_boxes[i];
                var rbBox = rb.Box;
                for (int j = 0; j < boxes.Length; j++)
                {
                    if (!boxInfos[j]) continue;
                    var box = boxes[j];
                    if (Intersect3DUtils.HasCollision(rbBox, box))
                    {
                        var mtv = Penetration3DUtils.PenetrationCorrection(rbBox, 1, box, 0);
                        var v = Penetration3DUtils.GetErasedVector3(rb.LinearV, mtv.normalized);
                        rb.SetLinearV(v);
                    }
                }
            }
        }

    }

}