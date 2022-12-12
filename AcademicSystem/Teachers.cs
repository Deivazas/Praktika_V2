using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AcademicSystem
{
    public partial class Teachers : Form
    {
        public Teachers()
        {
            InitializeComponent();
            DisplayTeachers();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\deivi\OneDrive\Dokumentai\AcademicDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayTeachers()
        {
            Con.Open();
            string Query = "Select Id,Name,LastName from Users where RoleId=2";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TeachersList.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Reset()
        {
            Tname.Text = "";
            Tsurname.Text = "";
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (Tname.Text == "" || Tsurname.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into Users(Name,LastName,GroupId,RoleId,FullName) values (@Name,@LastName,NULL,2,@FullName)", Con);
                    cmd.Parameters.AddWithValue("@Name", Tname.Text);
                    cmd.Parameters.AddWithValue("@LastName", Tsurname.Text);
                    cmd.Parameters.AddWithValue("@FullName", Tname.Text + " " + Tsurname.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Added");
                    Con.Close();
                    DisplayTeachers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        int Key = 0;
        private void TeachersList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tname.Text = TeachersList.SelectedRows[0].Cells[1].Value.ToString();
            Tsurname.Text = TeachersList.SelectedRows[0].Cells[2].Value.ToString();
            if (Tname.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(TeachersList.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select Teacher!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from Users where Id=@Key", Con);
                    cmd.Parameters.AddWithValue("@Key", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Deleted");
                    Con.Close();
                    DisplayTeachers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (Tname.Text == "" || Tsurname.Text == "")
            {
                MessageBox.Show("Select Teacher!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update Users set Name=@Name,LastName=@LastName", Con);
                    cmd.Parameters.AddWithValue("@Name", Tname.Text);
                    cmd.Parameters.AddWithValue("@LastName", Tsurname.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Updated");
                    Con.Close();
                    DisplayTeachers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }
    }
}
