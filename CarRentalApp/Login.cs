using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class Login : Form
    {
        private readonly CarRentalEntities db;
        public Login()
        {
            InitializeComponent();
            db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SHA256 sha = SHA256.Create();

                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text;

                var hashed_password = Utils.HashPassword(password);

                var user = db.Users.FirstOrDefault(q => q.userName == username && q.password == hashed_password && q.isActive==true);
                
                if (user == null)
                {
                    MessageBox.Show("Please provide valid credentials.");
                    txtUsername.Focus();
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                }
                else
                {
                    var mainWindow = new MainWindow(this, user);
                    mainWindow.Show();
                    Hide();
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
    }
}
