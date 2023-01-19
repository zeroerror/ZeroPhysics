using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class ElasticCollisionUtils {

        public static void ApplyElasticCollision_RR(in CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            var bodyB = collision.bodyB;
            var rbA = bodyA.RB;
            var rbB = bodyB.RB;
            FPVector3 hitDirBA = collision.HitDirBA;
            FP64 m1 = rbA.Mass;
            FP64 m2 = rbB.Mass;
            FPVector3 v1 = rbA.LinearV;
            FPVector3 v2 = rbB.LinearV;
            var va = v1 + (1 + rbA.BounceCoefficient) * (v2 - v1) / (1 + m1 / m2);
            var vb = v2 + (1 + rbB.BounceCoefficient) * (v1 - v2) / (1 + m2 / m1);
            rbA.SetLinearV(va);
            rbB.SetLinearV(vb);
            // UnityEngine.Debug.Log($"动态物体碰撞 va:{va} vb:{vb}");
        }

        public static void ApplyElasticCollision_RS(in CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            var bodyB = collision.bodyB;
            var rb = bodyA.RB;
            var hitDirBA = collision.HitDirBA;
            var v = rb.LinearV;
            var v_bounced = ApplyBounce(hitDirBA, rb.BounceCoefficient, v);
            // 弹力速度外力减益
            var hasBounced = !FPUtils.IsNear(v_bounced, v, FP64.EN1);
            if (hasBounced) {
                var dot = FPVector3.Dot(rb.OutForce, hitDirBA);
                if (dot < 0) {
                    var offsetV = ForceUtils.GetOffsetV_ByForce(dot * hitDirBA, rb.Mass, dt);
                    v_bounced += 2 * offsetV;
                }
            }
            rb.SetLinearV(v_bounced);
            return;
        }

        static FPVector3 ApplyBounce(in FPVector3 beHitDir, in FP64 bounceCoefficient, in FPVector3 linearV) {
            // 特殊条件规避运算
            if (beHitDir == FPVector3.Zero || linearV == FPVector3.Zero) {
                return linearV;
            }

            var bouncedVLen = -FPVector3.Dot(linearV, beHitDir);
            if (bouncedVLen <= 0) {
                return linearV;
            }

            FPVector3 bounceDir = beHitDir.normalized;
            FPVector3 bouncedV = bouncedVLen * beHitDir;
            FPVector3 offset = (1 + bounceCoefficient) * bouncedVLen * bounceDir;
            return linearV + offset;
        }

    }

}