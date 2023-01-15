using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class ElasticCollisionUtils {

        static readonly FP64 Bounce_Epsilon = FP64.EN1;

        public static void ApplyElasticCollision(CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            var bodyB = collision.bodyB;
            FPVector3 hitDirBA = collision.HitDirBA;

            // RB & RB
            if (bodyA is Box3DRigidbody rbA && bodyB is Box3DRigidbody rbB) {
                // 根据动量守恒和动能守恒公式计算 
                FP64 m1 = rbA.Mass;
                FP64 m2 = rbB.Mass;
                FPVector3 v1 = rbA.LinearV;

                // 特殊条件规避运算
                if (FPVector3.Dot(v1, hitDirBA) >= 0) {
                    return;
                }

                FPVector3 v2 = rbB.LinearV;
                // var m1Addm2 = m1 + m2;
                // var m1Subm2 = m1 - m2;
                // var va = (m1Subm2 * v1 + 2 * m2 * v2) / m1Addm2;
                // var vb = (-m1Subm2 * v2 + 2 * m1 * v1) / m1Addm2;
                var va = v1 + (1 + rbA.BounceCoefficient) * (v2 - v1) / (1 + m1 / m2);
                var vb = v2 + (1 + rbB.BounceCoefficient) * (v1 - v2) / (1 + m2 / m1);
                bool hasBouncedA = va != v1;
                bool hasBouncedB = vb != v2;

                // 弹力速度外力减益
                if (hasBouncedA) {
                    var dot = FPVector3.Dot(rbA.OutForce, hitDirBA);
                    if (dot < 0) {
                        var offsetV = ForceUtils.GetOffsetV_ByForce(dot * hitDirBA, m1, dt);
                        va += offsetV;
                    }
                }
                if (hasBouncedB) {
                    var dot = FPVector3.Dot(rbB.OutForce, -hitDirBA);
                    if (dot < 0) {
                        var offsetV = ForceUtils.GetOffsetV_ByForce(dot * -hitDirBA, m2, dt);
                        vb += offsetV;
                    }
                }

                rbA.SetLinearV(va);
                rbB.SetLinearV(vb);
                // UnityEngine.Debug.Log($"动态物体碰撞 va:{va} vb:{vb}");
            }

            // RB & Static
            if (bodyA is Box3DRigidbody rb_a) {
                var v = rb_a.LinearV;
                var v_bounced = ApplyBounce(hitDirBA, rb_a.BounceCoefficient, v);
                // 弹力速度外力减益
                bool hasBounced = v_bounced != v;
                if (hasBounced) {
                    var dot = FPVector3.Dot(rb_a.OutForce, hitDirBA);
                    if (dot < 0) {
                        var offsetV = ForceUtils.GetOffsetV_ByForce(dot * hitDirBA, rb_a.Mass, dt);
                        v_bounced += offsetV;
                    }
                }

                rb_a.SetLinearV(v_bounced);
                return;
            }
            if (bodyB is Box3DRigidbody rb_b) {
                var v = rb_b.LinearV;
                var v_bounced = ApplyBounce(-hitDirBA, rb_b.BounceCoefficient, v);
                // 弹力速度外力减益
                bool hasBounced = v_bounced != v;
                if (hasBounced) {
                    var dot = FPVector3.Dot(rb_b.OutForce, -hitDirBA);
                    if (dot < 0) {
                        var offsetV = ForceUtils.GetOffsetV_ByForce(rb_b.OutForce, rb_b.Mass, dt);
                        v_bounced += offsetV;
                        UnityEngine.Debug.Log($"offsetV:{offsetV} v_bounced:{v_bounced}");
                    }
                }

                rb_b.SetLinearV(v_bounced);
                return;
            }
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