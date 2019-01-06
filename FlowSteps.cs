using System.Linq;
using System.Text.RegularExpressions;

namespace FuelDistributorConsoleApplication
{
    public class FlowSteps
    {
        public static void MainFlow(string[] argsIn)
        {
            if (!argsIn.Length.Equals(0) && argsIn.Any(ar => !ar.Equals(string.Empty)))
            {
                if (Regex.Match(argsIn.First(ar => !ar.Equals(string.Empty)), @"-{1}\w+\.xml").Success)
                {
                    var xmlParser = new XmlParser(argsIn[0].Replace("-", string.Empty));
                    var vehiclesList = xmlParser.GetVehiclesList();
                    Logger.log.Info("Mozliwe jest dotankowanie nastepujacych aut");
                    xmlParser.ShowAvailableVehicles(vehiclesList);
                    var fuelDistributor = new FuelDistributor(vehiclesList);
                    fuelDistributor.ChooseCarMark();
                    fuelDistributor.ChooseFuelKind();
                    fuelDistributor.ChooseFuelCapacity();
                    fuelDistributor.TankCar();
                    fuelDistributor.VerifyOperation();
                }
                else
                {
                    Logger.log.Error("Start aplikacji przez \"Applicationname.exe -nazwaPlikuXml.xml");
                }
            }
        }
    }
}
