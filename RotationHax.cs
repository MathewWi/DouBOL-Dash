/*
    Copyright 2016 MrRean

    This file is part of DouBOLDash.
    DouBOLDash is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published by the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.

    DouBOLDash is distributed in the hope that it will be useful, but WITHOUT ANY 
    WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
    FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along 
    with DouBOLDash. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * RotationHax.cs
 * This is a function that returns the proper rotations.
 * Thanks to Marionumber1 for helping me with this part.
*/
namespace DouBOLDash
{
    class RotationHax
    {
        public static double returnRotations(int xrot, int yrot, int zrot)
        {
            double angle1;
            double radToDeg = 57.2957795;
            angle1 = radToDeg * Math.Atan2((double)yrot / (double)655360000, (double)xrot / (double)655360000);
            angle1 = 90 - angle1;

            return Math.Round(angle1);
        }
    }
}
