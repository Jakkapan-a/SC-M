using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M.Modules
{
    public class HistoryData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string softwareLabel { get; set; }
        public string softwareECU { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        // Get all data
        public static List<HistoryData> GetAll()
        {
            return SQliteDataAccess.GetAll<HistoryData>("history_data");
        }

        // Get row
        public static List<HistoryData> GetRow(string sql)
        {
            return SQliteDataAccess.GetRow<HistoryData>(sql);
        }

        // Insert to Db
        public void Insert()
        {
            string sql = "insert into history_data (name, softwareLabel, softwareECU, created_at, updated_at) values (@name, @softwareLabel, @softwareECU, @created_at, @updated_at)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", this.name);
            parameters.Add("@softwareLabel", this.softwareLabel);
            parameters.Add("@softwareECU", this.softwareECU);
            parameters.Add("@created_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("@updated_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SQliteDataAccess.InserTnputDB(sql, parameters);
        }

        // Update
        public void Update()
        {
            string sql = "update history_data set name = @name, softwareLabel = @softwareLabel, softwareECU = @softwareECU, updated_at = @updated_at where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", this.id);
            parameters.Add("@name", this.name);
            parameters.Add("@softwareLabel", this.softwareLabel);
            parameters.Add("@softwareECU", this.softwareECU);
            parameters.Add("@updated_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SQliteDataAccess.Update(sql, parameters);
        }

        // Delete
        public void Delete()
        {
            string sql = "delete from history_data where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", this.id);
            SQliteDataAccess.Update(sql, parameters);
        }
                
    }
}
