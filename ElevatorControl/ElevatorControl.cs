using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ElevatorControl
{
    public enum Direction { up, down }

    class StartHere
    {
        static void Main(string[] args)
        {
            Floor firstFloor = new Floor(0, true, false);
            Floor secondFloor = new Floor(1, true, true);
            Floor thirdFloor = new Floor(2, true, true);
            Floor fourthFloor = new Floor(3, false, true);
            Elevator elevator1 = new Elevator(firstFloor);
            Elevator elevator2 = new Elevator(firstFloor);
            ElevatorManager manager = ElevatorManager.Instance;
            Floor [] floors = new Floor[] { firstFloor, secondFloor, thirdFloor, fourthFloor };
            manager.setFloors(floors);
            Elevator[] elevators = new Elevator[] { elevator1, elevator2 };
            manager.setElevators(elevators);

            manager.getFloors()[2].pressUpButton();
            manager.getFloors()[1].pressDownButton();

            Console.ReadLine();

        }
    }

    public class ElevatorManager
    {
        private static ElevatorManager manager;
        private Elevator[] elevators;
        private Floor[] floors;

        private ElevatorManager() { }

        public static ElevatorManager Instance
        {
            get
            {
                if(manager == null)
                {
                    manager = new ElevatorManager();
                }
                return manager;
            }
        }

        public void setElevators(Elevator[] elevators)
        {
            this.elevators = elevators;
        }

        public void setFloors(Floor[] floors)
        {
            this.floors = floors;
        }

        public Floor[] getFloors()
        {
            return this.floors;
        }

        public void sendElevator(Floor floor, Direction direction)
        {
            if (this.elevators.Length > 0)
            {
                //Should dispatch an Elevator
                IEnumerable<Elevator>freeElevators = from elevator in elevators where !elevator.isBusy() select elevator;
                freeElevators.ElementAt<Elevator>(0).goToFloor(floor); //Dispatch the first free elevator it can find.
                /*Still needs the following
                 * Handling if no elevators are available
                 * Handling direction. If elevator is coming by a floor where the button is pressed, stop there
                 * Handling the button pressing inside the elevator to go to a floor 
                 */
            }
            else
            {
                throw new NoElevatorFoundException("No Elevators added to the manager");
            }
        }

    }

    public class Elevator
    {
        Floor defaultFloor;
        Floor destFloor;
        Floor currentFloor;

        public Elevator(Floor defaultFloor)
        {
            this.defaultFloor = defaultFloor;
            this.currentFloor = defaultFloor;
            this.destFloor = defaultFloor;
        }

        public void goToFloor(Floor floor)
        {
            this.destFloor = floor;
            this.goToFloorAsync(floor);
        }

        private async Task goToFloorAsync(Floor floor)
        {
            //Simulate an elevator actually going to a floor
            await Task.Run(() =>
                {
                    Console.Write("Dispatching");
                    Floor[] floors = ElevatorManager.Instance.getFloors();
                    if (this.destFloor > this.currentFloor)
                    {
                        do
                        {
                            this.currentFloor = floors[Array.IndexOf(floors, this.currentFloor) + 1];
                            Thread.Sleep(1000);
                        } while (this.destFloor != this.currentFloor);
                    }
                    else
                    {
                        do
                        {
                            this.currentFloor = floors[Array.IndexOf(floors, this.currentFloor) - 1];
                            Thread.Sleep(1000);
                        } while (this.destFloor != this.currentFloor);
                    }
                }
            );
            
        }

        public bool isBusy()
        {
            if(this.destFloor.Equals(this.currentFloor)){
                return false;
            }else
            {
                return true;
            }
        }
    }

    public class Floor{
        public int floorNr;
        private bool hasUpButton;
        private bool hasDownButton;

        public Floor(int floorNr, Boolean hasUpButton, Boolean hasDownButton)
        {
            this.floorNr = floorNr;
            this.hasUpButton = hasUpButton;
            this.hasDownButton = hasDownButton; 
        }

        public void pressDownButton()
        {
            if (this.hasDownButton)
            {
                ElevatorManager.Instance.sendElevator(this, Direction.down);
            }else
            {
                //TODO Throw exception
            }
        }

        public void pressUpButton()
        {
            if (this.hasUpButton)
            {
                ElevatorManager.Instance.sendElevator(this, Direction.up);
            }else
            {
                //TODO Throw exception
            }
        }

        public static bool operator <(Floor floor1, Floor floor2)
        {
            return floor1.floorNr < floor2.floorNr;
        }

        public static bool operator >(Floor floor1, Floor floor2)
        {
            return floor1.floorNr > floor2.floorNr;
        }
    }

    public class NoElevatorFoundException: Exception
    {
        public NoElevatorFoundException()
        {

        }

        public NoElevatorFoundException(string message) : base(message)
        {
        }

        public NoElevatorFoundException(string message, Exception inner): base(message, inner)
        {

        }
    }
}
