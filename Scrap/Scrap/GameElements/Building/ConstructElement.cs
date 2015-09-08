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
        Segment entity;
        Construct construct;
        Body body;
        ScrapGame game;
        public ConstructElement(ScrapGame game, Segment entity, Construct construct)
        {
            body = BodyFactory.CreateRectangle(game.world, 1.0f, 1.0f, 1.1f, this);
            body.IsSensor = true;
            this.entity = entity;
            this.game = game;
            this.construct = construct;
            body.Position = entity.Position;
            AddSensors(entity);
            body.FixtureList.Remove(body.FixtureList[0]);
            //JointFactory.CreateWeldJoint(game.world, entity.body, body, new Vector2( 0,0), new Vector2(0, 0));
        }
        public bool OnCollide(Fixture a, Fixture b, Contact contact)
        {
            if (b.Body.UserData != null && b.Body.UserData.GetType() == typeof(PlayerController))
            {
                if (a.UserData != null && a.UserData.GetType() == typeof(Scrap.GameElements.Entities.Segment.Direction))
                {
                    if (a.Body.UserData != null) {
                        Vector2 target = GetSensorOffset((Scrap.GameElements.Entities.Segment.Direction)a.UserData);
                        Transform t;
                        a.Body.GetTransform(out t);
                        target = Vector2.Transform(target, Quaternion.CreateFromAxisAngle(Vector3.Backward, a.Body.Rotation));
                        target += a.Body.WorldCenter;
                        b.Body.ApplyLinearImpulse(((target - b.Body.Position) / (target - b.Body.Position).Length()) * .1f);

                        //b.Body.Position = MathHelper.Lerp(b.Body.Position, a.Body.Position, .03f);
                        //ToDo: Use lerp to move selected block
                        b.Body.Rotation =MathHelper.Lerp( b.Body.Rotation , a.Body.Rotation,.03f);
                        b.Body.AngularVelocity = 0f;

                        if ((target - b.Body.Position).Length() < .3f )
                        {
                            b.Body.Rotation = a.Body.Rotation;
                            b.Body.Position = target;
                            b.Body.LinearVelocity = Vector2.Zero;
                            if (b.Body.UserData != null)
                            {
                                foreach (Segment item in game.entityList)
                                {
                                    if(item.body == b.Body)
                                    {
                                        ((PlayerController)(b.Body.UserData)).PlaceSegment();
                                        if (!construct.ContainsFixture(b))
                                            construct.JoinEntities(entity, item, (Scrap.GameElements.Entities.Segment.Direction)a.UserData);
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
            }
            return false;
        }
        private Vector2 GetSensorOffset(Scrap.GameElements.Entities.Segment.Direction direction)
        { 
            Vector2 offset=Vector2.Zero;
            if (direction == Segment.Direction.Right)
                {
                    offset= Vector2.UnitX * 1.2f;

                }
                if (direction == Segment.Direction.Left)
                {
                    offset= Vector2.UnitX * -1.2f;

                }
                if (direction == Segment.Direction.Down)
                {
                    offset= Vector2.UnitY * 1.2f;

                }
                if (direction == Segment.Direction.Up)
                {
                    offset=Vector2.UnitY * -1.2f;

                }
                return offset;
        }
        private void AddSensors(Segment entity)
        {
            foreach (var direction in entity.JointDirections())
            {
                var t = FixtureFactory.AttachRectangle(1.0f, 1.0f, 1.1f, GetSensorOffset(direction), body, direction);
                t.OnCollision += OnCollide;
                t.IsSensor = true;
            }
        }
        public void Update()
        {
            body.Position = entity.Position;
            body.Rotation = entity.Rotation;
        }
    }
}