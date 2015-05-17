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
    class DriveWheel : PhysicalEntity
    {

        public DriveWheel(ScrapGame game)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>("wheel");


        }

        public DriveWheel(ScrapGame game, Vector2 position)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>("wheel");

            body = BodyFactory.CreateCircle(game.world, .5f, 1f);
            body.BodyType = BodyType.Dynamic;
            body.Position =  position;
        }
        public override void Update(GameTime gameTime)
        {

        }



    }
}
