using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AcademicSystem
{
    public partial class Students : Form
    {
        public Students()
        {
            InitializeComponent();
            DisplayStudent();
        }
SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\deivi\OneDrive\Dokumentai\AcademicDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayStudent()
        {
            Con.Open();
            string Query = "Select U.Id,U.Name,LastName,G.Name as GroupName from Users U left join Groups G on U.GroupId=G.Id where RoleId=3";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            StudentsList.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void SaveBtn_Click_1(object sender, EventArgs e)
        {
            if (StName.Text == "" || StSurname.Text == "" || StGroup.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into Users(Name,LastName,GroupId,RoleId,FullName) values (@Sname,@Ssurname,@GroupId,3,@FullName)", Con);
                    cmd.Parameters.AddWithValue("@Sname", StName.Text);
                    cmd.Parameters.AddWithValue("@Ssurname", StSurname.Text);
                    cmd.Parameters.AddWithValue("@GroupId", StGroup.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@FullName", StName.Text + " " + StSurname.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Added");
                    Con.Close();
                    DisplayStudent();
                    Reset();
                }
                catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Reset()
        {
            Key = 0;
            StName.Text = "";
            StSurname.Text = "";
            StGroup.Text = "";
        }
        int Key = 0;
        private void StudentsList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StName.Text = StudentsList.SelectedRows[0].Cells[1].Value.ToString();
            StSurname.Text = StudentsList.SelectedRows[0].Cells[2].Value.ToString();
            StGroup.SelectedItem = StudentsList.SelectedRows[0].Cells[3].Value.ToString();
            if (StName.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(StudentsList.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select student!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from Users where Id=@Key",Con);
                    cmd.Parameters.AddWithValue("@Key", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Deleted");
                    Con.Close();
                    DisplayStudent();
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
            if (StName.Text == "" || StSurname.Text == "" || StGroup.SelectedIndex == -1)
            {
                MessageBox.Show("Select Student!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update Users set Name=@Sname,LastName=@Ssurname,GroupId=@Sgroup where Id=@Key",Con);
                    cmd.Parameters.AddWithValue("@Sname", StName.Text);
                    cmd.Parameters.AddWithValue("@Ssurname", StSurname.Text);
                    cmd.Parameters.AddWithValue("@Sgroup", StGroup.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Key", Key.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Updated");
                    Con.Close();
                    DisplayStudent();
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
        private void Group_fill()
        {
            Con.Open();
            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = ("select * from Groups");
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            StGroup.ValueMember = "Id";
            StGroup.DisplayMember = "Name";
            StGroup.DataSource = dt;
            Con.Close();
        }
        private void Students_Load_1(object sender, EventArgs e)
        {
            Group_fill();
        }
    }
}
