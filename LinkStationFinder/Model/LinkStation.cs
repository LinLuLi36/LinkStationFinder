namespace LinkStationFinder.Model
{
    public class LinkStation
    {
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public double Reach { get; set; }

        public LinkStation(double xPosition, double yPosition, double reach)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            Reach = reach;
        }
    }
}
