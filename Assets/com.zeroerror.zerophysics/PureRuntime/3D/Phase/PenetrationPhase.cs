using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics.Context;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics {

    public class PenetrationPhase {

        PhysicsContext physicsContext;

        public PenetrationPhase() {
        }

        public void Inject(PhysicsContext physicsContext) {
            this.physicsContext = physicsContext;
        }

        public void Tick(in FP64 time) {
            var service = physicsContext.Service;
            var collisionService = service.CollisionService;
            var allCollision_RS = collisionService.GetAllCollisions_RS();
            var allCollision_RR = collisionService.GetAllCollisions_RR();

            // ====== Clear
            var rbs = physicsContext.rbs;
            var idService = service.IDService;
            var rbInfos = idService.rbIDInfos;
            for (int i = 0; i < rbs.Length; i++) {
                if (!rbInfos[i]) {
                    continue;
                }
                var rb = rbs[i];
                rb.SetIsDirty(false);
            }

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
                mtv *= FPUtils.multy_penetration_rs;
                var hitDirBA = mtv.normalized;
                collisionService.UpdateHitDirBA_RS(rb, bodyB, hitDirBA);

                // NoTrigger之间才有摩擦力和交叉恢复
                if (!bodyA.IsTrigger && !bodyB.IsTrigger) {
                    var firctionCoeA = bodyA.FrictionCoe;
                    var firctionCoeB = bodyB.FrictionCoe;
                    var firctionCoe_combined = firctionCoeA < firctionCoeB ? firctionCoeA : firctionCoeB;
                    collisionService.UpdateFrictionCoeCombined_RS(rb, bodyB, firctionCoe_combined);
                    rb.ApplyMTV(mtv);
                    rb.SetIsDirty(true);
                }
            }

            // ====== RB & RB
            for (int i = 0; i < allCollision_RR.Length; i++) {
                var collision = allCollision_RR[i];
                if (collision.CollisionType != CollisionType.Enter && collision.CollisionType != CollisionType.Stay) {
                    continue;
                }

                var bodyA = collision.bodyA;
                var bodyB = collision.bodyB;
                var rbA = bodyA.RB;
                var rbB = bodyB.RB;
                var mtv = Penetration3DUtils.GetMTV(bodyA, bodyB);
                mtv *= FPUtils.multy_penetration_rr;
                var hitDirBA = mtv.normalized;
                collisionService.UpdateHitDirBA_RR(rbA, rbB, hitDirBA);
                // NoTrigger之间才有摩擦力和交叉恢复
                if (!bodyA.IsTrigger && !bodyB.IsTrigger) {
                    var firctionCoeA = bodyA.FrictionCoe;
                    var firctionCoeB = bodyB.FrictionCoe;
                    var firctionCoe_combined = firctionCoeA < firctionCoeB ? firctionCoeA : firctionCoeB;
                    collisionService.UpdateFrictionCoeCombined_RR(rbA, rbB, firctionCoe_combined);
                    if (rbA.IsDirty && !rbB.IsDirty) {
                        rbB.ApplyMTV(-mtv * 2);
                        rbB.SetIsDirty(true);
                        continue;
                    }
                    if (!rbA.IsDirty && rbB.IsDirty) {
                        rbA.ApplyMTV(mtv * 2);
                        rbA.SetIsDirty(true);
                        continue;
                    }
                    if (!rbA.IsDirty && !rbB.IsDirty) {
                        rbA.ApplyMTV(mtv);
                        rbB.ApplyMTV(-mtv);
                        continue;
                    }
                    if (rbA.IsDirty && rbB.IsDirty) {
                        continue;
                    }
                }
            }

        }

    }

}