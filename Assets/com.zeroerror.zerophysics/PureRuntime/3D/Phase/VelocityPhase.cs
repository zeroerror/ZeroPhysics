using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public class VelocityPhase {

        Physics3DFacade physicsFacade;

        public VelocityPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time) {
            var rbBoxes = physicsFacade.boxRBs;
            var rbBoxIDInfos = physicsFacade.Service.IDService.boxRBIDInfos;
            for (int i = 0; i < rbBoxes.Length; i++) {
                if (!rbBoxIDInfos[i]) continue;
                var rb = rbBoxes[i];
                var linearV = rb.LinearV;
                var f = rb.TotalForce;
                var m = rb.Mass;
                var a = f / m;
                var offset = a * time;
                linearV += offset;
                rb.SetLinearV(linearV);
                UnityEngine.Debug.Log($"速度:{linearV}");

                // // 跟所有其他RB、SB进行 F = UN 计算 ，并且累加
                // // - 摩擦力累加
                // // - With SB
                // var crossAxis = FPVector3.Cross(v, beHitDir);
                // crossAxis.Normalize();
                // var rot = FPQuaternion.CreateFromAxisAngle(crossAxis, FPUtils.RAD_90);
                // var frictionDir = rot * beHitDir;    // 撞击方向 绕轴旋转
                // CalculateFriction(rb, frictionDir, totalForce_outForce, collision, ref totalForce_friction);
            }
        }

    }

}