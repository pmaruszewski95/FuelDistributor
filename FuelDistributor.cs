using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace FuelDistributorConsoleApplication
{
    public class FuelDistributor
    {
        enum FuelKinds
        {
            Pb95,
            Pb98,
            ON,
            LPG
        }

        private uint? carIndex;
        private uint? literAmmount;
        private FuelKinds? fuelKind;
        private readonly List<Vehicle> vehiclesList;

        public FuelDistributor(List<Vehicle> vehiclesList)
        {
            this.vehiclesList = vehiclesList;
        }

        public void ChooseCarMark()
        {
            string inputNumber;
          
            Logger.log.Info("Wybierz auto ktore chcesz zatankowac podajac numer");
            inputNumber = Console.ReadLine();
            Logger.log.InfoFormat("Wybrano pojazd nr. {0}", inputNumber);
            if (!uint.TryParse(inputNumber, out uint indexVehicle) || this.vehiclesList.Count <= indexVehicle)
            {
                this.carIndex = null;
                return;
            }

            this.carIndex = indexVehicle;
        }        

        public void ChooseFuelKind()
        {           
            Logger.log.Info("Wybierz rodzaj paliwa które chcesz zatankować");            
            var inputKindOfFuel = Console.ReadLine();
            Logger.log.InfoFormat("Wybrano paliwo - {0}", inputKindOfFuel);

            FuelKinds fuelKind;
            if (!Enum.TryParse(inputKindOfFuel, out fuelKind))
            {
                this.fuelKind = null;
                return;
            }            
            
            this.fuelKind = fuelKind;
        }

        public void ChooseFuelCapacity()
        {
            Logger.log.Info("Podaj liczbe litrow ktore chcesz zatankowac");
            var inputLiterNumber = Console.ReadLine();
            Logger.log.InfoFormat("Wybrano ilosc paliwa - {0}", inputLiterNumber);

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
                Console.WriteLine("Tankowanie...");
                Task.Delay(300).Wait();
            } while (tanking); 
        }

        public void VerifyOperation()
        {  
            if (this.carIndex == null)
            {
                Logger.log.Error("Brak auta");
                return;
            }

            if (this.fuelKind == null || !this.fuelKind.ToString().Equals(this.vehiclesList.ElementAt((int)this.carIndex).FuelType, StringComparison.InvariantCultureIgnoreCase))
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

            Logger.log.Error("Niepoprawna ilosc zatankowanego paliwa");
        }
    }
}
