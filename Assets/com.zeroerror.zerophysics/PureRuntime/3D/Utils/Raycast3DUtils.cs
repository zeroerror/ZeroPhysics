using System;
using System.Collections.Generic;
using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics {

    public static class Raycast3DUtils {

        public static bool RayWithSphere(Ray ray, Sphere sphere, out List<FPVector3> hitPoints) {
            hitPoints = new List<FPVector3>();
            var sphereCenter = sphere.Center;
            var sphereRadius = sphere.Radius_scaled;
            var sphereRadius_sqr = sphereRadius * sphereRadius;
            var rayDir = ray.dir;
            var rayOrigin = ray.origin;
            var rayLen = ray.length;

            var t1 = FPVector3.Dot(sphereCenter - rayOrigin, rayDir);

            var p1 = rayOrigin + t1 * rayDir;
            var d_sqr = FPVector3.DistanceSquared(p1, sphereCenter);
            if (d_sqr > sphereRadius_sqr) return false;

            if (d_sqr == sphereRadius_sqr) {
                if (t1 <= rayLen) hitPoints.Add(p1);
                return false;
            }

            var diff_sqr = sphereRadius_sqr - d_sqr;
            var t_delta = FP64.Sqrt(diff_sqr);
            var t2 = (t1 - t_delta);
            var t3 = (t1 + t_delta);
            var p2 = rayOrigin + rayDir * t2;
            var p3 = rayOrigin + rayDir * t3;
            if (t2 <= rayLen) hitPoints.Add(p2);
            if (t3 <= rayLen) hitPoints.Add(p3);
            return true;
        }

        public static bool RayCubeWithPoints(Ray ray, BoxModel cube, out FPVector3 p1, out FPVector3 p2) {
            p1 = FPVector3.Zero;
            p2 = FPVector3.Zero;
            if (!RayCubeWithLens(ray, cube, out FP64 len1, out FP64 len2)) return false;

            FPVector3 o = ray.origin;
            FPVector3 rd = ray.dir;
            if (len1 <= ray.length) p1 = o + len1 * rd;
            if (len2 <= ray.length) p2 = o + len2 * rd;
            return true;
        }

        public static bool RayCubeWithLens(Ray ray, BoxModel cube, out FP64 len1, out FP64 len2) {
            FPVector3 origin = ray.origin;
            FPVector3 dir = ray.dir;
            return RayWithCube(origin, dir, cube, out len1, out len2);
        }

        public static bool RayWithCube(FPVector3 origin, FPVector3 dir, BoxModel cube, out FP64 len1, out FP64 len2) {
            FP64 epsilon = FP64.Epsilon;
            FP64 tmin = FP64.Zero;
            FP64 tmax = FP64.MaxValue;
            FPVector3 min = cube.Min;
            FPVector3 max = cube.Max;
            len1 = FP64.Zero;
            len2 = FP64.Zero;

            // X
            if (FP64.Abs(dir.x) < epsilon) {
                if (origin.x < min.x || origin.x > max.x) {
                    return false;
                }
            } else {
                var invDir = FP64.One / dir.x;
                var t1 = (min.x - origin.x) * invDir;
                var t2 = (max.x - origin.x) * invDir;
                FP64.SwapMinorToLeft(ref t1, ref t2);
                tmin = FP64.Max(tmin, t1);
                tmax = FP64.Min(tmax, t2);
                if (tmin > tmax) {
                    return false;
                }
            }

            // Y
            if (FP64.Abs(dir.y) < epsilon) {
                if (origin.y < min.y || origin.y > max.y) {
                    return false;
                }
            } else {
                var invDir = FP64.One / dir.y;
                var t1 = (min.y - origin.y) * invDir;
                var t2 = (max.y - origin.y) * invDir;
                FP64.SwapMinorToLeft(ref t1, ref t2);
                tmin = FP64.Max(tmin, t1);
                tmax = FP64.Min(tmax, t2);
                if (tmin > tmax) {
                    return false;
                }
            }

            // Z
            if (FP64.Abs(dir.z) < epsilon) {
                if (origin.z < min.z || origin.z > max.z) {
                    return false;
                }
            } else {
                var invDir = FP64.One / dir.z;
                var t1 = (min.z - origin.z) * invDir;
                var t2 = (max.z - origin.z) * invDir;
                FP64.SwapMinorToLeft(ref t1, ref t2);
                tmin = FP64.Max(tmin, t1);
                tmax = FP64.Min(tmax, t2);
                if (tmin > tmax) {
                    return false;
                }
            }

            len1 = tmin;
            len2 = tmax;
            return true;
        }

        static bool RayWithCube(BoxModel cube, Ray ray) {
            bool HasCrossSub(in FPVector2 sub1, in FPVector2 sub2, in FP64 epsilon) {
                return !(sub1.y < sub2.x - epsilon || sub2.y < sub1.x - epsilon);
            }

            var rayOrigin = ray.origin;
            var rayDir = ray.dir;
            var rayLen = ray.length;

            // - Axis x
            var axis = cube.GetAxisX();
            var pjSub1 = cube.GetAxisX_SelfProjectionSub();
            var pjSub2 = ray.GetProjectionSub(axis);
            if (!HasCrossSub(pjSub1, pjSub2, 0)) return false;

            // - Axis y  
            axis = cube.GetAxisY();
            pjSub1 = cube.GetAxisY_SelfProjectionSub();
            pjSub2 = ray.GetProjectionSub(axis);
            if (!HasCrossSub(pjSub1, pjSub2, 0)) return false;

            // - Axis z
            axis = cube.GetAxisZ();
            pjSub1 = cube.GetAxisZ_SelfProjectionSub();
            pjSub2 = ray.GetProjectionSub(axis);
            if (!HasCrossSub(pjSub1, pjSub2, 0)) return false;

            return true;
        }

        // 算法解析：
        // ① 射线方程 R = Ro + t * Rd
        // ② 平面的特性: 在任意平面上,求得法向量(单位向量)D后，任意2个点P0,P1,都满足方程 P0 · D = P1 · D 
        // ③ 根据①②可求得 t = ( P0 · d - Ro · d ) / ( Rd · d ) = ( P0 - Ro ) · d / ( Rd · d )
        // 分别与面进行射线交点检测,可求得t,再求得对应R值,即hitPoint
        [Obsolete]
        public static bool RayWithCube(Ray ray, BoxModel cube, out List<FPVector3> hitPoints) {
            hitPoints = new List<FPVector3>();

            var ro = ray.origin;
            var rd = ray.dir;
            var rl = ray.length;
            var min = cube.Min;
            var max = cube.Max;

            // AxisX's Two Planes
            FPVector3 d = cube.GetAxisX().dir;
            FP64 t = FPVector3.Dot(min - ro, d) / FPVector3.Dot(rd, d);
            if (t > 0 && t <= rl) {
                var r = ro + t * rd;
                if (Intersect3DUtil.IsInsideCube(cube, r, FP64.Epsilon)) hitPoints.Add(r);
            }

            t = FPVector3.Dot(max - ro, d) / FPVector3.Dot(rd, d);
            if (t > 0 && t <= rl) {
                var r = ro + t * rd;
                if (Intersect3DUtil.IsInsideCube(cube, r, FP64.Epsilon)) hitPoints.Add(r);
            }

            // AxisY's Two Planes
            d = cube.GetAxisY().dir;
            t = FPVector3.Dot(min - ro, d) / FPVector3.Dot(rd, d);
            if (t > 0 && t <= rl) {
                var r = ro + t * rd;
                if (Intersect3DUtil.IsInsideCube(cube, r, FP64.Epsilon)) hitPoints.Add(r);
            }

            t = FPVector3.Dot(max - ro, d) / FPVector3.Dot(rd, d);
            if (t > 0 && t <= rl) {
                var r = ro + t * rd;
                if (Intersect3DUtil.IsInsideCube(cube, r, FP64.Epsilon)) hitPoints.Add(r);
            }

            // AxisZ's Two Planes
            d = cube.GetAxisZ().dir;
            t = FPVector3.Dot(min - ro, d) / FPVector3.Dot(rd, d);
            if (t > 0 && t <= rl) {
                var r = ro + t * rd;
                if (Intersect3DUtil.IsInsideCube(cube, r, FP64.Epsilon)) hitPoints.Add(r);
            }

            t = FPVector3.Dot(max - ro, d) / FPVector3.Dot(rd, d);
            if (t > 0 && t <= rl) {
                var r = ro + t * rd;
                if (Intersect3DUtil.IsInsideCube(cube, r, FP64.Epsilon)) hitPoints.Add(r);
            }

            return hitPoints.Count != 0;
        }

    }

}