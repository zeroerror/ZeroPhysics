using FixMath.NET;
using ZeroPhysics.Generic;
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
        // 重力：最先计算的力

        // 外力，即上层调接口施加的力

        // 弹力: 根据改变的速度量进行力的计算  得出力

        // 摩擦力: 速度为0时为0，在速度不为0时根据其他所有力和速度方向计算，所以在力的最后一个计算  得出力

        public void Tick(in FP64 dt, in FPVector3 gravity) {
            var boxRBs = physicsFacade.boxRBs;
            var boxes = physicsFacade.boxes;

            var service = physicsFacade.Service;
            var idService = service.IDService;
            var collisionService = service.CollisionService;

            var boxRBIDInfos = idService.boxRBIDInfos;
            var boxInfos = idService.boxIDInfos;

            // - RB & SB
            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

                FPVector3 outForce = FPVector3.Zero;
                FPVector3 bounceForce = FPVector3.Zero;

                var rb = boxRBs[i];
                var rbBox = rb.Box;
                FP64 mass = rb.Mass;
                FPVector3 v = rb.LinearV;
                var v_normalized = v.normalized;

                // ====== 重力计算
                ApplyGravity(gravity, rb, ref outForce);

                if (!collisionService.HasCollision(rb)) {
                    rb.SetOutForce(outForce);
                    continue;
                }

                FPVector3 v_bounced = FPVector3.Zero;
                for (int j = 0; j < boxes.Length; j++) {
                    if (!boxInfos[j]) {
                        continue;
                    }
                    var box = boxes[j];
                    if (!collisionService.TryGetCollision(rb, box, out var collision)) {
                        continue;
                    }
                    if (collision.CollisionType == Generic.CollisionType.Exit) {
                        continue;
                    }

                    FPVector3 beHitDirA = collision.BeHitDirA;
                    FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;
                    // - 弹力累加
                    CalculateBounce(rb, box, beHitDir, outForce, dt, ref bounceForce, ref v_bounced);
                    if (v_bounced == FPVector3.Zero) {
                        continue;
                    }

                }

                // ====== 弹力 
                rb.SetBounceForce(bounceForce);
            }
        }

        void ApplyGravity(in FPVector3 gravity, Box3DRigidbody rb, ref FPVector3 outForce) {
            var rbBox = rb.Box;
            var mass = rb.Mass;
            outForce += gravity * mass;
        }

        void CalculateBounce(Box3DRigidbody rb, Box3D box, in FPVector3 beHitDir, in FPVector3 totalForce_outForce, in FP64 dt,
        ref FPVector3 totalForce_bounce, ref FPVector3 v_bounced) {
            var linearV = rb.LinearV;
            v_bounced = Bounce3DUtils.GetBouncedV(linearV, beHitDir, rb.BounceCoefficient);
            var deltaV = v_bounced - linearV;
            var bounceF = deltaV * rb.Mass / dt;
            totalForce_bounce += bounceF;
        }

    }

}