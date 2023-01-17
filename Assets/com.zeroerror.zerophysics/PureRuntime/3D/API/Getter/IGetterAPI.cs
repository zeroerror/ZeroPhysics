using System;
using System.Collections.Generic;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D.API
{

    public interface IGetterAPI
    {

        List<Rigidbody3D> GetAllCubeRBs();
        List<Cube> GetAllCubes();
        CollisionModel[] GetCollisionInfos();

    }

}