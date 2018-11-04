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

namespace SQL_Parser
{
    public partial class Form1 : Form
    {
        string[] palabras_reservadas = { "CREATE", "DROP", "DELETE", "INSERT", "DELETE", "UPDATE", "SELECT", "FROM", "TABLE", "DATABASE", "WHERE", "ORDER BY", "INTO", "VALUES", "SET", "ASC", "DEC" };
        string[] tipos_dato = { "INT", "FLOAT", "VARCHAR", "CHAR" };
        string[] agrupadores = { "(", ")" };
        string[] separadores = { "," };
        string[] operadores = { "+", "-", "*", "/" };
        string[] comparadores = { "=", "<", ">", "<=", ">=" };
        string identificador = "^[a-zA-Z]+[a-zA-Z0-9]+$";
        string cadenas = "'+.+'";
        string numero = "^[0-9]*\\.?[0-9]+$";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            listLexico.Items.Clear();
            List<string> Tokens = new List<string>();
            List<string> cadena = new List<string>();
            string temp = "";
            bool validacion_espacio = false;
            for (int i = 0; i < txtQuery.Text.Length; i++)
            {
                if (txtQuery.Text[i] == '\'' || temp.ToUpper() == "ORDER" || temp.ToUpper() == "ORDER ")
                {
                    validacion_espacio = !validacion_espacio;
                }
                if (txtQuery.Text[i] != ' ')
                {
                    if (txtQuery.Text[i] == '(' || txtQuery.Text[i] == ')' || txtQuery.Text[i] == ',')
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
                        Tokens.Add("Palabra_Reservada - " + cad);
                        validacion = true;
                        break;
                    }
                }
                foreach (string tipo in tipos_dato)
                {
                    if (cad.ToUpper() == tipo)
                    {
                        Tokens.Add("Tipo_Dato - " + cad);
                        validacion = true;
                        break;
                    }
                }
                foreach (string agrupador in agrupadores)
                {
                    if (cad == agrupador)
                    {
                        Tokens.Add("Agrupador - " + cad);
                        validacion = true;
                        break;
                    }
                }
                foreach (string separador in separadores)
                {
                    if (cad == separador)
                    {
                        Tokens.Add("Separador - " + cad);
                        validacion = true;
                        break;
                    }
                }
                foreach (string operador in operadores)
                {
                    if (cad == operador)
                    {
                        Tokens.Add("Operador - " + cad);
                        validacion = true;
                        break;
                    }
                }
                foreach (string comparador in comparadores)
                {
                    if  (cad == comparador)
                    {
                        Tokens.Add("Comparador - " + cad);
                        validacion = true;
                        break;
                    }
                }
                if (!validacion)
                {
                    if (Regex.IsMatch(cad, identificador))
                    {
                        Tokens.Add("Identificador - " + cad);
                    }
                    else if(Regex.IsMatch(cad, numero))
                    {
                        Tokens.Add("Numero - " + cad);
                    }
                    else if (Regex.IsMatch(cad, cadenas))
                    {
                        Tokens.Add("Cadena - " + cad);
                    }
                    else
                    {
                        Tokens.Add("INVALIDO - "+cad);
                    }
                }
            }
            foreach (string palabra in Tokens)
            {
                listLexico.Items.Add(palabra);
            }
        }
    }
}
