using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Scrap.UserInterface
{
    abstract class BaseScreen : Microsoft.Xna.Framework.GameComponent
    {

        protected Texture2D mBackground;


        public BaseScreen(Game game):
            base(game)
        {


        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {


        }
        public virtual void LoadContent()
        {
        }



    }
}
