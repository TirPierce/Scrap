using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Scrap.GameElements.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{

    public class ConstructElement
    {
        public const float OFFSET = 1.2f;

        public Segment entity;
        public Construct construct;
        Body body;
        ScrapGame game;
        public Point offSet;
        private Joint rootJoint;
        public List<Joint> branchJoints;

        public ConstructElement(ScrapGame game, Segment entity, Construct construct, Point offSet, Joint rootJoint)
        {
            branchJoints = new List<Joint>(); 
            this.rootJoint = rootJoint;
            this.entity = entity;
            this.game = game;
            this.construct = construct;
            this.offSet = offSet;

            AddSensors();

            //JointFactory.CreateWeldJoint(game.world, entity.body, body, new Vector2( 0,0), new Vector2(0, 0));
        }

        public void RemoveFromConstruct()
        {
            if (construct != null)
            {
                BreakRoot();
                BreakBranch();
                if (construct.buildElements.ContainsKey(offSet))
                    construct.buildElements.Remove(offSet);
                entity.constructElement.construct = null;
            }
        }
        public void BreakBranch()
        {
            foreach (Joint item in branchJoints)
            {
                item.Breakpoint = 0;
            }
        }

        public void BreakRoot()
        {
            if (construct != null)
            {
                if (construct.buildElements.ContainsKey(new Point(1, 0) + this.offSet))
                    construct.buildElements[new Point(1, 0) + this.offSet].AddSensors();

                if (construct.buildElements.ContainsKey(new Point(-1, 0) + this.offSet))
                    construct.buildElements[new Point(-1, 0) + this.offSet].AddSensors();

                if (construct.buildElements.ContainsKey(new Point(0, 1) + this.offSet))
                    construct.buildElements[new Point(0, 1) + this.offSet].AddSensors();

                if (construct.buildElements.ContainsKey(new Point(0, -1) + this.offSet))
                    construct.buildElements[new Point(0, -1) + this.offSet].AddSensors();
            }


            this.AddSensors();
            if (rootJoint != null)
                rootJoint.Breakpoint = 0;

        }
        public bool OnCollide(Fixture a, Fixture b, Contact contact)
        {
            if (b.Body != this.body)
            {
                if (construct != null && game.playerController.selectedSegment != null)
                {
                    if (game.playerController.selectedSegment.body == b.Body)
                    {
                        if (a.UserData is Scrap.GameElements.Entities.Segment.Direction)
                        {
                            Vector2 target = GetSensorOffset((Scrap.GameElements.Entities.Segment.Direction)a.UserData);
                            Transform t;
                            a.Body.GetTransform(out t);
                            target = Vector2.Transform(target, Quaternion.CreateFromAxisAngle(Vector3.Backward, a.Body.Rotation));
                            target += a.Body.WorldCenter;
                            //b.Body.ApplyLinearImpulse(((target - b.Body.Position) / (target - b.Body.Position).Length()) * .1f);

                            //b.Body.Position = MathHelper.Lerp(b.Body.Position, a.Body.Position, .03f);
                            //ToDo: Use lerp to move selected block
                            //b.Body.Rotation = MathHelper.Lerp(b.Body.Rotation, a.Body.Rotation, .03f);
                            //b.Body.AngularVelocity = 0f;

                            if ((target - b.Body.Position).Length() < .5f)
                            {
                                b.Body.Rotation = a.Body.Rotation;
                                b.Body.Position = target;
                                b.Body.LinearVelocity = Vector2.Zero;

                                foreach (Segment item in game.entityList)
                                {
                                    if (item.body == b.Body)
                                    {
                                        //Todo: join code revamp needed
                                        //if (!construct.ContainsFixture(b))
                                       // {
                                            game.playerController.PlaceSegment();
                                            construct.JoinEntities(this, item, (Scrap.GameElements.Entities.Segment.Direction)a.UserData);
                                       // }
                                    }
                                }


                            }
                        }
                    }
                }
            }

            return false;
        }
        public bool Draggable() 
        {
            //Todo:check grid to see if draggable
            if (construct != null && construct.KeyObject.constructElement == this)
                return false;
            return true;
        }
        private Vector2 GetSensorOffset(Scrap.GameElements.Entities.Segment.Direction direction)
        {
            Vector2 offset = Vector2.Zero;
            if (direction == Segment.Direction.Right)
            {
                offset = Vector2.UnitX * OFFSET;

            }
            if (direction == Segment.Direction.Left)
            {
                offset = Vector2.UnitX * -OFFSET;

            }
            if (direction == Segment.Direction.Down)
            {
                offset = Vector2.UnitY * OFFSET;

            }
            if (direction == Segment.Direction.Up)
            {
                offset = Vector2.UnitY * -OFFSET;

            }
            return offset;
        }
        public void AddSensors()
        {
            if(body!=null)
                game.world.RemoveBody(body);

            body = BodyFactory.CreateRectangle(game.world, 1.0f, 1.0f, 1.1f, this);
            body.Position = entity.Position;
            body.IsSensor = true;
            body.DestroyFixture(body.FixtureList[0]);
            
            if (this.construct != null)
            {

                Fixture sensorFixture;
                Vertices verts;
                ConstructElement element;
                bool exists = false;
                foreach (var direction in entity.JointDirections())
                {
                    switch (direction)
                    {
                        case Segment.Direction.Left:

                            exists = this.construct.buildElements.TryGetValue(this.offSet + new Point(-1, 0), out element);
                            if (exists)
                                continue;
                            verts = new Vertices();
                            verts.Add(new Vector2(-.6f, -.6f));
                            verts.Add(new Vector2(-.6f, .6f));
                            verts.Add(new Vector2(-1.2f, 0f));
                            break;
                        case Segment.Direction.Right:

                            exists = this.construct.buildElements.TryGetValue(this.offSet + new Point(1, 0), out element);
                            if (exists)
                                continue;
                            verts = new Vertices();
                            verts.Add(new Vector2(1.2f, 0f));
                            verts.Add(new Vector2(.6f, .6f));
                            verts.Add(new Vector2(.6f, -.6f));

                            break;
                        case Segment.Direction.Up:
                            exists = this.construct.buildElements.TryGetValue(this.offSet + new Point(0, 1), out element);
                            if (exists)
                                continue;

                            verts = new Vertices();

                            verts.Add(new Vector2(.6f, .6f));
                            verts.Add(new Vector2(0f, 1.2f));
                            verts.Add(new Vector2(-.6f, .6f));

                            break;
                        case Segment.Direction.Down:
                            exists = this.construct.buildElements.TryGetValue(this.offSet + new Point(0, -1), out element);
                            if (exists)
                                continue;

                            verts = new Vertices();


                            verts.Add(new Vector2(-.6f, -.6f));
                            verts.Add(new Vector2(0f, -1.2f));
                            verts.Add(new Vector2(.6f, -.6f));
                            break;
                        default:
                            verts = new Vertices();
                            break;
                    }
                    sensorFixture = FixtureFactory.AttachPolygon(verts, 1f, body, direction);


                    sensorFixture.OnCollision += OnCollide;
                    sensorFixture.IsSensor = true;
                }
            }
        }
        public void Update()
        {
            body.Position = entity.Position;
            body.Rotation = entity.Rotation;
        }
    }
}