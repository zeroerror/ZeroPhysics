using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics2D {

    public class Circle {

        #region [Field]

        FPVector2 center;
        public FPVector2 Center => center;

        public FP64 rotAngle;
        public FP64 RotAngle => rotAngle;

        FP64 scale;
        public FP64 Scale => scale;

        FP64 radius_scaled;
        public FP64 Radius_scaled => radius_scaled;

        FP64 radius;
        public FP64 Radius => radius;

        Rectangle rectangle;
        public Rectangle Box => rectangle;

        #endregion

        public Circle(FPVector2 center, FP64 radius, FP64 rotAngle, FP64 scale) {
            rectangle = new Rectangle(center, radius, radius, 0, new FPVector2(scale, scale));
            rectangle.SetRectangleType(RectangleType.AABB);
            UpdateCenter(center);
            UpdateRadius(radius);
            UpdateRotAngle(rotAngle);
            UpdateScale(scale);
        }

        public void UpdateCenter(FPVector2 v) {
            center = v;
            rectangle.UpdateCenter(center);
        }

        public void UpdateScale(FP64 v) {
            scale = v;
            radius_scaled = radius * v;
            rectangle.UpdateScale(new FPVector2(v, v));
        }

        public void UpdateRadius(FP64 v) {
            radius = v;
            FP64 len = v * 2;
            rectangle.UpdateWidth(len);
            rectangle.UpdateHeight(len);
        }

        public void UpdateRotAngle(FP64 v) {
            rotAngle = v;
        }

        public FPVector2 GetProjectionSub(Axis2D axis) {
            var axisCenter = axis.center;
            var axisDir = axis.dir;
            var p0 = FPVector2.Project(center - axisCenter, axisDir).Length();
            FPVector2 sub = new FPVector2(p0 - radius_scaled, p0 + radius_scaled);
            return sub;
        }

        public bool HasCollisionWithSphere(FPVector2 tarPos) {
            var xDiff = center.x - tarPos.x;
            var yDiff = center.y - tarPos.y;
            return (xDiff * xDiff + yDiff * yDiff) <= (radius * radius);
        }

    }

}
