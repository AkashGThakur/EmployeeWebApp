using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace EmployeeWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load the employee details table on initial page load
                LoadEmployeeDetails();

                // Populate the states and cities dropdown lists
                PopulateStatesDropdown();
                PopulateCitiesDropdown();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Save employee details to the database
            SaveEmployeeDetails();

            // Clear the form fields after saving
            ClearForm();

            // Reload the employee details table
            LoadEmployeeDetails();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Get the employee ID from the hidden field
            int empId = Convert.ToInt32(hfEmpID.Value);

            // Update the employee details in the database
            UpdateEmployeeDetails(empId);

            // Clear the form fields after updating
            ClearForm();

            // Reload the employee details table
            LoadEmployeeDetails();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the employee ID from the hidden field
            int empId = Convert.ToInt32(hfEmpID.Value);

            // Delete the employee details from the database
            DeleteEmployeeDetails(empId);

            // Clear the form fields after deleting
            ClearForm();

            // Reload the employee details table
            LoadEmployeeDetails();
        }

        private void SaveEmployeeDetails()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employee (Emp_Name, Emp_State, Emp_City, Emp_Contact, Emp_Hobbies, Emp_Resume) " +
                    "VALUES (@Emp_Name, @Emp_State, @Emp_City, @Emp_Contact, @Emp_Hobbies, @Emp_Resume);" +
                    "SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Emp_Name", txtEmpName.Text);
                    command.Parameters.AddWithValue("@Emp_State", ddlState.SelectedValue);
                    command.Parameters.AddWithValue("@Emp_City", ddlCity.SelectedValue);
                    command.Parameters.AddWithValue("@Emp_Contact", txtContact.Text);
                    command.Parameters.AddWithValue("@Emp_Hobbies", GetSelectedHobbies());
                    command.Parameters.AddWithValue("@Emp_Resume", fileResume.FileName);

                    connection.Open();
                    int empId = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();

                    // Save additional employee details to Emp_Details table
                    SaveAdditionalDetails(empId);
                }
            }
        }

        private void UpdateEmployeeDetails(int empId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Employee SET Emp_Name = @Emp_Name, Emp_State = @Emp_State, Emp_City = @Emp_City, " +
                    "Emp_Contact = @Emp_Contact, Emp_Hobbies = @Emp_Hobbies, Emp_Resume = @Emp_Resume WHERE Emp_ID = @Emp_ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Emp_ID", empId);
                    command.Parameters.AddWithValue("@Emp_Name", txtEmpName.Text);
                    command.Parameters.AddWithValue("@Emp_State", ddlState.SelectedValue);
                    command.Parameters.AddWithValue("@Emp_City", ddlCity.SelectedValue);
                    command.Parameters.AddWithValue("@Emp_Contact", txtContact.Text);
                    command.Parameters.AddWithValue("@Emp_Hobbies", GetSelectedHobbies());
                    command.Parameters.AddWithValue("@Emp_Resume", fileResume.FileName);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    // Update additional employee details in Emp_Details table
                    UpdateAdditionalDetails(empId);
                }
            }
        }

        private void DeleteEmployeeDetails(int empId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employee WHERE Emp_ID = @Emp_ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Emp_ID", empId);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    // Delete additional employee details from Emp_Details table
                    DeleteAdditionalDetails(empId);
                }
            }
        }

        private string GetSelectedHobbies()
        {
            string hobbies = string.Empty;

            foreach (ListItem item in cblHobbies.Items)
            {
                if (item.Selected)
                {
                    hobbies += item.Value + ",";
                }
            }

            return hobbies.TrimEnd(',');
        }

        private void SaveAdditionalDetails(int empId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Emp_Details (Emp_ID, Emp_DOJ, Emp_Designation) " +
                    "VALUES (@Emp_ID, @Emp_DOJ, @Emp_Designation)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Emp_ID", empId);
                    command.Parameters.AddWithValue("@Emp_DOJ", DateTime.Now); // Set the date of joining
                    command.Parameters.AddWithValue("@Emp_Designation", "Employee"); // Set the designation

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        private void UpdateAdditionalDetails(int empId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Emp_Details SET Emp_DOJ = @Emp_DOJ, Emp_Designation = @Emp_Designation " +
                    "WHERE Emp_ID = @Emp_ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Emp_ID", empId);
                    command.Parameters.AddWithValue("@Emp_DOJ", DateTime.Now); // Set the updated date of joining
                    command.Parameters.AddWithValue("@Emp_Designation", "Employee"); // Set the updated designation

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        private void DeleteAdditionalDetails(int empId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Emp_Details WHERE Emp_ID = @Emp_ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Emp_ID", empId);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        private void LoadEmployeeDetails()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT E.Emp_ID, E.Emp_Name, E.Emp_State, E.Emp_City, E.Emp_Contact, E.Emp_Hobbies, D.Emp_DOJ, D.Emp_Designation " +
                    "FROM Employee E " +
                    "INNER JOIN Emp_Details D ON E.Emp_ID = D.Emp_ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string empId = reader["Emp_ID"].ToString();
                        string empName = reader["Emp_Name"].ToString();
                        string empState = reader["Emp_State"].ToString();
                        string empCity = reader["Emp_City"].ToString();
                        string empContact = reader["Emp_Contact"].ToString();
                        string empHobbies = reader["Emp_Hobbies"].ToString();
                        string empDOJ = reader["Emp_DOJ"].ToString();
                        string empDesignation = reader["Emp_Designation"].ToString();

                        AddTableRow(empId, empName, empState, empCity, empContact, empHobbies, empDOJ, empDesignation);
                    }

                    connection.Close();
                }
            }
        }

        private void AddTableRow(string empId, string empName, string empState, string empCity, string empContact, string empHobbies, string empDOJ, string empDesignation)
        {
            HtmlTableRow row = new HtmlTableRow();
            row.ID = "empRow_" + empId;

            HtmlTableCell cellId = new HtmlTableCell();
            cellId.InnerText = empId;
            row.Cells.Add(cellId);

            HtmlTableCell cellName = new HtmlTableCell();
            cellName.InnerText = empName;
            row.Cells.Add(cellName);

            HtmlTableCell cellState = new HtmlTableCell();
            cellState.InnerText = empState;
            row.Cells.Add(cellState);

            HtmlTableCell cellCity = new HtmlTableCell();
            cellCity.InnerText = empCity;
            row.Cells.Add(cellCity);

            HtmlTableCell cellContact = new HtmlTableCell();
            cellContact.InnerText = empContact;
            row.Cells.Add(cellContact);

            HtmlTableCell cellHobbies = new HtmlTableCell();
            cellHobbies.InnerText = empHobbies;
            row.Cells.Add(cellHobbies);

            HtmlTableCell cellDOJ = new HtmlTableCell();
            cellDOJ.InnerText = empDOJ;
            row.Cells.Add(cellDOJ);

            HtmlTableCell cellDesignation = new HtmlTableCell();
            cellDesignation.InnerText = empDesignation;
            row.Cells.Add(cellDesignation);

            HtmlTableCell cellActions = new HtmlTableCell();
            Button btnEdit = new Button();
            btnEdit.ID = "btnEdit_" + empId;
            btnEdit.Text = "Edit";
            btnEdit.CssClass = "btnEdit";
            btnEdit.Click += new EventHandler(btnEdit_Click);
            btnEdit.Attributes.Add("data-empid", empId); // Add the employee ID as a data attribute
            cellActions.Controls.Add(btnEdit);

            Button btnDelete = new Button();
            btnDelete.ID = "btnDelete_" + empId;
            btnDelete.Text = "Delete";
            btnDelete.CssClass = "btnDelete";
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnDelete.Attributes.Add("data-empid", empId); // Add the employee ID as a data attribute
            cellActions.Controls.Add(btnDelete);

            row.Cells.Add(cellActions);

            tblEmployeeDetails.Rows.Add(row);
        }


        private void ClearForm()
        {
            txtEmpID.Text = string.Empty;
            txtEmpName.Text = string.Empty;
            ddlState.SelectedIndex = 0;
            ddlCity.SelectedIndex = 0;
            txtContact.Text = string.Empty;
            ClearHobbiesSelection();
        }

        private void ClearHobbiesSelection()
        {
            foreach (ListItem item in cblHobbies.Items)
            {
                item.Selected = false;
            }
        }

        private void PopulateStatesDropdown()
        {
            ddlState.Items.Add(new ListItem("Select State", ""));
            ddlState.Items.Add(new ListItem("Maharashtra"));
            ddlState.Items.Add(new ListItem("Telangana"));
            ddlState.Items.Add(new ListItem("Karnataka"));
            ddlState.Items.Add(new ListItem("Delhi"));
            ddlState.Items.Add(new ListItem("Tamil Nadu"));

        }

        private void PopulateCitiesDropdown()
        {
            ddlCity.Items.Add(new ListItem("Select City", ""));
            ddlCity.Items.Add(new ListItem("Pune"));
            ddlCity.Items.Add(new ListItem("Hyderabad"));
            ddlCity.Items.Add(new ListItem("Bangalore"));
            ddlCity.Items.Add(new ListItem("Delhi"));
            ddlCity.Items.Add(new ListItem("Chennai"));

        }
    }
}
