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
        public HUDConstruct(Construct construct, Game game)
        {
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
                spriteBatch.DrawString(font, currentSegment.constructElement.mappedKey.ToString(), new Vector2((index.X + xMin * -1) * 25 + 15, (index.Y + yMin * -1) * 25 + 15), Color.Cyan, 0, font.MeasureString(currentSegment.constructElement.mappedKey.ToString()) * .5f, .2f, SpriteEffects.None, 0);
            
                //spriteBatch.Draw(texture, new Rectangle((index.X + xMin * -1) * 25, (index.Y + yMin * -1) * 25, 20, 20), null, Color.White);
                //spriteBatch.Draw(construct.buildElements[index].segment.sprite.Texture, new Rectangle((index.X + xMin * -1) * 25, (index.Y + yMin * -1) * 25, 20, 20), null, Color.White);
                spriteBatch.Draw(currentSegment.sprite.Texture, new Rectangle((index.X + xMin * -1) * 25+15, (index.Y + yMin * -1) * 25+15, 20, 20), null, Color.White, Orientation.DirectionToRadians(currentSegment.constructElement.orientation), new Vector2(currentSegment.sprite.Texture.Width/2, currentSegment.sprite.Texture.Height/2), SpriteEffects.None, 1);
            }
        }
    }
}
