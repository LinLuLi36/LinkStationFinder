namespace LinkStationFinder.Model
{
    public class Device
    {
        public double XPosition { get; set; }
        public double YPosition { get; set; }

        public Device(double xPosition, double yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }
    }
}
