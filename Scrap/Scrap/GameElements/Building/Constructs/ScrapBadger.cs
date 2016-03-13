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
            torsoMiddle = new Crate(game, pos);
            torsoFront = new Crate(game, pos +  new Vector2(1.2f, 0));
            JoinEntities(torsoMiddle, torsoFront, new Point(1, 0), 0);

            torsoBack = new Crate(game, pos +  new Vector2(-1.2f, 0));
            JoinEntities(torsoMiddle, torsoBack, new Point(-1, 0), 0);

            backWheel = new Wheel(game, pos + new Vector2(-1.2f, 1.2f));
            JoinEntities(torsoBack, backWheel, new Point(-1, -1), 0);

            frontWheel = new Wheel(game, pos + new Vector2(1.2f, 1.2f));
            JoinEntities(torsoFront, frontWheel, new Point(1, -1), 0);

            rocket1 = new Rocket(game, pos + new Vector2(0, -1.2f));
            JoinEntities(torsoMiddle, rocket1, new Point(0,1), 0);
            KeyObject = torsoMiddle;

            game.camera.Follow(torsoMiddle, game.camera.Magnification);
            
        }
    }
}
