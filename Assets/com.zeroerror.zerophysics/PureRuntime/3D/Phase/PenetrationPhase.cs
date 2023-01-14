using System;
using System.Collections.Generic;
using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D.Facade;
using ZeroPhysics.Utils;

namespace ZeroPhysics.Physics3D {

    public class PenetrationPhase {

        Physics3DFacade physicsFacade;

        Queue<MTVModel> mtvModelQueue;

        public PenetrationPhase() {
            mtvModelQueue = new Queue<MTVModel>();
        }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public void Tick(in FP64 time) {
            var idService = physicsFacade.Service.IDService;
            var collisionService = physicsFacade.Service.CollisionService;
            var boxRBs = physicsFacade.boxRBs;
            var boxRBIDInfos = idService.boxRBIDInfos;
            var boxes = physicsFacade.boxes;
            var boxInfos = idService.boxIDInfos;

            // - RB & SB
            for (int i = 0; i < boxRBs.Length; i++) {
                if (!boxRBIDInfos[i]) continue;

                var rb = boxRBs[i];
                var rbBox = rb.Box;
                mtvModelQueue.Clear();

                for (int j = 0; j < boxes.Length; j++) {
                    if (!boxInfos[j]) continue;

                    var box = boxes[j];
                    if (!collisionService.TryGetCollision(rb, box, out var collision)) continue;
                    if (collision.CollisionType == Generic.CollisionType.Exit) continue;

                    // 确定摩擦力系数
                    var firctionCoe1 = rbBox.FrictionCoe;
                    var firctionCoe2 = box.FrictionCoe;
                    var firctionCoe_combined = firctionCoe1 < firctionCoe2 ? firctionCoe1 : firctionCoe2;
                    collision.SetFirctionCoe_combined(firctionCoe_combined);

                    // 计算MTV 有bug，一平地，一斜的强，撞击时因根据撞击方向也就是 mtv方向 ，还有在其方向上的 速度分量 才是正确的撞击方向即反弹速度方向
                    var mtv = Penetration3DUtils.GetMTV(rbBox.GetModel(), box.GetModel());
                    mtvModelQueue.Enqueue(new MTVModel { instanceID = box.InstanceID, mtv = mtv, collision = collision });
                }

                // 统计MTV
                FPVector3 mtv_final = FPVector3.Zero;
                FPVector3 beHitDir_final = FPVector3.Zero;
                while (mtvModelQueue.TryDequeue(out var mtvModel)) {
                    var mtv = mtvModel.mtv;
                    var instanceID = mtvModel.instanceID;
                    var collision = mtvModel.collision;

                    var box = boxes[instanceID];
                    var beHitDir = mtv.normalized;
                    collisionService.UpdateBeHitDir(rb, box, beHitDir);
                    mtv_final += mtv;
                    beHitDir_final -= FPVector3.Dot(rb.LinearV, beHitDir) * beHitDir;
                }

                // 交叉恢复处理
                mtv_final *= FPUtils.mtv_multy;
                rbBox.SetCenter(rbBox.Center + mtv_final);

                // 设置RB被撞击方向
                beHitDir_final.Normalize();
                rb.SetBeHitDir(beHitDir_final);
            }
        }

    }

}