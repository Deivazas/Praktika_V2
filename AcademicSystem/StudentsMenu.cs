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
    public partial class StudentsMenu : Form
    {
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\deivi\OneDrive\Dokumentai\AcademicDb.mdf;Integrated Security=True;Connect Timeout=30");
        string StudentId = "";
        public StudentsMenu(string Id)
        {
            InitializeComponent();
            StudentId = Id;
        }
       
        private void DisplayGrades()
        {
            Con.Open();
            string Query = "Select U.Id,U.Name,LastName,SG.Grade,S.Name from Users U left join StudentGrades SG on U.Id=SG.StudentId left join Subjects S on S.Id=SG.SubjectId where RoleId=3 and U.Id=" +StudentId;
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            StudentGList.DataSource = ds.Tables[0];
            Con.Close();
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
        private void StudentsMenu_Load(object sender, EventArgs e)
        {
            DisplayGrades();
        }
    }
}
