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
    public class ConstructBuilder
    {
        Point outlinePosition;
        Texture2D selectionOutline;
        Texture2D availableOutline;
        //Texture2D texture;
        PlayerConstruct construct;
        private SpriteFont font;
        ScrapGame game;
        List<Segment> segmentsThatCanBeAdded;
        Segment selectedSegment;

        public ConstructBuilder(PlayerConstruct construct, ScrapGame game)
        {
            construct.attachmentArea.OnCollision += AttachmentArea_OnCollision;
            //construct.attachmentArea.OnSeparation += AttachmentArea_OnSeparation;
            this.game = game;
            this.construct = construct;
            selectionOutline = game.Content.Load<Texture2D>("SelectionOutline");
            availableOutline = game.Content.Load<Texture2D>("AvailableOutline");
            font = game.Content.Load<SpriteFont>("Keys");
            segmentsThatCanBeAdded = new List<Segment>();
        }

            
        //private void AttachmentArea_OnSeparation(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB)
        //{
        //    if (fixtureB.UserData as Segment != null)
        //    {
        //        segmentsThatCanBeAdded.Remove(fixtureB.UserData as Segment);
                
        //    }
        //}

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
        public void SelectOrPlace()
        {
            if (selectedSegment != null)
            {
                if (selectedSegment.constructElement.construct != null)
                {
                    selectedSegment.constructElement.RemoveFromConstruct();
                }
                if(construct.GetValidJoinPositions().Keys.Contains(outlinePosition))
                {
                    
                    construct.AttachSegmenAtSensorAndOrientateCorrectly(selectedSegment.constructElement, construct.GetValidJoinPositions()[outlinePosition]);

                }
                //construct.AddSegmentAtSensorPosition
                selectedSegment = null;
            }
            else
            {
                int xMin = construct.buildElements.Keys.Min(point => point.X);
                int yMin = construct.buildElements.Keys.Min(point => point.Y);
                int xMax = construct.buildElements.Keys.Max(point => point.X);
                int yMax = construct.buildElements.Keys.Max(point => point.Y);

                if(this.construct.buildElements.Keys.Contains<Point>(outlinePosition))
                {
                    selectedSegment = this.construct.buildElements[outlinePosition].segment;
                }
                else if (outlinePosition.X > (xMax + xMin * -1) 
                    && outlinePosition.X < xMax + xMin * -1 + segmentsThatCanBeAdded.Count())
                {
                    selectedSegment = segmentsThatCanBeAdded[outlinePosition.X - xMax+xMin];
                }
            }
        }
        public void MoveSelection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    outlinePosition.Y = outlinePosition.Y - 1;
                    break;
                case Direction.Right:
                    outlinePosition.X = outlinePosition.X + 1;
                    break;
                case Direction.Down:
                    outlinePosition.Y = outlinePosition.Y + 1;
                    break;
                case Direction.Left:
                    outlinePosition.X = outlinePosition.X - 1;
                    break;
                default:
                    break;
            }
        }

        public void Update()
        {

            


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            int xMin = construct.buildElements.Keys.Min(point => point.X);
            int yMin = construct.buildElements.Keys.Min(point => point.Y);
            int xMax = construct.buildElements.Keys.Max(point => point.X);
            int yMax = construct.buildElements.Keys.Max(point => point.Y);
            Segment currentSegment;

            foreach (Point index in construct.buildElements.Keys)
            {
                currentSegment= construct.buildElements[index].segment;
                String inputText = "";
                if (this.game.playerController.leftTrigger.ContainsKey(index))
                    inputText = "L";
                else if (this.game.playerController.rightTrigger.ContainsKey(index))
                    inputText = "R";
                spriteBatch.DrawString(font, inputText, new Vector2((index.X + xMin * -1) * 25 + 15, (index.Y + yMin * -1) * 25 + 15), Color.Red, 0, font.MeasureString(currentSegment.constructElement.mappedKey.ToString()) * .5f, .3f, SpriteEffects.None, 0);
            
                //spriteBatch.Draw(texture, new Rectangle((index.X + xMin * -1) * 25, (index.Y + yMin * -1) * 25, 20, 20), null, Color.White);
                //spriteBatch.Draw(construct.buildElements[index].segment.sprite.Texture, new Rectangle((index.X + xMin * -1) * 25, (index.Y + yMin * -1) * 25, 20, 20), null, Color.White);
                spriteBatch.Draw(currentSegment.sprite.Texture, new Rectangle((index.X + xMin * -1) * 25+15, (index.Y + yMin * -1) * 25+15, 20, 20), null, Color.White, Orientation.DirectionToRadians(currentSegment.constructElement.orientation), new Vector2(currentSegment.sprite.Texture.Width/2, currentSegment.sprite.Texture.Height/2), SpriteEffects.None, 1);
            }

            for (int i = 0; i < segmentsThatCanBeAdded.Count(); i++)
            {
                spriteBatch.Draw(segmentsThatCanBeAdded[i].sprite.Texture, new Rectangle((i+ xMax + xMin * -1+1) * 25 + 15, 15, 20, 20), null, Color.White, Orientation.DirectionToRadians(segmentsThatCanBeAdded[i].constructElement.orientation), new Vector2(segmentsThatCanBeAdded[i].sprite.Texture.Width / 2, segmentsThatCanBeAdded[i].sprite.Texture.Height / 2), SpriteEffects.None, 1);
                spriteBatch.Draw(availableOutline, new Rectangle((i + xMax + xMin * -1 + 1) * 25 + 15, 15, 20, 20), null, Color.White, 0, new Vector2(availableOutline.Width / 2, availableOutline.Height / 2), SpriteEffects.None, 1);


            }
            if (game.buildMode)
            {
                if (selectedSegment != null)
                {
                    spriteBatch.Draw(selectedSegment.sprite.Texture, new Rectangle((outlinePosition.X + xMin * -1) * 25 + 15, (outlinePosition.Y + yMin * -1) * 25 + 15, 20, 20), null,
                        Color.White, 0, new Vector2(this.selectionOutline.Width / 2, this.selectionOutline.Height / 2), SpriteEffects.None, 1);

                }
                else
                {
                    spriteBatch.Draw(this.selectionOutline, new Rectangle((outlinePosition.X + xMin * -1) * 25 + 15, (outlinePosition.Y + yMin * -1) * 25 + 15, 20, 20), null,
                        Color.White, 0, new Vector2(this.selectionOutline.Width / 2, this.selectionOutline.Height / 2), SpriteEffects.None, 1);
                }
            }
        }
    }
}
