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
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities db;

        public AddEditRentalRecord()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental";
            isEditMode = false;
            db = new CarRentalEntities();
        }

        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";
            if (recordToEdit == null)
            {
                MessageBox.Show("Select a record to Edit.");
                Close();
            }
            else
            {
                isEditMode = true;
                db = new CarRentalEntities();
                PopulateFields(recordToEdit);
            }
        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            txtUsername.Text =recordToEdit.customerName ;
            dtRented.Value = (DateTime)recordToEdit.dateRented;
            dtReturned.Value = (DateTime)recordToEdit.dateReturned;
            txtCost.Text = recordToEdit.cost.ToString();
            lblRecorId.Text = recordToEdit.id.ToString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = txtUsername.Text;
                var dateOut = dtRented.Value;
                var dateIn = dtReturned.Value;
                var carType = cboCarType.Text;
                double cost = Convert.ToDouble(txtCost.Text);
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(carType))
                {
                    isValid = false;
                   errorMessage+="Error: Please enter missing data.\n\r";
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                    errorMessage+= "Error: Illegal Date Selection.\n\r";
                }

                if (isValid)
                {
                    //Declare an object of the record to be added
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        var id = int.Parse(lblRecorId.Text);
                        rentalRecord = db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    }

                    //populate record object with values from the form
                    rentalRecord.customerName = customerName;
                    rentalRecord.dateRented = dateOut;
                    rentalRecord.dateReturned = dateIn;
                    rentalRecord.cost = (decimal)cost;
                    rentalRecord.carID = (int)cboCarType.SelectedValue;

                    // if not in edit mode, then add the record object to DB
                    if (!isEditMode)
                        db.CarRentalRecords.Add(rentalRecord);
                    
                    //Save changes to entity
                    db.SaveChanges();

                    MessageBox.Show("Data Saved Successfully");
                    DialogResult dialogResult = MessageBox.Show("Want to Add new Record?", "New Record", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        txtUsername.Text = "";
                        txtUsername.Focus();
                        txtCost.Text = "";
                        var addRentalRecord = new AddEditRentalRecord();
                        addRentalRecord.Show();
                    }
                    else
                    {
                        var manageRentalRecord = new ManageRentalRecords();
                        manageRentalRecord.Show();
                        this.Close();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillCarCombo()
        {
            // select * from TypesOfCars
            var cars = db.TypesOfCars
                .Select(q => new {
                    Id=q.Id,
                    Name = q.Make + " " + q.Model}).ToList();

            cboCarType.DisplayMember = "Name";
            cboCarType.ValueMember = "Id";
            cboCarType.DataSource = cars;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillCarCombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var manageRentalRecords = new ManageRentalRecords();
            manageRentalRecords.Show();
            this.Close();
        }
    }
}
