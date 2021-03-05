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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            db = new CarRentalEntities();

        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord
            {
                MdiParent = this.MdiParent
            };
            addRentalRecord.Show();
            this.Close();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected Row
                var id = (int)dgvRecordList.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var record = db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                //launch AddEditVehicle window with data
                var addEditRentalRecord = new AddEditRentalRecord(record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to delete this vehicle?", "Delete Car", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //get ID of selected Row
                    var id = (int)dgvRecordList.SelectedRows[0].Cells["Id"].Value;
                    //query database for record
                    var record = db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    //delete vehicle from table
                    db.CarRentalRecords.Remove(record);
                    db.SaveChanges();
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

        private void fillDataGrid()
        {
            // select * from TypesOfCars
            //var cars = db.TypesOfCars.ToList();
            var records = db.CarRentalRecords
                .Select(q => new {
                    Customer= q.customerName,
                    DateOut = q.dateRented,
                    DateIn=q.dateReturned,
                    Id=q.id,
                    Cost=q.cost,
                    Car=q.TypesOfCar.Make + " " + q.TypesOfCar.Model
                }).ToList();

            dgvRecordList.DataSource = records;
            dgvRecordList.Columns["Id"].Visible = false;
            dgvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            dgvRecordList.Columns["DateIn"].HeaderText = "Date In";
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
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
