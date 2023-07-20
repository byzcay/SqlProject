using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //Dosya işlemleri için kullanılan kütüphane

using System.Data.SqlClient; //sistem kütüphanesi

namespace EmployeeManagementProject
{
    public partial class InformationScreen : Form
    {
        public InformationScreen()
        {
            InitializeComponent();
        }

        //Datatable için oluşturduğum table objeleri
        DataSetInquiryTableAdapters.EmployeesTableAdapter dt_e = new DataSetInquiryTableAdapters.EmployeesTableAdapter();
        DataSetInquiryTableAdapters.DepartmentTableAdapter dt_d = new DataSetInquiryTableAdapters.DepartmentTableAdapter();
        DataSetInquiryTableAdapters.GenderTableAdapter dt_g = new DataSetInquiryTableAdapters.GenderTableAdapter();
        DataSetInquiryTableAdapters.JobsTableAdapter dt_j = new DataSetInquiryTableAdapters.JobsTableAdapter();
        DataSetInquiryTableAdapters.LocationTableAdapter dt_l = new DataSetInquiryTableAdapters.LocationTableAdapter();
        DataSetInquiryTableAdapters.UsersTableAdapter dt_u = new DataSetInquiryTableAdapters.UsersTableAdapter();
        DataSetInquiryTableAdapters.ProfiencyTableAdapter dt_p = new DataSetInquiryTableAdapters.ProfiencyTableAdapter();


        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-DPC8B16\\SQLEXPRESS;Integrated Security=True");

        private void data()
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("Select * from EmployeeManagement.dbo.Employees_1", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = read["first_name"].ToString();
                ekle.SubItems.Add(read["last_name"].ToString());

            }
            baglantı.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
           
            baglantı.Open();

            try
            {
                if (String.IsNullOrEmpty(txt_experience.Text))
                {
                    txt_experience.Text = "0";
               
                }

                if (String.IsNullOrEmpty(txtRegNum.Text))
                {
                    MessageBox.Show("Register number cannot be empty!", "error!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                else
                {
                    if(EmployeePictureBox.Image == null)
                    {
                        dt_e.EmployeesInsertQuery(txt_firstName.Text, txt_lastName.Text, txt_idNum.Text, txtEmail.Text,
                        TimePickerBirthdate.Value.ToString(), Convert.ToInt32(cmbLocation.SelectedValue), txtPhoneNum.Text,
                        Convert.ToInt32(cmbJCode.SelectedValue), Convert.ToInt32(cmbGender.SelectedValue),
                        Convert.ToInt32(cmbDCode.SelectedValue), txtRegNum.Text, Convert.ToInt32(cmbProfiency.SelectedValue), Convert.ToInt32(txt_experience.Text));
                        
                    }
                    else
                    {
                        dt_e.EmployeesInsertQuery(txt_firstName.Text, txt_lastName.Text, txt_idNum.Text, txtEmail.Text,
                        TimePickerBirthdate.Value.ToString(), Convert.ToInt32(cmbLocation.SelectedValue), txtPhoneNum.Text,
                        Convert.ToInt32(cmbJCode.SelectedValue), Convert.ToInt32(cmbGender.SelectedValue),
                        Convert.ToInt32(cmbDCode.SelectedValue), txtRegNum.Text, Convert.ToInt32(cmbProfiency.SelectedValue), Convert.ToInt32(txt_experience.Text));
                        SavePicture(txtRegNum.Text);
                        /*string _sql = string.Format("Use EmployeeManagement update dbo.Employees set picture values (NULL) where registration_num = " + txtRegNum, baglantı);
                        SqlCommand Komut = new SqlCommand(_sql, baglantı);
                        Komut.ExecuteNonQuery();
                        */
                    }

                    baglantı.Close();


                    MessageBox.Show("Personnel information has been successfully saved", "Congrats!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                

            }
            catch(FormatException)
            {

                MessageBox.Show("Lütfen sadece sayı giriniz.");
                baglantı.Close();
            }

            
            txt_firstName.Clear();
            txt_lastName.Clear();
            txt_idNum.Clear();
            txtEmail.Clear();
            txtPhoneNum.Clear();
            txtRegNum.Clear();
            txt_experience.Clear();
            EmployeePictureBox.Image = null;

        }

        //image insertion code line start
        string imagepath; //Keeps the path of the selected image file
        private void btnUpload_Click(object sender, EventArgs e)
        {
            opnFilePictureBox.Title = "Choose picture";
            opnFilePictureBox.Filter = "JPEG Files(*.jpg; *jpeg; *jpe)|*.jpg; *jpeg; *jpe| PNG Files (*.png)|*.png| All files (*.*)|*.*";
            if (opnFilePictureBox.ShowDialog()==DialogResult.OK)
            {
                EmployeePictureBox.Image = Image.FromFile(opnFilePictureBox.FileName);
                imagepath = opnFilePictureBox.FileName;
            }

        }

       
        private void SavePicture(string reg_num)
        {
            baglantı.Close();
            FileStream fileStream = new FileStream(imagepath, FileMode.Open, FileAccess.Read); //Reading selected image file.(Takes the file path, file mode, file read parameters respectively.)
            
            BinaryReader binaryReader = new BinaryReader(fileStream); //Takes a stream parameter.
            byte[] picture = binaryReader.ReadBytes((int)fileStream.Length); //Byte type object is defined and read file path is assigned to this object.
            binaryReader.Close();
            fileStream.Close();

            baglantı.Open();
            SqlCommand Komut = new SqlCommand("Use EmployeeManagement update dbo.Employees set picture = @picture where registration_num = "+ reg_num, baglantı);
            Komut.Parameters.Add("@picture", SqlDbType.Image, picture.Length).Value = picture;
            Komut.ExecuteNonQuery();
            baglantı.Close();
            

        }

        private void btnUpdatePic_Click(object sender, EventArgs e)
        {
            SavePicture(lblUpdateRnumber.Text);
            MessageBox.Show("The personnel image has been successfully updated.", "Congrats!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void dataGrid_inquiry_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selected = dataGrid_inquiry.SelectedCells[0].RowIndex;
            btnUpdatePic.Enabled = true;
            btnUpdatePic.Visible = true;
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void btn_inquiry_Click(object sender, EventArgs e)
        {
            tab_.SelectedIndex = 2;

        }

        private void btnMenuPI_Click(object sender, EventArgs e)
        {
            tab_.SelectedIndex = 0;
            EmployeePictureBox.Image = null;

        }

        private void btnMenuSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnMenuCI_Click(object sender, EventArgs e)
        {
            tab_.SelectedIndex = 1;
            btnUpdatePic.Enabled = true;
            btnUpdatePic.Visible = true;
        }

        private void btnMenuExp_Click(object sender, EventArgs e)
        {
            tab_.SelectedIndex = 2;
            EmployeePictureBox.Image = null;
        }

        private void cmbBox_Gender()
        {
            cmbGender.DataSource = dt_g.GetDataGender();
            cmbGender.DisplayMember = "gender_name";
            cmbGender.ValueMember = "gender_id";
        }
        private void cmbBox_Department()
        {
            cmbDCode.DataSource = dt_d.GetDataDepartment();
            cmbDCode.DisplayMember = "department_name";
            cmbDCode.ValueMember = "department_id";
            cmbQuiryDep.DataSource = dt_d.GetDataDepartment();
            cmbQuiryDep.DisplayMember = "department_name";
            cmbQuiryDep.ValueMember = "department_id";
        }
        private void cmbBox_Job()
        {
            cmbJCode.DataSource = dt_j.GetDataJobs();
            cmbJCode.DisplayMember = "job_title";
            cmbJCode.ValueMember = "job_id";
        }
        private void cmbBox_Profiency()
        {
            cmbProfiency.DataSource = dt_p.GetDataProfiency();
            cmbProfiency.DisplayMember = "profiency_name";
            cmbProfiency.ValueMember = "profiency_id";
        }

        private void cmbBox_Location()
        {
            cmbLocation.DataSource = dt_l.GetDataLocation();
            cmbLocation.DisplayMember = "location_name";
            cmbLocation.ValueMember = "location_id";
        } 
        private void InformationScreen_Load(object sender, EventArgs e)
        {
            EmployeePictureBox.Image = null;
            this.departmentTableAdapter1.Fill(this.employeeManagementDataSet5.Department);
            this.profiencyTableAdapter.Fill(this.employeeManagementDataSet5.Profiency);
            this.locationTableAdapter.Fill(this.employeeManagementDataSet.Location);
            this.jobsTableAdapter.Fill(this.employeeManagementDataSet.Jobs);
            this.genderTableAdapter.Fill(this.employeeManagementDataSet.Gender);
            this.employeesTableAdapter1.Fill(this.employeeManagementDataSet4.Employees);
            WindowState = FormWindowState.Maximized;
            cmbBox_Gender();
            cmbBox_Department();
            cmbBox_Job();
            cmbBox_Profiency();
            cmbBox_Location();
        }

        private void dataGrid_inquiry_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selected = dataGrid_inquiry.SelectedCells[0].RowIndex;
            btnUpdatePic.Enabled = true;
            btnUpdatePic.Visible = true;

        }
        
        private void btnList_Click(object sender, EventArgs e)
        {
            
            dataGrid_inquiry.DataSource =dt_e.GetDataEmployees();
            dataGrid_inquiry.Columns[0].HeaderText = "Employees ID";
            dataGrid_inquiry.Columns[1].HeaderText = "First Name";
            dataGrid_inquiry.Columns[2].HeaderText = "Last Name";
            dataGrid_inquiry.Columns[3].HeaderText = "Identity Number";
            dataGrid_inquiry.Columns[4].HeaderText = "E-Mail";
            dataGrid_inquiry.Columns[5].HeaderText = "Birthdate";
            dataGrid_inquiry.Columns[6].HeaderText = "Phone Number";
            dataGrid_inquiry.Columns[7].HeaderText = "Registiration Number";
            dataGrid_inquiry.Columns[8].HeaderText = "Years of Experience";
            dataGrid_inquiry.Columns[9].HeaderText = "Location";
            dataGrid_inquiry.Columns[10].HeaderText = "Job";
            dataGrid_inquiry.Columns[11].HeaderText = "Gender";
            dataGrid_inquiry.Columns[12].HeaderText = "Department";
            dataGrid_inquiry.Columns[13].HeaderText = "Proficiency";
            dataGrid_inquiry.Columns[14].HeaderText = "Picture";
        }
        public void arabul()

        {
            baglantı.Open();
            string Name = txtQuiryName.Text;
            string Lname = txtQuiryLname.Text;
            string RegNum = txtQuiryRegNum.Text;
            string kayit = "Use EmployeeManagement SELECT * from dbo.Employees where (@registration_num is not null and RegNum like '%' + @registration_num + '%') and (@first_name is not null and Name = @first_name) or (@last_name is not null and Lname = @last_name)";
            SqlCommand komut = new SqlCommand(kayit, baglantı);

            if (!string.IsNullOrEmpty(txtQuiryRegNum.Text))
                komut.Parameters.AddWithValue("@registration_num", txtQuiryRegNum.Text);

            else komut.Parameters.AddWithValue("@registration_num", DBNull.Value);



            if (!string.IsNullOrEmpty(txtQuiryName.Text))
                komut.Parameters.AddWithValue("@first_name ", txtQuiryName.Text);

            else komut.Parameters.AddWithValue("@first_name ", DBNull.Value);



            if (!string.IsNullOrEmpty(txtQuiryLname.Text))
                komut.Parameters.AddWithValue("@last_name ", txtQuiryName.Text);

            else komut.Parameters.AddWithValue("@last_name ", DBNull.Value);

            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGrid_inquiry.DataSource = dt;
            baglantı.Close();

        }
        /*private void FilterDataGrid()
        {
            var _txtName = Convert.ToString(txtQuiryName.Text);
            var _comboDepText = !string.IsNullOrEmpty(cmbQuiryDep.Text) ? Convert.ToString(cmbQuiryDep.SelectedItem) : string.Empty;
            var result =Use EmployeeManagement Employees.Where(Srodek => Srodek.Srodek.category1 == _comboDepText || Srodek.Srodek.ID.Device == _txtName).ToList();
            //
            dataGrid_inquiry.DataSource = result;
        }
        */

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //arabul();
           // FilterDataGrid();

            
            string Name = txtQuiryName.Text;
            string Lname = txtQuiryLname.Text;
            string RegNum = txtQuiryRegNum.Text;
            

            /*
            if (RegNum != "")
                Filter += "registration_num like '%" + RegNum + "%' and ";
            if (Name != "")
                Filter += "first_name like '%" + Name + "%' and ";
            if (Lname != "")
                Filter += "last_name like '%" + Lname + "%' and ";

            if (Filter.Length > 0 )
            {
                string FinalFilter = Filter.Remove(Filter.Length -4, 3);
                MessageBox.Show(FinalFilter);
                //dataGrid_inquiry.DataSource = dt_e.List(Finalfilter);
               
                //dataGrid_inquiry.Select(FinalFilter);
               // dataGrid_inquiry.DataSource = dt_e.List(FinalFilter);
            }*/
            /*
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGrid_inquiry.DataSource;
            bs.Filter = "[registration_num] like '%" + RegNum + "%' " +
                "AND [first_name] like '%" + Name + "%'" +
                "AND [last_name] like '%" + Lname + "%'";
                //"AND [ColumnName4] like '%" + textBox1.Text + "%'";
            dataGrid_inquiry.DataSource = bs;*/
            
            dataGrid_inquiry.DataSource = dt_e.ListByRegNum(txtQuiryRegNum.Text);
             if(txtQuiryName.Text.Length > 0)
                 dataGrid_inquiry.DataSource = dt_e.ListByName(txtQuiryName.Text);
             if(txtQuiryLname.Text.Length > 0)
                 dataGrid_inquiry.DataSource = dt_e.ListByLname(txtQuiryLname.Text);
             else if(cmbQuiryDep != null)
                  dataGrid_inquiry.DataSource = dt_e.ListByPosition(Convert.ToInt32(cmbQuiryDep.SelectedValue));

        }
        private void cmbQuiryDep_SelectedValueChanged(object sender, EventArgs e)
        {
            // FilterDataGrid();
        }
    

        private void btnSelect_Click(object sender, EventArgs e)
        {
            tab_.SelectedIndex = 1;
            btnUpdatePic.Enabled = true;
            btnUpdatePic.Visible = true;

        }
        private byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        private void dataGrid_inquiry_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tab_.SelectedIndex = 1;
            btnUpdatePic.Enabled = true;
            btnUpdatePic.Visible = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGrid_inquiry.Rows[e.RowIndex];
                try
                {
                    txtUpdateName.Text = row.Cells[1].Value.ToString();
                    txtUpdateLname.Text = row.Cells[2].Value.ToString();
                    txtUpdateID.Text = row.Cells[3].Value.ToString();
                    txtUpdateMail.Text = row.Cells[4].Value.ToString();
                    TimePickerBirthdate.Value = DateTime.Parse(row.Cells[5].Value.ToString()); //kontrol et 
                    txtUpdatePnum.Text = row.Cells[6].Value.ToString();
                    lblUpdateRnumber.Text = row.Cells[7].Value.ToString();
                    txtUpdateExp.Text = row.Cells[8].Value.ToString();
                    cmbUpdateLoc.Text = row.Cells[9].Value.ToString();
                    cmbUpdateJob.Text = row.Cells[10].Value.ToString();
                    cmbUpdateGender.Text = row.Cells[11].Value.ToString();
                    cmbUpdateDpart.Text = row.Cells[12].Value.ToString();
                    cmbUpdateProf.Text = row.Cells[13].Value.ToString();
                    
                    if (row.Cells[14].Value != DBNull.Value)
                    {
                        byte[] data = new byte[0];
                        data = (byte[])row.Cells[14].Value;
                        MemoryStream mem = new MemoryStream(data);
                        EmployeePictureBox.Image = System.Drawing.Image.FromStream(mem, false, true);

                    }
                    else
                    {
                        EmployeePictureBox.Image = null;
                        return;
                    }
                }
                catch(Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                    return;
                }
            }

           
        }
        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUpdateExp.Text))
            {
                txtUpdateExp.Text = "0";

            }
            dt_e.UpdateQuery(txtUpdateName.Text, txtUpdateLname.Text, txtUpdateID.Text, txtUpdateMail.Text,
                    UpdateBirthdate.Value.ToString(), Convert.ToInt32(cmbUpdateLoc.SelectedValue), txtUpdatePnum.Text,
                    Convert.ToInt32(cmbUpdateJob.SelectedValue), Convert.ToInt32(cmbUpdateGender.SelectedValue),
                    Convert.ToInt32(cmbUpdateDpart.SelectedValue), Convert.ToInt32(cmbUpdateProf.SelectedValue), Convert.ToInt32(txtUpdateExp.Text), lblUpdateRnumber.Text);
            
            if (EmployeePictureBox.Image != null)
            {
                SavePicture(lblUpdateRnumber.Text);
            }
            else
            {

            }
            
            
            MessageBox.Show("Personnel information has been successfully updated", "Congrats!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void cmbQuiryDep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tab__SelectedIndexChanged(object sender, EventArgs e)
        {
            EmployeePictureBox.Image = null;
            btnUpdatePic.Enabled = false;
            btnUpdatePic.Visible = false;
            //tab_Update.Selected = false;
        }
    }
}
