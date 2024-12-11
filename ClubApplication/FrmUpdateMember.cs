using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubApplication
{
    public partial class FrmUpdateMember : Form
    {
        public FrmUpdateMember()
        {
            InitializeComponent();
            query = new ClubRegistrationQuery();
            LoadIDs(); // Loads the list of IDs into the ComboBox
        }

        ClubRegistrationQuery query; // Declares a ClubRegistrationQuery object
        private bool isComboBoxInitialized = false;

        // Method to load student IDs into the ComboBox
        private void LoadIDs()
        {
            List<int> ids = query.GetIDs();

            if (ids.Count == 0)
            {
                Console.WriteLine("No IDs retrieved. Check the database and query.");
            }

            cmbUpStudentID.SelectedIndexChanged -= cmbUpStudentID_SelectedIndexChanged;

            // Bind IDs to ComboBox
            cmbUpStudentID.DataSource = null; // Reset to avoid binding issues
            cmbUpStudentID.DataSource = ids;
            cmbUpStudentID.SelectedIndex = -1;

            // Debugging output
            Console.WriteLine($"ComboBox DataSource: {string.Join(", ", ids)}");

            cmbUpStudentID.SelectedIndexChanged += cmbUpStudentID_SelectedIndexChanged;
        }
        

        private void FrmUpdateMember_Load(object sender, EventArgs e)
        {
            // Populate ComboBoxes
            PopulateComboBoxes();

            // Clear input fields
            ClearInputFields();

            // Test database connection
            query.TestDatabaseConnection();
        }

        // Method to populate ComboBoxes for gender and program
        private void PopulateComboBoxes()
        {
            // Populating the gender ComboBox
            cmbUpGender.Items.Clear();
            cmbUpGender.Items.Add("Male");
            cmbUpGender.Items.Add("Female");

            // Populating the program ComboBox
            cmbUpProgram.Items.Clear();
            cmbUpProgram.Items.Add("BS in Information Technology");
            cmbUpProgram.Items.Add("BS in Computer Science");
            cmbUpProgram.Items.Add("BS in Business Management");
            cmbUpProgram.Items.Add("BS in Hospitality Management");

            // After populating, sets default value
            cmbUpGender.SelectedIndex = 0;  // Default to "Male" 
            cmbUpProgram.SelectedIndex = 0;  // Default to BS in Information Technology
        }

        //clears all fields
        private void ClearInputFields()
        {
            txtUpFirstName.Clear();
            txtUpMiddleName.Clear();
            txtUpLastName.Clear();
            txtUpAge.Clear();
            cmbUpGender.SelectedIndex = -1; 
            cmbUpProgram.SelectedIndex = -1; 
        }

        // Confirms and updates student details
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (cmbUpStudentID.SelectedIndex != -1)
            {
                int selectedID = (int)cmbUpStudentID.SelectedItem;

                // Gets the selected gender and program values
                string gender = cmbUpGender.SelectedItem.ToString(); 
                string program = cmbUpProgram.SelectedItem.ToString();

                // Validates if the fields are correct and update the student details
                bool isUpdated = query.UpdateStudent(
                    selectedID,
                    txtUpFirstName.Text,
                    txtUpMiddleName.Text,
                    txtUpLastName.Text,
                    int.Parse(txtUpAge.Text),
                    gender,
                    program
                );

                if (isUpdated)
                {
                    MessageBox.Show("Member details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update member details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a member ID first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbUpStudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUpStudentID.SelectedIndex != -1)
            {
                // Retrieves the selected ID from the ComboBox
                int selectedID = (int)cmbUpStudentID.SelectedItem;

                // Fetches details using the ID
                DataRow details = query.GetDetailsByID(selectedID);

                if (details != null)
                {
                    // Populates the fields with the retrieved data
                    txtUpFirstName.Text = details["FirstName"].ToString();
                    txtUpMiddleName.Text = details["MiddleName"].ToString();
                    txtUpLastName.Text = details["LastName"].ToString();
                    txtUpAge.Text = Convert.ToInt32(details["Age"]).ToString();

                    // Sets the Gender and Program values
                    cmbUpGender.Text = details["Gender"].ToString(); // Populate gender ComboBox
                    cmbUpProgram.Text = details["Program"].ToString(); // Populate program ComboBox
                }
                else
                {
                    // If no details are found, clear the fields
                    ClearInputFields();
                    MessageBox.Show("No member details found for the selected ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Set the gender and program values
        public void SetComboBoxValues(string gender, string program)
        {
            cmbUpGender.SelectedItem = gender;
            cmbUpProgram.SelectedItem = program;
        }
    }
}
