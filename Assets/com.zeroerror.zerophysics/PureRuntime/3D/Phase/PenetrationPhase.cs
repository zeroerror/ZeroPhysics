using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D {

    public class PenetrationPhase {

        Physics3DFacade physicsFacade;

        public PenetrationPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time) {
            var idService = physicsFacade.Service.IDService;
            var collisionService = physicsFacade.Service.CollisionService;
            var boxRBs = physicsFacade.boxRBs;
            var boxRBIDInfos = idService.boxRBIDInfos;
            var boxes = physicsFacade.boxes;
            var boxInfos = idService.boxIDInfos;

            // - RB & SB
            // TODO 用Span数组存储 rb和box和 MTV
            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

                var rb = boxRBs[i];
                var rbBox = rb.Box;

                for (int j = 0; j < boxes.Length; j++) {
                    if (!boxInfos[j]) continue;

                    var box = boxes[j];
                    if (!collisionService.TryGetCollision(rb, box, out var collision)) continue;
                    if (collision.CollisionType == Generic.CollisionType.Exit) continue;

                    // bug 如果第一个MTV假如是一个斜向上的，那么如果当前MTV计算就会有问题 变成拉近 所以先缓存MTV，不做位移
                    var mtv = Penetration3DUtils.GetMTV(rbBox.GetModel(), box.GetModel());

                    var firctionCoe1 = rbBox.FrictionCoe;
                    var firctionCoe2 = box.FrictionCoe;
                    var firctionCoe_combined = firctionCoe1 < firctionCoe2 ? firctionCoe1 : firctionCoe2;
                    collision.SetFirctionCoe_combined(firctionCoe_combined);
                }

                // 这里运用
                // var beHitDir = mtv.normalized;
                // UnityEngine.Debug.Log($"beHitDir:{beHitDir} mtv:{mtv}");
                // collisionService.UpdateBeHitDir(rb, box, beHitDir);


            }
        }

    }

}