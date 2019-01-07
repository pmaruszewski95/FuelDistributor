using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FuelDistributorConsoleApplication
{
    public class FlowSteps
    {
        public static void MainFlow(string[] argsIn)
        {
            while (argsIn.Length.Equals(0) || !Regex.Match(argsIn.First(ar => !ar.Equals(string.Empty)), @"-{1}\w+\.xml").Success)
            {
                Logger.log.Error("Start aplikacji przez \"Applicationname.exe -nazwaPlikuXml.xml");
                Environment.Exit(0);
            }           
          
            var xmlParser = new XmlParser(argsIn[0].Replace("-", string.Empty));
            var vehiclesList = xmlParser.GetVehiclesList();
            System.Console.Clear();
            Logger.log.Info("Mozliwe jest dotankowanie nastepujacych aut");
            xmlParser.ShowAvailableVehicles(vehiclesList);
            var fuelDistributor = new FuelDistributor(vehiclesList);
            fuelDistributor.ChooseCarMark();
            fuelDistributor.ChooseFuelKind();
            fuelDistributor.ChooseFuelCapacity();
            fuelDistributor.TankCar();
            fuelDistributor.VerifyOperation();
            Environment.Exit(0);
            
        }
    }
}
