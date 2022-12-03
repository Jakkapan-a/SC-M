using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M.Modules
{
    public class MasterData
    {
        public int id { get; set; }
        public string softwareLabel { get; set; }
        public string softwareECU { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public static List<MasterData> GetAll()
        {
            return SQliteDataAccess.GetAll<MasterData>("master_data");
        }

        public static List<MasterData> GetRow(string sql)
        {
            return SQliteDataAccess.GetRow<MasterData>(sql);
        }

        // Insert to Db
        public void Save()
        {
            string sql = "insert into master_data (softwareLabel, softwareECU, created_at, updated_at) values (@softwareLabel, @softwareECU, @created_at, @updated_at)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@softwareLabel", this.softwareLabel);
            parameters.Add("@softwareECU", this.softwareECU);
            parameters.Add("@created_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("@updated_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SQliteDataAccess.InserTnputDB(sql, parameters);
        }

        // Update
        public void Update()
        {
            string sql = "update master_data set softwareLabel = @softwareLabel, softwareECU = @softwareECU, updated_at = @updated_at where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", this.id);
            parameters.Add("@softwareLabel", this.softwareLabel);
            parameters.Add("@softwareECU", this.softwareECU);
            parameters.Add("@updated_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SQliteDataAccess.Update(sql, parameters);
        }

        // Delete
        public void Delete()
        {
            string sql = "delete from master_data where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", this.id);
            SQliteDataAccess.Update(sql, parameters);
         }
    }
}
