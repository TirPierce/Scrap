using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{
    class Crate : PhysicalEntity
    {
        Body body;
        public Crate(ScrapGame game)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>("Crate");
            
            
        }

        public Crate(ScrapGame game, Vector2 position)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>("Crate");

            body = BodyFactory.CreateRoundedRectangle(((ScrapGame)game).world,1f,1f,.2f,.2f,5,1f);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
        }
        public override void Update(GameTime gameTime)
        {

        }


        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, body.WorldCenter, null, Color.White, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 0);
            
            //batch.Draw(texture, body.Position, null, Color.White);
        }
    }
}
