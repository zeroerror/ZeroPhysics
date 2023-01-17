using FixMath.NET;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Service;

namespace ZeroPhysics.Physics3D {

    public class IntersectPhase {

        Physics3DFacade physicsFacade;

        public IntersectPhase() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time) {
            var allServices = physicsFacade.Service;
            var collisionService = allServices.CollisionService;
            var idService = allServices.IDService;
            var rbs = physicsFacade.rbs;
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
                    if (!Intersect3DUtils.HasCollision(rb1, rb2)) {
                        collisionService.RemoveCollision(rb1, rb2);
                        continue;
                    }

                    collisionService.AddCollision(rb1, rb2);
                }

            }
        }

        void RBAndStatic(Rigidbody3D rb) {
            var allServices = physicsFacade.Service;
            var collisionService = allServices.CollisionService;
            var idService = allServices.IDService;
            var cubeIDInfos = idService.cubeIDInfos;
            var cubes = physicsFacade.cubes;
            for (int j = 0; j < cubes.Length; j++) {
                if (!cubeIDInfos[j]) {
                    continue;
                }
                var cube = cubes[j];
                if(cube.IsTrigger){
                    continue;
                }
                if (!Intersect3DUtils.HasCollision(rb, cube)) {
                    collisionService.RemoveCollision(rb, cube);
                    continue;
                }

                collisionService.AddCollision(rb, cube);
            }
        }

    }

}