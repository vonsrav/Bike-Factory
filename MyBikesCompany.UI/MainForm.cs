using MyBikesFactoy.Business;
using MyBikesFactoy.Data;
using MyBikesFactoy.Business.Enums;
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
    public partial class MainForm : Form
    {
        private List<Bikes> listOfBikes = BikesXMLData.Load();
        public MainForm()
        {
            InitializeComponent();
            RefreshDisplayList();
        }
        #region Support Methods
        private Bikes? FindBikeBySerialNumber(int serialNumber)

        {
            Bikes? bikeFound = null;
            foreach (var bike in listOfBikes)
            {
                if (bike.SerialNumber == serialNumber)
                {
                    bikeFound = bike;
                    break;
                }
            }
            return bikeFound;
        }
        private Bikes? FindBikeBySerialNumber2(int serialNumber)
        {

            foreach (var bike in listOfBikes)
            {
                if (bike.SerialNumber == serialNumber)
                {
                    return bike;
                }

            }
            return null;
        }
        private Bikes? FindBikeBySerialNumber3(int serialNumber)
        {

            foreach (var bike in listOfBikes)
            {
                if (bike.SerialNumber == serialNumber)
                {
                    return bike;
                }

            }
            return null;
            //LINQ
            //return listOfBikes.FirstOrDefault(s => s.SerialNumber == serialNumber);
        }

        private void RefreshDisplayList()
        {
            lstBikes.Items.Clear();
            foreach (var bike in listOfBikes)
            {
                bool include = false;
                if (rbAll.Checked)
                {
                    include = true;
                }
                else if (rbMountain.Checked && bike is MountainBikes)
                {
                    include = true;
                }
                else if (rbRoads.Checked && bike is RoadBikes)
                {
                    include = true;
                }
                if (include)
                {
                    lstBikes.Items.Add(bike.ToString());
                }

            }

        }
        private bool AllFieldOk()
        {
            if (cbBikeType.Text == "")
            {
                MessageBox.Show("Please select a Bike Type");
                return false;
            }
            else if (txtSerialNumber.Text == "" || !Validator.ValidateSearialNumber(txtSerialNumber.Text))
            {
                MessageBox.Show("Serial Number is required and and contain Number only.");
                return false;
            }
            else if (txtModel.Text == "")
            {
                MessageBox.Show("Please inform a model");
                return false;
            }
            else if (!Validator.ValidateModel(txtModel.Text))
            {
                MessageBox.Show("Model should contain 5 characters(number or letter");
                return false;
            }
            else if (cbColor.Text == "")
            {
                MessageBox.Show("Please select a color");
                return false;
            }
            else if (txtManufacturingYear.Text == "")
            {
                MessageBox.Show("Please inform a Manufacturing Year");
                return false;
            }
            else if (!Validator.ValidateManufacturingYear(txtManufacturingYear.Text))
            {
                MessageBox.Show("Manufacturing Year should contain 4 number.");
                return false;
            }
            else if (cbBikeType.Text == "Mountain" && cbSuspensionType.Text=="")
            {
                MessageBox.Show("Please select a Suspension Type");
                return false;
            }
            else if (cbBikeType.Text == "Road" && cbTireType.Text=="") 
            {
                MessageBox.Show("Please select a Tire Type");
                return false;
            }

            return true;
        }
       private void ResetFromField()  // Or ResetFormFields
        {
            cbBikeType.SelectedIndex = -1;
            txtSerialNumber.Text = "";
            txtModel.Text = "";
            txtManufacturingYear.Text = "";
            txtOriginalid.Text = "";
            cbSuspensionType.SelectedIndex = -1;
            cbTireType.SelectedIndex = -1;
            cbColor.SelectedIndex = -1;
        }
        #endregion



        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!AllFieldOk())
            {
                return;
            }
            else if (!Validator.ValidateUniqueSerialNumber(txtSerialNumber.Text, listOfBikes))
            {
                MessageBox.Show("This Serial Number is already used");
                return;
            }


            Bikes bikeToAdd;
            if (cbBikeType.Text == "Mountain")
            {
                ESuspensionType suspensionType;
                if (cbSuspensionType.Text == "Front")
                {
                    suspensionType = ESuspensionType.Front;
                }
                else if(cbSuspensionType.Text == "Rear")
                {
                    suspensionType = ESuspensionType.Rear;
                }
                else
                {
                    suspensionType = ESuspensionType.Dual;
                }
                bikeToAdd = new MountainBikes(suspensionType);
            }
            else
            {
                    ETireType tireType;
                if(cbTireType.Text == "Regular")
                {
                    tireType = ETireType.Regular;
                }

                else if (cbTireType.Text == "Commuter")
                {
                     tireType = ETireType.Commuter;
                }
                else
                {
                     tireType = ETireType.Gravel;
                }
                    
                
                bikeToAdd = new RoadBikes(tireType);

            }

            bikeToAdd.SerialNumber = Convert.ToInt32(txtSerialNumber.Text);
            bikeToAdd.Model = txtModel.Text;
            bikeToAdd.ManufacturingYear = Convert.ToInt32(txtManufacturingYear.Text);
            bikeToAdd.Color = (Ecolor)Enum.Parse(typeof(Ecolor), cbColor.Text);
            

            listOfBikes.Add(bikeToAdd);
            ResetFromField ();
            RefreshDisplayList();
            MessageBox.Show("The Bike has been added");



        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BikesXMLData.Save(listOfBikes);
            ResetFromField();
            MessageBox.Show("The list of Bikes has been saved");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!AllFieldOk())
            {
                return;
            }
            else if(txtSerialNumber.Text != txtOriginalid.Text)
            {
                 if (!Validator.ValidateUniqueSerialNumber(txtSerialNumber.Text, listOfBikes))
                {
                    MessageBox.Show("This Serial Number is already used");
                    return;
                }
            }
            



            var bikeToUpdate = FindBikeBySerialNumber(Convert.ToInt32(txtOriginalid.Text));
            if (bikeToUpdate is MountainBikes && cbBikeType.Text == "Road" ||
                bikeToUpdate is RoadBikes && cbBikeType.Text == "Mountain")
            {
                MessageBox.Show("It is not possible to change the Bike type. Alternatively, " +
                                "you may remove and add this Bike with a different type");
                return;
            }

            if (cbBikeType.Text=="Mountain")
            {
                (bikeToUpdate as MountainBikes).SuspensionType = (ESuspensionType)Enum.Parse(typeof(ESuspensionType), cbSuspensionType.Text);
 
            }
            else
            {
                (bikeToUpdate as RoadBikes).TireType = (ETireType)Enum.Parse(typeof(ETireType), cbTireType.Text);

            }

            bikeToUpdate.SerialNumber = Convert.ToInt32(txtSerialNumber.Text);
            bikeToUpdate.Model = txtModel.Text;
            bikeToUpdate.ManufacturingYear = Convert.ToInt32(txtManufacturingYear.Text);
            bikeToUpdate.Color = (Ecolor)Enum.Parse(typeof(Ecolor), cbColor.Text);
            RefreshDisplayList();
            ResetFromField();
            txtOriginalid.Text = "";
            MessageBox.Show("The Bike has beed updated");
        }
        private void cbBikeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBikeType.Text == "Mountain")
            {
                cbSuspensionType.Enabled = true;
                cbTireType.Enabled = false;
                cbSuspensionType.SelectedIndex = 0;
                cbTireType.SelectedIndex = -1;

            }
            else 
            {
                
                cbSuspensionType.Enabled = false;
                cbTireType.Enabled = true;
                cbTireType.SelectedIndex = 0;
                cbSuspensionType.SelectedIndex = -1;

            }
        }
        private void cbSuspensionType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDisplayList();
            lstBikes.Enabled = true;
            btnRemove.Enabled = true; 
        }

        private void rbMountain_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDisplayList();
            lstBikes.Enabled = false;
            btnRemove.Enabled = false;

        }

        private void rbRoads_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDisplayList();
            lstBikes.Enabled = false;
            btnRemove.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSerialNumber.Text == "" || !Validator.ValidateSearialNumber(txtSerialNumber.Text))
            {
                MessageBox.Show("Serial Number is required and should be numeric");
                return;
            }
            int serialNumber=Convert.ToInt32(txtSerialNumber.Text);
            var bikeFound = FindBikeBySerialNumber(serialNumber);
            if(bikeFound == null)
            {
                MessageBox.Show("Bike not found");
                return;
            }
            string message = bikeFound.ToString().Replace(",", Environment.NewLine);
            MessageBox.Show(message);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = lstBikes.SelectedIndex;
            if(index<0)
            {
                MessageBox.Show("Please select a Bike");
                return;
            }
            var result = MessageBox.Show("Do you really want to remove?",
                "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            listOfBikes.RemoveAt(index);
            lstBikes.Items.RemoveAt(index);
            ResetFromField();

            MessageBox.Show("Bike is Removed");
        }

        private void lstBikes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBikes.SelectedIndex < 0)
                return;
            var bike = listOfBikes[lstBikes.SelectedIndex];
            if (bike is MountainBikes)
            {
                cbBikeType.SelectedIndex = 0;
                var bikeAsMountain = (MountainBikes)bike;
                cbSuspensionType.SelectedIndex = (int)bikeAsMountain.SuspensionType;
            }
            else
            {
                cbBikeType.SelectedIndex = 1;
                var bikeAsRoad = (RoadBikes)bike;
                cbTireType.SelectedIndex = (int)bikeAsRoad.TireType;
            }
            txtSerialNumber.Text = bike.SerialNumber.ToString();
            txtOriginalid.Text = txtSerialNumber.Text;
            txtModel.Text = bike.Model;
            cbColor.SelectedIndex = (int)bike.Color;
            txtManufacturingYear.Text=bike.ManufacturingYear.ToString();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to exit?",
                                         "Confirmation",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            if (result == DialogResult.No)
                e.Cancel = true;    
        }
    }
}
