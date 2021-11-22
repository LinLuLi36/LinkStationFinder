using System;
using System.Collections.Generic;
using System.Linq;
using LinkStationFinder.Model;

namespace LinkStationFinder
{
    class Program
    {
        static void Main(string[] args)
        {
			if (args.Length == 0)
			{
				Console.WriteLine("Error: Locations of the device and link stations are required");
				return;
			}

            string devicesInput = null;
            string linkStationsInput = null; 

			for (int i = 0; i < args.Length - 1;)
            {
                string option = args[i];
				switch (option)
				{
					case "-devicePoints":
                        devicesInput = args[i + 1];
                        i += 2;
						break;
					case "-linkStations":
                        linkStationsInput = args[i + 1];
                        i += 2;
                        break;
					default:
						Console.WriteLine("Invalid command, only input of Locations of the device x,y and link stations x,y,r are allowed");
						break;
				}
			}

            if (devicesInput == null)
            {
                Console.WriteLine("Error: Device points data is missing");
                return;
            }

            var deviceInputList = devicesInput.Split(";");

            if (linkStationsInput == null)
            {
                Console.WriteLine("Error: LinkStations data is missing");
                return;
            }

            var linkStationInputList = linkStationsInput.Split(";");

            foreach (var deviceInput in deviceInputList)
            {
                var powerResults = new List<PowerCalculationResult>();
                var deviceInfo = deviceInput.Split(",");
                
                if (deviceInfo.Length == 2)
                {
                    if (double.TryParse(deviceInfo[0], out var xDevicePosition))
                    {
                        if (double.TryParse(deviceInfo[1], out var yDevicePosition))
                        {
                            var device = new Device(xDevicePosition, yDevicePosition);

                            foreach (var linkStationInput in linkStationInputList)
                            {
                                var linkStationInfo = linkStationInput.Split(",");

                                if (linkStationInfo.Length == 3)
                                {
                                    if (double.TryParse(linkStationInfo[0], out var xLinkStationPosition))
                                    {
                                        if (double.TryParse(linkStationInfo[1], out var yLinkStationPosition))
                                        {
                                            if (double.TryParse(linkStationInfo[2], out var reach))
                                            {
                                                var linkStation = new LinkStation(xLinkStationPosition,
                                                    yLinkStationPosition, reach);
                                                var powerResult = PowerCalculator(device, linkStation);
                                                powerResults.Add(powerResult);
                                            }
                                            else
                                                Console.WriteLine(
                                                    ErrorMessageGenerator($"because the reach of the link station {linkStationInput} must be an number.", deviceInput));
                                        }
                                        else
                                            Console.WriteLine(
                                                ErrorMessageGenerator($"because the y position of the link station {linkStationInput} must be an number.", deviceInput));
                                    }
                                    else
                                        Console.WriteLine(
                                            ErrorMessageGenerator($"because the x position of the link station {linkStationInput} must be an number.", deviceInput));
                                }
                                else
                                    Console.WriteLine(
                                        ErrorMessageGenerator($"because the link station {linkStationInput} must be written in the format: x,y,r", deviceInput));
                            }
                        }
                        else
                            Console.WriteLine(
                                ErrorMessageGenerator("because its y position must be an number.", deviceInput));
                    }
                    else
                        Console.WriteLine(
                            ErrorMessageGenerator("because its x position must be an number.", deviceInput));
                }
                else
                    Console.WriteLine(
                        ErrorMessageGenerator($"because it must be written in the format: x,y", deviceInput));

                if (powerResults.Any())
                {
                    var highestPower = powerResults.Max(r => r.Power);
                    if (highestPower == 0)
                        Console.WriteLine($"No link station within reach for device at point {deviceInput}");
                    else
                    {
                        var bestLinkStation = powerResults.FirstOrDefault(p => p.Power.Equals(highestPower));
                        Console.WriteLine($"Best link station for device at point {deviceInput} is {bestLinkStation?.LinkStation.XPosition},{bestLinkStation?.LinkStation.YPosition},{bestLinkStation?.LinkStation.Reach} with power {bestLinkStation?.Power}");
                    }
                }
            }
        }

        static PowerCalculationResult PowerCalculator(Device device, LinkStation linkStation)
        {
            var power = linkStation.Reach - Math.Sqrt((Math.Pow(linkStation.XPosition - device.XPosition, 2) +
                                                       Math.Pow(linkStation.YPosition - device.YPosition, 2)));
            return new PowerCalculationResult(device, linkStation, power >= 0 ? power : 0);
        }

        static string ErrorMessageGenerator(string message, string inputVariable)
        {
            return $"Error: Power of the device at point {inputVariable} cannot be calculated, {message}";
        }
    }
}
