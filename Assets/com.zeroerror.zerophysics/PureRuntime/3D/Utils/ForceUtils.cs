using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics {

    public static class ForceUtils {

        public static FPVector3 GetOffsetV_ByForce(in FPVector3 f, in FP64 m, in FP64 t) {
            var a = f / m;
            var offset = a * t;
            return offset;
        }

        public static void ApplyForceHitErase_RR(in CollisionModel collisionModel, in FP64 dt) {
            var rbA = collisionModel.bodyA.RB;
            var rbB = collisionModel.bodyB.RB;
            FPVector3 bHitA_Dir = collisionModel.HitDirBA;
            rbA.SetOutForce(GetErasedForce(rbA.OutForce, bHitA_Dir));
            rbB.SetOutForce(GetErasedForce(rbB.OutForce, -bHitA_Dir));
        }

        public static void ApplyForceHitErase_RS(in CollisionModel collisionModel, in FP64 dt) {
            var rb = collisionModel.bodyA.RB;
            var body = collisionModel.bodyB;
            FPVector3 bHitA_Dir = collisionModel.HitDirBA;
            FPVector3 dirtyOutForce = GetErasedForce(rb.OutForce, bHitA_Dir);
            rb.SetDirtyOutForce(dirtyOutForce);
        }

        public static FPVector3 GetErasedForce(in FPVector3 force, in FPVector3 beHitDir) {
            var force_pj = FPVector3.Dot(force, beHitDir);
            var f = force - force_pj * beHitDir;
            return f;
        }

    }

}