using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class ElasticCollisionUtils {

        static readonly FP64 Bounce_Epsilon = FP64.EN1;

        public static void ApplyForceHitErase(CollisionModel collisionModel, in FP64 dt) {
            var bodyA = collisionModel.bodyA;
            var bodyB = collisionModel.bodyB;
            FP64 m1 = FP64.Zero; ;
            FP64 m2 = FP64.Zero; ;
            FPVector3 v1 = FPVector3.Zero;
            FPVector3 v2 = FPVector3.Zero;
            FPVector3 bHitA_Dir = collisionModel.BHitA_Dir;

            if (bodyA is Box3DRigidbody rb_a) {
                var outForce = GetErasedForce(rb_a.OutForce, bHitA_Dir);
                rb_a.SetOutForce(outForce);
                UnityEngine.Debug.Log($"outForce1 {outForce}");
            }
            if (bodyB is Box3DRigidbody rb_b) {
                var outForce = GetErasedForce(rb_b.OutForce, -bHitA_Dir);
                rb_b.SetOutForce(outForce);
                UnityEngine.Debug.Log($"outForce2 {outForce}");
            }
        }

        static FPVector3 GetErasedForce(in FPVector3 force, in FPVector3 beHitDir) {
            var force_pj = FPVector3.Dot(force, beHitDir);
            var f = force - force_pj * beHitDir;
            return f;
        }

        public static void ApplyElasticCollision(CollisionModel collisionModel, in FP64 dt) {
            var bodyA = collisionModel.bodyA;
            var bodyB = collisionModel.bodyB;
            FP64 m1 = FP64.Zero; ;
            FP64 m2 = FP64.Zero; ;
            FPVector3 v1 = FPVector3.Zero;
            FPVector3 v2 = FPVector3.Zero;
            FPVector3 bHitA_Dir = collisionModel.BHitA_Dir;

            // RB & RB
            // 根据动量守恒公式计算
            if (bodyA is Box3DRigidbody rbA && bodyB is Box3DRigidbody rbB) {
                m1 = rbA.Mass;
                m2 = rbB.Mass;
                v1 = ((m1 - m2) * v1 + 2 * m2 * v2 / (m1 + m2)) / (m1 + m2);
                v2 = ((m2 - m1) * v2 + 2 * m1 * v1 / (m1 + m2)) / (m1 + m2);
                rbA.SetLinearV(v1);
                rbB.SetLinearV(v2);
                UnityEngine.Debug.Log($"v1:{v1} v2:{v2}");
            }

            // RB & Static
            if (bodyA is Box3DRigidbody rb_a) {
                var v = rb_a.LinearV;
                var v_bounced = ApplyBounce(bHitA_Dir, rb_a.BounceCoefficient, v);
                var v_bounced_len = v_bounced.Length();
                bool hasBounced = v_bounced != v;
                if (hasBounced && v_bounced_len < rb_a.BounceCoefficient * FPUtils.epsilon_bounce) {
                    v_bounced = FPVector3.Zero;
                }
                if (hasBounced) {
                    var dot = FPVector3.Dot(rb_a.OutForce, bHitA_Dir);
                    if (dot < 0) {
                        var offsetV = GetOffsetV_ByForce(dot * bHitA_Dir, rb_a.Mass, dt);
                        v_bounced += offsetV;
                    }
                }

                rb_a.SetLinearV(v_bounced);
                return;
            }
            if (bodyB is Box3DRigidbody rb_b) {
                var v = rb_b.LinearV;
                var aHitB_Dir = -bHitA_Dir;
                var v_bounced = ApplyBounce(aHitB_Dir, rb_b.BounceCoefficient, v);
                var v_bounced_len = v_bounced.Length();
                bool hasBounced = v_bounced != v;
                if (hasBounced && v_bounced_len < rb_b.BounceCoefficient * FPUtils.epsilon_bounce) {
                    v_bounced = FPVector3.Zero;
                }
                if (hasBounced) {
                    var dot = FPVector3.Dot(rb_b.OutForce, aHitB_Dir);
                    if (dot < 0) {
                        var offsetV = GetOffsetV_ByForce(rb_b.OutForce, rb_b.Mass, dt);
                        v_bounced += offsetV;
                    }
                }

                rb_b.SetLinearV(v_bounced);
                return;
            }

        }

        public static FPVector3 GetOffsetV_ByForce(in FPVector3 f, in FP64 m, in FP64 t) {
            var a = f / m;
            var offset = a * t;
            return offset;
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