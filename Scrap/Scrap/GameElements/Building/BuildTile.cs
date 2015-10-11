using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Building
{
    public abstract class BuildTile
    {
        public Point offSet;
        public abstract void UpdateNeighbours();
    }
}
