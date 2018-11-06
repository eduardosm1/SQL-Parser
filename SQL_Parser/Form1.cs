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
            bool lexico = true;
            bool sintactico = false;
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
                    if  (cad == comparador)
                    {
                        Tokens.Add("Comparador");
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
                                            if (Tokens[x] == "Tipo_Dato")
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
            }
            lbLexico.Text = "Analisis Lexico: " + lexico.ToString();
            lbSintactico.Text = "Analisis Sintactico: " + sintactico.ToString();
        }
    }
}