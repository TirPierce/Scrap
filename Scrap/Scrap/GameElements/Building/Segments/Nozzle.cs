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
    class Nozzle : Segment
    {
        public Nozzle(ScrapGame game)
            : base(game)
        {
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Crate"), 0, false);


        }

        public Nozzle(ScrapGame game, Vector2 position)
            : base(game)
        {
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Crate"), 0, false);

            body = BodyFactory.CreateRoundedRectangle(((ScrapGame)game).world, 1f, 1f, .2f, .2f, 5, 1f, this);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            
        }
        public override void Update(GameTime gameTime)
        {

        }


    }
}
