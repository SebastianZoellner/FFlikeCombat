public class PowerHoverTip : HoverTip
{
    private PowerSO power;

    public void SetPower(PowerSO power) => this.power = power;



    protected override string GetTip()
    {
        string tip;
        tip = power.name;
        tip += "\n" + power.description;
        tip += "\nEndurance Cost:  " + power.GetEnduranceCost().ToString("0");

        return tip;
    }
}
