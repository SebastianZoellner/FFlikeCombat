

public enum Attribute 
{
    Agillity,
    Armor,
    Combat,
    Hardiness,
    Initiative,
    Power,
    Recovery,
    Speed
}

[System.Serializable]
public struct AttributeValue
{
    public Attribute attribute;
    public float value;
}
