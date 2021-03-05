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
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            db = new CarRentalEntities();
        }

        public void fillDataGrid()
        {
            // select * from TypesOfCars
            //var cars = db.TypesOfCars.ToList();
            var cars = db.TypesOfCars
                .Select(q => new {
                    q.Id,
                    Make= q.Make,
                    Model=q.Model,
                    VIN=q.VIN,
                    Year=q.Year,
                    LicensePlateNumber=q.LicensePlateNumber }).ToList();
           
            dgvVehicleList.DataSource = cars;
            dgvVehicleList.Columns["Id"].Visible = false;
            dgvVehicleList.Columns[4].HeaderText="License Plate Number";
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
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

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            var addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
           
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected Row
                var id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;
                //query database for record
                var car = db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                //launch AddEditVehicle window with data
                if (!Utils.FormIsOpen("AddEditVehicle"))
                {
                    var addEditVehicle = new AddEditVehicle(car, this);
                    addEditVehicle.MdiParent = this.MdiParent;
                    addEditVehicle.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of the selected Row
                var id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var car = db.TypesOfCars.FirstOrDefault(q => q.Id == id);

                DialogResult dialogResult = MessageBox.Show("Are you sure to delete this vehicle?", "Delete", 
                    MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    //delete vehicle from table
                    db.TypesOfCars.Remove(car);
                    db.SaveChanges();

                    dgvVehicleList.Refresh();
                    fillDataGrid();
                }
                else 
                {
                    //do something else
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
