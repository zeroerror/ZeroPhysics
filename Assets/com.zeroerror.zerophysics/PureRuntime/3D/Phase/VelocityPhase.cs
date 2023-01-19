using System;
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
            var allCollision_RS = collisionService.GetAllCollisions_RS();
            var allCollision_RR = collisionService.GetAllCollisions_RR();

            ApplyForceHitErase(allCollision_RR, dt);
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
            ApplyElasticCollision_RS(allCollision_RS, dt);
            ApplyElasticCollision_RR(allCollision_RR, dt);
            ApplyFriction_RS(allCollision_RS, dt);
            ApplyFriction_RR(allCollision_RR, dt);
        }

        void ApplyFriction_RS(CollisionModel[] allCollision_RS, in FP64 dt) {
            for (int i = 0; i < allCollision_RS.Length; i++) {
                var collision = allCollision_RS[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    if (collision.FirctionCoe_combined == 0) {
                        continue;
                    }
                    if (collision.bodyA.IsTrigger || collision.bodyB.IsTrigger) {
                        continue;
                    }
                    FrictionUtils.ApplyFriction_RS(collision, dt);
                }
            }
        }

        void ApplyFriction_RR(CollisionModel[] allCollision_RR, in FP64 dt) {
            for (int i = 0; i < allCollision_RR.Length; i++) {
                var collision = allCollision_RR[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    if (collision.FirctionCoe_combined == 0) {
                        continue;
                    }
                    if (collision.bodyA.IsTrigger || collision.bodyB.IsTrigger) {
                        continue;
                    }
                    FrictionUtils.ApplyFriction_RR(collision, dt);
                }
            }
        }

        void ApplyElasticCollision_RS(CollisionModel[] collisions_RS, in FP64 dt) {
            for (int i = 0; i < collisions_RS.Length; i++) {
                var collision = collisions_RS[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    if (collision.bodyA.IsTrigger || collision.bodyB.IsTrigger) {
                        continue;
                    }
                    ElasticCollisionUtils.ApplyElasticCollision_RS(collision, dt);
                }
            }
        }

        void ApplyElasticCollision_RR(CollisionModel[] collisions_RR, in FP64 dt) {
            for (int i = 0; i < collisions_RR.Length; i++) {
                var collision = collisions_RR[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    if (collision.bodyA.IsTrigger || collision.bodyB.IsTrigger) {
                        continue;
                    }
                    ElasticCollisionUtils.ApplyElasticCollision_RR(collision, dt);
                }
            }
        }

        void ApplyForceHitErase(CollisionModel[] allCollision, in FP64 dt) {
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    ForceUtils.ApplyForceHitErase_RR(collision, dt);
                }
            }
        }

    }

}