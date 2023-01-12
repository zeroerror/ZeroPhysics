using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Service;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public class VelocityPhase {

        Physics3DFacade physicsFacade;

        public VelocityPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 dt) {
            var rbBoxes = physicsFacade.boxRBs;
            var service = physicsFacade.Service;
            var idService = service.IDService;
            var collisionService = service.CollisionService;
            var rbBoxIDInfos = idService.boxRBIDInfos;

            for (int i = 0; i < rbBoxes.Length; i++) {
                if (!rbBoxIDInfos[i]) continue;
                var rb = rbBoxes[i];
                var linearV = rb.LinearV;
                var m = rb.Mass;
                var totalF = rb.GetTotalForce();
                var outForce = rb.OutForce;
                linearV += GetOffsetV_ByForce(totalF, m, dt);

                if (!collisionService.HasCollision(rb)) {
                    rb.SetLinearV(linearV);
                    continue;
                }

                ApplyFriction(collisionService, rb, dt, m, outForce, ref linearV);

                rb.SetLinearV(linearV);
            }
        }

        void ApplyFriction(CollisionService collisionService, Box3DRigidbody rb, in FP64 dt, in FP64 mass, in FPVector3 outForce, ref FPVector3 linearV) {
            var boxes = physicsFacade.boxes;
            var boxIDInfos = physicsFacade.Service.IDService.boxIDInfos;
            FPVector3 allFrictionForce = FPVector3.Zero;
            // 跟所有其他RB、SB进行 F = UN 计算 ，并且累加
            for (int j = 0; j < boxes.Length; j++) {
                if (!boxIDInfos[j]) continue;
                var box = boxes[j];
                if (!collisionService.TryGetCollision(rb, box, out var collision)) {
                    continue;
                }

                // - 摩擦力累加
                // - With SB
                FPVector3 beHitDirA = collision.BeHitDirA;
                FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;
                // 摩擦力不可能与外力在同一力线上
                var cos = FPVector3.Dot(linearV.normalized, beHitDir);
                if (FPUtils.IsNear(cos, FP64.One, FP64.EN1) || FPUtils.IsNear(cos, -FP64.One, FP64.EN1)) {
                    continue;
                }

                FP64 n = FPVector3.Dot(outForce, -beHitDir);
                FP64 u = collision.FirctionCoe_combined;
                FPVector3 force = u * n * GetFrictionDir(linearV, beHitDir);
                allFrictionForce += force;
            }

            // 对累加后的总滑动摩擦力进行计算
            // 根据速度计算摩擦力方向
            if (allFrictionForce != FPVector3.Zero
            && allFrictionForce.LengthSquared() != 0) {
                var offsetV_friction = GetOffsetV_ByForce(-allFrictionForce, mass, dt);
                var offsetLen = offsetV_friction.Length();
                var linearVLen = linearV.Length();
                if (offsetLen > linearVLen) {
                    linearV = FPVector3.Zero;
                    UnityEngine.Debug.Log("静摩擦力");
                } else if (linearVLen - offsetLen > FPUtils.epsilon_friction) {
                    UnityEngine.Debug.Log($"滑动摩擦力   差:{linearVLen - offsetLen} linearV:{linearV} offsetV_friction:{offsetV_friction}");
                    linearV += offsetV_friction;
                }
            }

        }

        FPVector3 GetFrictionDir(in FPVector3 linearV, in FPVector3 beHitDir) {
            var crossAxis = FPVector3.Cross(linearV, beHitDir);
            crossAxis.Normalize();
            var rot = FPQuaternion.CreateFromAxisAngle(crossAxis, FPUtils.rad_90);
            var frictionDir = rot * -beHitDir;    // 撞击方向 绕轴旋转
            return frictionDir;
        }

        FPVector3 GetOffsetV_ByForce(in FPVector3 f, in FP64 m, in FP64 t) {
            var a = f / m;
            var offset = a * t;
            return offset;
        }

    }

}