using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D {

    public class FrictionPhase {

        Physics3DFacade physicsFacade;

        static readonly FP64 NegativeOne_Small_Epsilon = -1 - FP64.EN4;
        static readonly FP64 NegativeOne_Big_Epsilon = -1 + FP64.EN4;

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
                var rbBox = rb.Box;
                var mass = rb.Mass;
                var totalForce = rb.TotalForce;
                FPVector3 beHitDirA = collision.BeHitDirA;
                FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;

                var cos = FPVector3.Dot(totalForce.normalized, beHitDir);
                if (cos <= NegativeOne_Big_Epsilon) {
                    continue;
                }

                // === Friction
                var U = rbBox.FirctionCoe_combined;
                var N = FPVector3.Dot(totalForce, -beHitDir);
                FP64 f = U * N;
                var maxFrictionForce = linearV.Length() * (mass / time);
                f = f > maxFrictionForce ? maxFrictionForce : f;
                FPVector3 frictionForce = -f * linearV_normalized;
                // - Set
                var m = rb.Mass;
                var a = frictionForce / m;
                var offset = a * time;
                var newV = linearV + offset;
                if (offset != FPVector3.Zero) {
                    UnityEngine.Debug.Log($"cos:{cos} maxFrictionForce:{maxFrictionForce}  f:{f} offset:{offset}");
                }
                rb.SetLinearV(newV);
            }
        }

    }

}