using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl
{
    public enum Direction { up, down }

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

        public Elevator sendElevator(Floor floor, Direction direction)
        {
            if (this.elevators.Length > 0)
            {
                return this.elevators[0];
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
        public Elevator(Floor defaultFloor)
        {
            this.defaultFloor = defaultFloor;

        }
    }

    public class Floor{
        int floorNr;

        public Floor(int floorNr, Boolean hasUpButton, Boolean hasDownButton)
        {
            this.floorNr = floorNr;   
        }

        public void pressDownButton()
        {
            ElevatorManager.Instance.sendElevator(this, Direction.down);
        }

        public void pressUpButton()
        {
            ElevatorManager.Instance.sendElevator(this, Direction.up);
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
