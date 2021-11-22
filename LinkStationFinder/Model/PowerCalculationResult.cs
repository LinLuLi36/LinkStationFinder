namespace LinkStationFinder.Model
{
    public class PowerCalculationResult
    {
        public Device Device { get; set; }
        public LinkStation LinkStation { get; set; }
        public double Power { get; set; }

        public PowerCalculationResult(Device device, LinkStation linkStation, double power)
        {
            Device = device;
            LinkStation = linkStation;
            Power = power;
        }
    }
}
