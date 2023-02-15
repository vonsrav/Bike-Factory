using MyBikesFactoy.Business;
using MyBikesFactoy.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyBikesFactoy.UI
{
    public partial class Login : Form
    {
        private List<User> listOfUsers = UserSequentialData.Load();
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool existingUser = false;

            foreach (var user in listOfUsers)
            {
                if (user.Username == txtUsername.Text && user.Password == txtPassword.Text)
                {
                    existingUser = true;
                    break;
                }
            }
            if (existingUser)
            {
                var frmMainForm = new MainForm();
                frmMainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username and Password");
            }
        }
    }
}
