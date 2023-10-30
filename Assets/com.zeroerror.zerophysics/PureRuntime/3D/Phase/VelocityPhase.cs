using System;
using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics.Context;

namespace ZeroPhysics.Physics {

    public class VelocityPhase {

        PhysicsContext physicsContext;

        public VelocityPhase() { }

        public void Inject(PhysicsContext physicsContext) {
            this.physicsContext = physicsContext;
        }

        public void Tick(in FP64 dt) {
            var service = physicsContext.Service;
            var idService = service.IDService;
            var collisionService = service.CollisionService;
            var allCollision_RS = collisionService.GetAllCollisions_RS();
            var allCollision_RR = collisionService.GetAllCollisions_RR();

            var rbCubes = physicsContext.rbs;
            var rbCubeIDInfos = idService.rbIDInfos;

            // - Update DirtyOutForce
            ApplyDirtyOutForceByHit(allCollision_RS, dt);

            // - Force To Velocity
            for (int i = 0; i < rbCubes.Length; i++) {
                if (!rbCubeIDInfos[i]) continue;
                var rb = rbCubes[i];
                var linearV = rb.LinearV;
                var offsetV = ForceUtils.GetOffsetV_ByForce(rb.DirtyOutForce, rb.Mass, dt);
                linearV += offsetV;
                rb.SetLinearV(linearV);
            }

            // - Elastic
            ApplyElasticCollision_RR(allCollision_RR, dt);
            ApplyElasticCollision_RS(allCollision_RS, dt);
            ApplyFriction_RR(allCollision_RR, dt);
            ApplyFriction_RS(allCollision_RS, dt);
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

        void ApplyForceHitErase_RR(CollisionModel[] allCollision, in FP64 dt) {
            for (int i = 0; i < allCollision.Length; i++) {
                var collision = allCollision[i];
                if (collision.CollisionType == CollisionType.Enter
                || collision.CollisionType == CollisionType.Stay) {
                    ForceUtils.ApplyForceHitErase_RR(collision, dt);
                }
            }
        }

        void ApplyDirtyOutForceByHit(CollisionModel[] allCollision, in FP64 dt) {
            for (int i = 0; i < allCollision.Length; i++) {
                var collisionModel = allCollision[i];
                var rb = collisionModel.bodyA.RB;
                rb.SetDirtyOutForce(rb.OutForce);
                if (collisionModel.CollisionType == CollisionType.Enter
                || collisionModel.CollisionType == CollisionType.Stay) {
                    ForceUtils.ApplyForceHitErase_RS(collisionModel, dt);
                }
            }
        }

    }

}