using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D
{

    public class FrictionPhase
    {

        Physics3DFacade physicsFacade;

        public FrictionPhase() { }

        public void Inject(Physics3DFacade physicsFacade)
        {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time)
        {
            var boxRBs = physicsFacade.boxRBs;
            var rbBoxInfos = physicsFacade.Service.IDService.boxRBIDInfos;
            for (int i = 0; i < boxRBs.Length; i++)
            {
                if (!rbBoxInfos[i]) continue;

                var rb = boxRBs[i];
                if (!rb.IsCollisionStay) continue;

                var linearV = rb.LinearV;
                var rbBox = rb.Box;
                var mass = rb.Mass;
                var linearV_normalized = linearV.normalized;
                FPVector3 totalForce = rb.TotalForce;

                // === Friction
                var U = rbBox.FirctionCoe_combined;
                var N = FPVector3.Dot(totalForce, -rb.BeHitDir);
                FP64 f = U * N;
                var maxFrictionForce = linearV.Length() * (mass / time);
                f = f > maxFrictionForce ? maxFrictionForce : f;
                FPVector3 frictionForce = -f * linearV_normalized;
                // - Set
                var m = rb.Mass;
                var a = frictionForce / m;
                var offset = a * time;
                var newV = linearV + offset;
                rb.SetLinearV(newV);
            }
        }

    }

}