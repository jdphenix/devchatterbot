using DevChatter.Bot.Core.Overlay;

namespace DevChatter.Bot.Web.Bot.Overlay
{
    public class DisplaySimpleImage : IDisplayEventTrigger
    {
        private readonly string _imagePath;

        public DisplaySimpleImage(string imagePath)
        {
            _imagePath = imagePath;
        }

        public void ShowImage()
        {
            // This is where we trigger our signalr to have the page display the image located at imagePath
        }
    }
}