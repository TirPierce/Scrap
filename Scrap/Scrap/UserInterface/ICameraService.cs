using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Scrap.UserInterface
{
    public interface ICameraService
    {

        void SetBackground(Texture2D near,Texture2D Far);

         void FocusCamera(Vector2 Target);
         void Follow(ref Vector2 Target);
         Vector2 Position { get; set; }
         double Rotation { get; set; } 
         void Update();
         Matrix Transformation { get; }

         void Zoom(int magnification);
    }
}
