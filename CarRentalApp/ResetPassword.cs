using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ResetPassword : Form
    {
        private readonly CarRentalEntities db;
        private User _user;
        public ResetPassword(User user)
        {
            InitializeComponent();
            db = new CarRentalEntities();
            _user = user;
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                var password = txtPassword.Text;
                var confirm_password = txtConfirmPassword.Text;
                var user = db.Users.FirstOrDefault(q => q.Id == _user.Id);

                if (password != confirm_password)
                {
                    MessageBox.Show("Passwords don't match. Please try again");
                    txtPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    txtPassword.Focus();
                }
                else
                {
                    user.password = Utils.HashPassword(password);
                    db.SaveChanges();
                    MessageBox.Show("Password has been changed");
                    Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An Error has occured. Please try again");
            }

        }
    }
}
