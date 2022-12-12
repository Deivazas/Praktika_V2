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
    public partial class Subject : Form
    {
        public Subject()
        {
            InitializeComponent();
            DisplaySubjects();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\deivi\OneDrive\Dokumentai\AcademicDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplaySubjects()
        {
            Con.Open();
            string Query = "Select S.Id,S.Name,U.Name as TeacherName,U.LastName as TeacherLastName from Subjects S left join Users U on S.TeacherId=U.Id and RoleId=2";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            SubjectList.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Reset()
        {
           Sname.Text = "";
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (Sname.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into Subjects(Name,TeacherId) values (@Name,@TeacherId)", Con);
                    cmd.Parameters.AddWithValue("@Name", Sname.Text);
                    cmd.Parameters.AddWithValue("@TeacherId", Steacher.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Subject Added");
                    Con.Close();
                    DisplaySubjects();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        int Key = 0;
        private void SubjectList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Sname.Text = SubjectList.SelectedRows[0].Cells[1].Value.ToString();
            if (Sname.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(SubjectList.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select Subject!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from Subjects where Id= @Key", Con);
                    cmd.Parameters.AddWithValue("@Key", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Subject Deleted");
                    Con.Close();
                    DisplaySubjects();
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
            if (Sname.Text == "" )
            {
                MessageBox.Show("Select Subject!");
            }
            else
            {
                try
                {
                    Con.Open();
                    string[] NameArray = Steacher.SelectedItem.ToString().Split(" ");
                    SqlCommand cmb = new SqlCommand("select Id from Users where Name=@Name and LastName=@LastName", Con);
                    cmb.Parameters.AddWithValue("@Name", NameArray[0]);
                    cmb.Parameters.AddWithValue("@LastName", NameArray[1]);
                    cmb.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmb);
                    da.Fill(dt);
                    string TeacherId = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        TeacherId = dr["Id"].ToString();
                        break;
                    }
                    SqlCommand cmd = new SqlCommand("Update Subjects set Name=@Name,TeacherId=@TeacherId where Id=@Key", Con);
                    cmd.Parameters.AddWithValue("@Name", Sname.Text);
                    cmd.Parameters.AddWithValue("@TeacherId", TeacherId);
                    cmd.Parameters.AddWithValue("@Key", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Subject Updated");
                    Con.Close();
                    DisplaySubjects();
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
        private void Teacher_Fill()
        {
            Con.Open();
            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = ("select Id,FullName from Users where RoleId=2");
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            Steacher.ValueMember = "Id";
            Steacher.DisplayMember = "FullName";
            Steacher.DataSource = dt;
            Con.Close();
        }

        private void Subject_Load(object sender, EventArgs e)
        {
            Teacher_Fill();
        }

        private void Sname_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
