using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AcademicSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\deivi\OneDrive\Dokumentai\AcademicDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            UnameTbl.Text = "";
            PasswordTbl.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(UnameTbl.Text == "" || PasswordTbl.Text == "")
            {
                MessageBox.Show("Enter Username and Password");
            }
            Con.Open();
            SqlCommand cmb = new SqlCommand("select Id,RoleId from Users where Name=@Name and LastName=@LastName", Con);
            cmb.Parameters.AddWithValue("@Name", UnameTbl.Text);
            cmb.Parameters.AddWithValue("@LastName", PasswordTbl.Text);
            cmb.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmb);
            da.Fill(dt);
            if(dt.Rows.Count == 0)
            {
                MessageBox.Show("Incorrect Username or Password!");
            }
            string RoleId = "";
            string Id = "";
            foreach (DataRow dr in dt.Rows)
            {
                RoleId = dr["RoleId"].ToString();
                Id = dr["Id"].ToString();
                break;
            }
            switch(RoleId)
            {
                case "1":
                    MainMenu Obj = new MainMenu();
                    Obj.Show();
                    this.Hide();
                    break;
                case "2":
                    TeachersMenu Obj1 = new TeachersMenu(Id);
                    Obj1.Show();
                    this.Hide();
                    break;
                case "3":
                    StudentsMenu Obj2 = new StudentsMenu(Id);
                    Obj2.Show();
                    this.Hide();
                    break;
            }

        }

        private void UnameTbl_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
