using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;
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
            var boxes = physicsFacade.boxes;
            var service = physicsFacade.Service;
            var idService = service.IDService;
            var collisionService = service.CollisionService;
            var rbBoxIDInfos = idService.boxRBIDInfos;
            var boxIDInfos = idService.boxIDInfos;

            for (int i = 0; i < rbBoxes.Length; i++) {
                if (!rbBoxIDInfos[i]) continue;
                var rb = rbBoxes[i];
                var linearV = rb.LinearV;
                var m = rb.Mass;
                var totalF = rb.GetTotalForce();
                var outForce = rb.OutForce;
                linearV += GetOffsetV_ByForce(totalF, m, dt);

                if (!collisionService.HasCollision(rb)) {
                    UnityEngine.Debug.Log($"notHasCollision");
                    rb.SetLinearV(linearV);
                    continue;
                }

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
                    FP64 n = FPVector3.Dot(outForce, -beHitDir);
                    FP64 u = collision.FirctionCoe_combined;
                    FPVector3 force = u * n * GetFrictionDir(linearV, beHitDir);
                    allFrictionForce += force;
                }

                // 对累加后的总滑动摩擦力进行计算
                // 根据速度计算摩擦力方向
                if (allFrictionForce.LengthSquared() != 0) {
                    var offsetV_friction = GetOffsetV_ByForce(-allFrictionForce, m, dt);
                    var offsetLen = offsetV_friction.Length();
                    var linearVLen = linearV.Length();
                    if (offsetLen - linearVLen > FPUtils.epsilon_friction) {
                        linearV = FPVector3.Zero;
                        UnityEngine.Debug.Log("因为摩擦力停下");
                    } else if (linearVLen - offsetLen > FPUtils.epsilon_friction) {
                        linearV += offsetV_friction;
                        UnityEngine.Debug.Log($"摩擦力   差:{linearVLen - offsetLen} ");
                    }

                }

                rb.SetLinearV(linearV);
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