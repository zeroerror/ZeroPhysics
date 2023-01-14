using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Utils {

    public static class FPUtils {

        public static readonly FP64 rad_180 = 180 * FP64.Deg2Rad;
        public static readonly FP64 rad_90 = 90 * FP64.Deg2Rad;
        public static readonly FP64 epsilon_intersect = 0;

        public static bool IsNear(in FP64 v1, in FP64 v2, in FP64 epsilon) {
            return v1 < (v2 + epsilon) && v1 > (v2 - epsilon);
        }

    }

}