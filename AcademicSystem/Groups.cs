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
    public partial class Groups : Form
    {
        public Groups()
        {
            InitializeComponent();
            DisplayGroups();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\deivi\OneDrive\Dokumentai\AcademicDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayGroups()
        {
            Con.Open();
            string Query = "Select G.Id,G.Name from Groups G";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            GroupList.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Reset()
        {
            Gname.Text = "";
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (Gname.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into Groups(Name) values (@Name)", Con);
                    cmd.Parameters.AddWithValue("@Name", Gname.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Group Added");
                    Con.Close();
                    DisplayGroups();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int Key = 0;
        private void GroupList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Gname.Text = GroupList.SelectedRows[0].Cells[1].Value.ToString();
            if (Gname.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(GroupList.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select Group!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from Groups where Id=@Key", Con);
                    cmd.Parameters.AddWithValue("@Key", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Group Deleted");
                    Con.Close();
                    DisplayGroups();
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
            if (Gname.Text == "" )
            {
                MessageBox.Show("Select Group!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update Groups set Name=@Name where Id=@Key", Con);
                    cmd.Parameters.AddWithValue("@Name", Gname.Text);
                    cmd.Parameters.AddWithValue("@Key", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Group Updated");
                    Con.Close();
                    DisplayGroups();
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

        private void Groups_Load(object sender, EventArgs e)
        {
            Subject_Fill();
        }
        private void Subject_Fill()
        {
            Con.Open();
            SqlCommand cmd = Con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = ("select Id,Name from Subjects");
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            Gsubject.ValueMember = "Id";
            Gsubject.DisplayMember = "Name";
            Gsubject.DataSource = dt;
            Con.Close();
        }

        private void Gname_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddSbj_Click(object sender, EventArgs e)
        {
            if (Gsubject.SelectedValue.ToString() == "" || Key == 0)
            {
                MessageBox.Show("Select Group and Subject!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into GroupSubjects (GroupId,SubjectId) values (@GroupId,@SubjectId)", Con);
                    cmd.Parameters.AddWithValue("@GroupId", Key);
                    cmd.Parameters.AddWithValue("@SubjectId", Gsubject.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Subject added to Group");
                    Con.Close();
                    DisplayGroups();
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
