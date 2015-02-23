using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Scrap.Entities;
using Scrap.GameState;
using Microsoft.Xna.Framework.Input.Touch;
//using Scrap.Physics;
using GameStateManagement;
using Microsoft.Xna.Framework.Input;

namespace Scrap.UserInterface
{
    enum GUIButtonType
    {
        PilotMode,
        BuildMode,
        Fuel,
        Girder,
        Engine,
        VirtualSwitch,
        Default
    }
    
    class GUIButton:Button
    {
        public GUIButtonType mState;
        
        public GUIButton( Point position, int width, int height, GUIButtonType type, Texture2D texture )
            :base( position, width, height, texture)
        {
            mState = type;
        }
        
    }
    class GRIDButton : Button
    {
        public Entity mEntity;
        

        public GRIDButton( Point position, int width, int height, Texture2D texture, Entity entity)
            : base(position, width, height, texture)
        {
            mEntity = entity;
        }
    }
    public class GRIDTile
    {
        public GRIDTile(Rectangle rectangle , Entity entity, float rotation )
        {

            this.rectangle = rectangle;
            this.entity = entity;
            this.rotation = rotation;
        }

        public float rotation;
        public Rectangle rectangle;
        public Entity entity;
    }
    class GUI: IGUI
    {
        //Rectangle GUIRegion;//If not over then ignore press. Else suppress event. :D
        List<GRIDButton> mGRIDButtons;
        
        Game mGame;

        public GamePlayState mGameState;
        GUIButtonType lastButton;
        //GUIButton mPilotMode;
        GUIButton mGo;
        Invention invention; 
        GRIDTile[,] grid;

        Rectangle pointerRectangle;

        Texture2D mTile;
        
        Texture2D mRotationPointer;
        Texture2D mPointer;
        
        GestureSample gestureSample;
        GRIDButton selectedButton;
        SoundEffect buttonClick;
        Vector2 objectiveLocation;

        public GUI(Game game)
        {//replace with xml layout 

            game.Services.AddService(typeof(IGUI), this);
            
            mGame = game;
            mGRIDButtons = new List<GRIDButton>();
            grid = new GRIDTile[5, 5];
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    grid[x,y] = new GRIDTile(new Rectangle(100 + x * 90, 10 + y * 90, 80, 80),null, 0);
                }
            }
        }
        public void SetObjectiveLocation(Vector2 location)
        {
            objectiveLocation = location;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if (mGameState == GamePlayState.PilotMode)
            {
                

            }
            else if (mGameState == GamePlayState.BuildMode)
            {


                mGo.Draw(spritebatch);
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        spritebatch.Draw(mTile, grid[x, y].rectangle, Color.Purple);
                    }
                }
                foreach (GRIDButton button in mGRIDButtons)
                {
                    button.Draw(spritebatch);
                }

                spritebatch.Draw(mPointer, pointerRectangle, Color.White);
            }
            if (editPlacement)
            {
                spritebatch.Draw(mRotationPointer,
                    new Vector2(grid[rotatedGrid.X, rotatedGrid.Y].rectangle.Center.X, grid[rotatedGrid.X, rotatedGrid.Y].rectangle.Center.Y), 
                    null, 
                    Color.White, 
                    rotation, 
                    new Vector2(mRotationPointer.Width/2, mRotationPointer.Height/2),
                    .5f, SpriteEffects.None, 0);
            }

        }
        Point position;
        Point position2;
        GamePadState previousState = new GamePadState();
        bool editPlacement = false;
        float rotation;
        Point rotatedGrid;
        KeyboardState keyLast;
        MouseState mouseLast;
        public void Update()
        {
            GamePadState currentState = GamePad.GetState(0);
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            if (mGameState == GamePlayState.BuildMode && editPlacement)
            {

               
                if (currentState.IsConnected)
                {
                    rotation = (float)Math.Atan2(currentState.ThumbSticks.Left.X, currentState.ThumbSticks.Left.Y);
                }
                else
                {
                    rotation = (float)Math.Atan2(mouseState.Y - grid[rotatedGrid.X, rotatedGrid.Y].rectangle.Center.Y, mouseState.X - grid[rotatedGrid.X, rotatedGrid.Y].rectangle.Center.X);
                    pointerRectangle.X = mouseState.X;
                    pointerRectangle.Y = mouseState.Y;
                }
                if ((currentState.Buttons.A == ButtonState.Released && previousState.Buttons.A == ButtonState.Pressed)
                   ||(mouseState.LeftButton == ButtonState.Released && mouseLast.LeftButton == ButtonState.Pressed))
                {
                    editPlacement = false;
                    selectedButton = null;
                    grid[rotatedGrid.X,rotatedGrid.Y].rotation = rotation;
                }
            }
            else
            {

                if (currentState.IsConnected)
                {
                    if (currentState.ThumbSticks.Left.X > 0 && pointerRectangle.X < mGame.GraphicsDevice.Viewport.Width - 20)
                    {
                        pointerRectangle.X += 10;
                    }
                    if (currentState.ThumbSticks.Left.X < 0 && pointerRectangle.X > 0)
                    {
                        pointerRectangle.X -= 10;
                    }
                    if (currentState.ThumbSticks.Left.Y > 0 && pointerRectangle.Y > 0)
                    {
                        pointerRectangle.Y -= 10;
                    }
                    if (currentState.ThumbSticks.Left.Y < 0 && pointerRectangle.Y < mGame.GraphicsDevice.Viewport.Height - 20)
                    {
                        pointerRectangle.Y += 10;
                    }

                }
                else 
                {
                    pointerRectangle.X = mouseState.X;
                    pointerRectangle.Y = mouseState.Y;
                }

                //Point position = Point.Zero;
                //Point position2 = Point.Zero;


                //position = new Point((int)gesture.Position.X, (int)gesture.Position.Y);
                //position2 = new Point((int)gesture.Position2.X, (int)gesture.Position2.Y);


                if (mGameState == GamePlayState.BuildMode && 
                    ((currentState.Buttons.A == ButtonState.Pressed && previousState.Buttons.A == ButtonState.Pressed)
                    || (mouseState.LeftButton == ButtonState.Pressed && mouseLast.LeftButton == ButtonState.Pressed)))
                {
                    if (selectedButton == null)
                    {
                        foreach (GRIDButton item in mGRIDButtons)
                        {
                            if (item.OverButton(pointerRectangle.Center))
                            {

                                selectedButton = item;
                                break;
                            }
                        }
                    }
                    else
                    {
                        selectedButton.mButtonArea.Location = pointerRectangle.Center;
                        selectedButton.mButtonArea.Offset(-40, -40);
                    }
                }

                if (mGameState == GamePlayState.BuildMode && selectedButton != null && ((currentState.Buttons.A == ButtonState.Released && previousState.Buttons.A == ButtonState.Pressed)
                    ||(mouseState.LeftButton == ButtonState.Released && mouseLast.LeftButton == ButtonState.Pressed)))
                {
                    //buttonClick.Play();
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            if (grid[x, y].rectangle.Contains(selectedButton.mButtonArea.Center))
                            {
                                selectedButton.mButtonArea.Location = grid[x, y].rectangle.Location;
                                grid[x, y].entity = selectedButton.mEntity;
                                rotatedGrid = new Point(x, y);
                                editPlacement = true;
                            }
                            else if (grid[x, y].entity == selectedButton.mEntity)
                            {
                                grid[x, y].entity = null;
                            }


                        }
                    }

                    

                }
            }
            if (mGameState == GamePlayState.BuildMode && 
                ((currentState.Buttons.A == ButtonState.Released && previousState.Buttons.A == ButtonState.Pressed) 
                ||(mouseState.LeftButton == ButtonState.Released && mouseLast.LeftButton == ButtonState.Pressed)
                ||(keyState.IsKeyUp(Keys.G) && keyLast.IsKeyDown(Keys.G))))
            {
                if (mGo.OverButton(pointerRectangle.Center) ||  (keyState.IsKeyUp(Keys.G) && keyLast.IsKeyDown(Keys.G)) )
                {
                    //buttonClick.Play();
                    //invention.Launch(grid);
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            grid[x, y].entity = null;
                        }
                    }

                    mGameState = GamePlayState.PilotMode;
                    
                }
                selectedButton = null;
            }




            mouseLast = mouseState;
            previousState = currentState;
            keyLast = keyState;
        }

        public void LoadContent()
        {
            mGo = new GUIButton(new Point(0, 180), 90, 90, GUIButtonType.PilotMode,mGame.Content.Load<Texture2D>("GUI/go"));
            mTile = mGame.Content.Load<Texture2D>("GUI/tile");
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DragComplete | GestureType.FreeDrag;
            mPointer = mGame.Content.Load<Texture2D>("pointer");
            buttonClick = mGame.Content.Load<SoundEffect>("Sound/blip");
            
            mRotationPointer = mGame.Content.Load<Texture2D>("rotationPointer");

            pointerRectangle = new Rectangle(50, 50, 50, 50);



        }


        public GUIButtonType OverInterface(Point clickPoint)
        {//if a click is over a button the button type will be returned



            return GUIButtonType.Default;
        }


        public void InventionCreator(Entity entity, Invention invention)
        {//called when an object is collided with

            mGRIDButtons.Clear();
            this.invention = invention;
            this.mGameState = GamePlayState.BuildMode;
            int buttonCount = 0;
            int yOffSet= 0;
            List<Entity> entityList = invention.entityList;

            //%3 = 0 left column 
            //%3 = 1 middle column
            //%3 = 2 right column


            const int columns = 2;
            foreach (Entity item in entityList)
            {
                //yOffSet =  buttonCount % 3 == 0 ? buttonCount/3 * 100 : yOffSet; 
                grid[item.gridPosition.X, item.gridPosition.Y].entity = item;
                mGRIDButtons.Add(new GRIDButton(grid[item.gridPosition.X, item.gridPosition.Y].rectangle.Location, 80, 80, mGame.Content.Load<Texture2D>("GUI/" + item.objectType), item));
                

            }
            if (entity != null)
            {
                mGRIDButtons.Add(new GRIDButton(new Point((buttonCount % columns) * 90 + 550, yOffSet), 80, 80, mGame.Content.Load<Texture2D>("GUI/" + entity.objectType), entity));
            }
        }
    }
}
