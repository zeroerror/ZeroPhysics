using System.Collections.Generic;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D.Domain {

    public class DataDomain {

        Physics3DFacade physicsFacade;

        public DataDomain() { }

        public void Inject(Physics3DFacade physicsFacade) {
            this.physicsFacade = physicsFacade;
        }

        public List<Cube> GetAllCubes() {
            var cubes = physicsFacade.cubes;
            var idService = physicsFacade.Service.IDService;
            var infos = idService.cubeIDInfos;
            var len = infos.Length;
            List<Cube> all = new List<Cube>();
            for (int i = 0; i < len; i++) {
                if (infos[i]) all.Add(cubes[i]);
            }
            return all;
        }

        public List<Rigidbody3D> GetAllRBs() {
            var rbCubees = physicsFacade.rbs;
            var idService = physicsFacade.Service.IDService;
            var infos = idService.rbIDInfos;
            var len = infos.Length;
            List<Rigidbody3D> all = new List<Rigidbody3D>();
            for (int i = 0; i < len; i++) {
                if (infos[i]) {
                    all.Add(rbCubees[i]);
                }
            }
            return all;
        }

    }

}