using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    class ScrapBadger:Construct
    {
        Wheel backWheel;
        Wheel frontWheel;
        Crate torsoBack;
        Crate torsoMiddle;
        Crate torsoFront;
        Rocket rocket1;
        Nozzle face;

        public ScrapBadger(ScrapGame game, Vector2 pos):base(game)
        {
            torsoBack = new Crate(game, pos + new Vector2(-1,0));
            torsoMiddle = new Crate(game, pos);
            torsoFront = new Crate(game, pos + new Vector2(1, 0));
            backWheel = new Wheel(game, pos + new Vector2(-1, 1));
            frontWheel = new Wheel(game, pos + new Vector2(1, 1));
            rocket1 = new Rocket(game, pos + new Vector2(0, -1));

            KeyObject = torsoMiddle;
            JoinEntities(torsoMiddle, torsoFront, Direction.Right);
            JoinEntities(torsoMiddle, rocket1, Direction.Up);
            JoinEntities(torsoMiddle, torsoBack, Direction.Left);

            JoinEntities(torsoBack, backWheel, Direction.Down);
            //backWheel.body.UserData = this;
            //frontWheel.body.UserData = this;
            JoinEntities(torsoFront, frontWheel, Direction.Down);
            

            game.camera.Follow(torsoMiddle, game.camera.Magnification);
            
        }
    }
}
