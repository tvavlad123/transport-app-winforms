using System;
using System.Windows.Forms;
using Transport.Repository;
using Transport.Service;

namespace Transport.Controller
{
    public partial class Login : Form
    {
        private readonly EmployeeService _employeeService;

        public Login()
        {
            InitializeComponent();
            _employeeService = new EmployeeService(new EmployeeDBRepository(DBUtils.GetProperties()));
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (_employeeService.Login(UsernameBox.Text, PasswordBox.Text)) {
                var transportWindow = new TransportWindow();
                Hide();
                transportWindow.FormClosed += (s, ev) => Show();
                transportWindow.Show();
            }
            else
            {
                MessageBox.Show("Username or password is incorrect.", "Login error", MessageBoxButtons.OK);
                UsernameBox.Clear();
                PasswordBox.Clear();
            }
        }
    }
}
