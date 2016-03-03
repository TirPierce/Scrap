using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.UserInterface
{
    public enum ButtonStatus { Up, Down}
    class UIButton
    {
        private float layer = 0;
        Segment segment = null;
        public float Layer
        {
            get { return layer; }
            set { layer = value; }
        }
        public UIButton(Vector2 postition, float x, float y)
        {

        }
        Func<Matrix> trackingMatrix = null;

        public Func<Matrix> TrackingMatrix
        {
            get { return trackingMatrix; }
            set { trackingMatrix = value; }
        }  
        Rectangle boundingBox;
        private ButtonStatus buttonStatus = ButtonStatus.Up;
        Dictionary<String, Func<String>> callBacklisteners;
        public void AddCallBack(String callerID, Func<String> callBack)
        {
            callBacklisteners.Add(callerID, callBack);
        }
        public void RemoveCallBack(String callerID)
        {
            callBacklisteners.Remove(callerID);
        }
        public void Draw(SpriteBatch spriteBatch)
        { 
            //use tracking matrix to orientate button
        }
        public void Update()
        {

        }
        public void OnClick()
        {
            foreach (Func<String> callBackListener in callBacklisteners.Values)
            {
                callBackListener.Invoke();
            }
        }
    }
    
}
