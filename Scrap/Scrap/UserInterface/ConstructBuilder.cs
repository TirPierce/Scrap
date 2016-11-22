using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Building;
using Scrap.GameElements.Entities;
using Scrap.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.UserInterface
{
    public enum BuildMode{ Inactive=0,Seek, Selected,ControlBind};
    public class ConstructBuilder
    {
        BuildMode buildMode;
        public BuildMode BuildMode
        {
            get
            {
                return buildMode;
            }

            set
            {
                //ToDo: don't put logic in setter
                buildMode = value;
                if (buildMode == BuildMode.Inactive)
                {
                    Disable();
                }
                else if (buildMode == BuildMode.Seek)
                {
                    outlinePosition = Point.Zero;
                    hoverSegment = construct.buildElements[outlinePosition].segment;
                }
                else if (buildMode == BuildMode.ControlBind)
                {
                    hudButtonMapping.EnableForSegment(hoverSegment);
                }
            }
        }

        public Direction SelectedObjectDirection
        {
            get
            {
                return selectedObjectDirection;
            }

            set
            {
                selectedObjectDirection = value;
            }
        }

        Point outlinePosition;
        Texture2D selectionOutline;
        Texture2D availableOutline;
        Texture2D hudBar;
        PlayerConstruct construct;
        private SpriteFont font;
        ScrapGame game;
        List<Segment> segmentsThatCanBeAdded;
        Segment selectedSegment;
        Segment hoverSegment;
        Vector2 positionTopLeft;
        Vector2 scale = Vector2.One;
        //List<Point> validModePoints = new List<Point>();
        public HUDButtonMapping hudButtonMapping;

        Sprite selectionIcon;

        Direction selectedObjectDirection = Direction.None;

        public ConstructBuilder(ScrapGame game)
        {
            
            positionTopLeft = new Vector2(90, 90);
            this.game = game;
            

            segmentsThatCanBeAdded = new List<Segment>();
            BuildMode = BuildMode.Inactive;
            

        }

        public void LoadContent(PlayerConstruct construct)
        {
            hudButtonMapping = new HUDButtonMapping(construct, game);
            this.construct = construct;
            construct.attachmentArea.OnCollision += AttachmentArea_OnCollision;
            selectionOutline = game.Content.Load<Texture2D>("SelectionOutline");
            availableOutline = game.Content.Load<Texture2D>("AvailableOutline");
            hudBar = game.Content.Load<Texture2D>("hudBar");
            font = game.Content.Load<SpriteFont>("Keys");
            selectionIcon = new Sprite(selectionOutline, 100, 100, 1);
        }

        public void BindInput(bool input)
        {
            hudButtonMapping.BindInput(hoverSegment, input);
        }

        private bool AttachmentArea_OnCollision(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if(fixtureB.UserData as Segment != null)
            {
                Segment tempSegment = fixtureB.UserData as Segment;
                if (!this.construct.ContainsSegment(tempSegment) && !segmentsThatCanBeAdded.Contains(tempSegment))
                {
                    segmentsThatCanBeAdded.Add(tempSegment);
                }
            }
            return false;
        }
        public void PlaceSelectedSegmentAtCurrentPositionAndSetModeToSeek()
        {
            if (selectedSegment.constructElement.construct != null)
            {
                selectedSegment.constructElement.RemoveFromConstruct();
            }
            if (construct.GetValidJoinPositions().Keys.Contains(outlinePosition))
            {

                construct.AttachSegmenAtSensorAndOrientateCorrectly(selectedSegment.constructElement, construct.GetValidJoinPositions()[outlinePosition], selectedObjectDirection);

            }
            
            selectedSegment = null;
            buildMode = BuildMode.Seek;
        }
        public void SetSelectedSegmentAndSetBuildModeToSelect()
        {
            if(hoverSegment!= construct.KeyObject)
            {
                selectedSegment = hoverSegment;
                if (selectedSegment != null)
                    buildMode = BuildMode.Selected;

                outlinePosition = Point.Zero;
            }
        }

        public void Move(Direction direction, List<Point> validPoints)
        {
            Point max;
            Point min;
            max = GetPointBoundingRectangle(validPoints).Size;
            min = GetPointBoundingRectangle(validPoints).Location;
            Point newOutlinePosition = outlinePosition;
            switch (direction)
            {
                //add updating reference to selected object and make it glow on construct
                case Direction.Up:

                    do
                    {
                        newOutlinePosition.Y--;
                        if(newOutlinePosition.Y <min.Y)
                        {
                            newOutlinePosition.Y = max.Y;
                        }
                        
                    }
                    while (!validPoints.Contains<Point>(newOutlinePosition));

                    outlinePosition = newOutlinePosition;


                    break;
                case Direction.Right:

                    do
                    {
                        newOutlinePosition.X++;
                        if (newOutlinePosition.X > max.X)
                        {
                            newOutlinePosition.X = min.X;
                        }

                    }
                    while (!validPoints.Contains<Point>(newOutlinePosition));

                    outlinePosition = newOutlinePosition;


                    break;
                case Direction.Down:

                    do
                    {
                        newOutlinePosition.Y++;
                        if (newOutlinePosition.Y > max.Y)
                        {
                            newOutlinePosition.Y = min.Y;
                        }

                    }
                    while (!validPoints.Contains<Point>(newOutlinePosition));

                    outlinePosition = newOutlinePosition;

                    break;
                case Direction.Left:
                    do
                    {
                        newOutlinePosition.X--;
                        if (newOutlinePosition.X < min.X)
                        {
                            newOutlinePosition.X = max.X;
                        }

                    }
                    while (!validPoints.Contains<Point>(newOutlinePosition));

                    outlinePosition = newOutlinePosition;


  
                    break;
                default:
                    break;
            }
        }
        public void MoveSelection(Direction direction)
        {
            if (direction != Direction.None)
            {
                Move(direction, GetValidSelectPoints());
            }
        }

        public void MoveSeek(Direction direction)
        {
            if (direction != Direction.None)
            {
                List<Point> seekableSegments = GetValidSeekPoints();
                int rightMostElement = GetPointBoundingRectangle(seekableSegments).Right+1;

                int newSeekIndex = rightMostElement;
                foreach (Segment segment in segmentsThatCanBeAdded)
                {
                    newSeekIndex++;
                    seekableSegments.Add(new Point(newSeekIndex, 0));

                }
                Move(direction, seekableSegments);

                int lastConnectedElementX = GetPointBoundingRectangle(GetValidSeekPoints()).Right+1;

                if (construct.buildElements.ContainsKey(outlinePosition))
                {
                    hoverSegment = construct.buildElements[outlinePosition].segment;
                }
                else
                {
                    hoverSegment = segmentsThatCanBeAdded[outlinePosition.X- rightMostElement-1];
                }
                
                hudButtonMapping.EnableForSegment(hoverSegment);
            }

            //selectedSegment = this.construct.buildElements[outlinePosition].segment;
        }
        private List<Point> GetValidSeekPoints()
        {
            return this.construct.buildElements.Keys.ToList<Point>();
        }
        private List<Point> GetValidSelectPoints()
        {
            var temp = this.construct.buildElements.Keys.ToList<Point>();
            temp.AddRange(this.construct.GetValidJoinPositions().Keys.ToList<Point>());
            return temp;  
            
        }
        private Rectangle GetPointBoundingRectangle(List<Point> validPoints)
        {
            int xMin = 0;
            int yMin = 0;
            int xMax = 0;
            int yMax = 0;

            xMin = validPoints.Min(point => point.X);
            yMin = validPoints.Min(point => point.Y);
            xMax = validPoints.Max(point => point.X);
            yMax = validPoints.Max(point => point.Y);
            return new Rectangle(xMin, yMin, xMax, yMax);
        }
        public void Update()
        {


            switch (buildMode)
            {
                case BuildMode.Inactive:
                    segmentsThatCanBeAdded.Clear();
                    break;
                case BuildMode.Seek:
                    break;
                case BuildMode.Selected:
                    break;
                case BuildMode.ControlBind:
                    hudButtonMapping.Update();//ToDo: this needs to be conditional
                    break;
                default:
                    break;
            }


        }
        private void Enable()
        {
            
        }
        private void Disable()
        {

        }
        protected List<GameButton> buttons = new List<GameButton>();


        public GameButton AddButton(Segment segment, Direction direction, Action<Direction> callback)
        {
            GameButton button = new GameButton(segment, direction, callback);
            buttons.Add(button);
            return button;
        }

        //public void SelectDirection(Direction direction)
        //{
        //    foreach (GameButton button in buttons)
        //    {

        //        if (button.offsetDirection == direction && button.status == UIStatus.Active)
        //        {
        //            button.DoActions();
        //            break;
        //        }
        //    }
        //}
        public void DrawGUI(SpriteBatch spriteBatch)
        {
            if(buildMode != BuildMode.Inactive)
            {
                Rectangle hudTargetRectangle = hudBar.Bounds;
                hudTargetRectangle.Location =  new Point(spriteBatch.GraphicsDevice.Viewport.Bounds.Center.X- hudTargetRectangle.Width/2, spriteBatch.GraphicsDevice.Viewport.Bounds.Height - hudTargetRectangle.Height);
                spriteBatch.Draw(hudBar, hudTargetRectangle, Color.White);

                Point hoverPosition = new Point((int)(spriteBatch.GraphicsDevice.Viewport.Bounds.Width*0.30f), (int)(spriteBatch.GraphicsDevice.Viewport.Bounds.Height * 0.70f));
                Point sourceSize = new Point(hoverSegment.sprite.frameWidth, hoverSegment.sprite.frameHeight);
                Point targetSize = new Point(hoverSegment.sprite.frameWidth*2, hoverSegment.sprite.frameHeight*2);
                Rectangle hoverTargetRectangle = new Rectangle(hoverPosition, targetSize);
                Rectangle hoverSourceRectangle = new Rectangle(Point.Zero, sourceSize);
                spriteBatch.Draw(hoverSegment.sprite.Texture, hoverTargetRectangle,hoverSourceRectangle, Color.White);

                Point segmentToAddPosition = hoverPosition + new Point(0, targetSize.Y);
                Point segmentToAddSize = new Point((int)(targetSize.X * .2f), (int)(targetSize.Y * .2f));
                Rectangle segmentsToAdd = new Rectangle(segmentToAddPosition , segmentToAddSize);
                int xOffset = 0;
                foreach (Segment segment in segmentsThatCanBeAdded)
                {
                    segmentToAddPosition = hoverPosition + new Point(xOffset, targetSize.Y);
                    segmentsToAdd = new Rectangle(segmentToAddPosition, segmentToAddSize);
                    sourceSize = new Point(segment.sprite.frameWidth, segment.sprite.frameHeight);
                    hoverSourceRectangle = new Rectangle(Point.Zero, sourceSize);
                    spriteBatch.Draw(segment.sprite.Texture, segmentsToAdd, hoverSourceRectangle, Color.White);
                    if(segment == hoverSegment)
                    {
                        spriteBatch.Draw(selectionOutline, segmentsToAdd, Color.White);
                    }
                    xOffset += segmentsToAdd.Width;
                }
                
            }
            if (buildMode== BuildMode.ControlBind)
                hudButtonMapping.Draw(spriteBatch);

            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (buildMode)
            {
                case BuildMode.Inactive:
                    break;
                case BuildMode.Seek:
                    foreach (Segment segment in segmentsThatCanBeAdded)
                    {
                        selectionIcon.Draw(spriteBatch, segment.Position, segment.Rotation, Color.PaleGoldenrod);
                    }
                    selectionIcon.Draw(spriteBatch, hoverSegment.Position, hoverSegment.Rotation, Color.White);

                    break;
                case BuildMode.Selected:
                    Vector2 selectedObjectDrawLocation = Vector2.Zero;
                    if (this.construct.buildElements.Keys.Contains<Point>(outlinePosition))
                    {
                        selectedObjectDrawLocation = this.construct.buildElements[outlinePosition].segment.Position;
                    }
                    else if (this.construct.GetValidJoinPositions().Keys.Contains<Point>(outlinePosition))
                    {
                        selectedObjectDrawLocation = this.construct.GetValidJoinPositions()[outlinePosition].CalculateSensorTileCentreInWorld();
                    }

                    selectedSegment.sprite.Draw(spriteBatch, selectedObjectDrawLocation, construct.KeyObject.Rotation+ Orientation.DirectionToRadians(selectedObjectDirection), Color.GreenYellow);

                    break;
                case BuildMode.ControlBind:
                    //hudButtonMapping.Draw(spriteBatch);//ToDo: this needs to be conditional
                    break;
                default:
                    break;
            }

            Segment currentSegment;

            //foreach (Point index in construct.buildElements.Keys)
            //{
            //    currentSegment= construct.buildElements[index].segment;
            //    String inputText = "";
            //    if (this.game.playerController.leftTrigger.ContainsKey(index))
            //        inputText = "L";
            //    else if (this.game.playerController.rightTrigger.ContainsKey(index))
            //        inputText = "R";
            //    spriteBatch.DrawString(font, inputText, new Vector2((index.X - xMin) * 25 + 15, (index.Y - yMin) * 25 + 15), Color.Red, 0, font.MeasureString(currentSegment.constructElement.mappedKey.ToString()) * .5f, .3f, SpriteEffects.None, 0);

            //    //spriteBatch.Draw(texture, new Rectangle((index.X + xMin * -1) * 25, (index.Y + yMin * -1) * 25, 20, 20), null, Color.White);
            //    //spriteBatch.Draw(construct.buildElements[index].segment.sprite.Texture, new Rectangle((index.X + xMin * -1) * 25, (index.Y + yMin * -1) * 25, 20, 20), null, Color.White);
            //    float rotation = Orientation.DirectionToRadians(currentSegment.constructElement.orientation);

            //    Rectangle segmentDrawTarget = new Rectangle((index.X - xMin) * 25 + (int)positionTopLeft.X, (index.Y - yMin) * 25 + (int)positionTopLeft.Y, 20, 20);
            //    spriteBatch.Draw(currentSegment.sprite.Texture, segmentDrawTarget, 
            //        null, Color.White, rotation, segmentDrawTarget.Center.ToVector2(), SpriteEffects.None, 1);
            //}

            //for (int i = 0; i < segmentsThatCanBeAdded.Count(); i++)
            //{
            //    Rectangle segmentDrawTarget = new Rectangle((i + GetContructBoundingRectangle().Right + 1) * 25 + (int)positionTopLeft.X, +(int)positionTopLeft.Y, (int)(20 * scale.X), (int)(20 * scale.Y));

            //    float rotation = Orientation.DirectionToRadians(segmentsThatCanBeAdded[i].constructElement.orientation);
            //    spriteBatch.Draw(segmentsThatCanBeAdded[i].sprite.Texture, segmentDrawTarget, null,
            //        Color.White, rotation, segmentDrawTarget.Center.ToVector2(), SpriteEffects.None, 1);




            //}
            //if (buildMode > 0)
            //{

            //}
        }
    }
}
