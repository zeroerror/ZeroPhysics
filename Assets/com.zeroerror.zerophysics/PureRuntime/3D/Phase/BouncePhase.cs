using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D {

    public class BouncePhase {

        Physics3DFacade physicsFacade;

        public BouncePhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time, in FPVector3 gravity) {
            var idService = physicsFacade.Service.IDService;
            var collisionService = physicsFacade.Service.CollisionService;
            var boxRBs = physicsFacade.boxRBs;
            var boxRBIDInfos = idService.boxRBIDInfos;
            var boxes = physicsFacade.boxes;
            var boxInfos = idService.boxIDInfos;

            // - RB & SB
            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

                var rb = boxRBs[i];
                var rbBox = rb.Box;
                if (!collisionService.HasCollision(rb)) continue;

                for (int j = 0; j < boxes.Length; j++) {
                    if (!boxInfos[j]) continue;

                    var box = boxes[j];
                    if (!collisionService.TryGetCollision(rb, box, out var collision)) continue;
                    if (collision.CollisionType == Generic.CollisionType.Exit) continue;

                    FPVector3 beHitDirA = collision.BeHitDirA;
                    FPVector3 beHitDir = collision.bodyA == rb ? beHitDirA : -beHitDirA;
                    var linearV = rb.LinearV;
                    var v = Bounce3DUtils.GetBouncedV(linearV, beHitDir, rb.BounceCoefficient);
                    rb.SetLinearV(v);
                }

            }
        }

    }

}