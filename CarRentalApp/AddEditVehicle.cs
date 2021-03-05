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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        private readonly CarRentalEntities db;

        
        public AddEditVehicle(ManageVehicleListing manageVehicleListing=null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            db = new CarRentalEntities();
        }

        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing=null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            if (carToEdit==null)
            {
                MessageBox.Show("Select a record to Edit.");
                Close();
            }
            else
            {
                isEditMode = true;
                db = new CarRentalEntities();
                PopulateFields(carToEdit);
            }
        }

        private void PopulateFields(TypesOfCar car)
        {
            lblID.Text = car.Id.ToString();
            txtMake.Text = car.Make;
            txtModel.Text = car.Model;
            txtVin.Text = car.VIN;
            txtYear.Text = car.Year.ToString();
            txtLicense.Text = car.LicensePlateNumber;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMake.Text) || string.IsNullOrWhiteSpace(txtModel.Text))
                {
                    MessageBox.Show("Please provide a make and a model");
                }
                else
                {
                    if (isEditMode)
                    {
                        //Edit Code here
                        var id = int.Parse(lblID.Text);
                        var car = db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                        car.Model = txtModel.Text;
                        car.Make = txtMake.Text;
                        car.VIN = txtVin.Text;
                        car.Year = int.Parse(txtYear.Text);
                        car.LicensePlateNumber = txtLicense.Text;
                    }
                    else
                    {
                        //Add Code here
                        var newCar = new TypesOfCar
                        {
                            LicensePlateNumber = txtLicense.Text,
                            Make = txtMake.Text,
                            Model = txtModel.Text,
                            VIN = txtVin.Text,
                            Year = int.Parse(txtYear.Text)
                        };
                        db.TypesOfCars.Add(newCar);
                    }
                    db.SaveChanges();
                    _manageVehicleListing.fillDataGrid();
                    MessageBox.Show("Operation Completed");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var vehicleListing = new ManageVehicleListing();
            vehicleListing.MdiParent = this.MdiParent;
            vehicleListing.Show();
            this.Close();
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
