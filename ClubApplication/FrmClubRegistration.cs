using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubApplication
{
    public partial class FrmClubRegistration : Form
    {
        
        private ClubRegistrationQuery clubRegistrationQuery; // Instance of the ClubRegistrationQuery class

        private int ID, Age, count;
        private string FirstName, MiddleName, LastName, Gender, Program;
        private long StudentId;

        //opens the FrmUpdateMember form for updating a member
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmUpdateMember frm = new FrmUpdateMember(); //goes to FrmUpdateMember
            frm.Show();
        }

        //refreshes the list of club members displayed in the DataGridView
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshListOfClubMemebrs();//calls this method
        }
        //Regsters new club member
        private void btnRegister_Click(object sender, EventArgs e)
        {
            count = clubRegistrationQuery._count;
            ID = RegistrationID(count);
            StudentId = Convert.ToInt64(txtStudentID.Text);
            FirstName = txtFirstName.Text;
            MiddleName = txtMiddleName.Text;
            LastName = txtLastName.Text;
            Age = Convert.ToInt32(txtAge.Text);
            Gender = cmbGender.SelectedItem.ToString();
            Program = cmbProgram.SelectedItem.ToString();
            clubRegistrationQuery.RegisterStudent(ID, StudentId, FirstName, MiddleName, LastName, Age, Gender, Program);
        }
        //Initializes the ClubRegistrationQuery instance and refresh the list
        private void FrmClubRegistration_Load(object sender, EventArgs e)
        {
            clubRegistrationQuery = new ClubRegistrationQuery();
            RefreshListOfClubMemebrs();
        }
        //Populates the Gender and Programs comboboxes
        public FrmClubRegistration()
        {
            InitializeComponent();

            string[] genders = { "Male", "Female" };
            foreach (string gender in genders)
            {
                cmbGender.Items.Add(gender);
            }
            string[] programs = { "BS in Information Technology", "BS in Computer Science", "BS in Business Management", "BS in Hospitality Management" };
            foreach (string program in programs)
            {
                cmbProgram.Items.Add(program);
            }
        }
        //refresh then display the list
        public void RefreshListOfClubMemebrs()
        {
            clubRegistrationQuery.displayList();
            dataGridView1.DataSource = clubRegistrationQuery.bindingSource;
        }
        //generates new Registration ID
        public int RegistrationID(int num)
        {
            return num++;
        }
    }
}
