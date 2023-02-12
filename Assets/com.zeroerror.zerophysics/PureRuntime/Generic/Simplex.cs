using System.Collections.Generic;
using FixMath.NET;

namespace ZeroPhysics.Generic {

    public class Simplex {

        public List<FPVector3> points;
        public int Count => points.Count;

        public Simplex() {
            points = new List<FPVector3>(3);
        }

        public void Add(in FPVector3 supportPoint) {
            points.Add(supportPoint);
        }

        public void LeaveTwoPoints() {
            var p0 = points[0];
            var p1 = points[1];
            var p2 = points[2];
            int removeIndex = 2;
            var minDis = GetDisSquared(p0, p1);

            var dis = GetDisSquared(p1, p2);
            if (dis < minDis) {
                minDis = dis;
                removeIndex = 0;
            }

            dis = GetDisSquared(p2, p0);
            if (dis < minDis) {
                removeIndex = 1;
            }

            points.RemoveAt(removeIndex);

        }

        public FPVector3 GetNormal() {
            var p0 = points[0];
            var p1 = points[1];
            var line1 = p1 - p0;
            var line2 = -p0;
            var axis = FPVector3.Cross(line1, line2).normalized;
            var rot = FPQuaternion.CreateFromAxisAngle(axis, 90 * FP64.Deg2Rad);
            var normal = rot * line1;
            normal.Normalize();
            return normal;
        }

        public FP64 GetDisSquared(in FPVector3 startPos, in FPVector3 endPos) {
            FPVector3 line = endPos - startPos;
            FPVector3 line_nor = line.normalized;

            FPVector3 line_ori = -startPos;
            FPVector3 line_ori_nor = line_ori.normalized;
            FP64 line_ori_disSquared = line_ori.LengthSquared();

            FP64 cos = FPVector3.Dot(line_nor, line_ori_nor);
            FP64 cosSquared = cos * cos;
            FP64 disSquared1 = (1 - cosSquared) * line_ori_disSquared;
            FP64 disSquared = line_ori_disSquared - disSquared1;
            return disSquared;
        }

        public bool IsInsideSimplex(in FPVector3 pos) {
            var p0 = points[0];
            var p1 = points[1];
            var p2 = points[2];
            var l0 = (p1 - p0);
            var l1 = (p2 - p1);
            var l2 = (p0 - p2);
            var cross0 = FPVector3.Cross(l0, (pos - p0)).normalized;
            var cross1 = FPVector3.Cross(l1, (pos - p1)).normalized;
            var cross2 = FPVector3.Cross(l2, (pos - p2)).normalized;
            int valid0 = cross0 != FPVector3.Zero ? 1 : 0;
            int valid1 = cross1 != FPVector3.Zero ? 1 : 0;
            int valid2 = cross2 != FPVector3.Zero ? 1 : 0;
            int validCount = valid0 + valid1 + valid2;

            if (validCount == 1) {
                return true;
            }

            if (validCount == 2) {
                if (valid0 == 0) {
                    return cross1 == cross2;
                }
                if (valid1 == 0) {
                    return cross0 == cross2;
                }
                if (valid2 == 0) {
                    return cross0 == cross1;
                }
            }

            return cross0 == cross1 && cross1 == cross2;
        }

    }

}