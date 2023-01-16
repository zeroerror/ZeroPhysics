using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public class PenetrationPhase {

        Physics3DFacade physicsFacade;

        public PenetrationPhase() {
        }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time) {
            var service = physicsFacade.Service;
            var idService = service.IDService;
            var collisionService = service.CollisionService;
            var allCollision = collisionService.GetAllCollisions();
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    // --- RB & RB
                    if (collision.bodyA is Box3DRigidbody A && collision.bodyB is Box3DRigidbody B) {
                        var boxA = A.Box;
                        var boxB = B.Box;
                        var mtv = Penetration3DUtils.GetMTV(boxA.GetModel(), boxB.GetModel());
                        mtv *= FPUtils.multy_penetration_rbNrb;
                        // 计算MTV
                        if (mtv.Length() > FPUtils.epsilon_mtv) {
                            var hitDirBA = mtv.normalized;
                            collisionService.UpdateHitDirBA(A, B, hitDirBA);
                        }
                        // NoTrigger之间才有摩擦力和交叉恢复
                        if (!boxA.IsTrigger && !boxB.IsTrigger) {
                            var firctionCoeA = boxA.FrictionCoe;
                            var firctionCoeB = boxB.FrictionCoe;
                            var firctionCoe_combined = firctionCoeA < firctionCoeB ? firctionCoeA : firctionCoeB;
                            collision.SetFirctionCoe_combined(firctionCoe_combined);
                            A.ApplyMTV(mtv);
                            B.ApplyMTV(-mtv);
                        }
                    }
                    // --- RB & Static
                    if (collision.bodyA is Box3DRigidbody rb && collision.bodyB is Box3D box) {
                        var boxA = rb.Box;
                        var mtv = Penetration3DUtils.GetMTV(boxA.GetModel(), box.GetModel());
                        mtv *= FPUtils.multy_penetration_rbNstatic;
                        // 计算MTV
                        if (mtv.Length() >= FPUtils.epsilon_mtv) {
                            var hitDirBA = mtv.normalized;
                            collisionService.UpdateHitDirBA(rb, box, hitDirBA);
                        }
                        // NoTrigger之间才有摩擦力和交叉恢复
                        if (!boxA.IsTrigger && !box.IsTrigger) {
                            var firctionCoeA = boxA.FrictionCoe;
                            var firctionCoeB = box.FrictionCoe;
                            var firctionCoe_combined = firctionCoeA < firctionCoeB ? firctionCoeA : firctionCoeB;
                            collision.SetFirctionCoe_combined(firctionCoe_combined);
                            rb.ApplyMTV(mtv);
                        }
                    }
                }
            }
        }

    }

}