using System.Data;
using MySql.Data.MySqlClient;
namespace KalkulatorMaui
{
    public partial class MainPage : ContentPage
    {
        private MySqlConnection connection;
        double equal = .0;
        double pi = Math.PI;
        double e_ = Math.E;
        bool operandPressed = false;
        string action = "";
        List<string> operands = new List<string>();
        string[] tmp = { "+", "-", "*", "/", "=", "%", "pi", ",", "mod", "back", "e", "exp", "|x|", "2nd", "x2", "In", "+/-", "log", "10x", "²√x" };
        public MainPage()
        {
            InitializeComponent();
            operands.AddRange(tmp.ToList());

        }
        private bool IsValidInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string allowedCharacters = "0123456789+-*/.%";

            foreach (char c in input)
            {
                if (!allowedCharacters.Contains(c))
                {
                    return false; 
                }
            }
            if (input.Contains("/0") || input.Contains("%0")) return false;

            return true;
        }

        public void DatabaseConnection()
        {
            string connectionString = "SERVER=localhost;DATABASE=kalkulator;UID=root";
            connection = new MySqlConnection(connectionString);
            connection.Open();

        }
        private void btnnumber_Click(object sender, EventArgs e)
        {
            var data = ((Button)sender).Text.ToString();

            if (data == "=")
            {
                string x = ekran1.Text.Replace(',', '.');
                if (IsValidInput(x))
                {
                    DataTable dt = new DataTable();
                    var v = dt.Compute(x, "");
                    ekran1.Text = v.ToString();
                    DatabaseConnection();
                    string query = "INSERT INTO wyniki (`liczba`) VALUES (@v);";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@v", v);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                else
                {
                    ekran1.Text = "Error: Invalid input";
                }
            }
            else
            {
                if (ekran1.Text == "Error: Invalid input")
                {
                    ekran1.Text = "";
                }
                ekran1.Text += data.ToString();
            }
        }

        private void ce(object sender, EventArgs e)
        {
            equal = .0;
            operandPressed = false;
            action = "";
            ekran1.Text = "";
        }

        private void comma_Click(object sender, EventArgs e)
        {
            ekran1.Text += ".";
        }

        private void plusminus_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            if (math[0] == '-')
            {
                math = math.Substring(1);
            }
            else
            {
                math = "-" + math;
            }
            ekran1.Text = math;
        }

        private void pi_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            char last = math[math.Length - 1];
            if (last == '+' || last == '-' || last == '/' || last == '*')
            {
                math += pi.ToString();
                ekran1.Text = math;
            }
            else
            {
                ekran1.Text = pi.ToString();
            }
        }

        private void pierw_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();

            double sqrt = Math.Sqrt(Convert.ToInt32(math));
            ekran1.Text = sqrt.ToString();
        }

        private void e_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            char last = math[math.Length - 1];
            if (last == '+' || last == '-' || last == '/' || last == '*')
            {
                math += e_.ToString();
                ekran1.Text = math;
            }
            else
            {
                ekran1.Text = e_.ToString();
            }
        }

        private void log_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            double log = 0;
            if (((Button)sender).Text.ToString() == "log")
            {
                log = Math.Log(Convert.ToDouble(math), 10);

            }
            else
            {
                log = Math.Log(Convert.ToDouble(math));
            }
            ekran1.Text = log.ToString();
        }

        private void Pow_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            int y = Convert.ToInt32(math);
            double x = 0;
            if (((Button)sender).Text.ToString() == "10x")
            {
                x = Math.Pow(10, y);
            }
            else
            {
                x = Math.Pow(y, 2);
            }
            ekran1.Text = x.ToString();
        }

        private void Mianownik_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            if (IsValidInput(math))
            {
                double x = Convert.ToDouble(math);
                if (x != 0)
                {
                    double y = 1 / x;
                    ekran1.Text = y.ToString();
                }
                else
                {
                    ekran1.Text = "Error: Division by zero";
                }
            }
            else
            {
                ekran1.Text = "Error: Invalid input";
            }
        }

        private void Nawias_Click(object sender, EventArgs e)
        {
            ekran1.Text += ((Button)sender).Text;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            if (ekran1.Text != "")
            {
                string back = math.Substring(0, math.Length - 1);
                ekran1.Text = back;
            }
        }

        private void Tan_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            double rad = Convert.ToDouble(math) * (pi / 180);
            double wynik = Math.Tan(rad);
            ekran1.Text = wynik.ToString();

        }

        private void SinCos_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            double kat = pi * Convert.ToDouble(math) / 180;
            double x = 0;
            if (((Button)sender).Text.ToString() == "sin")
            {
                x = Math.Sin(kat);
            }
            else
            {
                x = Math.Cos(kat);
            }
            ekran1.Text = x.ToString();
        }

        private void rand_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            double x = rand.NextDouble();
            ekran1.Text = x.ToString();
        }

        private void Mod_Click(object sender, EventArgs e)
        {
            ekran1.Text += "%";
        }

        private void Exp_Click(object sender, EventArgs e)
        {

        }

        private void Fact_Click(object sender, EventArgs e)
        {
            string math = ekran1.Text.ToString();
            int x = Convert.ToInt32(math);
            for (int i = x - 1; i > 0; i--)
            {
                x = x * i;
            }
            ekran1.Text = x.ToString();
        }
    }

}
