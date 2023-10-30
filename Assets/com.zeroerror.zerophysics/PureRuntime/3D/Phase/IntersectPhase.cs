using FixMath.NET;
using ZeroPhysics.Physics.Context;
using ZeroPhysics.Service;

namespace ZeroPhysics.Physics {

    public class IntersectPhase {

        PhysicsContext physicsContext;

        public IntersectPhase() { }

        public void Inject(PhysicsContext physicsContext) {
            this.physicsContext = physicsContext;
        }

        public void Tick(in FP64 time) {
            var allServices = physicsContext.Service;
            var collisionService = allServices.CollisionService;
            var idService = allServices.IDService;
            var rbs = physicsContext.rbs;
            var rbIDinfos = idService.rbIDInfos;

            for (int i = 0; i < rbs.Length - 1; i++) {

                if (!rbIDinfos[i]) {
                    continue;
                }

                var rb1 = rbs[i];
                // RB & SB
                RBAndStatic(rb1);
                // RB & RB
                for (int j = i + 1; j < rbs.Length; j++) {
                    if (!rbIDinfos[j]) {
                        continue;
                    }
                    var rb2 = rbs[j];
                    if (!Intersect3DUtil.HasCollision(rb1, rb2)) {
                        collisionService.RemoveCollision_RR(rb1, rb2);
                        continue;
                    }

                    collisionService.AddCollision_RR(rb1, rb2);
                }

            }
        }

        void RBAndStatic(Rigidbody rb) {
            var allServices = physicsContext.Service;
            var collisionService = allServices.CollisionService;
            var idService = allServices.IDService;
            var cubeIDInfos = idService.cubeIDInfos;
            var cubes = physicsContext.cubes;
            for (int j = 0; j < cubes.Length; j++) {
                if (!cubeIDInfos[j]) {
                    continue;
                }
                var cube = cubes[j];
                if(cube.IsTrigger){
                    continue;
                }
                if (!Intersect3DUtil.HasCollision(rb, cube)) {
                    collisionService.RemoveCollision_RS(rb, cube);
                    continue;
                }

                collisionService.AddCollision_RS(rb, cube);
            }
        }

    }

}