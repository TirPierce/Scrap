using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Building
{
    class AnalogueTile:BehaviourTile
    {
        public AnalogueTile(Action<float> action, Texture2D texture)
        {
            tileTexture = texture;
            this.action = action;
        }
        public Texture2D tileTexture;
        public Action<float> action;

        public Texture2D TileTexture
        {
            get
            {
                return tileTexture;
            }

        }
    }
}
