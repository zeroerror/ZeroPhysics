using System;
using System.Collections.Generic;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D.API
{

    public class GetterAPI : IGetterAPI
    {

        Physics3DFacade facade;

        public GetterAPI() { }

        public void Inject(Physics3DFacade facade)
        {
            this.facade = facade;
        }

        List<Box3D> IGetterAPI.GetAllBoxes()
        {
            var domain = facade.Domain.DataDomain;
            return domain.GetAllBoxes();
        }

        List<Rigidbody3D_Box> IGetterAPI.GetAllRBBoxes()
        {
            var domain = facade.Domain.DataDomain;
            return domain.GetAllRBBoxes();
        }

    }

}