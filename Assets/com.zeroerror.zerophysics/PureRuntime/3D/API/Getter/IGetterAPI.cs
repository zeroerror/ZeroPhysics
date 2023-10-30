using System;
using System.Collections.Generic;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics.API
{

    public interface IGetterAPI
    {

        List<Rigidbody> GetAllCubeRBs();
        List<Box> GetAllCubes();
        CollisionModel[] GetAllCollisions_RS();

    }

}