using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Utils {

    public static class FPUtils {

        public static readonly FP64 rad_180 = 180 * FP64.Deg2Rad;
        public static readonly FP64 rad_90 = 90 * FP64.Deg2Rad;
        public static readonly FP64 epsilon_intersect = FP64.Epsilon;
        public static readonly FP64 epsilon_mtv = FP64.Epsilon;
        public static readonly FP64 epsilon_friction = FP64.EN1;
        public static readonly FP64 multy_penetration_rbNrb = FP64.Half;
        public static readonly FP64 multy_penetration_rbNstatic = FP64.One;

        public static bool IsNear(in FP64 v1, in FP64 v2, in FP64 epsilon) {
            return v1 <= (v2 + epsilon) && v1 >= (v2 - epsilon);
        }

        public static bool IsNear(in FPVector3 v1, in FPVector3 v2, in FP64 epsilon) {
            return IsNear(v1.x, v2.x, epsilon) && IsNear(v1.y, v2.y, epsilon) && IsNear(v1.z, v2.z, epsilon);
        }

    }

}