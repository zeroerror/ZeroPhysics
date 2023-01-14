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

            if (bodyA is Box3DRigidbody rb_a) {
                var v = rb_a.LinearV;
                var v_bounced = ApplyBounce(bHitA_Dir, rb_a.BounceCoefficient, v);
                var dot = FPVector3.Dot(rb_a.OutForce, v_bounced);
                if (dot < 0) {
                    var offsetV = GetOffsetV_ByForce(rb_a.OutForce, rb_a.Mass, dt);
                    v_bounced += offsetV;
                }
                rb_a.SetLinearV(v_bounced);
                UnityEngine.Debug.Log($"dot:{dot}");
                UnityEngine.Debug.Log($"v_bounced:{v_bounced}");
                return;
            }
            if (bodyB is Box3DRigidbody rb_b) {
                var v = rb_b.LinearV;
                var v_bounced = ApplyBounce(bHitA_Dir, rb_b.BounceCoefficient, v);
                rb_b.SetLinearV(v_bounced);
                UnityEngine.Debug.Log($"v_bounced:{v_bounced}");
                return;
            }

            if (bodyA is Box3DRigidbody rbA && bodyB is Box3DRigidbody rbB) {
                m1 = rbA.Mass;
                m2 = rbB.Mass;
                v1 = ((m1 - m2) * v1 + 2 * m2 * v2 / (m1 + m2)) / (m1 + m2);
                v2 = ((m2 - m1) * v2 + 2 * m1 * v1 / (m1 + m2)) / (m1 + m2);
                rbA.SetLinearV(v1);
                rbB.SetLinearV(v2);
                UnityEngine.Debug.Log($"v1:{v1} v2:{v2}");
            }
        }

        public static FPVector3 GetOffsetV_ByForce(in FPVector3 f, in FP64 m, in FP64 t) {
            var a = f / m;
            var offset = a * t;
            return offset;
        }

        static FPVector3 ApplyBounce(in FPVector3 beHitDir, in FP64 bounceCoefficient, in FPVector3 linearV) {
            if (beHitDir == FPVector3.Zero || linearV == FPVector3.Zero) {
                return linearV;
            }

            var bouncedVLen = FPVector3.Dot(linearV, beHitDir);
            FPVector3 bouncedV = bouncedVLen * beHitDir;
            FPVector3 linearV_normalized = linearV.normalized;
            FPVector3 bounceDir = bouncedV.normalized;
            FPVector3 offset = (1 + bounceCoefficient) * bouncedVLen * bounceDir;
            return linearV + offset;
        }

    }

}