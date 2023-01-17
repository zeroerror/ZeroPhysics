using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D {

    public class VelocityPhase {

        Physics3DFacade physicsFacade;

        public VelocityPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 dt) {
            var service = physicsFacade.Service;
            var idService = service.IDService;
            var collisionService = service.CollisionService;
            var allCollision = collisionService.GetAllCollisions();

            // ApplyForceHitErase(allCollision, dt);
            var rbCubees = physicsFacade.rbs;
            var rbCubeIDInfos = idService.rbIDInfos;
            for (int i = 0; i < rbCubees.Length; i++) {
                if (!rbCubeIDInfos[i]) continue;
                var rb = rbCubees[i];
                var linearV = rb.LinearV;
                var offsetV = ForceUtils.GetOffsetV_ByForce(rb.OutForce, rb.Mass, dt);
                linearV += offsetV;
                rb.SetLinearV(linearV);
            }
            ApplyElasticCollision(allCollision, dt);
            ApplyFriction(allCollision, dt);
        }

        void ApplyFriction(CollisionModel[] allCollision, in FP64 dt) {
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    if (collision.FirctionCoe_combined == 0) {
                        continue;
                    }
                    if (collision.bodyA.IsTrigger || collision.bodyB.IsTrigger) {
                        continue;
                    }
                    FrictionUtils.ApplyFriction(collision, dt);
                }
            }
        }

        void ApplyElasticCollision(CollisionModel[] allCollision, in FP64 dt) {
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    if (collision.bodyA.IsTrigger || collision.bodyB.IsTrigger) {
                        continue;
                    }
                    ElasticCollisionUtils.ApplyElasticCollision(collision, dt);
                }
            }
        }

        void ApplyForceHitErase(CollisionModel[] allCollision, in FP64 dt) {
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    ForceUtils.ApplyForceHitErase(collision, dt);
                }
            }
        }

    }

}