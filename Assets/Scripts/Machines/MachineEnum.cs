namespace Assets.Scripts.Machines
{
    public enum MachineEnum
    {
        None = 0,

        BeltLeft = 0b010001,
        BeltRight = 0b010010,
        BeltUp = 0b010100,
        BeltDown = 0b011000,

        RotationMachine = 0b100001,
        SizeChangerMachine = 0b100010
    }
}
