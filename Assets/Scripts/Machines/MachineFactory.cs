namespace Assets.Scripts.Machines
{
    internal static class MachineFactory
    {
        public static BaseMachine CreateMachine(MachineEnum machineType)
        {
            return machineType switch
            {
                MachineEnum.BeltDown => new Belt { Direction = DirectionEnum.Down },
                MachineEnum.BeltLeft => new Belt { Direction = DirectionEnum.Left },
                MachineEnum.BeltRight => new Belt { Direction = DirectionEnum.Right },
                MachineEnum.BeltUp => new Belt { Direction = DirectionEnum.Up },
                MachineEnum.RotationMachine => new RotationMachine(),
                MachineEnum.SizeChangerMachine => new SizeChangerMachine(),
                _ => null
            };
        }
    }
}
