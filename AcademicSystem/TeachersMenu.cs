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
    public partial class TeachersMenu : Form
    {
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\deivi\OneDrive\Dokumentai\AcademicDb.mdf;Integrated Security=True;Connect Timeout=30");
        string TeacherId = "";
        public TeachersMenu(string Id)
        {
            InitializeComponent();
            TeacherId = Id;
            DisplayStudents();
        }
        private void DisplayStudents()
        {
            Con.Open();
            string Query = "Select SG.Id,U.FullName,SG.Grade,S.Name as SubjectName from Users U left join GroupSubjects GS on U.GroupId = GS.GroupId left join Subjects S on GS.SubjectId = S.Id left join (select * from Users U2 where U2.RoleId=2) T on S.TeacherId = T.Id  left join StudentGrades SG on SG.StudentId = U.Id  where U.RoleId=3 and T.Id=" +TeacherId;
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TeachersGList.DataSource = ds.Tables[0];
            Con.Close();
        }
        int Key = 0;

        private void TeachersGList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Student.Text = TeachersGList.SelectedRows[0].Cells[1].Value.ToString();
            Subject.Text = TeachersGList.SelectedRows[0].Cells[2].Value.ToString();
            if (Student.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(TeachersGList.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void TeachersMenu_Load(object sender, EventArgs e)
        {
            DisplayStudents();
            Student_Fill();
            Subject_Fill();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }
           
        private void Reset()
        {
            Student.Text = "";
            Grade.Text = "";
            Subject.Text = "";
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (Student.SelectedIndex == -1 || Grade.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into StudentGrades(StudentId,Grade,SubjectId) values (@StudentId,@Grade,@SubjectId)", Con);
                    cmd.Parameters.AddWithValue("@StudentId", Student.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Grade", Grade.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SubjectId", Subject.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Grade Added");
                    Con.Close();
                    DisplayStudents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void Student_Fill()
        {
            Con.Open();
            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = ("Select distinct U.Id,U.FullName from Users U left join GroupSubjects GS on U.GroupId = GS.GroupId left join Subjects S on GS.SubjectId = S.Id left join (select * from Users U2 where U2.RoleId=2) T on S.TeacherId = T.Id  left join StudentGrades SG on SG.StudentId = U.Id  where U.RoleId=3");
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            Student.ValueMember = "Id";
            Student.DisplayMember = "FullName";
            Student.DataSource = dt;
            Con.Close();
        }
        private void Subject_Fill()
        {
            Con.Open();
            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = ("select S.Id,S.Name from Subjects S where S.TeacherId=@Id");
            cmd.Parameters.AddWithValue("@Id", TeacherId);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            Subject.ValueMember = "Id";
            Subject.DisplayMember = "Name";
            Subject.DataSource = dt;
            Con.Close();
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
                    SqlCommand cmd = new SqlCommand("delete from StudentGrades where Id=@Key", Con);
                    cmd.Parameters.AddWithValue("@Key", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Grade Deleted");
                    Con.Close();
                    DisplayStudents();
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
            if (Student.Text == "" || Grade.Text == "")
            {
                MessageBox.Show("Select Student!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update StudentGrades set Grade=@Grade where Id=@Key", Con);
                    cmd.Parameters.AddWithValue("@Grade", Grade.Text);
                    cmd.Parameters.AddWithValue("@Key", Key.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Grade Updated");
                    Con.Close();
                    DisplayStudents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
    }
}
