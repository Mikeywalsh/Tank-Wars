using System;

public enum Equipment
{
    Laser,
    Mine
}

static class EquipmentMethods
{
    public static string PluralText(this Equipment e)
    {
        switch(e)
        {
            case (Equipment.Laser):
                return "Laser Charge";
            case (Equipment.Mine):
                return "Mines";
            default:
                throw new Exception("Unknown Equipment type");
        }
    }
}