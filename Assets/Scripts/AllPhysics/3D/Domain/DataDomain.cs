using System.Collections.Generic;
using ZeroPhysics.AllPhysics.Physics3D.Facade;

namespace ZeroPhysics.AllPhysics.Physics3D.Domain
{

    public class DataDomain
    {

        Physics3DFacade facade;

        public DataDomain() { }

        public void Inject(Physics3DFacade facade)
        {
            this.facade = facade;
        }

        public List<Box3D> GetAllBoxes()
        {
            var boxes = facade.boxes;
            var idService = facade.IDService;
            var infos = idService.boxIDInfos;
            var len = infos.Length;
            List<Box3D> all = new List<Box3D>();
            for (int i = 0; i < len; i++)
            {
                if (infos[i]) all.Add(boxes[i]);
            }
            return all;
        }

        public List<Rigidbody3D_Box> GetAllRBBoxes()
        {
            var rbBoxes = facade.rb_boxes;
            var idService = facade.IDService;
            var infos = idService.rbBoxIDInfos;
            var len = infos.Length;
            List<Rigidbody3D_Box> all = new List<Rigidbody3D_Box>();
            for (int i = 0; i < len; i++)
            {
                if (infos[i]) all.Add(rbBoxes[i]);
            }
            return all;
        }

    }

}