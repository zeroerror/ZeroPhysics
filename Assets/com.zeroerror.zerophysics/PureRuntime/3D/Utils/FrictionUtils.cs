using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public static class FrictionUtils {

        public static void ApplyFriction_RS(in CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            FPVector3 hitDirBA = collision.HitDirBA;
            FPVector3 hitDirAB = -hitDirBA;
            FP64 u = collision.FirctionCoe_combined;
            var rb = bodyA.RB;

            if (hitDirBA == FPVector3.Zero) {
                return;
            }

            FPVector3 linearV = rb.LinearV;
            FP64 linearVLen = linearV.Length();

            // 速度和外力同一条力线上，则退出
            var cos = FPVector3.Dot(linearV.normalized, hitDirBA);
            if (FPUtils.IsNear(cos, FP64.One, FP64.EN2) || FPUtils.IsNear(cos, -FP64.One, FP64.EN2)) {
                return;
            }

            FP64 n = FPVector3.Dot(rb.OutForce, hitDirAB);
            FPVector3 frictionForce = -u * n * GetFrictionDir(linearV, hitDirBA);

            // 计算摩擦力
            var frictionOffsetV = ForceUtils.GetOffsetV_ByForce(frictionForce, rb.Mass, dt);
            if (frictionOffsetV == FPVector3.Zero) {
                return;
            }
            linearV += frictionOffsetV;
            var cosFlag = FPVector3.Dot(frictionOffsetV, linearV);
            if (cosFlag > FP64.EN4) {
                linearV = FPVector3.Zero;
                // UnityEngine.Debug.Log($"{rb} 摩擦力 停下");
            }

            rb.SetLinearV(linearV);
        }

        public static void ApplyFriction_RR(in CollisionModel collision, in FP64 dt) {
            var bodyA = collision.bodyA;
            var bodyB = collision.bodyB;
            FPVector3 hitDirBA = collision.HitDirBA;
            FPVector3 hitDirAB = -hitDirBA;
            FP64 coe = collision.FirctionCoe_combined;
            var rbA = bodyA.RB;
            var rbB = bodyB.RB;

            // 1. 相对运动
            var vA = rbA.LinearV;
            var vB = rbB.LinearV;
            var v1_proj = FPVector3.Dot(vA, hitDirAB);
            var v2_proj = FPVector3.Dot(vB, hitDirBA);
            var totalVProj = v1_proj + v2_proj;
            if (totalVProj <= 0) {
                UnityEngine.Debug.Log($"相对运动为 分离，无摩擦力作用");
                UnityEngine.Debug.Log($"rbA{rbA} rbB{rbB} ");
                UnityEngine.Debug.Log($"v1{vA} v2{vB}");
                UnityEngine.Debug.Log($"hitDirAB{hitDirAB} hitDirBA{hitDirBA}");
                UnityEngine.Debug.Log($"v1_proj{v1_proj} v2_proj{v2_proj} ");
                return;
            }
            // 2. 力的大小
            var outF1 = rbA.OutForce;
            var outF2 = rbB.OutForce;
            var outF1_proj = FPVector3.Dot(outF1, hitDirAB);
            var outF2_proj = FPVector3.Dot(outF2, hitDirBA);
            var totalFProj = outF1_proj + outF2_proj;
            if (totalFProj <= 0) {
                UnityEngine.Debug.Log($"挤压力为0，无摩擦力作用");
                UnityEngine.Debug.Log($"outF1{outF1} hitDirAB{hitDirAB} ");
                UnityEngine.Debug.Log($"outF2{outF2} hitDirBA{hitDirBA} ");
                UnityEngine.Debug.Log($"outF1_proj{outF1_proj} outF2_proj{outF2_proj} ");
                return;
            }
            // 3. 计算摩擦力方向
            var frictionDirA = GetFrictionDir(vA, hitDirBA);
            var frictionDirB = -frictionDirA;
            var v1_fricProj = FPVector3.Dot(vA, frictionDirA);
            var v2_fricProj = FPVector3.Dot(vB, frictionDirA);
            var dot = FPVector3.Dot(vA, vB);
            if (dot > 0 && (v1_fricProj - v2_fricProj) < 0) {
                // 同向且V2更快,在摩擦力线上V1相当于静止
                frictionDirA = -frictionDirA;
                frictionDirB = -frictionDirB;
            }
            // 4. 得出摩擦力矢量
            var f = coe * totalFProj;
            FPVector3 frictionForceA = f * frictionDirA;
            FPVector3 frictionForceB = f * frictionDirB;
            var frictionOffsetVA = ForceUtils.GetOffsetV_ByForce(frictionForceA, rbA.Mass, dt);
            var frictionOffsetVB = ForceUtils.GetOffsetV_ByForce(frictionForceB, rbB.Mass, dt);
            UnityEngine.Debug.Log($" frictionDirA {frictionDirA} frictionDirB {frictionDirB} ");
            UnityEngine.Debug.Log($"得出摩擦力大小 f{f}  矢量frictionForceA {frictionForceA} 矢量frictionForceB {frictionForceB}");
            UnityEngine.Debug.Log($"得出摩擦力Offset frictionOffsetVA {frictionOffsetVA} frictionOffsetVB {frictionOffsetVB}");
            UnityEngine.Debug.Log($" vA {vA} vB {vB} dot {dot}");
            UnityEngine.Debug.Log($" v1_fricProj {v1_fricProj} v2_fricProj {v2_fricProj} ");
            vA -= frictionOffsetVA;
            vB -= frictionOffsetVB;
            rbA.SetLinearV(vA);
            rbB.SetLinearV(vB);
        }

        static FPVector3 GetFrictionDir(in FPVector3 linearV, in FPVector3 beHitDir) {
            var crossAxis = FPVector3.Cross(linearV, beHitDir);
            crossAxis.Normalize();
            if (crossAxis == FPVector3.Zero) {
                return crossAxis;
            }
            var rot = FPQuaternion.CreateFromAxisAngle(crossAxis, FPUtils.rad_90);
            var frictionDir = rot * -beHitDir;    // 撞击方向 绕轴旋转
            return frictionDir;
        }


    }

}