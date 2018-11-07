using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace SQL_Parser
{
    public partial class Form1 : Form
    {
        string[] palabras_reservadas = { "CREATE", "DROP", "DELETE", "INSERT", "UPDATE", "SELECT", "FROM", "TABLE", "DATABASE", "WHERE", "ORDER BY", "INTO", "VALUES", "SET", "ASC", "DESC" };
        string[] tipos_dato = { "INT", "FLOAT", "VARCHAR", "CHAR" };
        string[] agrupadores = { "(", ")" };
        string[] separadores = { "," };
        string[] operadores = { "+", "-", "*", "/" };
        string[] comparadores = { "=", "<", ">", "<=", ">=", "!=", "BETWEEN", "LIKE", "IN" };
        string[] uniones = { "AND", "OR", "NOT" };
        string identificador = "^[a-zA-Z]+[a-zA-Z0-9]+$";
        string cadenas = "'+.+'";
        string numero = "^[0-9]*\\.?[0-9]+$";

        Conexion conn = new Conexion();
        public Form1()
        {
            InitializeComponent();
            conn.Open();
            fill_Database();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            listLexico.Items.Clear();
            List<string> Tokens = new List<string>();
            List<string> cadena = new List<string>();
            bool lexico = true;
            bool sintactico = false;
            string temp = "";
            bool validacion_espacio = false;

            // ANALISIS LEXICO
            for (int i = 0; i < txtQuery.Text.Length; i++)
            {
                if (txtQuery.Text[i] == '\'' || temp.ToUpper() == "ORDER" || temp.ToUpper() == "ORDER " || temp.ToUpper() == "!" || temp.ToUpper() == "!=")
                {
                    validacion_espacio = !validacion_espacio;
                }
                if (txtQuery.Text[i] == '=' && validacion_espacio)
                    temp += txtQuery.Text[i];
                else if (txtQuery.Text[i] != ' ')
                {
                    if (txtQuery.Text[i] == '(' || txtQuery.Text[i] == ')' || txtQuery.Text[i] == ',' || txtQuery.Text[i] == '+' || txtQuery.Text[i] == '-' || txtQuery.Text[i] == '/' || txtQuery.Text[i] == '*' || txtQuery.Text[i] == '=')
                    {
                        if (temp != "")
                            cadena.Add(temp);
                        cadena.Add(txtQuery.Text[i].ToString());
                        temp = "";
                    }
                    else
                        temp += txtQuery.Text[i];
                }
                if (txtQuery.Text[i] == ' ' || txtQuery.Text.Length == (i+1))
                {
                    if (validacion_espacio)
                    {
                        temp += txtQuery.Text[i];
                    }
                    else if (temp != "")
                    {
                        cadena.Add(temp);
                        temp = "";
                    }
                }
            }

            foreach (string cad in cadena)
            {
                bool validacion = false;
                foreach (string reservada in palabras_reservadas)
                {
                    if (cad.ToUpper() == reservada)
                    {
                        Tokens.Add("Palabra_Reservada");
                        validacion = true;
                        break;
                    }
                }
                foreach (string tipo in tipos_dato)
                {
                    if (cad.ToUpper() == tipo)
                    {
                        Tokens.Add("Tipo_Dato");
                        validacion = true;
                        break;
                    }
                }
                foreach (string agrupador in agrupadores)
                {
                    if (cad == agrupador)
                    {
                        Tokens.Add("Agrupador");
                        validacion = true;
                        break;
                    }
                }
                foreach (string separador in separadores)
                {
                    if (cad == separador)
                    {
                        Tokens.Add("Separador");
                        validacion = true;
                        break;
                    }
                }
                foreach (string operador in operadores)
                {
                    if (cad == operador)
                    {
                        Tokens.Add("Operador");
                        validacion = true;
                        break;
                    }
                }
                foreach (string comparador in comparadores)
                {
                    if  (cad.ToUpper() == comparador)
                    {
                        Tokens.Add("Comparador");
                        validacion = true;
                        break;
                    }
                }
                foreach (string union in uniones)
                {
                    if (cad.ToUpper() == union)
                    {
                        Tokens.Add("Union");
                        validacion = true;
                        break;
                    }
                }
                if (!validacion)
                {
                    if (Regex.IsMatch(cad, identificador))
                    {
                        Tokens.Add("Identificador");
                    }
                    else if(Regex.IsMatch(cad, numero))
                    {
                        Tokens.Add("Numero");
                    }
                    else if (Regex.IsMatch(cad, cadenas))
                    {
                        Tokens.Add("Cadena");
                    }
                    else
                    {
                        Tokens.Add("INVALIDO - "+cad);
                        lexico = false;
                    }
                }
            }
            foreach (string palabra in Tokens)
            {
                listLexico.Items.Add(palabra);
            }

            //ANALISIS SINTACTICO
            switch (cadena[0].ToUpper())
            {
                case "CREATE":
                    try
                    {
                        if (cadena[1].ToUpper() == "DATABASE")
                        {
                            if (Tokens[2] == "Identificador")
                            {
                                sintactico = true;
                            }
                        }
                        else if (cadena[1].ToUpper() == "TABLE")
                        {
                            if (Tokens[2] == "Identificador")
                            {
                                if (cadena[3] == "(")
                                {
                                    int x = 4;
                                    while (cadena[x] != ")")
                                    {
                                        if (Tokens[x] == "Identificador")
                                        {
                                            x++;
                                            if (cadena[x].ToUpper() == "VARCHAR" || cadena[x].ToUpper() == "CHAR")
                                            {
                                                x++;
                                                if (cadena[x] == "(")
                                                {
                                                    x++;
                                                    if (Tokens[x] == "Numero")
                                                    {
                                                        x++;
                                                        if (cadena[x] == ")")
                                                        {
                                                            x++;
                                                            if (cadena[x] == ")")
                                                                sintactico = true;
                                                            else if (cadena[x] != ",")
                                                                break;
                                                            else
                                                                x++;
                                                        }
                                                        else
                                                            break;
                                                    }
                                                    else
                                                        break;
                                                }
                                                else
                                                    break;
                                            }
                                            else if (cadena[x].ToUpper() == "INT" || cadena[x].ToUpper() =="FLOAT")
                                            {
                                                x++;
                                                if (cadena[x] == ")")
                                                    sintactico = true;
                                                else if (cadena[x] != ",")
                                                    break;
                                                else
                                                    x++;
                                            }
                                            else
                                                break;
                                        }
                                        else
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                    break;
                case "DROP":
                    try
                    {
                        if (cadena[1].ToUpper() == "TABLE" ||cadena[1].ToUpper() == "DATABASE")
                        {
                            if (Tokens[2] == "Identificador")
                            {
                                sintactico = true;
                            }
                        }
                    }
                    catch { }
                    break;
                case "INSERT":
                    try
                    {
                        if (cadena[1].ToUpper() == "INTO")
                        {
                            if (Tokens[2] == "Identificador")
                            {
                                int x = 3;
                                bool bandera = true;
                                if (cadena[x] == "(")
                                {
                                    x++;
                                    bandera = false;
                                    while (cadena[x] != ")")
                                    {
                                        if (Tokens[x] == "Identificador")
                                        {
                                            x++;
                                            if (cadena[x] == ")")
                                            {
                                                bandera = true;
                                            }
                                            else if (cadena[x] != ",")
                                                break;
                                            else
                                                x++;
                                        }
                                    }
                                    x++;
                                }

                                if (bandera)
                                {
                                    if (cadena[x].ToUpper() == "VALUES")
                                    {
                                        x++;
                                        if (cadena[x] == "(")
                                        {
                                            x++;
                                            while (cadena[x] != ")")
                                            {
                                                if (Tokens[x] == "Cadena" ||Tokens[x] == "Numero")
                                                {
                                                    x++;
                                                    if (cadena[x] == ")")
                                                        sintactico = true;
                                                    else if (cadena[x] != ",")
                                                        break;
                                                    else
                                                        x++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                    break;
                case "DELETE":
                    try
                    {
                        if (cadena[1].ToUpper() == "FROM")
                        {
                            if (Tokens[2] == "Identificador")
                            {
                                if (cadena.Count == 3)
                                    sintactico = true;
                                if (cadena[3].ToUpper() == "WHERE")
                                {
                                    if (Tokens[4] == "Cadena" || Tokens[4] == "Numero" || Tokens[4] == "Identificador")
                                    {
                                        if (Tokens[5] == "Comparador")
                                        {
                                            if (Tokens[6] == "Cadena" || Tokens[6] == "Numero" || Tokens[6] == "Identificador")
                                            {
                                                sintactico = true;
                                                int x = 7;
                                                while (Tokens[x] == "Union")
                                                {
                                                    sintactico = false;
                                                    x++;
                                                    if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                    {
                                                        x++;
                                                        if (Tokens[x] == "Comparador")
                                                        {
                                                            x++;
                                                            if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                            {
                                                                if (x == cadena.Count - 1)
                                                                {
                                                                    sintactico = true;
                                                                }
                                                                else
                                                                    x++;
                                                            }
                                                            else
                                                                break;
                                                        }
                                                    }
                                                    else
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                    break;
                case "UPDATE":
                    try
                    {
                        if (Tokens[1] == "Identificador")
                        {
                            if (cadena[2].ToUpper() == "SET")
                            {
                                if (Tokens[3] == "Identificador" || Tokens[3] == "Numero" || Tokens[3] == "Cadena")
                                {
                                    if (Tokens[4] == "Comparador")
                                    {
                                        if (Tokens[5] == "Identificador" || Tokens[5] == "Numero" || Tokens[5] == "Cadena")
                                        {
                                            bool bandera = false;
                                            if (cadena.Count == 6)
                                            {
                                                sintactico = true;
                                                bandera = true;
                                            }

                                            int x = 6;
                                            while (cadena[x] == ",")
                                            {
                                                x++;
                                                if (Tokens[x] == "Identificador" || Tokens[x] == "Numero" || Tokens[x] == "Cadena")
                                                {
                                                    x++;
                                                    if (Tokens[x] == "Comparador")
                                                    {
                                                        x++;
                                                        if (Tokens[x] == "Identificador" || Tokens[x] == "Numero" || Tokens[x] == "Cadena")
                                                        {
                                                            if (x < cadena.Count - 1)
                                                                x++;
                                                            if (cadena[x] != ",")
                                                                bandera = true;
                                                        }
                                                    }
                                                }
                                            }
                                            if (bandera)
                                            {
                                                if (x == cadena.Count - 1)
                                                    sintactico = true;
                                                else if(cadena[x].ToUpper() == "WHERE")
                                                {
                                                    x++;
                                                    if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                    {
                                                        x++;
                                                        if (Tokens[x] == "Comparador")
                                                        {
                                                            x++;
                                                            if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                            {
                                                                x++;
                                                                if (x == cadena.Count)
                                                                    sintactico = true;
                                                                while (Tokens[x] == "Union")
                                                                {
                                                                    x++;
                                                                    if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                                    {
                                                                        x++;
                                                                        if (Tokens[x] == "Comparador")
                                                                        {
                                                                            x++;
                                                                            if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                                            {
                                                                                if (x == cadena.Count - 1)
                                                                                {
                                                                                    sintactico = true;
                                                                                }
                                                                                else
                                                                                    x++;
                                                                            }
                                                                            else
                                                                                break;
                                                                        }
                                                                    }
                                                                    else
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                    break;
                case "SELECT":
                    try
                    {
                        if (Tokens[1] == "Identificador" || cadena[1] == "*")
                        {
                            if (cadena[2].ToUpper() == "FROM")
                            {
                                if (Tokens[3] == "Identificador")
                                {
                                    int x = 4;
                                    if (cadena.Count == 4)
                                    {
                                        sintactico = true;
                                    }
                                    if (cadena[x].ToUpper() == "WHERE")
                                    {
                                        x++;
                                        if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                        {
                                            x++;
                                            if (Tokens[x] == "Comparador")
                                            {
                                                x++;
                                                if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                {
                                                    x++;
                                                    if (x == cadena.Count)
                                                        sintactico = true;
                                                    while (Tokens[x] == "Union")
                                                    {
                                                        x++;
                                                        if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                        {
                                                            x++;
                                                            if (Tokens[x] == "Comparador")
                                                            {
                                                                x++;
                                                                if (Tokens[x] == "Cadena" || Tokens[x] == "Numero" || Tokens[x] == "Identificador")
                                                                {
                                                                    if (x == cadena.Count - 1)
                                                                    {
                                                                        sintactico = true;
                                                                    }
                                                                    else
                                                                        x++;
                                                                }
                                                                else
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (cadena[x].ToUpper() == "ORDER BY")
                                    {
                                        x++;
                                        if (cadena[x].ToUpper() == "ASC" || cadena[x].ToUpper() == "DESC")
                                        {
                                            sintactico = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                    break;
            }

            lbLexico.Text = "Analisis Lexico: " + lexico.ToString();
            lbSintactico.Text = "Analisis Sintactico: " + sintactico.ToString();
            if (sintactico && lexico)
                SQLexistente();
        }

        //Variables
        StringBuilder AlertaSQL = new StringBuilder();//es como una variable string pero más chingon :v, si no furula es porque estas usando un framework defasado
        private SqlCommand insert1;
        public void SQLexistente() //Metodo creado para verficar si ya existe tabla o base de datos creados
        {
            try
            {
                if (txtQuery.Text.ToString() == "")
                {
                    //Evitando pruebas de bobos :v
                }
                else
                {
                    //"conn" es el nombre que me crea el enlace con mi base de datos, ¡OJO PERRO! tienes que declarar "private SqlConnection conn;"
                    //"insert1" es el nombre que me va hacer la consulta con la base de datos. ¡OJO PERRO! tienes que declarar "private SqlCommand insert1;"
                    try
                    {
                        String cadenabase = "use " + listBox1.Items[listBox1.SelectedIndex].ToString();
                        SqlCommand basededatos = new SqlCommand(cadenabase, conn.conn);
                    }
                    catch { }
                    insert1 = new SqlCommand(txtQuery.Text.ToString(), conn.conn);
                    insert1.ExecuteNonQuery();
                    MessageBox.Show("Se ha ejecutado la consulta correctamente");
                    actualizarTablas();
                }
            }
            catch (SqlException ex) //Aqui mostrara las alertas de SQL
            {
                txtMsg.Text = "";
                AlertaSQL.Clear();
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    AlertaSQL.Append(ex.Message);
                    if (ex.Errors[i].Number == 2714)//2714 es el numero de error que se da por crear una tabla con un nombre que ya existe en la base de datos
                    {
                        //AlertaSQL.Append(....) aqui guardara el indice, numero de error y el numero de linea donde se encuentra el error
                        AlertaSQL.Append("Indice # " + i + "\r\n" +
                            "Mensaje: " + ex.Errors[i].Message + "\r\n" +
                            "Numero: " + ex.Errors[i].Number + "\r\n" +
                            "Numero de linea: " + ex.Errors[i].LineNumber + "\r\n");
                        //AlertaSQL.Reaplace(....) aqui remplazamos el mensaje de error en ingles por español :v
                        AlertaSQL.Replace("There is already an object named", "Ya hay un objeto nombrado");
                        AlertaSQL.Replace("in the database.", "en la base datos.");
                    }
                    else if (ex.Errors[i].Number == 1801)//1801 es el numero de error que se da por crear una base de datos con un nombre que ya existe en la base de datos
                    {
                        //AlertaSQL.Append(....) aqui guardara el indice, numero de error y el numero de linea donde se encuentra el error
                        AlertaSQL.Append("Indice # " + i + "\r\n" +
                            "Mensaje: " + ex.Errors[i].Message + "\r\n" +
                            "Numero: " + ex.Errors[i].Number + "\r\n" +
                            "Numero de linea: " + ex.Errors[i].LineNumber + "\r\n");
                        //AlertaSQL.Reaplace(....) aqui remplazamos el mensaje de error en ingles por español :v
                        AlertaSQL.Replace("Database", "Base de datos");
                        AlertaSQL.Replace("already exists. Choose a different database name.", "ya existe. Elija un nombre database diferente"); //remplaza "already exists. Choose a different database name." por 
                    }
                }
                //Actualiza Texbox, se puede cambiar por mensaje de error
                txtMsg.Text += AlertaSQL.ToString() + "\r\n";

                /*PERROS SI QUIEREN CREAR TABLAS NO OLVIDEN DE USAR: "USE NOMBRE_DE_BASE_DATOS"*/
            }
        }

        public void fill_Database()
        {
            try
            {
                insert = new SqlCommand("SELECT name from sys.databases", conn.conn);
                SqlDataReader dr = insert.ExecuteReader();
                while (dr.Read())
                {
                    listBox1.Items.Add(dr.GetString(0));
                }
                dr.Close();
            }
            catch
            {

            }
        }

        private SqlCommand insert;
        StringBuilder tabla = new StringBuilder();
        public void fill_tables()
        {
            try
            {
                tabla.Append("USE N1; SELECT TABLE_NAME FROM N1.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'");
                tabla.Replace("N1", listBox1.SelectedItem.ToString());
                insert = new SqlCommand(tabla.ToString(), conn.conn);
                SqlDataReader dr = insert.ExecuteReader();
                while (dr.Read())
                {
                    listTables.Items.Add(dr.GetString(0));
                }
                dr.Close();
            }
            catch
            {

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listTables.Items.Clear();
            fill_tables();
            tabla.Clear();
        }

        private void actualizarTablas()
        {
            listTables.Items.Clear();
            listBox1.Items.Clear();
            fill_tables();
            fill_Database();
            //tabla.Clear();
        }
    }
}