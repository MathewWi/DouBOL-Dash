﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * RotationHax.cs
 * This file exists because MK:DD has the stupidest way of doing rotations.
 * It's an algorithm I don't feel like cracking. Yes, lazy me and lazy coding.
 * So we just define the rotation values here. Yay.
*/
namespace DouBOLDash
{
    class RotationHax
    {
        public static int returnRotations(uint xrot, uint yrot, uint zrot)
        {
            if (xrot == 0 && yrot == 655360000 && zrot == 655360000)
                return 0;
            else if (xrot == 463409500 && yrot == 463409500 && zrot == 655360000)
                return 45;
            else if (xrot == 655360000 && yrot == 0 && zrot == 655360000)
                return 90;
            else if (xrot == 0 && yrot == 3639607296 && zrot == 655360000)
                return 180;
            else if (xrot == 3831557796 && yrot == 3831557796 && zrot == 655360000)
                return 225;
            else if (xrot == 3639607296 && yrot == 0 && zrot == 655360000)
                return 270;
            else if (xrot == 3831557796 && yrot == 463409500 && zrot == 655360000)
                return 315;
            else
                return 0;
        }
    }
}
