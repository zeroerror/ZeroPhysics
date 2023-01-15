using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class FrictionUtils {

        public static void ApplyFriction(CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            var bodyB = collision.bodyB;
            FP64 m1 = FP64.Zero; ;
            FP64 m2 = FP64.Zero; ;
            FPVector3 v1 = FPVector3.Zero;
            FPVector3 v2 = FPVector3.Zero;
            FPVector3 hitDirBA = collision.HitDirBA;
            FP64 u = collision.FirctionCoe_combined;

            // RB & Static
            if (bodyA is Box3DRigidbody rbA) {
                ApplyFriction(rbA, u, hitDirBA, dt);
                return;
            }
            if (bodyB is Box3DRigidbody rbB) {
                ApplyFriction(rbB, u, hitDirBA, dt);
                return;
            }
            // RB & RB

        }

        static void ApplyFriction(Box3DRigidbody rb, in FP64 u, in FPVector3 beHitDir, in FP64 dt) {
            FPVector3 linearV = rb.LinearV;
            FP64 linearVLen = linearV.Length();

            // 速度和外力同一条力线上，则退出
            var cos = FPVector3.Dot(linearV.normalized, beHitDir);
            if (FPUtils.IsNear(cos, FP64.One, FP64.EN2) || FPUtils.IsNear(cos, -FP64.One, FP64.EN2)) {
                return;
            }

            FP64 n = FPVector3.Dot(rb.OutForce, -beHitDir);
            FPVector3 frictionForce = -u * n * GetFrictionDir(linearV, beHitDir);

            // 计算摩擦力
            var frictionOffsetV = ForceUtils.GetOffsetV_ByForce(frictionForce, rb.Mass, dt);
            var offsetLen = frictionOffsetV.Length();
            if (offsetLen > linearVLen - FPUtils.epsilon_friction) {
                // UnityEngine.Debug.Log($"摩擦力 停下");
                linearV = FPVector3.Zero;
            } else {
                // UnityEngine.Debug.Log($"摩擦力 减速 {frictionOffsetV}");
                linearV += frictionOffsetV;
            }

            rb.SetLinearV(linearV);
        }

        static FPVector3 GetFrictionDir(in FPVector3 linearV, in FPVector3 beHitDir) {
            var crossAxis = FPVector3.Cross(linearV, beHitDir);
            crossAxis.Normalize();
            var rot = FPQuaternion.CreateFromAxisAngle(crossAxis, FPUtils.rad_90);
            var frictionDir = rot * -beHitDir;    // 撞击方向 绕轴旋转
            return frictionDir;
        }


    }

}