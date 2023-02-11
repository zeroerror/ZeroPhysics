using System.Collections.Generic;
using FixMath.NET;

namespace ZeroPhysics.Generic {

    public class Simplex {

        public List<FPVector3> points;

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

    }

}