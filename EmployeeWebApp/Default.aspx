<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EmployeeWebApp.Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Employee Details</title>
    <style>
        .btnEdit,
        .btnDelete {
            padding: 5px 10px;
            margin-left: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Employee Details</h2>
        <table>
            <tr>
                <td>Employee ID:</td>
                <td>
                    <asp:HiddenField ID="hfEmpID" runat="server" />
                    <asp:TextBox ID="txtEmpID" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Employee Name:</td>
                <td>
                    <asp:TextBox ID="txtEmpName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>State:</td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>City:</td>
                <td>
                    <asp:DropDownList ID="ddlCity" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Contact:</td>
                <td>
                    <asp:TextBox ID="txtContact" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Hobbies:</td>
                <td>
                    <asp:CheckBoxList ID="cblHobbies" runat="server">
                        <asp:ListItem Text="Playing"></asp:ListItem>
                        <asp:ListItem Text="Reading"></asp:ListItem>
                        <asp:ListItem Text="Sky diving"></asp:ListItem>
                        <asp:ListItem Text="Writing"></asp:ListItem>
                        <asp:ListItem Text="Running"></asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td>Resume:</td>
                <td>
                    <asp:FileUpload ID="fileResume" runat="server"></asp:FileUpload>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
        <br />
        <h2>Employee Details Table</h2>
        <table id="tblEmployeeDetails" border="1" runat="server">
            <tr>
                <th>Employee ID</th>
                <th>Name</th>
                <th>State</th>
                <th>City</th>
                <th>Contact</th>
                <th>Hobbies</th>
                <th>DOJ</th>
                <th>Designation</th>
                <th>Actions</th>
            </tr>
        </table>
    </form>
    <script>
        const deleteConfirmMessage = 'Are you sure you want to delete this employee?';

        // Attach event handlers to dynamically added Edit and Delete buttons
        document.addEventListener('click', function (event) {
            if (event.target.matches('.btnEdit')) {
                const empId = event.target.getAttribute('data-empid');
                editEmployee(empId);
            } else if (event.target.matches('.btnDelete')) {
                const empId = event.target.getAttribute('data-empid');
                if (confirm(deleteConfirmMessage)) {
                    deleteEmployee(empId);
                }
            }
        });

        // Function to edit an employee
        function editEmployee(empId) {
            const empRow = document.getElementById('empRow_' + empId);
            if (empRow) {
                const empName = empRow.querySelector('.empName').innerText;
                const empState = empRow.querySelector('.empState').innerText;
                const empCity = empRow.querySelector('.empCity').innerText;
                const empContact = empRow.querySelector('.empContact').innerText;
                const empHobbies = empRow.querySelector('.empHobbies').innerText;

                document.getElementById('<%=txtEmpID.ClientID%>').value = empId;
                document.getElementById('<%=txtEmpName.ClientID%>').value = empName;
                document.getElementById('<%=ddlState.ClientID%>').value = empState;
                document.getElementById('<%=ddlCity.ClientID%>').value = empCity;
                document.getElementById('<%=txtContact.ClientID%>').value = empContact;

                const hobbiesList = document.getElementById('<%=cblHobbies.ClientID%>').getElementsByTagName('input');
                Array.from(hobbiesList).forEach(function (checkbox) {
                    checkbox.checked = empHobbies.includes(checkbox.value);
                });
            }
        }

        // Function to delete an employee
        function deleteEmployee(empId) {
            const empRow = document.getElementById('empRow_' + empId);
            if (empRow) {
                empRow.remove();
                // Perform an AJAX request to the server to delete the employee from the database
                // You can use JavaScript libraries like Axios or fetch API to send the request
            }
        }
    </script>
</body>
</html>
