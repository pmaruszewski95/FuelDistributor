using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace FuelDistributorConsoleApplication
{
    public class FuelDistributor
    {
        private uint carIndex;
        private string fuelKind;
        private uint? literAmmount;
        private List<string> fuelKindsList = new List<string> { "Pb95", "Pb98", "ON", "LPG" };
        private readonly List<Vehicle> vehiclesList;

        public FuelDistributor(List<Vehicle> vehiclesList)
        {
            this.vehiclesList = vehiclesList;
        }

        public void ChooseCarMark()
        {
            string inputNumber;
            do
            {
                Logger.log.Info("Wybierz auto ktore chcesz zatankowac podajac numer");
                inputNumber = Console.ReadLine();

                if (!uint.TryParse(inputNumber, out uint indexVehicle) || this.vehiclesList.Count <= indexVehicle)
                {
                    Logger.log.Warn("Niepoprawne dane lub numer spoza listy");
                    continue;
                }

                this.carIndex = indexVehicle;
                break;

            } while (true);
        }        

        public void ChooseFuelKind()
        {           
            Logger.log.Info("Wybierz rodzaj paliwa które chcesz zatankować");            
            var inputKindOfFuel = Console.ReadLine();
            
            if (!fuelKindsList.Contains(inputKindOfFuel, StringComparer.InvariantCultureIgnoreCase))
            {
                this.fuelKind = null;
                return;            
            }
            
            this.fuelKind = inputKindOfFuel;
        }

        public void ChooseFuelCapacity()
        {
            Logger.log.Info("Podaj liczbe litrow ktore chcesz zatankowac");
            var inputLiterNumber = Console.ReadLine();

            if (!uint.TryParse(inputLiterNumber, out uint outputLitres) || outputLitres == 0)
            {
                this.literAmmount = null;
                return;
            }

            this.literAmmount = outputLitres;
        }

        public void TankCar()
        {
            bool tanking = true;
            var timerEnded = new Timer { Interval = this.literAmmount == null ? 1 : (double)this.literAmmount * 100, AutoReset = false };
            timerEnded.Elapsed += (sender, args) =>
            {
                Logger.log.Info("Koniec tankowania!");
                tanking = false;
            };

            timerEnded.Start();

            do
            {
                Console.Write("Tankowanie...");
                Task.Delay(1000).Wait();
            } while (tanking); 
        }

        public void VerifyOperation()
        { 
            if (this.fuelKind == null || !this.fuelKind.Equals(this.vehiclesList.ElementAt((int)this.carIndex).FuelType, StringComparison.InvariantCultureIgnoreCase))
            {
                Logger.log.Error("Zatankowano zle paliwo");
            }
            else
            {
                Logger.log.Info("Zatankowano poprawne paliwo");
            }
           
            if (this.literAmmount <= this.vehiclesList.ElementAt((int)this.carIndex).FuelCapacity)
            {
                Logger.log.Info("Zatankowano prawidlowa ilosc paliwa");
                return;
            }

            Logger.log.Error("Zbyt duza ilosc zatankowanego paliwa");
        }
    }
}
