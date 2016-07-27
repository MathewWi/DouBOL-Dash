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
 * This file includes names of objects.
 * It takes the object ID...and returns the string.
*/
namespace DouBOLDash
{
    class ObjectNames
    {
        public static string objectIDToString(int objectID)
        {
            if (objectID == 0009)
                return "Item Box";
            else if (objectID == 0010)
                return "Moving Item Box";
            else if (objectID == 4701)
                return "Thwomp";
            else if (objectID == 5008)
                return "Cactus Piranha Plant";
            else if (objectID == 5010)
                return "Dry Dry Desert Tree";
            else
                return "DOCUMENT ME";
        }
    }
}
