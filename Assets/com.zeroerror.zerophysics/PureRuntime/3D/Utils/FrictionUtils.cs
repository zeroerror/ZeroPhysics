using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class FrictionUtils {

        public static void ApplyFriction_RS(in CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            FPVector3 hitDirBA = collision.HitDirBA;
            FP64 u = collision.FirctionCoe_combined;
            var rbA = bodyA.RB;
            ApplyFriction(rbA, u, hitDirBA, dt);
        }

        public static void ApplyFriction_RR(in CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            var bodyB = collision.bodyB;
            FPVector3 hitDirBA = collision.HitDirBA;
            FP64 u = collision.FirctionCoe_combined;
            var rbA = bodyA.RB;
            var rbB = bodyB.RB;
            ApplyFriction(rbA, u, hitDirBA, dt);
            ApplyFriction(rbB, u, hitDirBA, dt);
        }

        static void ApplyFriction(Rigidbody3D rb, in FP64 u, in FPVector3 beHitDir, in FP64 dt) {
            if (beHitDir == FPVector3.Zero) {
                return;
            }

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
            if (frictionOffsetV == FPVector3.Zero) {
                return;
            }
            linearV += frictionOffsetV;
            var cosFlag = FPVector3.Dot(frictionOffsetV, linearV);
            if (cosFlag > 0) {
                linearV = FPVector3.Zero;
                UnityEngine.Debug.Log($"摩擦力 停下 ");
            } else {
                UnityEngine.Debug.Log($"摩擦力 减速 {frictionOffsetV}");
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