using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Service;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public class VelocityPhase {

        Physics3DFacade physicsFacade;

        public VelocityPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 dt) {
            var rbBoxes = physicsFacade.boxRBs;
            var service = physicsFacade.Service;
            var idService = service.IDService;
            var collisionService = service.CollisionService;
            var rbBoxIDInfos = idService.boxRBIDInfos;

            for (int i = 0; i < rbBoxes.Length; i++) {
                if (!rbBoxIDInfos[i]) continue;
                var rb = rbBoxes[i];
                var linearV = rb.LinearV;
                var offsetV = ForceUtils.GetOffsetV_ByForce(rb.OutForce, rb.Mass, dt);
                linearV += offsetV;
                rb.SetLinearV(linearV);
            }

            // OutForce's Velcotiy Influence Erase By Collsion First
            // ApplyForceHitErase(dt);
            ApplyElasticCollision(dt);
            ApplyFriction(dt);
        }

        void ApplyFriction(in FP64 dt) {
            var service = physicsFacade.Service;
            var collisionService = service.CollisionService;
            var allCollision = collisionService.GetAllCollisions();
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    FrictionUtils.ApplyFriction(collision, dt);
                }
            }
        }

        void ApplyElasticCollision(in FP64 dt) {
            var service = physicsFacade.Service;
            var collisionService = service.CollisionService;
            var allCollision = collisionService.GetAllCollisions();
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    ElasticCollisionUtils.ApplyElasticCollision(collision, dt);
                }
            }
        }

        void ApplyForceHitErase(in FP64 dt) {
            var service = physicsFacade.Service;
            var collisionService = service.CollisionService;
            var allCollision = collisionService.GetAllCollisions();
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    ElasticCollisionUtils.ApplyForceHitErase(collision, dt);
                }
            }
        }

    }

}