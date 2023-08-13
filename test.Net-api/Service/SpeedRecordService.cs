namespace test.Net_api.Service;

public class SpeedRecordService
    {
        private const string DataDirectory = "Data";

        [Obsolete("Obsolete")]
        public async Task SaveSpeedRecordAsync(SpeedRecord record)
        {
            BinaryTree tree = new BinaryTree();
            
            var filePath = GetFilePathForDate(record.Timestamp.Date);
            var data = $"{record.Timestamp},{record.VehicleNumber},{record.Speed}\n";
            
            var projectDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(projectDirectory, DataDirectory, filePath);
            if (File.Exists(fullPath))
            {
                tree.DeserializeTree(fullPath);
                tree.Insert(record);
                tree.SerializeTree(fullPath);
            }
            else
            {
                tree.Insert(record);
                tree.SerializeTree(fullPath);
            }
            // Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            await File.AppendAllTextAsync(fullPath, data);
        }

        private string GetFilePathForDate(DateTime date)
        {
            var fileName = $"{date:yyyy-MM-dd}.bin";
            return fileName;
        }
    
        [Obsolete("Obsolete")]
        public IEnumerable<SpeedRecord> GetMinMaxForDate(DateTime date)
        {
            var filePath = GetFilePathForDate(date);

            var projectDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(projectDirectory, DataDirectory, filePath);

            if (!File.Exists(fullPath))
            {
                return Enumerable.Empty<SpeedRecord>();
            }

            BinaryTree tree = new BinaryTree();
            tree.DeserializeTree(fullPath);

            List<SpeedRecord> records = new List<SpeedRecord>();
            
            records.Add(tree.FindMinSpeedRecord(tree.Root));
            records.Add(tree.FindMaxSpeedRecord(tree.Root));

            return records;
        }
        
        [Obsolete("Obsolete")]
        public IEnumerable<SpeedRecord> GetSpeedRecordsForDate(DateTime date, double threshold)
        {
            var filePath = GetFilePathForDate(date);

            var projectDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(projectDirectory, DataDirectory, filePath);

            if (!File.Exists(fullPath))
            {
                return Enumerable.Empty<SpeedRecord>();
            }

            BinaryTree tree = new BinaryTree();
            tree.DeserializeTree(fullPath);
            
            IEnumerable<SpeedRecord> records = tree.FindSpeedRecordsAboveThreshold(tree.Root, threshold);

            return records;
        }
        
        [Obsolete("Obsolete")]
        public IEnumerable<SpeedRecord> GetAll(DateTime date)
        {
            var filePath = GetFilePathForDate(date);

            var projectDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(projectDirectory, DataDirectory, filePath);

            if (!File.Exists(fullPath))
            {
                return Enumerable.Empty<SpeedRecord>();
            }

            BinaryTree tree = new BinaryTree();
            tree.DeserializeTree(fullPath);

            List<SpeedRecord> records = new List<SpeedRecord>();
            
            tree.InorderTraversal(tree.Root, node =>
            {
                records.Add(new SpeedRecord
                {
                    Timestamp = node.Timestamp,
                    VehicleNumber = node.VehicleNumber,
                    Speed = node.Speed
                });
            });

            return records;
        }
    }