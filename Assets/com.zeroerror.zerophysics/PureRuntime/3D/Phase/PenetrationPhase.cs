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
            var allCollision_RS = collisionService.GetAllCollisions_RS();
            var allCollision_RR = collisionService.GetAllCollisions_RR();

            // ====== RB & Static
            for (int i = 0; i < allCollision_RS.Length; i++) {
                var collision = allCollision_RS[i];
                if (collision.CollisionType != CollisionType.Enter && collision.CollisionType != CollisionType.Stay) {
                    continue;
                }

                var bodyA = collision.bodyA;
                var bodyB = collision.bodyB;
                var rb = bodyA.RB;
                var mtv = Penetration3DUtils.GetMTV_RS(rb, bodyB);
                mtv *= FPUtils.multy_penetration_rbNstatic;
                // 计算MTV
                if (mtv.Length() >= FPUtils.epsilon_mtv) {
                    var hitDirBA = mtv.normalized;
                    collisionService.UpdateHitDirBA_RS(rb, bodyB, hitDirBA);
                }
                // NoTrigger之间才有摩擦力和交叉恢复
                if (!bodyA.IsTrigger && !bodyB.IsTrigger) {
                    var firctionCoeA = bodyA.FrictionCoe;
                    var firctionCoeB = bodyB.FrictionCoe;
                    var firctionCoe_combined = firctionCoeA < firctionCoeB ? firctionCoeA : firctionCoeB;
                    collisionService.UpdateFrictionCoeCombined_RS(rb, bodyB,firctionCoe_combined);
                    rb.ApplyMTV(mtv);
                }
            }

            // ====== RB & RB
            for (int i = 0; i < allCollision_RR.Length; i++) {
                var collision = allCollision_RR[i];
                var bodyA = collision.bodyA;
                var bodyB = collision.bodyB;
                var rbA = bodyA.RB;
                var rbB = bodyB.RB;
                var mtv = Penetration3DUtils.GetMTV(bodyA, bodyB);
                mtv *= FPUtils.multy_penetration_rbNrb;
                // 计算MTV
                if (mtv.Length() > FPUtils.epsilon_mtv) {
                    var hitDirBA = mtv.normalized;
                    collisionService.UpdateHitDirBA_RR(rbA, rbB, hitDirBA);
                }
                // NoTrigger之间才有摩擦力和交叉恢复
                if (!bodyA.IsTrigger && !bodyB.IsTrigger) {
                    var firctionCoeA = bodyA.FrictionCoe;
                    var firctionCoeB = bodyB.FrictionCoe;
                    var firctionCoe_combined = firctionCoeA < firctionCoeB ? firctionCoeA : firctionCoeB;
                    collisionService.UpdateFrictionCoeCombined_RR(rbA, rbB,firctionCoe_combined);
                    rbA.ApplyMTV(mtv);
                    rbB.ApplyMTV(-mtv);
                }
            }
        }

    }

}