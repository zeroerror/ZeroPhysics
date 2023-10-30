using System.Collections.Generic;
using ZeroPhysics.Physics.Context;

namespace ZeroPhysics.Physics.Domain {

    public class DataDomain {

        PhysicsContext physicsContext;

        public DataDomain() { }

        public void Inject(PhysicsContext physicsContext) {
            this.physicsContext = physicsContext;
        }

        public List<Box> GetAllCubes() {
            var cubes = physicsContext.cubes;
            var idService = physicsContext.Service.IDService;
            var infos = idService.cubeIDInfos;
            var len = infos.Length;
            List<Box> all = new List<Box>();
            for (int i = 0; i < len; i++) {
                if (infos[i]) all.Add(cubes[i]);
            }
            return all;
        }

        public List<Rigidbody> GetAllRBs() {
            var rbCubees = physicsContext.rbs;
            var idService = physicsContext.Service.IDService;
            var infos = idService.rbIDInfos;
            var len = infos.Length;
            List<Rigidbody> all = new List<Rigidbody>();
            for (int i = 0; i < len; i++) {
                if (infos[i]) {
                    all.Add(rbCubees[i]);
                }
            }
            return all;
        }

    }

}