using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FuelDistributorConsoleApplication
{
    public class FuelDistributor
    {
        private int carIndex;
        private string fuelKind;
        private uint literAmmount;
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

                if (!int.TryParse(inputNumber, out int indexVehicle) || this.vehiclesList.Count <= indexVehicle)
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
            string inputKindOfFuel;

            do
            {
                Logger.log.Info("Wybierz rodzaj paliwa które chcesz zatankować");
            
                inputKindOfFuel = Console.ReadLine();

                if (!fuelKindsList.Contains(inputKindOfFuel, StringComparer.InvariantCultureIgnoreCase))
                {
                    Logger.log.WarnFormat("Brak {0} paliwa w dystrybutorze", inputKindOfFuel);
                    continue;
                }

                this.fuelKind = inputKindOfFuel;
                break;

            } while (true);
        }

        public void ChooseFuelCapacity()
        {
            string inputLiterNumber;
            do
            {
                Logger.log.Info("Podaj liczbe litrow ktore chcesz zatankowac");
                inputLiterNumber = Console.ReadLine();

                if (!uint.TryParse(inputLiterNumber, out uint indexVehicle))
                {
                    Logger.log.Warn("Niepoprawna ilosc litrow");
                    continue;
                }

                this.literAmmount = indexVehicle;
                break;

            } while (true);
        }

        public void TankCar()
        {
            bool tanking = true;
            var timerEnded = new Timer { Interval = this.literAmmount * 100, AutoReset = false };
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
            if (!this.fuelKind.Equals(this.vehiclesList.ElementAt(this.carIndex).FuelType, StringComparison.InvariantCultureIgnoreCase))
            {
                Logger.log.Error("Zle paliwo");
                return;
            }
           
            if (this.literAmmount <= this.vehiclesList.ElementAt(this.carIndex).FuelCapacity)
            {
                Logger.log.Info("Prawidlowa ilosc zatankowanego paliwa");
                return;
            }

            Logger.log.Error("Zbyt duza ilosc zatankowanego paliwa");
        }
    }
}
