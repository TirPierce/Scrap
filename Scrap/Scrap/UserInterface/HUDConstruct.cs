using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Building;
using Scrap.GameElements.Entities;
using Scrap.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.UserInterface
{
    class HUDConstruct
    {
        Texture2D texture;
        Construct construct;
        private SpriteFont font;
        ScrapGame game;
        public HUDConstruct(Construct construct, ScrapGame game)
        {
            this.game = game;
            this.construct = construct;
            texture = game.Content.Load<Texture2D>("Player");
            font = game.Content.Load<SpriteFont>("Keys");
        }
        public void Update()
        {

            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            int xMin = construct.buildElements.Keys.Min(point => point.X);
            int yMin = construct.buildElements.Keys.Min(point => point.Y);
            Segment currentSegment;
            foreach (Point index in construct.buildElements.Keys)
            {
                currentSegment= construct.buildElements[index].segment;
                String inputText = "";
                if (this.game.playerController.leftTrigger.ContainsKey(index))
                    inputText = "L";
                else if (this.game.playerController.rightTrigger.ContainsKey(index))
                    inputText = "R";
                spriteBatch.DrawString(font, inputText, new Vector2((index.X + xMin * -1) * 25 + 15, (index.Y + yMin * -1) * 25 + 15), Color.Red, 0, font.MeasureString(currentSegment.constructElement.mappedKey.ToString()) * .5f, .3f, SpriteEffects.None, 0);
            
                //spriteBatch.Draw(texture, new Rectangle((index.X + xMin * -1) * 25, (index.Y + yMin * -1) * 25, 20, 20), null, Color.White);
                //spriteBatch.Draw(construct.buildElements[index].segment.sprite.Texture, new Rectangle((index.X + xMin * -1) * 25, (index.Y + yMin * -1) * 25, 20, 20), null, Color.White);
                spriteBatch.Draw(currentSegment.sprite.Texture, new Rectangle((index.X + xMin * -1) * 25+15, (index.Y + yMin * -1) * 25+15, 20, 20), null, Color.White, Orientation.DirectionToRadians(currentSegment.constructElement.orientation), new Vector2(currentSegment.sprite.Texture.Width/2, currentSegment.sprite.Texture.Height/2), SpriteEffects.None, 1);
            }
        }
    }
}
