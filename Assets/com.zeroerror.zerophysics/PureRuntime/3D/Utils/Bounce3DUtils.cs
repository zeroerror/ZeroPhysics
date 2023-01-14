using FixMath.NET;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class Bounce3DUtils {

        static readonly FP64 Bounce_Epsilon = FP64.EN1;

        public static void ApplyBounce(in FPVector3 beHitDir, in FP64 bounceCoefficient, ref FPVector3 linearV) {
            if (beHitDir == FPVector3.Zero || linearV == FPVector3.Zero) {
                return;
            }

            var bouncedVLen = FPVector3.Dot(linearV, beHitDir);
            FPVector3 bouncedV = bouncedVLen * beHitDir;
            FPVector3 linearV_normalized = linearV.normalized;
            FPVector3 bounceDir = bouncedV.normalized;
            FPVector3 offset = (1 + bounceCoefficient) * bouncedVLen * bounceDir;
            linearV += offset;
        }

    }

}