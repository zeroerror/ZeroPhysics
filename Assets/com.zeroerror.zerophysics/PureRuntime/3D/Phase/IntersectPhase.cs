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
            var boxRBs = physicsFacade.boxRBs;
            var boxRBIDInfos = idService.boxRBIDInfos;

            for (int i = 0; i < boxRBs.Length - 1; i++) {

                if (!boxRBIDInfos[i]) {
                    continue;
                }

                var rb1 = boxRBs[i];
                var rbBox1 = rb1.Box;
                // RB & SB
                RBNSB(rb1);
                // RB & RB
                for (int j = i + 1; j < boxRBs.Length; j++) {
                    if (!boxRBIDInfos[j]) {
                        continue;
                    }
                    var rb2 = boxRBs[j];
                    var rbBox2 = rb2.Box;
                    if (!Intersect3DUtils.HasCollision(rbBox1, rbBox2)) {
                        collisionService.RemoveCollision(rb1, rb2);
                        continue;
                    }

                    collisionService.AddCollision(rb1, rb2);
                }

            }
        }

        void RBNSB(Box3DRigidbody rb) {
            var allServices = physicsFacade.Service;
            var collisionService = allServices.CollisionService;
            var idService = allServices.IDService;
            var boxInfos = idService.boxIDInfos;
            var boxes = physicsFacade.boxes;
            var rbBox = rb.Box;
            for (int j = 0; j < boxes.Length; j++) {
                if (!boxInfos[j]) {
                    continue;
                }

                var box = boxes[j];
                if (!Intersect3DUtils.HasCollision(rbBox, box)) {
                    collisionService.RemoveCollision(rb, box);
                    continue;
                }

                collisionService.AddCollision(rb, box);
            }
        }

    }

}