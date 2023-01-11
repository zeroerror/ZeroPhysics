using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public class FrictionPhase {

        Physics3DFacade physicsFacade;

        public FrictionPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time) {
            var collisionService = physicsFacade.Service.CollisionService;
            var boxRBs = physicsFacade.boxRBs;
            var rbBoxInfos = physicsFacade.Service.IDService.boxRBIDInfos;

            for (int i = 0; i < boxRBs.Length; i++) {
                if (!rbBoxInfos[i]) continue;

                var rb = boxRBs[i];
                if (!collisionService.TryGetCollision(rb, out var collision)) continue;

                var linearV = rb.LinearV;
                var linearV_normalized = linearV.normalized;
                var totalForce = rb.TotalForce;

                var rbBox = rb.Box;
                var mass = rb.Mass;
                FPVector3 beHitDirA = collision.BeHitDirA;
                FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;

                // Cross instead?
                var cos_vnh = FPVector3.Dot(linearV_normalized, beHitDir);
                if (FPUtils.IsNear(cos_vnh, -1, FP64.EN4) || FPUtils.IsNear(cos_vnh, 1, FP64.EN4)) {
                    continue;
                }

                // === Friction
                var U = rbBox.FirctionCoe_combined;
                var N = FPVector3.Dot(totalForce, -beHitDir);

                FP64 f = U * N;
                var linearV_Len = linearV.Length();
                var maxFrictionForce = linearV_Len * (mass / time);
                f = f > maxFrictionForce ? maxFrictionForce : f;
                UnityEngine.Debug.Log($"摩擦系数:{U}  总力{totalForce} -> 摩擦垂直力N:{N}  最大摩擦力:{maxFrictionForce}  摩擦力{f}");

                FPVector3 frictionForce = -f * linearV_normalized;
                FP64 m = rb.Mass;
                FPVector3 a = frictionForce / m;
                FPVector3 offset = a * time;
                FPVector3 newV = linearV + offset;
                rb.SetLinearV(newV);
            }
      
        }

    }

}