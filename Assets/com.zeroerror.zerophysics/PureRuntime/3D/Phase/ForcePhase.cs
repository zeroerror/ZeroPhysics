using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public class ForcePhase {

        Physics3DFacade physicsFacade;

        public ForcePhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        // -----------------所有力都在ForcePhase 

        // 重力：
        // 最先计算的力

        // 弹力:
        // 也是一个力，根据改变的速度量进行力的计算  得出力

        // 摩擦力:
        // 摩擦力在速度为0时为0，在速度不为0时根据其他所有力和速度方向计算，所以在力的最后一个计算  得出力

        public void Tick(in FP64 time, in FPVector3 gravity) {
            var boxRBs = physicsFacade.boxRBs;
            var boxRBIDInfos = physicsFacade.Service.IDService.boxRBIDInfos;
            var collisionService = physicsFacade.Service.CollisionService;
            FPVector3 totalForce = FPVector3.Zero;

            // ====== 重力计算
            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

                var rb = boxRBs[i];
                var rbBox = rb.Box;
                var mass = rb.Mass;

                // === Gravity
                totalForce += gravity * mass;
                UnityEngine.Debug.Log($"重力:{totalForce}");
            }

            // ===== 弹力计算
            var boxes = physicsFacade.boxes;
            var boxInfos = physicsFacade.Service.IDService.boxIDInfos;
            // - RB & SB
            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

                var rb = boxRBs[i];
                var rbBox = rb.Box;
                if (!collisionService.HasCollision(rb)) continue;

                for (int j = 0; j < boxes.Length; j++) {
                    if (!boxInfos[j]) continue;

                    var box = boxes[j];
                    if (!collisionService.TryGetCollision(rb, box, out var collision)) continue;
                    if (collision.CollisionType == Generic.CollisionType.Exit) continue;

                    //   f /m  *time = v  f = vm/time;
                    FPVector3 beHitDirA = collision.BeHitDirA;
                    FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;
                    var linearV = rb.LinearV;
                    var v = Bounce3DUtils.GetBouncedV(linearV, beHitDir, rb.BounceCoefficient);
                    var deltaV = v - linearV;
                    var bounceF = deltaV * rb.Mass / time;
                    UnityEngine.Debug.Log($"弹力:{bounceF}");
                    totalForce += bounceF;
                }
            }

            // ====== 摩擦力计算
            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

                var rb = boxRBs[i];
                rb.SetTotalForce(totalForce);
                UnityEngine.Debug.Log($"力:{totalForce}");

                if (!collisionService.TryGetCollision(rb, out var collision)) continue;

                var linearV = rb.LinearV;
                var linearV_normalized = linearV.normalized;

                var rbBox = rb.Box;
                var mass = rb.Mass;
                FPVector3 beHitDirA = collision.BeHitDirA;
                FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;

                // Cross instead?
                var cos_vnh = FPVector3.Dot(linearV_normalized, beHitDir);
                if (FPUtils.IsNear(cos_vnh, -1, FP64.EN4) || FPUtils.IsNear(cos_vnh, 1, FP64.EN4)) {
                    continue;
                }

                var U = rbBox.FirctionCoe_combined;
                var N = FPVector3.Dot(totalForce, -beHitDir);
                FP64 frictionF = U * N;
                totalForce += frictionF * -linearV_normalized;
                UnityEngine.Debug.Log($"摩擦力:{frictionF}");
            }

        }

    }

}