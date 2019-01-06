using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            this.VerifyCorrectnessOfInsertedData(vehiclesList);
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

        private void VerifyCorrectnessOfInsertedData(List<Vehicle> vehiclesList)
        {
            vehiclesList.ForEach(vehicle =>
            {
                if (vehicle.GetType().GetProperties().Any(prop => this.ValidateIfProperyDoesNotSetCorrectly(prop.GetValue(vehicle, null),prop.PropertyType.Name)))
                {
                    Logger.log.Error(("Sprawdz poprawnosc pliku XML"));
                    Environment.Exit(0);
                }
            });
        }    

        private bool ValidateIfProperyDoesNotSetCorrectly(object obj, string TypeName)
        {
            if (obj == null)
            {
                Logger.log.Error(("Sprawdz poprawnosc pliku XML"));
                Environment.Exit(0);
            }

            switch (TypeName)
            {
                case "String":
                    return obj.Equals(string.Empty);
                case "Int32":
                    return obj.Equals(default(int));                    
                default:
                    return true;
            }
        }
    }
}