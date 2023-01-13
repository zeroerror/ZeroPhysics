using FixMath.NET;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class Bounce3DUtils {

        static readonly FP64 Bounce_Epsilon = FP64.EN1;

        public static void ApplyBounce(in FPVector3 beHitDir, in FP64 bounceCoefficient, ref FPVector3 linearV) {
            if (beHitDir == FPVector3.Zero || linearV == FPVector3.Zero) {
                return;
            }

            var v_proj = FPVector3.Dot(linearV, beHitDir);
            FPVector3 bouncedV = -v_proj * beHitDir;
            FPVector3 linearV_normalized = linearV.normalized;
            FP64 cosv = FPVector3.Dot(linearV_normalized, beHitDir);

            if (cosv == 1) {
                // 速度和撞击速度相同
                linearV -= (1 + bounceCoefficient) * bouncedV;
                return;
            }

            if (cosv == -1) {
                // 速度和撞击速度相反
                UnityEngine.Debug.Log($"linearV_normalized{linearV_normalized} beHitDir:{beHitDir} bouncedV:{bouncedV}");
                linearV += (1 + bounceCoefficient) * bouncedV;
                return;
            }

            // 速度和撞击呈斜角
            FP64 sinv = -cosv;
            FP64 bouncedVLen = bouncedV.Length();
            bouncedVLen *= sinv;
            FPVector3 offset = (1 + bounceCoefficient) * bouncedVLen * bouncedV.normalized;
            linearV += offset;
        }

    }

}