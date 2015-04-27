using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Scrap.UserInterface
{
    class StartScreen: BaseScreen
    {


        public StartScreen(Game game)
            : base(game)
        {

        }
        public override void Update(GameTime time)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            spriteBatch.Draw(mBackground, new Vector2(0, 0), Color.OliveDrab);
            //spriteBatch.End();

        }
        public override void LoadContent()
        {

            mBackground = Game.Content.Load<Texture2D>("Background/background");

        }


    }
}
