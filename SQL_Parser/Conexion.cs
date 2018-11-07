using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SQL_Parser
{
    class Conexion
    {
        public string sentencia = "Data Source=localhost; Integrated Security=True";
        public SqlConnection conn;
        public Conexion()
        {
            conn = new SqlConnection(sentencia);
        }
        
        public void Open()
        {
            conn.Open();
        }
    }
}
