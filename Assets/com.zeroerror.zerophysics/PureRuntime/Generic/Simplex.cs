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

        /// <summary>
        /// 去除单纯形中相对另外两个点最远的那一个点
        /// </summary> 
        public void LeaveTwoPoints() {
            var p0 = points[0];
            var p1 = points[1];
            var p2 = points[2];
            int removeIndex = 2;
            var minDis = GetOriginToLineSquaredDis(p0, p1);

            var dis = GetOriginToLineSquaredDis(p1, p2);
            if (dis < minDis) {
                minDis = dis;
                removeIndex = 0;
            }

            dis = GetOriginToLineSquaredDis(p2, p0);
            if (dis < minDis) {
                removeIndex = 1;
            }

            points.RemoveAt(removeIndex);

        }

        /// <summary>
        /// 获取单纯形剩余2点构成的线段的法线
        /// </summary>
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

        /// <summary>
        /// 获取原点到给定2点组成的直线的距离的平方
        /// </summary>
        public FP64 GetOriginToLineSquaredDis(in FPVector3 startPos, in FPVector3 endPos) {
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

        /// <summary>
        /// 判断点是否在单纯形内部
        /// </summary>
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
            int valid0 = cross0 == FPVector3.Zero ? 0 : 1;
            int valid1 = cross1 == FPVector3.Zero ? 0 : 1;
            int valid2 = cross2 == FPVector3.Zero ? 0 : 1;
            int validCount = valid0 + valid1 + valid2;

            if (validCount == 3) {
                // 说明点不在单纯形的边上, 进行常规判断
                return cross0 == cross1 && cross1 == cross2;
            }

            if (validCount == 2) {
                // 说明点在单纯形的边上
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

            // 说明点在单纯形的两边交点上
            return true;
        }

    }

}