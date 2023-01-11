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

                FPVector3 totalForce = FPVector3.Zero;
                var rb = boxRBs[i];
                var rbBox = rb.Box;

                // ====== 重力计算
                ApplyGravity(gravity, rb, ref totalForce);

                if (!collisionService.HasCollision(rb)) {
                    UnityEngine.Debug.Log($"力:{totalForce}");
                    rb.SetTotalForce(totalForce);
                    continue;
                }

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

                    // ====== 弹力计算
                    ApplyBounce(rb, box, collision, dt, ref totalForce);
                    // ====== 摩擦力计算
                    ApplyFriction(rb, ref totalForce, collision, gravity);
                }

                UnityEngine.Debug.Log($"力:{totalForce}");
                rb.SetTotalForce(totalForce);
            }
        }

        void ApplyGravity(in FPVector3 gravity, Box3DRigidbody rb, ref FPVector3 totalForce) {
            var rbBox = rb.Box;
            var mass = rb.Mass;
            totalForce += gravity * mass;
            UnityEngine.Debug.Log($"重力:{totalForce}");
        }

        void ApplyBounce(Box3DRigidbody rb, Box3D box, CollisionModel collision, in FP64 dt, ref FPVector3 totalForce) {

            //   f /m  *time = v  f = vm/time;  + 重力需要抵消的力
            FPVector3 beHitDirA = collision.BeHitDirA;
            FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;
            var linearV = rb.LinearV;
            var v = Bounce3DUtils.GetBouncedV(linearV, beHitDir, rb.BounceCoefficient);
            var deltaV = v - linearV;
            var bounceF = deltaV * rb.Mass / dt;
            UnityEngine.Debug.Log($"弹力:{bounceF}");
            totalForce += bounceF;
        }

        void ApplyFriction(Box3DRigidbody rb, ref FPVector3 totalForce, CollisionModel collision, in FPVector3 gravity) {
            var linearV = rb.LinearV;
            var linearV_normalized = linearV.normalized;

            var rbBox = rb.Box;
            var mass = rb.Mass;
            FPVector3 beHitDirA = collision.BeHitDirA;
            FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;

            // Cross instead?
            var cos_vnh = FPVector3.Dot(linearV_normalized, beHitDir);
            if (FPUtils.IsNear(cos_vnh, -1, FP64.EN4) || FPUtils.IsNear(cos_vnh, 1, FP64.EN4)) {
                return;
            }

            var U = rbBox.FirctionCoe_combined;
            var N = FPVector3.Dot(gravity, -beHitDir);
            FPVector3 frictionF = U * N * -linearV_normalized;
            totalForce += frictionF;
            if (frictionF != FPVector3.Zero) {
                UnityEngine.Debug.Log($"摩擦力:{frictionF}");
                UnityEngine.Debug.Log($"摩擦力细节:  N {N} totalForce:{totalForce} ");
            }
        }

    }

}