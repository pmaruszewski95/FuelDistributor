using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FuelDistributorConsoleApplication
{
    public class XmlParser
    {
        private readonly string path;

        public XmlParser(string path)
        {
            this.path = path;
        }

        public List<Vehicle> GetVehiclesList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vehicle>), new XmlRootAttribute("Vehicles"));

            if (!File.Exists(path))
            {
                Logger.log.ErrorFormat("Nie znaleziono pliku \"{0}\"", path);
                Environment.Exit(0);
            }

            List<Vehicle> vehiclesList = new List<Vehicle>();
            using (var stringReader = new StringReader(File.ReadAllText(path)))
            {
                try
                {
                    vehiclesList = (List<Vehicle>)serializer.Deserialize(stringReader);
                }
                catch (InvalidOperationException)
                {
                    Logger.log.Error(("Serializacja plik'u XML nieudana."));
                    Environment.Exit(0);
                }
            }

            return vehiclesList;
        }

        public void ShowAvailableVehicles(List<Vehicle> vehiclesList)
        {
            Logger.log.Info(@"Marka | Pojemnosc baku [L] | Typ paliwa [Pb95|Pb98|LPG|ON]");

            foreach (var vehicle in vehiclesList)
            {
                Logger.log.InfoFormat(@"{0}. {1} | {2} | {3}", vehiclesList.IndexOf(vehicle), vehicle.Name, vehicle.FuelCapacity, vehicle.FuelType);
            }
        }        
    }
}