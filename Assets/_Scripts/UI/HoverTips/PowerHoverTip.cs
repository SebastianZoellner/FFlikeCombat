public class PowerHoverTip : HoverTip
{
    private PowerSO power;

    public void SetPower(PowerSO power) => this.power = power;



    protected override string GetTip()
    {
        string tip;
        tip = power.name;
        tip += "\n" + power.description;
        tip += "Endurance Cost:  " + power.enduranceCost.ToString("0");

        return tip;
    }
}
