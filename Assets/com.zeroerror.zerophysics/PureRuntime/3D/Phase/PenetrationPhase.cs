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
                    UnityEngine.Debug.Log($"collision {collision.bodyA} {collision.bodyB}");
                if (collision.CollisionType != CollisionType.Enter && collision.CollisionType != CollisionType.Stay) {
                    continue;
                }

                var bodyA = collision.bodyA;
                var bodyB = collision.bodyB;

                // --- RB & RB
                if (bodyA is Rigidbody3D A && bodyB is Rigidbody3D B) {
                    var boxA = A.Body;
                    var boxB = B.Body;
                    var mtv = Penetration3DUtils.GetMTV(boxA, boxB);
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
                if (bodyA is Rigidbody3D rb) {
                    UnityEngine.Debug.Log($"RB & Static");
                    var mtv = Penetration3DUtils.GetMTV(rb, bodyB);
                    mtv *= FPUtils.multy_penetration_rbNstatic;
                    // 计算MTV
                    if (mtv.Length() >= FPUtils.epsilon_mtv) {
                        var hitDirBA = mtv.normalized;
                        collisionService.UpdateHitDirBA(rb, bodyB, hitDirBA);
                    }
                    // NoTrigger之间才有摩擦力和交叉恢复
                    if (!bodyA.IsTrigger && !bodyB.IsTrigger) {
                        var firctionCoeA = bodyA.FrictionCoe;
                        var firctionCoeB = bodyB.FrictionCoe;
                        var firctionCoe_combined = firctionCoeA < firctionCoeB ? firctionCoeA : firctionCoeB;
                        collision.SetFirctionCoe_combined(firctionCoe_combined);
                        rb.ApplyMTV(mtv);
                    }
                }
            }
        }

    }

}