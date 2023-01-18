using System;
using System.Collections.Generic;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D.API
{

    public class GetterAPI : IGetterAPI
    {

        Physics3DFacade physicsFacade;

        public GetterAPI() { }

        public void Inject(Physics3DFacade physicsFacade)
        {
            this.physicsFacade = physicsFacade;
        }

        List<Cube> IGetterAPI.GetAllCubes()
        {
            var domain = physicsFacade.Domain.DataDomain;
            return domain.GetAllCubes();
        }

        List<Rigidbody3D> IGetterAPI.GetAllCubeRBs()
        {
            var domain = physicsFacade.Domain.DataDomain;
            return domain.GetAllRBs();
        }

        CollisionModel[] IGetterAPI.GetAllCollisions_RS()
        {
            var collisionService = physicsFacade.Service.CollisionService;
            return collisionService.GetAllCollisions_RS();
        }
    }

}