using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class ForceUtils {

        public static FPVector3 GetOffsetV_ByForce(in FPVector3 f, in FP64 m, in FP64 t) {
            var a = f / m;
            var offset = a * t;
            return offset;
        }

        public static void ApplyForceHitErase(in CollisionModel collisionModel, in FP64 dt) {
            var bodyA = collisionModel.bodyA;
            var bodyB = collisionModel.bodyB;
            FP64 m1 = FP64.Zero; ;
            FP64 m2 = FP64.Zero; ;
            FPVector3 v1 = FPVector3.Zero;
            FPVector3 v2 = FPVector3.Zero;
            FPVector3 bHitA_Dir = collisionModel.HitDirBA;

            if (bodyA is Box3DRigidbody rb_a) {
                var outForce = GetErasedForce(rb_a.OutForce, bHitA_Dir);
            }
            if (bodyB is Box3DRigidbody rb_b) {
                var outForce = GetErasedForce(rb_b.OutForce, -bHitA_Dir);
            }
        }

        static FPVector3 GetErasedForce(in FPVector3 force, in FPVector3 beHitDir) {
            var force_pj = FPVector3.Dot(force, beHitDir);
            var f = force - force_pj * beHitDir;
            return f;
        }

    }

}