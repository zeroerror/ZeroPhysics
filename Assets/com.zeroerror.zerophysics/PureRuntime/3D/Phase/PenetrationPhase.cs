using System;
using System.Collections.Generic;
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
            var idService = physicsFacade.Service.IDService;
            var collisionService = physicsFacade.Service.CollisionService;
            var boxRBs = physicsFacade.boxRBs;
            var boxes = physicsFacade.boxes;
            var boxRBIDInfos = idService.boxRBIDInfos;

            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

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
                    if (!collisionService.TryGetCollision(rb1, rb2, out var collision)) {
                        continue;
                    }
                    if (collision.CollisionType == Generic.CollisionType.Exit) {
                        continue;
                    }

                    // 确定摩擦力系数
                    var rbBox2 = rb2.Box;
                    var firctionCoe1 = rbBox1.FrictionCoe;
                    var firctionCoe2 = rbBox2.FrictionCoe;
                    var firctionCoe_combined = firctionCoe1 < firctionCoe2 ? firctionCoe1 : firctionCoe2;
                    collision.SetFirctionCoe_combined(firctionCoe_combined);

                    // 计算MTV
                    var mtv = Penetration3DUtils.GetMTV(rbBox1.GetModel(), rbBox2.GetModel());
                    var mtv_half = mtv * FP64.Half;
                    rb1.AddMTV(mtv_half);
                    rb2.AddMTV(-mtv_half);
                }

                // 交叉恢复处理
                rb1.ApplyMTV();
            }

        }

        void RBNSB(Box3DRigidbody rb) {
            var idService = physicsFacade.Service.IDService;
            var collisionService = physicsFacade.Service.CollisionService;
            var boxInfos = idService.boxIDInfos;
            var boxes = physicsFacade.boxes;
            var rbBox = rb.Box;
            var firctionCoe1 = rbBox.FrictionCoe;
            var rbBoxModel = rbBox.GetModel();
            for (int j = 0; j < boxes.Length; j++) {
                if (!boxInfos[j]) {
                    continue;
                }

                var box = boxes[j];
                if (!collisionService.TryGetCollision(rb, box, out var collision)) {
                    continue;
                }
                if (collision.CollisionType == Generic.CollisionType.Exit) {
                    continue;
                }

                // 确定摩擦力系数
                var firctionCoe2 = box.FrictionCoe;
                var firctionCoe_combined = firctionCoe1 < firctionCoe2 ? firctionCoe1 : firctionCoe2;
                collision.SetFirctionCoe_combined(firctionCoe_combined);

                // 计算MTV
                var mtv = Penetration3DUtils.GetMTV(rbBoxModel, box.GetModel());
                var beHitDir = mtv.normalized;
                rb.AddMTV(mtv);
                collisionService.UpdateBHitA_Dir(rb, box, beHitDir);
            }
        }

    }

}