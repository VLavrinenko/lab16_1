using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    class RawDataItem
    {
        public String Name { get; set; }//+
        public int Rooms { get; set; }//+
        public int Meters { get; set; } //+
        public int Price { get; set; }//+
        public float OneMeterPrice//+
        {
            get { return Price / Meters; }
        }
    }
    class SummaryDataItem
    {
        public String GroupName { get; set; }
        public float GroupSum { get; set; }
    }
    interface DataInterface
    {
        List<RawDataItem> GetRawData();
        List<SummaryDataItem> GetSummaryData();
    }
    class DataStorage : DataInterface
    {
        public bool IsReady
        {
            get
            {
                if (rawdata == null)
                    return false;
                else
                    return true;
            }
        }
        private List<RawDataItem> rawdata;
        private List<SummaryDataItem> sumdata;
        private char devider = '*';
        public DataStorage() { }

        private bool InitData(string datapath)
        {
            rawdata = new List<RawDataItem>();

            try
            {
                StreamReader sr = new StreamReader(datapath, Encoding.UTF8);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] items = line.Split(devider);
                    var item = new RawDataItem()
                    {
                        Name = items[0].Trim(),
                        Rooms = Convert.ToInt32(items[1].Trim()),
                        Meters = Convert.ToInt32(items[2].Trim()),
                        Price = Convert.ToInt32(items[3].Trim())
                    };
                    rawdata.Add(item);
                }
                sr.Close();
                BuildSummary();
            }
            catch (IOException ex)
            {
                return false;
            }
            return true;
        }
        private void BuildSummary()
        {
            Dictionary<string, int> tmp = new Dictionary<string, int>();
            foreach (var item in rawdata)
            {
                if (tmp.ContainsKey(item.Name))
                    tmp[item.Name] += item.Price;
                else
                    tmp.Add(item.Name, item.Price);
            }
            sumdata = new List<SummaryDataItem>();
            foreach (var item in tmp)
            {
                sumdata.Add(new SummaryDataItem()
                {
                    GroupName = item.Key,
                    GroupSum = item.Value
                }) ;
            }
        }
        public static DataStorage DataCreator(String path)
        {
            DataStorage d = new DataStorage();
            if (d.InitData(path))
                return d;
            else
                return null;
        }
        public List<RawDataItem> GetRawData()
        {
            if (this.IsReady)
                return rawdata;
            else
                return null;
        }

        public List<SummaryDataItem> GetSummaryData()
        {
            if (this.IsReady)
                return sumdata;
            else
                return null;

        }
    }
}