using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics2D
{

    public static class CollisionHelper2D
    {

        public static bool HasCollision(Rectangle rectangle1, Rectangle rectangle2)
        {
            if (rectangle1.RectangleType == RectangleType.OBB || rectangle2.RectangleType == RectangleType.OBB)
            {
                return HasCollision_OBB(rectangle1, rectangle2);
            }

            return HasCollision_AABB(rectangle1, rectangle2);
        }

        public static bool HasCollision(Sphere2D sphere1, Sphere2D sphere2)
        {
            var center1 = sphere1.Center;
            var center2 = sphere2.Center;
            var xDiff = center1.x - center2.x;
            var yDiff = center1.y - center2.y;
            var radiusSum = sphere1.Radius_scaled + sphere2.Radius_scaled;
            return (xDiff * xDiff + yDiff * yDiff) <= (radiusSum * radiusSum);
        }

        public static bool HasCollision(Sphere2D sphere, Rectangle rectangle)
        {
            if (!HasCollision(sphere.Box, rectangle)) return false;
            if (sphere.HasCollisionWithSphere(rectangle.A)) return true;
            if (sphere.HasCollisionWithSphere(rectangle.B)) return true;
            if (sphere.HasCollisionWithSphere(rectangle.C)) return true;
            if (sphere.HasCollisionWithSphere(rectangle.D)) return true;

            var axisX = rectangle.GetAxisX();
            var axisX_PjSub1 = rectangle.GetAxisX_SelfProjectionSub();
            var axisX_PjSub2 = sphere.GetProjectionSub(axisX);
            var spherePjCenter_X = (axisX_PjSub2.x + axisX_PjSub2.y) / 2;
            bool xOverlapCenter = spherePjCenter_X > axisX_PjSub1.x && spherePjCenter_X < axisX_PjSub1.y;

            // - AABB: 经前置条件过滤后, 以Box的2个轴做投影,若出现SphereCenter的投影在Box投影内，则必定碰撞
            if (rectangle.RectangleType == RectangleType.AABB) return xOverlapCenter;

            // - OBB: 经前置条件过滤后, 以Box的2个轴做投影,若出现SphereCenter的投影在Box投影内, 只要另外一个轴上的投影有相交，则必定碰撞
            var axisY = rectangle.GetAxisY();
            var axisY_PjSub1 = rectangle.GetAxisY_SelfProjectionSub();
            var axisY_PjSub2 = sphere.GetProjectionSub(axisY);
            if (xOverlapCenter)
            {
                return !(axisY_PjSub1.y < axisY_PjSub2.x || axisY_PjSub1.x > axisY_PjSub2.y);
            }

            var spherePjCenter_Y = (axisY_PjSub2.x + axisY_PjSub2.y) / 2;
            bool yOverlapCenter = spherePjCenter_Y > axisX_PjSub1.x && spherePjCenter_Y < axisX_PjSub1.y;
            return yOverlapCenter && !(axisX_PjSub1.y < axisX_PjSub2.x || axisX_PjSub1.x > axisX_PjSub2.y);
        }

        static bool HasCollision_AABB(Rectangle rectangle1, Rectangle rectangle2)
        {
            var ltPos1 = rectangle1.A;
            var rbPos1 = rectangle1.C;
            var ltPos2 = rectangle2.A;
            var rbPos2 = rectangle2.C;
            // - Axis x
            var diff_x1 = ltPos1.x - rbPos2.x;
            var diff_x2 = rbPos1.x - ltPos2.x;
            bool hasCollisionX = (diff_x1 < 0 && diff_x2 > 0) || diff_x1 == 0 || diff_x2 == 0;

            // - Axis y
            var diff_y1 = ltPos1.y - rbPos2.y;
            var diff_y2 = rbPos1.y - ltPos2.y;
            bool hasCollisionY = (diff_y1 > 0 && diff_y2 < 0) || diff_y1 == 0 || diff_y2 == 0;

            return hasCollisionX && hasCollisionY;
        }

        public static bool HasCollision_AABB(FPVector2 ltPos1, FPVector2 rbPos1, FPVector2 ltPos2, FPVector2 rbPos2)
        {
            // - Axis x
            var diff_x1 = ltPos1.x - rbPos2.x;
            var diff_x2 = rbPos1.x - ltPos2.x;
            bool hasCollisionX = (diff_x1 < 0 && diff_x2 > 0) || diff_x1 == 0 || diff_x2 == 0;

            // - Axis y
            var diff_y1 = ltPos1.y - rbPos2.y;
            var diff_y2 = rbPos1.y - ltPos2.y;
            bool hasCollisionY = (diff_y1 > 0 && diff_y2 < 0) || diff_y1 == 0 || diff_y2 == 0;

            return hasCollisionX && hasCollisionY;
        }

        static bool HasCollision_OBB(Rectangle rectangle1, Rectangle rectangle2)
        {
            var rectangle1_projSub = rectangle1.GetAxisX_SelfProjectionSub();
            var rectangle2_projSub = rectangle2.GetProjectionSub(rectangle1.GetAxisX());
            if (rectangle1_projSub.y < rectangle2_projSub.x) return false;
            if (rectangle1_projSub.x > rectangle2_projSub.y) return false;

            rectangle1_projSub = rectangle1.GetAxisY_SelfProjectionSub();
            rectangle2_projSub = rectangle2.GetProjectionSub(rectangle1.GetAxisY());
            if (rectangle1_projSub.y < rectangle2_projSub.x) return false;
            if (rectangle1_projSub.x > rectangle2_projSub.y) return false;

            rectangle2_projSub = rectangle2.GetAxisX_SelfProjectionSub();
            rectangle1_projSub = rectangle1.GetProjectionSub(rectangle2.GetAxisX());
            if (rectangle1_projSub.y < rectangle2_projSub.x) return false;
            if (rectangle1_projSub.x > rectangle2_projSub.y) return false;

            rectangle2_projSub = rectangle2.GetAxisY_SelfProjectionSub();
            rectangle1_projSub = rectangle1.GetProjectionSub(rectangle2.GetAxisY());
            if (rectangle1_projSub.y < rectangle2_projSub.x) return false;
            if (rectangle1_projSub.x > rectangle2_projSub.y) return false;

            return true;
        }

        public static void GetOBBMinMaxPos(Rectangle rectangle, ref FPVector2 pos_minX, ref FPVector2 pos_maxX, ref FPVector2 pox_minY, ref FPVector2 pos_maxY)
        {
            var a = rectangle.A;
            var b = rectangle.B;
            var c = rectangle.C;
            var d = rectangle.D;
            var rotAngle = rectangle.Rotation;
            var count = (int)(rotAngle / 90);
            bool isPositive = rotAngle > 0;
            if (count == 0)
            {
                if (isPositive)
                {
                    pos_minX = a;
                    pos_maxX = c;
                    pos_maxY = b;
                    pox_minY = d;
                }
                else
                {
                    pos_minX = d;
                    pos_maxX = b;
                    pos_maxY = a;
                    pox_minY = c;
                }
            }
            else if (count == 1)
            {
                if (isPositive)
                {
                    pos_minX = b;
                    pos_maxX = d;
                    pos_maxY = c;
                    pox_minY = a;
                }
                else
                {
                    pos_minX = c;
                    pos_maxX = a;
                    pos_maxY = d;
                    pox_minY = b;
                }
            }
            else if (count == 2)
            {
                if (isPositive)
                {
                    pos_minX = c;
                    pos_maxX = a;
                    pos_maxY = d;
                    pox_minY = b;
                }
                else
                {
                    pos_minX = b;
                    pos_maxX = d;
                    pos_maxY = c;
                    pox_minY = a;
                }
            }
            else if (count == 3)
            {
                if (isPositive)
                {
                    pos_minX = d;
                    pos_maxX = b;
                    pos_maxY = a;
                    pox_minY = c;
                }
                else
                {
                    pos_minX = a;
                    pos_maxX = c;
                    pos_maxY = b;
                    pox_minY = d;
                }
            }
        }

    }
}
