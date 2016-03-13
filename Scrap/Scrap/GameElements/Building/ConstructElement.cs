using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Scrap.GameElements.GameWorld;
using Scrap.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{

    public enum ElementStatus { Locked, Selected, Attached, Free };
    public class ConstructElement
    {
        public const float OFFSET = 1.2f;
        public Segment segment;
        public Construct construct;
        ScrapGame game;
        public Point offSet;
        public Joint rootJoint;
        public List<Joint> branchJoints;
        private List<GameButton> gameButtons = new List<GameButton>();
        public Dictionary<Direction, ConstructElement> adjacentElements = new Dictionary<Direction, ConstructElement>();
        List<Fixture> sensors = new List<Fixture>();
        ElementStatus status = ElementStatus.Free;
        private bool sensorsAdded = false; //ToDo: fix hack to solve issue where body is not initialised when addSensors is called.
        public ElementStatus Status
        {
            get { return status; }
            set { SetStatus(value); }
        }
        private void SetStatus(ElementStatus status)
        {
            if (status == ElementStatus.Locked)
            {
                EnableButtons();

            }
            else
            {
                DisableButtons();
            }

            this.status = status;
        }

        public ConstructElement(ScrapGame game, Segment entity)
        {
            this.segment = entity;
            this.game = game;
            branchJoints = new List<Joint>();
            GenerateGUIButtons();
            //AddSensors();
        }
        public void AddToConstruct(Construct construct, Point offSet, Joint rootJoint)
        {
            this.rootJoint = rootJoint;
            this.construct = construct;
            this.offSet = offSet; 
            adjacentElements = construct.AdjacentElements(offSet);
        }

        public void RemoveFromConstruct()
        {
            if (construct != null)
            {
                BreakRoot();
                BreakBranch();
                if (construct.buildElements.ContainsKey(offSet))
                    construct.buildElements.Remove(offSet);
                segment.constructElement.construct = null;
                
            }
            adjacentElements.Clear();
        }
        public static Vector2 GetSensorOffset(Scrap.GameElements.Entities.Direction direction)
        {
            Vector2 offset = Vector2.Zero;
            if (direction == Direction.Right)
            {
                offset = Vector2.UnitX * OFFSET;

            }
            if (direction == Direction.Left)
            {
                offset = Vector2.UnitX * -OFFSET;

            }
            if (direction == Direction.Down)
            {
                offset = Vector2.UnitY * -OFFSET;

            }
            if (direction == Direction.Up)
            {
                offset = Vector2.UnitY * OFFSET;

            }
            return offset;
        }
        public void SetValidAdjacentElements(List<ConstructElement> validElements)
        {

        }
        public void EnableButtons()
        {
            foreach (GameButton button in gameButtons)
            {
                button.status = UIStatus.Active;
            }
        }
        public void DisableButtons()
        {
            foreach (GameButton button in gameButtons)
            {
                button.status = UIStatus.Inactive;
            }
        }
        public void BreakBranch()
        {
            foreach (Joint item in branchJoints)
            {
                game.world.RemoveJoint(item);
            }
        }
        private void PlaceSegmentLeft()//temp function
        {
            construct.SetSegmentDirection(segment, MathHelper.ToRadians(270));

            SetStatus(ElementStatus.Attached);
        }
        private void PlaceSegmentRight()//temp function
        {
            construct.SetSegmentDirection(segment, MathHelper.ToRadians(90));
            SetStatus(ElementStatus.Attached);
        }
        private void PlaceSegmentUp()//temp function
        {
            construct.SetSegmentDirection(segment, MathHelper.ToRadians(0));
            SetStatus(ElementStatus.Attached);
        }
        private void PlaceSegmentDown()//temp function
        {
            construct.SetSegmentDirection(segment, MathHelper.ToRadians(180));
            SetStatus(ElementStatus.Attached);
        }
        public void EnableSensors()
        {
            AddSensors();
        }
        public void DisableSensors()
        {
            foreach (Fixture sensor in sensors)
            {
                sensor.Dispose();
            }
        }
        private void AddSensors()
        {
            Fixture sensorFixture;
            Vertices verts;
            foreach (var direction in segment.JointDirections())
            {
                switch (direction)
                {
                    case Direction.Left:
                        verts = new Vertices();
                        verts.Add(new Vector2(-.6f, -.4f));
                        verts.Add(new Vector2(-.6f, .4f));
                        verts.Add(new Vector2(-1.2f, 0f));
                        break;
                    case Direction.Right:
                        verts = new Vertices();
                        verts.Add(new Vector2(1.2f, 0f));
                        verts.Add(new Vector2(.6f, .4f));
                        verts.Add(new Vector2(.6f, -.4f));
                        break;
                    case Direction.Up:
                        verts = new Vertices();
                        verts.Add(new Vector2(-.4f, -.6f));
                        verts.Add(new Vector2(0f, -1.2f));
                        verts.Add(new Vector2(.4f, -.6f));
                        break;
                    case Direction.Down:
                        verts = new Vertices();
                        verts.Add(new Vector2(.4f, .6f));
                        verts.Add(new Vector2(0f, 1.2f));
                        verts.Add(new Vector2(-.4f, .6f));
                        break;
                    default:
                        verts = new Vertices();
                        break;
                }
                sensorFixture = FixtureFactory.AttachPolygon(verts, 0f, segment.body, direction);

                sensors.Add(sensorFixture);
                sensorFixture.OnCollision += OnCollide;
                sensorFixture.CollidesWith = Category.Cat10;
                sensorFixture.CollisionCategories = Category.Cat10;

            }

        }
        public bool OnCollide(Fixture a, Fixture b, Contact contact)
        {

            if (this.game.playerController.selectedSegment != null)
            {
                game.playerController.OnConstructSensorTriggered(this, Construct.DirectionToPoint((Direction)a.UserData) + this.offSet);
            }
            return true;

        }
        private void GenerateGUIButtons()
        {
            foreach (Direction direction in segment.JointDirections())//The 
            {
                switch (direction)
                {
                    case Direction.Left:
                        gameButtons.Add(
                        this.game.gui.AddButton(this.segment, direction, new Action(PlaceSegmentLeft)));
                        break;
                    case Direction.Right:
                        gameButtons.Add(
                        this.game.gui.AddButton(this.segment, direction, new Action(PlaceSegmentRight)));
                        break;
                    case Direction.Up:
                        gameButtons.Add(
                        this.game.gui.AddButton(this.segment, direction, new Action(PlaceSegmentUp)));
                        break;
                    case Direction.Down:
                        gameButtons.Add(
                        this.game.gui.AddButton(this.segment, direction, new Action(PlaceSegmentDown)));
                        break;
                    default:
                        break;
                }

            }
        }
        public void BreakRoot()
        {
            if (rootJoint != null)
                game.world.RemoveJoint(rootJoint);

        }
        public bool Draggable() 
        {
            //Todo:check grid to see if draggable
            if (construct != null && construct.KeyObject.constructElement == this)
                return false;
            return true;
        }
        public void Update()
        {

        }
    }

}