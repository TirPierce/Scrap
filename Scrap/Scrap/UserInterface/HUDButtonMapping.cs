using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.UserInterface
{
    struct HUDRecievingTile
    {
        public HUDRecievingTile(Rectangle placementArea, Texture2D buttonFace)
        {
            this.placementArea = placementArea;
            this.buttonFace = buttonFace;
        }
        public Rectangle placementArea;
        public Texture2D buttonFace;
    }

    class HUDButtonMapping
    {
        Dictionary<String, HUDRecievingTile> recievingTiles = new Dictionary<string, HUDRecievingTile>();
        Rectangle stickRight;
        Rectangle stickLeft;
        Rectangle triggerRight;
        Rectangle triggerLeft;

        Rectangle buttonA;
        Rectangle buttonB;
        Rectangle buttonC;
        Rectangle buttonD;

        Texture2D backgroundTexture;
        Construct construct;
        private SpriteFont font;
        ScrapGame game;
        public HUDButtonMapping(Construct construct, ScrapGame game)
        {
            this.game = game;
            this.construct = construct;
            backgroundTexture = game.Content.Load<Texture2D>("HUDBackground");


            Texture2D texture = game.Content.Load<Texture2D>("Buttons/ButtonA");
            recievingTiles.Add("ButtonA", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 110,50, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/ButtonB");
            recievingTiles.Add("ButtonB", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 50, 50, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/ButtonX");
            recievingTiles.Add("ButtonX", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 110, 110, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/ButtonY");
            recievingTiles.Add("ButtonY", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 50, 110, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/StickLeft");
            recievingTiles.Add("StickLeft", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 110, 170, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/StickRight");
            recievingTiles.Add("StickRight", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 50, 170, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/TriggerLeft");
            recievingTiles.Add("TriggerLeft", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 110, 230, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/TriggerRight");
            recievingTiles.Add("TriggerRight", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 50, 230, 50, 50), texture));
        }
        public void Update()
        {


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (HUDRecievingTile tile in recievingTiles.Values)
            {
                spriteBatch.Draw(this.backgroundTexture,tile.placementArea, null, Color.White, 0, this.backgroundTexture.Bounds.Center.ToVector2(), SpriteEffects.None, 1);
                spriteBatch.Draw(tile.buttonFace, new Rectangle(tile.placementArea.X-20, tile.placementArea.Y-20, 20, 20), null, Color.White, 0, tile.buttonFace.Bounds.Center.ToVector2(), SpriteEffects.None, .9f);
                //spriteBatch.Draw(tile.buttonFace, tile.placementArea., null, Color.White, 0, tile.placementArea.Center.ToVector2(), SpriteEffects.None, .9f);

            }

        }
    }
}
