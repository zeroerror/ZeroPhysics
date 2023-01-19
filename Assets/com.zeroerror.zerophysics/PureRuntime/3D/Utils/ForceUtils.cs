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

        public static void ApplyForceHitErase_RR(in CollisionModel collisionModel, in FP64 dt) {
            var rbA = collisionModel.bodyA.RB;
            var rbB = collisionModel.bodyB.RB;
            FP64 m1 = FP64.Zero; ;
            FP64 m2 = FP64.Zero; ;
            FPVector3 v1 = FPVector3.Zero;
            FPVector3 v2 = FPVector3.Zero;
            FPVector3 bHitA_Dir = collisionModel.HitDirBA;

            rbA.SetOutForce(GetErasedForce(rbA.OutForce, bHitA_Dir));
            rbB.SetOutForce(GetErasedForce(rbB.OutForce, -bHitA_Dir));
        }

        static FPVector3 GetErasedForce(in FPVector3 force, in FPVector3 beHitDir) {
            var force_pj = FPVector3.Dot(force, beHitDir);
            var f = force - force_pj * beHitDir;
            return f;
        }

    }

}