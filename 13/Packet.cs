using Newtonsoft.Json.Linq;

public enum PacketDataType
{
    Array,
    Integer
}

public class Packet
{
    public List<Packet> DataItems = new List<Packet>();
    public int? PacketValue;
    public PacketDataType PacketType;

    public Packet(Packet item)
    {
        DataItems.Add(item);
        PacketType = PacketDataType.Array;
    }

    public Packet(JToken? input)
    {
        if (input == null)
        {
            throw new Exception();
        }

        if (input.Type == JTokenType.Array)
        {
            PacketType = PacketDataType.Array;
            DataItems.AddRange(input.Children().Select(jt => new Packet(jt)).ToArray());
        }
        else if (input.Type == JTokenType.Integer)
        {
            PacketType = PacketDataType.Integer;
            PacketValue = input.Value<int>();
        }
    }
}