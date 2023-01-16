using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class Intersect3DUtils {

        public static bool HasCollision(Box3D box1, Box3D box2) {
            if (box1.GetBoxType() == BoxType.OBB || box2.GetBoxType() == BoxType.OBB) return HasCollision_OBB(box1.GetModel(), box2.GetModel());
            else return HasCollision_AABB(box1.GetModel(), box2.GetModel());
        }

        public static bool HasCollision_AABB(in Box3DModel box1, in Box3DModel box2) {
            var min1 = box1.Min;
            var max1 = box1.Max;
            var min2 = box2.Min;
            var max2 = box2.Max;

            return HasCollision_AABB(min1, max1, min2, max2);
        }

        public static bool HasCollision_AABB(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            bool hasIntersectX = HasIntersectXX(min1, max1, min2, max2);
            bool hasIntersectY = HasIntersectYY(min1, max1, min2, max2);
            bool hasIntersectZ = HasIntersectZZ(min1, max1, min2, max2);
            return hasIntersectX && hasIntersectY && hasIntersectZ;
        }

        public static bool HasCollision_OBB(in Box3DModel box1, in Box3DModel box2) {
            // - 6 Axis
            if (!HasIntersectsWithAxisX_LeftBox(box1, box2)) {
                return false;
            }
            if (!HasIntersectsWithAxisY_LeftBox(box1, box2)) {
                return false;
            }
            if (!HasIntersectsWithAxisZ_LeftBox(box1, box2)) {
                return false;
            }
            if (!HasIntersectsWithAxisX_LeftBox(box2, box1)) {
                return false;
            }
            if (!HasIntersectsWithAxisY_LeftBox(box2, box1)) {
                return false;
            }
            if (!HasIntersectsWithAxisZ_LeftBox(box2, box1)) {
                return false;
            }

            // - 9 Axis
            Axis3D axis = new Axis3D();
            var dirX1 = box1.GetXDir();
            var dirY1 = box1.GetYDir();
            var dirZ1 = box1.GetZDir();
            var dirX2 = box2.GetXDir();
            var dirY2 = box2.GetYDir();
            var dirZ2 = box2.GetZDir();
            var min1 = box1.Min;
            var max1 = box1.Max;
            var min2 = box2.Min;
            var max2 = box2.Max;

            // - x Cross x
            if (!FPUtils.IsNear(dirX1, dirX2, FP64.EN4) && !FPUtils.IsNear(dirX1, -dirX2, FP64.EN4)) {
                axis.dir = FPVector3.Cross(dirX1, dirX2);
                if (!HasIntersects_WithAxis(box1, box2, axis)) {
                    UnityEngine.Debug.Log($" dirX1{dirX1} dirX2{dirX2}");
                    return false;
                }
            }

            // - x Cross y
            if (!FPUtils.IsNear(dirX1, dirY2, FP64.EN4) && !FPUtils.IsNear(dirX1, -dirY2, FP64.EN4)) {
                axis.dir = FPVector3.Cross(dirX1, dirY2);
                if (!HasIntersects_WithAxis(box1, box2, axis)) {
                    UnityEngine.Debug.Log($"axis.dir {axis.dir}");
                    return false;
                }
            }

            // - x Cross z
            if (!FPUtils.IsNear(dirX1, dirZ2, FP64.EN4) && !FPUtils.IsNear(dirX1, -dirZ2, FP64.EN4)) {
                axis.dir = FPVector3.Cross(dirX1, dirZ2);
                if (!HasIntersects_WithAxis(box1, box2, axis)) {
                    UnityEngine.Debug.Log($"axis.dir {axis.dir}");
                    return false;
                }
            }

            // - y Cross x
            if (!FPUtils.IsNear(dirY1, dirX2, FP64.EN4) && !FPUtils.IsNear(dirY1, -dirX2, FP64.EN4)) {
                axis.dir = FPVector3.Cross(dirY1, dirX2);
                if (!HasIntersects_WithAxis(box1, box2, axis)) {
                    UnityEngine.Debug.Log($"axis.dir {axis.dir}");
                    return false;
                }
            }

            // - y Cross y
            if (!FPUtils.IsNear(dirY1, dirY2, FP64.EN4) && !FPUtils.IsNear(dirY1, -dirY2, FP64.EN4)) {
                axis.dir = FPVector3.Cross(dirY1, dirY2);
                if (!HasIntersects_WithAxis(box1, box2, axis)) {
                    UnityEngine.Debug.Log($"axis.dir {axis.dir}");
                    return false;
                }
            }

            // - y Cross z
            if (!FPUtils.IsNear(dirY1, dirZ2, FP64.EN4) && !FPUtils.IsNear(dirY1, -dirZ2, FP64.EN4)) {
                axis.dir = FPVector3.Cross(dirY1, dirZ2);
                if (!HasIntersects_WithAxis(box1, box2, axis)) {
                    UnityEngine.Debug.Log($"axis.dir {axis.dir}");
                    return false;
                }
            }

            // - z Cross x
            if (!FPUtils.IsNear(dirZ1, dirX2, FP64.EN4) && !FPUtils.IsNear(dirZ1, -dirX2, FP64.EN4)) {
                axis.dir = FPVector3.Cross(dirZ1, dirX2);
                if (!HasIntersects_WithAxis(box1, box2, axis)) {
                    UnityEngine.Debug.Log($"axis.dir {axis.dir}");
                    return false;
                }
            }

            // - z Cross y
            if (!FPUtils.IsNear(dirZ1, dirY2, FP64.EN4) && !FPUtils.IsNear(dirZ1, -dirY2, FP64.EN4)) {
                axis.dir = FPVector3.Cross(dirZ1, dirY2);
                if (!HasIntersects_WithAxis(box1, box2, axis)) {
                    UnityEngine.Debug.Log($"axis.dir {axis.dir}");
                    return false;
                }
            }

            // - z Cross z
            if (!FPUtils.IsNear(dirZ1, dirZ2, FP64.EN4) && !FPUtils.IsNear(dirZ1, -dirZ2, FP64.EN4)) {
                if ((dirZ1 != dirZ2 && dirZ1 != -dirZ2)) {
                    axis.dir = FPVector3.Cross(dirZ1, dirZ2);
                    if (!HasIntersects_WithAxis(box1, box2, axis)) {
                        UnityEngine.Debug.Log($"axis.dir {axis.dir}");
                        return false;
                    }
                }
            }

            return true;
        }

        internal static bool HasIntersectsWithAxisX_LeftBox(in Box3DModel box1, in Box3DModel box2) {
            var b1AxisX = box1.GetAxisX();
            var box1_projSub = box1.GetAxisX_SelfProjectionSub();
            var box2_projSub = Projection3DUtils.GetProjectionSub(box2, b1AxisX);
            return OBBHasIntersects(box1_projSub, box2_projSub);
        }

        internal static bool HasIntersectsWithAxisY_LeftBox(in Box3DModel box1, in Box3DModel box2) {
            var b1AxisY = box1.GetAxisY();
            var box1_projSub = box1.GetAxisY_SelfProjectionSub();
            var box2_projSub = Projection3DUtils.GetProjectionSub(box2, b1AxisY);
            return OBBHasIntersects(box1_projSub, box2_projSub);
        }

        internal static bool HasIntersectsWithAxisZ_LeftBox(in Box3DModel box1, in Box3DModel box2) {
            var b1AxisZ = box1.GetAxisZ();
            var box1_projSub = box1.GetAxisZ_SelfProjectionSub();
            var box2_projSub = Projection3DUtils.GetProjectionSub(box2, b1AxisZ);
            return OBBHasIntersects(box1_projSub, box2_projSub);
        }

        internal static bool HasIntersects_WithAxis(in Box3DModel model1, in Box3DModel model2, in Axis3D axis) {
            var box1_projSub = Projection3DUtils.GetProjectionSub(model1, axis);
            var box2_projSub = Projection3DUtils.GetProjectionSub(model2, axis);
            return OBBHasIntersects(box1_projSub, box2_projSub);
        }

        internal static bool IsInsideBox(in Box3DModel model, FPVector3 point, in FP64 epsilon) {
            var px = point.x;
            var py = point.y;
            var pz = point.z;
            if (model.GetBoxType() == BoxType.AABB) {
                var min = model.Min;
                var max = model.Max;
                return px >= (min.x - epsilon) && px <= (max.x + epsilon)
                && py <= (min.y + epsilon) && py >= (max.y - epsilon)
                && pz <= (min.z + epsilon) && pz >= (max.z - epsilon);
            } else {
                point -= model.Center;
                // - Axis x
                var axis = model.GetAxisX();
                var pj = FPVector3.Dot(point, axis.dir);
                var pjSub = model.GetAxisX_SelfProjectionSub();
                if (pj < (pjSub.x - epsilon) || pj > (pjSub.y + epsilon)) return false;

                // - Axis y
                axis = model.GetAxisY();
                pj = FPVector3.Dot(point, axis.dir);
                pjSub = model.GetAxisY_SelfProjectionSub();
                if (pj < (pjSub.x - epsilon) || pj > (pjSub.y + epsilon)) return false;

                // - Axis z
                axis = model.GetAxisZ();
                pj = FPVector3.Dot(point, axis.dir);
                pjSub = model.GetAxisZ_SelfProjectionSub();
                if (pj < (pjSub.x - epsilon) || pj > (pjSub.y + epsilon)) return false;

                return true;
            }
        }

        public static bool HasIntersects(in FPVector2 sub1, in FPVector2 sub2, in FP64 epsilon) {
            return !(sub1.y < sub2.x - epsilon || sub2.y < sub1.x - epsilon);
        }

        public static bool OBBHasIntersects(in FPVector2 sub1, in FPVector2 sub2) {
            return !(sub1.y < sub2.x - FPUtils.epsilon_intersect || sub2.y < sub1.x - FPUtils.epsilon_intersect);
        }

        #region [Intersect]

        static bool HasIntersectXX(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff1 = min1.x - max2.x;
            var diff2 = min2.x - max1.x;
            bool hasIntersect = !(diff1 > FPUtils.epsilon_intersect || diff2 > FPUtils.epsilon_intersect);
            return hasIntersect;
        }

        static bool HasIntersectXY(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff1 = min1.x - max2.y;
            var diff2 = min2.y - max1.x;
            bool hasIntersect = !(diff1 > FPUtils.epsilon_intersect || diff2 > FPUtils.epsilon_intersect);
            return hasIntersect;
        }

        static bool HasIntersectXZ(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff1 = min1.x - max2.z;
            var diff2 = min2.z - max1.x;
            bool hasIntersect = !(diff1 > FPUtils.epsilon_intersect || diff2 > FPUtils.epsilon_intersect);
            return hasIntersect;
        }

        static bool HasIntersectYX(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff1 = min1.y - max2.x;
            var diff2 = min2.x - max1.y;
            bool hasIntersect = !(diff1 > FPUtils.epsilon_intersect || diff2 > FPUtils.epsilon_intersect);
            return hasIntersect;
        }

        static bool HasIntersectYY(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff_y1 = min1.y - max2.y;
            var diff_y2 = min2.y - max1.y;
            bool hasIntersectY = !(diff_y1 > FPUtils.epsilon_intersect || diff_y2 > FPUtils.epsilon_intersect);
            return hasIntersectY;
        }

        static bool HasIntersectYZ(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff1 = min1.y - max2.z;
            var diff2 = min2.z - max1.y;
            bool hasIntersect = !(diff1 > FPUtils.epsilon_intersect || diff2 > FPUtils.epsilon_intersect);
            return hasIntersect;
        }

        static bool HasIntersectZX(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff1 = min1.z - max2.x;
            var diff2 = min2.x - max1.z;
            bool hasIntersect = !(diff1 > FPUtils.epsilon_intersect || diff2 > FPUtils.epsilon_intersect);
            return hasIntersect;
        }

        static bool HasIntersectZY(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff1 = min1.z - max2.y;
            var diff2 = min2.y - max1.z;
            bool hasIntersect = !(diff1 > FPUtils.epsilon_intersect || diff2 > FPUtils.epsilon_intersect);
            return hasIntersect;
        }

        static bool HasIntersectZZ(in FPVector3 min1, in FPVector3 max1, in FPVector3 min2, in FPVector3 max2) {
            var diff1 = min1.z - max2.z;
            var diff2 = min2.z - max1.z;
            bool hasIntersect = !(diff1 > FPUtils.epsilon_intersect || diff2 > FPUtils.epsilon_intersect);
            return hasIntersect;
        }

        #endregion

    }

}
