using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public static class Intersect3DUtils
    {

        #region [Penetration]

        public static void PenetrationCorrection(Box3D box1, FP64 mtvCoe1, Box3D box2, FP64 mtvCoe2)
        {
            var mtv = GetMTV(box1, box2);
            var mtv1 = mtv * mtvCoe1;
            var mtv2 = -mtv * mtvCoe2;
            var newCenter1 = box1.Center + mtv1;
            var newCenter2 = box2.Center + mtv2;
            box1.SetCenter(newCenter1);
            box2.SetCenter(newCenter2);
        }

        public static FPVector3 GetMTV(Box3D box1, Box3D box2)
        {
            FP64 len_min = FP64.MaxValue;
            FPVector3 dir = FPVector3.Zero;

            // 针对Box，求 3 + 3 = 6 个面的法向量上的投影，即12次投影计算

            var model1 = box1.GetModel();
            var model2 = box2.GetModel();
            var axis = model1.GetAxisX();
            var pjSub1 = model1.GetAxisX_SelfProjectionSub();
            var pjSub2 = Projection3DUtils.GetProjectionSub(model2, axis);
            UpdateMTV(ref len_min, ref dir, axis, pjSub1, pjSub2);

            axis = model1.GetAxisY();
            pjSub1 = model1.GetAxisY_SelfProjectionSub();
            pjSub2 = Projection3DUtils.GetProjectionSub(model2, axis);
            UpdateMTV(ref len_min, ref dir, axis, pjSub1, pjSub2);

            axis = model1.GetAxisZ();
            pjSub1 = model1.GetAxisZ_SelfProjectionSub();
            pjSub2 = Projection3DUtils.GetProjectionSub(model2, axis);
            UpdateMTV(ref len_min, ref dir, axis, pjSub1, pjSub2);

            bool isBox1Aixs = dir != FPVector3.Zero;

            axis = model2.GetAxisX();
            pjSub2 = model2.GetAxisX_SelfProjectionSub();
            pjSub1 = Projection3DUtils.GetProjectionSub(model1, axis);
            UpdateMTV(ref len_min, ref dir, axis, pjSub1, pjSub2);

            axis = model2.GetAxisY();
            pjSub2 = model2.GetAxisY_SelfProjectionSub();
            pjSub1 = Projection3DUtils.GetProjectionSub(model1, axis);
            UpdateMTV(ref len_min, ref dir, axis, pjSub1, pjSub2);

            axis = model2.GetAxisZ();
            pjSub2 = model2.GetAxisZ_SelfProjectionSub();
            pjSub1 = Projection3DUtils.GetProjectionSub(model1, axis);
            UpdateMTV(ref len_min, ref dir, axis, pjSub1, pjSub2);

            if (!isBox1Aixs) dir = -dir;

            return len_min * dir;
        }

        static void UpdateMTV(ref FP64 len_min, ref FPVector3 dir, Axis3D axis, FPVector2 pjSub1, FPVector2 pjSub2)
        {
            var l1 = pjSub2.y - pjSub1.x;
            var l2 = pjSub1.y - pjSub2.x;
            var lm = FP64.Min(l1, l2);
            if (lm < len_min)
            {
                len_min = lm;
                dir = l1 < l2 ? axis.dir : -axis.dir;
            }
        }

        #endregion

        public static bool HasCollision(Box3D box1, Box3D box2)
        {
            if (box1.GetBoxType() == BoxType.OBB || box2.GetBoxType() == BoxType.OBB) return HasCollision_OBB(box1.GetModel(), box2.GetModel());
            else return HasCollision_AABB(box1.GetModel(), box2.GetModel());
        }

        internal static bool HasCollision_AABB(Box3DModel box1, Box3DModel box2)
        {
            var min1 = box1.Min;
            var max1 = box1.Max;
            var min2 = box2.Min;
            var max2 = box2.Max;

            // - Axis x
            var diff_x1 = min1.x - max2.x;
            var diff_x2 = min2.x - max1.x;
            bool hasCollisionX = !(diff_x1 > 0 || diff_x2 > 0);

            // - Axis y
            var diff_y1 = min1.y - max2.y;
            var diff_y2 = min2.y - max1.y;
            bool hasCollisionY = !(diff_y1 > 0 || diff_y2 > 0);

            // - Axis y
            var diff_z1 = min1.z - max2.z;
            var diff_z2 = min2.z - max1.z;
            bool hasCollisionZ = !(diff_z1 > 0 || diff_z2 > 0);

            return hasCollisionX && hasCollisionY && hasCollisionZ;
        }

        public static bool HasCollision_AABB(FPVector3 maxPos1, FPVector3 minPos1, FPVector3 maxPos2, FPVector3 minPos2)
        {

            // - Axis x
            var diff_x1 = maxPos1.x - minPos2.x;
            var diff_x2 = minPos1.x - maxPos2.x;
            bool hasCollisionX = (diff_x1 < 0 && diff_x2 > 0) || diff_x1 == 0 || diff_x2 == 0;

            // - Axis y
            var diff_y1 = maxPos1.y - minPos2.y;
            var diff_y2 = minPos1.y - maxPos2.y;
            bool hasCollisionY = (diff_y1 > 0 && diff_y2 < 0) || diff_y1 == 0 || diff_y2 == 0;

            // - Axis y
            var diff_z1 = maxPos1.z - minPos2.z;
            var diff_z2 = minPos1.z - maxPos2.z;
            bool hasCollisionZ = (diff_z1 > 0 && diff_z2 < 0) || diff_z1 == 0 || diff_z2 == 0;

            return hasCollisionX && hasCollisionY && hasCollisionZ;
        }

        internal static bool HasCollision_OBB(Box3DModel box1, Box3DModel box2)
        {
            // - 6 Axis
            if (!HasIntersectsWithAxisX_LeftBox(box1, box2)) return false;
            if (!HasIntersectsWithAxisY_LeftBox(box1, box2)) return false;
            if (!HasIntersectsWithAxisZ_LeftBox(box1, box2)) return false;
            if (!HasIntersectsWithAxisX_LeftBox(box2, box1)) return false;
            if (!HasIntersectsWithAxisY_LeftBox(box2, box1)) return false;
            if (!HasIntersectsWithAxisZ_LeftBox(box2, box1)) return false;

            // - 9 Axis
            Axis3D axis = new Axis3D();
            var b1AxisX = box1.GetAxisX();
            var b1AxisY = box1.GetAxisY();
            var b1AxisZ = box1.GetAxisZ();
            var b2AxisX = box2.GetAxisX();
            var b2AxisY = box2.GetAxisY();
            var b2AxisZ = box2.GetAxisZ();
            // - x Cross x
            axis.dir = FPVector3.Cross(b1AxisX.dir, b2AxisX.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;
            // - x Cross y
            axis.dir = FPVector3.Cross(b1AxisX.dir, b2AxisY.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;
            // - x Cross z
            axis.dir = FPVector3.Cross(b1AxisX.dir, b2AxisZ.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;
            // - y Cross x
            axis.dir = FPVector3.Cross(b1AxisY.dir, b2AxisX.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;
            // - y Cross y
            axis.dir = FPVector3.Cross(b1AxisY.dir, b2AxisY.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;
            // - y Cross z
            axis.dir = FPVector3.Cross(b1AxisY.dir, b2AxisZ.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;
            // - z Cross x
            axis.dir = FPVector3.Cross(b1AxisZ.dir, b2AxisX.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;
            // - z Cross y
            axis.dir = FPVector3.Cross(b1AxisZ.dir, b2AxisY.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;
            // - z Cross z
            axis.dir = FPVector3.Cross(b1AxisZ.dir, b2AxisZ.dir);
            if (!HasIntersects_WithAxis(box1, box2, axis)) return false;

            return true;
        }

        internal static bool HasIntersectsWithAxisX_LeftBox(Box3DModel box1, Box3DModel box2)
        {
            var b1AxisX = box1.GetAxisX();
            var box1_projSub = box1.GetAxisX_SelfProjectionSub();
            var box2_projSub = Projection3DUtils.GetProjectionSub(box2, b1AxisX);
            return HasIntersects(box1_projSub, box2_projSub);
        }

        internal static bool HasIntersectsWithAxisY_LeftBox(Box3DModel box1, Box3DModel box2)
        {
            var b1AxisY = box1.GetAxisY();
            var box1_projSub = box1.GetAxisY_SelfProjectionSub();
            var box2_projSub = Projection3DUtils.GetProjectionSub(box2, b1AxisY);
            return HasIntersects(box1_projSub, box2_projSub);
        }

        internal static bool HasIntersectsWithAxisZ_LeftBox(Box3DModel box1, Box3DModel box2)
        {
            var b1AxisZ = box1.GetAxisZ();
            var box1_projSub = box1.GetAxisZ_SelfProjectionSub();
            var box2_projSub = Projection3DUtils.GetProjectionSub(box2, b1AxisZ);
            return HasIntersects(box1_projSub, box2_projSub);
        }

        internal static bool HasIntersects_WithAxis(Box3DModel model1, Box3DModel model2, Axis3D axis)
        {
            var box1_projSub = Projection3DUtils.GetProjectionSub(model1, axis);
            var box2_projSub = Projection3DUtils.GetProjectionSub(model2, axis);
            return HasIntersects(box1_projSub, box2_projSub);
        }

        internal static bool IsInsideBox(Box3DModel model, FPVector3 point, FP64 epsilon)
        {
            var px = point.x;
            var py = point.y;
            var pz = point.z;
            if (model.GetBoxType() == BoxType.AABB)
            {
                var min = model.Min;
                var max = model.Max;
                return px >= (min.x - epsilon) && px <= (max.x + epsilon)
                && py <= (min.y + epsilon) && py >= (max.y - epsilon)
                && pz <= (min.z + epsilon) && pz >= (max.z - epsilon);
            }
            else
            {
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

        public static bool HasIntersects(FPVector2 sub1, FPVector2 sub2)
        {
            bool cross = !(sub1.y < sub2.x || sub2.y < sub1.x);
            return cross;
        }

    }

}
