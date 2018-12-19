using System.Linq;
using System.Text.RegularExpressions;

namespace FuelDistributorConsoleApplication
{
    public class StartProgram
    {
        static void Main(string[] args)
        {            
            if (!args.Length.Equals(0) && args.Any(ar => !ar.Equals(string.Empty)))
            {
                if (Regex.Match(args.First(ar => !ar.Equals(string.Empty)), @"-{1}\w+\.xml").Success)
                {
                    var xmlParser = new XmlParser(args[0].Replace("-", string.Empty));
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
                    Logger.log.Error("Try to start application by put \"Applicationname.exe -xmlFileWithVehicles.xml");
                }
            }         
        }        
    }
}
