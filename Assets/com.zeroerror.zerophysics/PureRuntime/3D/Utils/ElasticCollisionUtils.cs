using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics {

    public static class ElasticCollisionUtils {

        public static void ApplyElasticCollision_RR(in CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            var bodyB = collision.bodyB;
            var rbA = bodyA.RB;
            var rbB = bodyB.RB;
            FPVector3 hitDirBA = collision.HitDirBA;
            FPVector3 hitDirAB = -hitDirBA;
            FP64 m1 = rbA.Mass;
            FP64 m2 = rbB.Mass;
            FPVector3 v1 = rbA.LinearV;
            FPVector3 v2 = rbB.LinearV;
            var v1_hitProj = FPVector3.Dot(v1, hitDirAB) * hitDirAB;
            var v2_hitProj = FPVector3.Dot(v2, hitDirBA) * hitDirBA;
            var v1_component = v1_hitProj + (1 + rbA.BounceCoefficient) * (v2_hitProj - v1_hitProj) / (1 + m1 / m2);
            var v2_component = v2_hitProj + (1 + rbB.BounceCoefficient) * (v1_hitProj - v2_hitProj) / (1 + m2 / m1);
            v1 = v1 - v1_hitProj + v1_component;
            v2 = v2 - v2_hitProj + v2_component;
            rbA.SetLinearV(v1);
            rbB.SetLinearV(v2);
            // Logger.Log($"动态物体碰撞 v1:{v1} v2:{v2}");
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
                var dot = FPVector3.Dot(rb.DirtyOutForce, hitDirBA);
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