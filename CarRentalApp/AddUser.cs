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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities db;
        private ManageUsers _manageUsers;
        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            db = new CarRentalEntities();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = db.Roles.ToList();
            cboRoles.DataSource = roles;
            cboRoles.DisplayMember = "name";
            cboRoles.ValueMember = "id";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUsername.Text;
                var roleId = (int)cboRoles.SelectedValue;
                var password = Utils.DefaultHashedPassword();

                var user = new User
                {
                    userName = username,
                    password = password,
                    isActive = true
                };
                db.Users.Add(user);
                db.SaveChanges();

                var userid = user.Id;

                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userid
                };

                db.UserRoles.Add(userRole);
                db.SaveChanges();

                MessageBox.Show("New User Added Successfully");
                _manageUsers.fillDataGrid();
                Close();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"An Error Has Ocured: {ex.Message}");
            }
        }


    }
}
