using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace ClubApplication
{
    public class ClubRegistrationQuery
    {
        //DECLARATION OF System.Data.SqlClient
        private SqlConnection sqlConnect;
        private SqlCommand sqlCommand;
        private SqlDataAdapter sqlAdapter;
        private SqlDataReader sqlReader;

        public DataTable dataTable;
        public BindingSource bindingSource;

        private string connectionString;

        public string _FirstName, _MiddleName, _LastName, _Gender, _Program;
        public int _Age;

        public int _count;

        public ClubRegistrationQuery()
        {
            connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ADMIN\\source\\repos\\ClubApplication\\ClubApplication\\ClubDB.mdf;Integrated Security=True";
            sqlConnect = new SqlConnection(connectionString);

            //INSTANTIATION OF bindingSource AND dataTable
            bindingSource = new BindingSource();
            dataTable = new DataTable();
        }
        //Displays the Club Members
        public bool displayList()
        {
            String ViewClubMembers = "SELECT StudentID, FirstName, MiddleName, LastName, Age, Gender, Program FROM ClubMembers";
            sqlAdapter = new SqlDataAdapter(ViewClubMembers, connectionString);
            dataTable.Clear();
            sqlAdapter.Fill(dataTable);
            bindingSource.DataSource = dataTable;
            return true;
        }
        //Method for Registering the Student
        public bool RegisterStudent(int ID, long StudentID, string FirstName, string MiddleName, string LastName, int Age, string Gender, string Program)
        {
            sqlCommand = new SqlCommand("INSERT INTO ClubMembers VALUES(@ID, @StudentID, @FirstName, @MiddleName, @LastName, @Age, @Gender, @Program)", sqlConnect);
            sqlCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
            sqlCommand.Parameters.Add("@StudentID", SqlDbType.BigInt).Value = StudentID;
            sqlCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCommand.Parameters.Add("@MiddleName", SqlDbType.VarChar).Value = MiddleName;
            sqlCommand.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;
            sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = Age;
            sqlCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Gender;
            sqlCommand.Parameters.Add("@Program", SqlDbType.VarChar).Value = Program;

            sqlConnect.Open();//opens the connection
            sqlCommand.ExecuteNonQuery();// executes SqlCommand query
            sqlConnect.Close();//closes the connection
            return true;
        }
        //Counts the students in the club
        public bool getCount()
        {
            string countNum = "SELECT ID FROM ClubMembers;";
            sqlAdapter = new SqlDataAdapter(countNum, connectionString);
            _count = Convert.ToInt32(sqlAdapter);
            return true;
        }
        //Gets the List of IDS from Students
        public List<int> GetIDs()
        {
            List<int> ids = new List<int>();
            string query = "SELECT StudentID FROM ClubMembers"; // Query for viewing the StudentID

            try
            {
                sqlConnect.Open();
                sqlCommand = new SqlCommand(query, sqlConnect);
                sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    // Check and parse the StudentID column as an integer
                    if (sqlReader["StudentID"] != DBNull.Value)
                    {
                        ids.Add(Convert.ToInt32(sqlReader["StudentID"]));
                    }
                }
            }
            //error handling
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving IDs: {ex.Message}");
            }
            finally
            {
                sqlReader?.Close();
                sqlConnect.Close();
            }

            // Debugging output to verify IDs
            Console.WriteLine($"Retrieved IDs: {string.Join(", ", ids)}");

            return ids;
        }

        //Method that get student details
        public DataRow GetDetailsByID(int id)
        {
            string query = "SELECT * FROM ClubMembers WHERE StudentID = @StudentID"; // Selects the row of StudentID
            sqlCommand = new SqlCommand(query, sqlConnect);
            sqlCommand.Parameters.Add("@StudentID", SqlDbType.Int).Value = id;

            DataTable detailsTable = new DataTable();
            sqlAdapter = new SqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(detailsTable);

            if (detailsTable.Rows.Count > 0)
            {
                return detailsTable.Rows[0]; // Returns the first row 
            }
            return null;
        }

        //Method that updates Student Info
        public bool UpdateStudent(int ID, string FirstName, string MiddleName, string LastName, int Age, string Gender, string Program)
        {
            try
            {
                string query = "UPDATE ClubMembers SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Age = @Age, Gender = @Gender, Program = @Program WHERE StudentID = @StudentID";
                sqlCommand = new SqlCommand(query, sqlConnect);

                sqlCommand.Parameters.Add("@StudentID", SqlDbType.Int).Value = ID;
                sqlCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
                sqlCommand.Parameters.Add("@MiddleName", SqlDbType.VarChar).Value = MiddleName;
                sqlCommand.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;
                sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = Age;
                sqlCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Gender;
                sqlCommand.Parameters.Add("@Program", SqlDbType.VarChar).Value = Program;

                sqlConnect.Open();
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                sqlConnect.Close();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Update successful.");
                    return true;
                }
                else
                {
                    Console.WriteLine("No rows affected.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student: {ex.Message}");
                return false;
            }
        }


        //testing the db connection
        public bool TestDatabaseConnection()
        {
            try
            {
                sqlConnect.Open();
                Console.WriteLine("Connection successful.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
            finally
            {
                sqlConnect.Close();
            }
        }

    }
}
