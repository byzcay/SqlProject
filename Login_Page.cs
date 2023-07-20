using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace EmployeeManagementProject
{
    public partial class Login_Page : Form
    {
        public Login_Page()
        {
            InitializeComponent();
        }

        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-DPC8B16\\SQLEXPRESS;Integrated Security=True");

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text != "" && txtPassword.Text != "")
                {
                    SqlCommand cmd = new SqlCommand();
                    baglantı.Open();
                    cmd.Connection = baglantı;
                    cmd.CommandText = "Use EmployeeManagement select * from Users where username ='" + txtUsername.Text + "' And password = '" + txtPassword.Text + "'";
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        this.Hide();
                        InformationScreen gecis = new InformationScreen();
                        gecis.Show();
                        //this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Wrong username or password!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    }

                }
                else
                    MessageBox.Show("Username and password cannot be empty !", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            baglantı.Close();
        }
    }
}
