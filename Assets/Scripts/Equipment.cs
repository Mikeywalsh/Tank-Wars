using System;

public enum Equipment
{
    Laser,
    Mine
}

static class EquipmentMethods
{
    public static string AmmoName(this Equipment e)
    {
        switch(e)
        {
            case (Equipment.Laser):
                return "Laser Charge";
            case (Equipment.Mine):
                return "Mine";
            default:
                throw new Exception("Unknown Equipment type");
        }
    }

    public static int PickupAmountMin(this Equipment e)
    {
        switch (e)
        {
            case (Equipment.Laser):
                return 50;
            case (Equipment.Mine):
                return 1;
            default:
                throw new Exception("Unknown Equipment type");
        }
    }

    public static int PickupAmountMax(this Equipment e)
    {
        switch (e)
        {
            case (Equipment.Laser):
                return 100;
            case (Equipment.Mine):
                return 3;
            default:
                throw new Exception("Unknown Equipment type");
        }
    }

    public static int MaxAmount(this Equipment e)
    {
        switch (e)
        {
            case (Equipment.Laser):
                return 250;
            case (Equipment.Mine):
                return 5;
            default:
                throw new Exception("Unknown Equipment type");
        }
    }
}