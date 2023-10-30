using System;
using System.Collections.Generic;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics.Context;

namespace ZeroPhysics.Physics.API
{

    public class GetterAPI : IGetterAPI
    {

        PhysicsContext physicsContext;

        public GetterAPI() { }

        public void Inject(PhysicsContext physicsContext)
        {
            this.physicsContext = physicsContext;
        }

        List<Box> IGetterAPI.GetAllCubes()
        {
            var domain = physicsContext.Domain.DataDomain;
            return domain.GetAllCubes();
        }

        List<Rigidbody> IGetterAPI.GetAllCubeRBs()
        {
            var domain = physicsContext.Domain.DataDomain;
            return domain.GetAllRBs();
        }

        CollisionModel[] IGetterAPI.GetAllCollisions_RS()
        {
            var collisionService = physicsContext.Service.CollisionService;
            return collisionService.GetAllCollisions_RS();
        }
    }

}