using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D
{

    public class FrictionPhase
    {

        Physics3DFacade facade;

        public FrictionPhase() { }

        public void Inject(Physics3DFacade facade)
        {
            this.facade = facade;
        }

        public void Tick(in FP64 time)
        {
            var boxRBs = facade.boxRBs;
            var rbBoxInfos = facade.IDService.boxRBIDInfos;
            for (int i = 0; i < boxRBs.Length; i++)
            {
                if (!rbBoxInfos[i]) continue;

                var rb = boxRBs[i];
                var linearV = rb.LinearV;
                if (linearV == FPVector3.Zero) continue;

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
                rb.SetFrictionForce(frictionForce);
            }
        }

    }

}