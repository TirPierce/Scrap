﻿using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Building;
using Scrap.GameElements.GameWorld;
using Scrap.UserInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{

    public enum ElementStatus { Locked, Selected, Attached, Free };
    public class ConstructElement
    {
        Linker linker;
        public Orientation orientation = new Orientation(Direction.Up);
        public Segment segment;
        public Construct construct;
        ScrapGame game;
        public Point offSet;

        public Joint rootJoint;
        public ConstructElement rootElement;
        
        public Dictionary<Point,Joint> branchJoints;
        private List<GameButton> gameButtons = new List<GameButton>();
        public List<Point> adjacentElements = new List<Point>();
        List<Sensor> sensors = new List<Sensor>();
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
            branchJoints = new Dictionary<Point,Joint>();
            GenerateGUIButtons();
            
        }
        public void AddToConstruct(Construct construct, Point offSet, Joint rootJoint,ConstructElement rootElement, Direction direction)
        {
            orientation.Direction = direction;
            this.rootJoint = rootJoint;
            this.rootElement = rootElement;
            this.construct = construct;
            this.offSet = offSet;
            
            adjacentElements = construct.AdjacentElements(offSet);
            linker = new Linker(this.game, rootJoint);
        }


        public void RemoveFromConstruct()
        {
            
            if (construct != null)
            {
                
                //foreach (Point branchElement in this.branchJoints.Keys)
                //{
                //    construct.buildElements[branchElement].RemoveFromConstruct();
                //}
                foreach (Point key in branchJoints.Keys)
                {
                    if (construct.buildElements.Keys.Contains(key))
                        construct.buildElements[key].RemoveFromConstruct();
                }
                this.branchJoints.Clear();
                if (rootJoint != null)
                {
                    //rootElement.branchJoints.Remove(this.offSet);
                    game.world.RemoveJoint(rootJoint);
                    rootElement = null;
                }
                if (construct.buildElements.ContainsKey(offSet))
                    construct.buildElements.Remove(offSet);
                construct.RecalculateAdjacentSegmentsAndActivateSensors();
                construct = null;

            }
            DisableSensors();
            this.SetStatus(ElementStatus.Free);
            adjacentElements.Clear();
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

        private void OrientateSegmentAndSetStatusToAttached(Direction direction)
        {
            //Debug.WriteLine("PlaceSegment():" + direction.ToString());
            orientation.Direction = direction;
            construct.SetSegmentDirection(segment, direction);
            SetStatus(ElementStatus.Attached);
        }
        public void EnableSensors()
        {
            if (sensors.Count == 0)//ToDo: this seems hacky
            {
                AddSensors();
            }
            adjacentElements = construct.AdjacentElements(offSet);
            DisableSensors();

            List<Sensor> matchingSensors = new List<Sensor>();
            matchingSensors.AddRange(sensors);
            List<ConstructElement> matches = new List<ConstructElement>();
            foreach (Sensor item in sensors)
            {
                //Point itemRelativeOffset =  Orientation.DirectionToPoint(this.orientation.AddDirectionsAsClockwiseAngles(item.direction));
                Point itemRelativeOffset =  Orientation.DirectionToPoint(item.GetOrientationRelativeToConstruct().Direction);
                if (!adjacentElements.Contains(item.constructElement.offSet + itemRelativeOffset))
                {
                    item.Enable();
                }
            }
        }
        public void DisableSensors()
        {
            foreach (Sensor sensor in sensors)
            {
                sensor.body.Position = Vector2.Zero;
                sensor.Disable();
            }
        }
        private void AddSensors()
        {
            Sensor sensor;
            foreach (var direction in segment.JointDirections())
            {
                sensor = new Sensor(this, direction, game);
                sensors.Add(sensor);
            }
        }

        private void GenerateGUIButtons()
        {
            foreach (Direction direction in segment.JointDirections())
            {
                gameButtons.Add(this.game.gui.AddButton(this.segment, direction, new Action<Direction>(OrientateSegmentAndSetStatusToAttached)));
            }
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (linker != null)
            {
                linker.Draw(batch);
            }
            foreach (Sensor sensor in sensors)
            {
                sensor.Draw(batch);
            }
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
            if (linker != null)
            {
                linker.Update();
            }
            foreach (Sensor sensor in sensors)
            {
                sensor.Update();
            }
        }
    }
}