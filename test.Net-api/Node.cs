namespace test.Net_api;

[Serializable]
public class Node
{
    public SpeedRecord Data { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
}