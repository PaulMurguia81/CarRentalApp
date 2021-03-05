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
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities db;
        public ManageUsers()
        {
            InitializeComponent();
            db = new CarRentalEntities();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }  
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected Row
                var id = (int)dgvUsersList.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var user = db.Users.FirstOrDefault(q => q.Id == id);
                var hashed_password = Utils.DefaultHashedPassword();
                user.password = hashed_password;
                db.SaveChanges();

                MessageBox.Show($"{user.userName}'s Password has been reset!");
                fillDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected Row
                var id = (int)dgvUsersList.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var user = db.Users.FirstOrDefault(q => q.Id == id);

                //if (user.isActive==true)
                //    user.isActive = false;
                //else
                //    user.isActive = true;
/* the same in one line*/   user.isActive = user.isActive == true ? false : true;
                db.SaveChanges();
                MessageBox.Show($"{user.userName}'s active status has changed!!");
                fillDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public void fillDataGrid()
        {
            // select * from TypesOfCars
            //var cars = db.TypesOfCars.ToList();
            var records = db.Users
                .Select(q => new {
                    q.Id,
                    q.userName,
                    q.UserRoles.FirstOrDefault().Role.name,
                    q.isActive                    
                }).ToList();

            dgvUsersList.DataSource = records;
            dgvUsersList.Columns["Id"].Visible = false;
            dgvUsersList.Columns["userName"].HeaderText = "User Name";
            dgvUsersList.Columns["name"].HeaderText = "Role Name";
            dgvUsersList.Columns["isActive"].HeaderText = "Active";

        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            try
            {
                fillDataGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
