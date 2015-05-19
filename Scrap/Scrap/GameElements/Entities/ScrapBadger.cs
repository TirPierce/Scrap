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
        DriveWheel backWheel;
        Wheel frontWheel;
        Crate torsoBack;
        Crate torsoMiddle;
        Crate torsoFront;
        Nozzle face;

        public ScrapBadger(ScrapGame game):base(game)
        {
            torsoBack = new Crate(game, new Vector2(22, -8));
            torsoMiddle = new Crate(game, new Vector2(23, -8));
            torsoFront = new Crate(game, new Vector2(24, -8));
            backWheel = new DriveWheel(game, new Vector2(22, -7));
            frontWheel = new Wheel(game, new Vector2(24, -7));

            JoinEntities(torsoBack, torsoMiddle, Entity.Direction.Right);
            JoinEntities(torsoMiddle, torsoFront, Entity.Direction.Right);
            JoinEntities(torsoBack, backWheel, Entity.Direction.Down);
            JoinEntities(torsoFront, frontWheel, Entity.Direction.Down);

            game.camera.Follow(torsoMiddle, game.camera.Magnification);

        }
    }
}
