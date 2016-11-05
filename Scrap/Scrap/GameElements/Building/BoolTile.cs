using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Building
{
    class BoolTile:BehaviourTile
    {
        public Texture2D tileTexture;
        public Action<Boolean> action;
        public BoolTile(Action<Boolean> action, Texture2D texture)
        {
            tileTexture = texture;
            this.action = action;
        }
        public Texture2D TileTexture
        {
            get
            {
                return tileTexture;
            }

        }
    }
}
