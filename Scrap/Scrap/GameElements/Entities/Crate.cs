using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    public class Crate : Entity
    {
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
            body.Restitution = .1f;
            body.Friction = .9f;
        }
        public override Direction[] JointDirections()
        {
            Direction[] validDirections = { Direction.Up, Direction.Right };
            return validDirections;
        }
        public override void Update(GameTime gameTime)
        {
        }



    }
}
