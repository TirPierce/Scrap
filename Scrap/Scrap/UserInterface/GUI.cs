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
    public class GUI
    {
        protected ScrapGame game;
        protected List<GameButton> buttons= new List<GameButton>();

        //buttons order by layer variable
        public GUI(ScrapGame game) 
        {
            this.game = game;
        }

        public void AddButton(Segment segment, Direction direction, Action callback)
        {
            buttons.Add(new GameButton(segment, direction, callback));
        }
        public void Init(ScrapGame game)
        {
            this.game = game;
        }
        public void Update(GameTime gameTime)
        {
            //mouse clicks are intercepted here and filtered throught the buttons before moving to the game screen
        }
        public void Draw(SpriteBatch batch)
        {
            foreach (GameButton button in buttons)
            {
                button.Draw(batch);
            }
            //batch.Draw(texture, body.WorldCenter, null, Color.White, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 0);
        }
    }
}
