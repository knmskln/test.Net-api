using System.Runtime.Serialization.Formatters.Binary;

namespace test.Net_api;

public class BinaryTree
{
    private Node _root;

    public Node Root
    {
        get { return _root; }
        private set { _root = value; }
    }
    
    public void Insert(SpeedRecord data)
    {
        _root = InsertRec(_root, data);
    }

    private Node InsertRec(Node root, SpeedRecord data)
    {
        if (root == null)
        {
            root = new Node { Data = data, Left = null, Right = null };
            return root;
        }

        if (data.Speed < root.Data.Speed)
            root.Left = InsertRec(root.Left, data);
        else if (data.Speed > root.Data.Speed)
            root.Right = InsertRec(root.Right, data);

        return root;
    }

    public void InorderTraversal(Node root, Action<SpeedRecord> action)
    {
        if (root != null)
        {
            InorderTraversal(root.Left, action);
            action(root.Data);
            InorderTraversal(root.Right, action);
        }
    }
    [Obsolete("Obsolete")]
    public void SerializeTree(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, _root);
        }
    }

    [Obsolete("Obsolete")]
    public void DeserializeTree(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            _root = (Node)binaryFormatter.Deserialize(fileStream);
        }
    }
    public SpeedRecord FindMinSpeedRecord(Node node)
    {
        if (node == null)
            return null;

        SpeedRecord currentMin = node.Data;
        SpeedRecord leftMin = FindMinSpeedRecord(node.Left);

        if (leftMin != null && leftMin.Speed < currentMin.Speed)
            currentMin = leftMin;

        return currentMin;
    }

    public SpeedRecord FindMaxSpeedRecord(Node node)
    {
        if (node == null)
            return null;

        SpeedRecord currentMax = node.Data;
        SpeedRecord rightMax = FindMaxSpeedRecord(node.Right);

        if (rightMax != null && rightMax.Speed > currentMax.Speed)
            currentMax = rightMax;

        return currentMax;
    }
    
    public IEnumerable<SpeedRecord> FindSpeedRecordsAboveThreshold(Node node, double threshold)
    {
        if (node == null)
            return Enumerable.Empty<SpeedRecord>();

        List<SpeedRecord> records = new List<SpeedRecord>();

        if (node.Data.Speed > threshold)
        {
            records.Add(node.Data);
            records.AddRange(FindSpeedRecordsAboveThreshold(node.Left, threshold));
            records.AddRange(FindSpeedRecordsAboveThreshold(node.Right, threshold));
        }

        if (node.Data.Speed <= threshold)
        {
            records.AddRange(FindSpeedRecordsAboveThreshold(node.Right, threshold));
        }

        return records;
    }
}