using System.Collections.Generic;

namespace Scrap.Entities
{
    public interface IAutomotive
    {
        void OnAnalogueIn(int input);
        void OnDigitalIn(bool input);
        
        
    }
}
