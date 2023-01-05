using System;
using System.Collections.Generic;

namespace ZeroPhysics.Physics3D.API
{

    public interface IGetterAPI
    {

        List<Rigidbody3D_Box> GetAllRBBoxes();
        List<Box3D> GetAllBoxes();

    }

}