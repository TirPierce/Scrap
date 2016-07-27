using Microsoft.Xna.Framework;
using Scrap.GameElements.Building;
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
        //load Level objects
        Player torsoMiddle;

        Crate torsoBack;        
        Crate torsoFront;
        Crate torsoFront2;
        Crate torsoFront3;
        DriveWheel backWheel;
        Wheel frontWheel;
        Wheel frontWheel2;
        Wheel frontWheel3;
        Rocket rocket1;
        Rocket rocket2;
        Nozzle face;

        public ScrapBadger(ScrapGame game, Vector2 pos):base(game)
        {
            //Define joins, Define Join side, Define joined srap direction//

            //Player
            torsoMiddle = new Player(game, pos);
            KeyObject = torsoMiddle;
            /**********************             
             /-1-1/01/1-1/
             /-10/00/10/
             /-11/01/11/
             ***********************/
            //Crates
            torsoFront = new Crate(game, pos +  new Vector2(1.2f, 0));
            AddNewSegmentToConstruct(torsoMiddle, torsoFront, new Point(1, 0), Direction.Up);            

            torsoFront2 = new Crate(game, pos + new Vector2(1.2f, 0));
            AddNewSegmentToConstruct(torsoFront, torsoFront2, new Point(1, 0), Direction.Up);

            torsoFront3 = new Crate(game, pos + new Vector2(1.2f, 0));
            AddNewSegmentToConstruct(torsoFront2, torsoFront3, new Point(1, 0), Direction.Up);

            torsoBack = new Crate(game, pos + new Vector2(-1.2f, 0));
            AddNewSegmentToConstruct(torsoMiddle, torsoBack, new Point(-1, 0), Direction.Up);

            //Wheels
            backWheel = new DriveWheel(game, pos + new Vector2(-1.2f, 1.2f));
            AddNewSegmentToConstruct(torsoBack, backWheel, new Point(0, 1), Direction.Up);

            frontWheel = new Wheel(game, pos + new Vector2(1.2f, 1.2f));
            AddNewSegmentToConstruct(torsoFront, frontWheel, new Point(0, 1), Direction.Up);

            frontWheel2 = new Wheel(game, pos + new Vector2(1.2f, 1.2f));
            AddNewSegmentToConstruct(torsoFront2, frontWheel2, new Point(0, 1), Direction.Up);

            frontWheel3 = new Wheel(game, pos + new Vector2(1.2f, 1.2f));
            AddNewSegmentToConstruct(torsoFront3, frontWheel3, new Point(0, 1), Direction.Up);

            //Rockets
            rocket1 = new Rocket(game, pos + new Vector2(0, -1.2f));
            AddNewSegmentToConstruct(torsoMiddle, rocket1, new Point(0, -1), Direction.Left);

            rocket2 = new Rocket(game, pos + new Vector2(0, -1.2f));
            AddNewSegmentToConstruct(torsoFront, rocket2, new Point(0, -1), Direction.Left);

                        //ToDo:gameplay hack
            this.game.playerController.leftTrigger.Add(rocket1.constructElement.offSet,rocket1.AnalogueInputCallback);
            this.game.playerController.rightTrigger.Add(rocket2.constructElement.offSet, rocket2.AnalogueInputCallback);
            this.game.playerController.leftTrigger.Add(backWheel.constructElement.offSet, backWheel.AnalogueInputCallback);
            


            //init sensors
            foreach(var constructElement in this.buildElements)
            {

                constructElement.Value.DisableSensors();
                constructElement.Value.EnableSensors();
            }

            game.camera.Follow(torsoMiddle, game.camera.Magnification);
            
        }
    }
}
