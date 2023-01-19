using FixMath.NET;

namespace ZeroPhysics.Utils {

    public static class FPUtils {

        public static readonly FP64 rad_180 = 180 * FP64.Deg2Rad;
        public static readonly FP64 rad_90 = 90 * FP64.Deg2Rad;
        public static readonly FP64 epsilon_intersect = -FP64.EN2;
        public static readonly FP64 epsilon_mtv = FP64.EN2 + FP64.EN4;
        public static readonly FP64 multy_penetration_rr = 45 * FP64.EN2;
        public static readonly FP64 multy_penetration_rs = 99 * FP64.EN2;

        public static bool IsNear(in FP64 v1, in FP64 v2, in FP64 epsilon) {
            return v1 <= (v2 + epsilon) && v1 >= (v2 - epsilon);
        }

        public static bool IsNear(in FPVector3 v1, in FPVector3 v2, in FP64 epsilon) {
            return IsNear(v1.x, v2.x, epsilon) && IsNear(v1.y, v2.y, epsilon) && IsNear(v1.z, v2.z, epsilon);
        }

    }

}