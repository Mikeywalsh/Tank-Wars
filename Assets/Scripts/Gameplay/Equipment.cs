using System;

public enum Equipment
{
    Cannon = 0,
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

    public static string KillString(this Equipment e, bool suicide, bool environmental, string p1, string p2)
    {
        switch (e)
        {
            case (Equipment.Cannon):
                if (suicide)
                    return new string[] { "{0} Shot {1}", "{1} was blew up by {0}'s Cannon" }[UnityEngine.Random.Range(0, 2)];
                else
                    return "{0} shot themself, whoops";
            case (Equipment.Laser):
                return new string[] { "{0} Lasered {1}", "{1} was fried by {2}'s Laser" }[UnityEngine.Random.Range(0, 2)];
            case (Equipment.Mine):
                if (suicide)
                    return "{0} was blown up by their own mine, whoops";
                else if (environmental)
                    return "{0} was killed by a mine";
                else
                    return "{0} was blown up by {1}'s mine";
            default:
                throw new Exception("Unknown Equipment type");
        }
    }
}