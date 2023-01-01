using Entities.Dtos.UserDtos;
using System.Net.Http.Json;

namespace WebApiWithWindowsForm
{
    public partial class Form1 : Form
    {
        #region Defines
        private string url = "https://localhost:7176/api/";
        private int selectedID = 0;
        #endregion

        #region Form1
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            await DataGridViewFill();
            cmbGenderFill();
        }
#endregion

        #region Crud
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                UserAddDto userAddDto = new UserAddDto()
                {
                    FirstName = txtFname.Text,
                    Address = txtAdrs.Text,
                    DateOfBirth = Convert.ToDateTime(dtpDOB.Text),
                    Email = txtEmail.Text,
                    Gender = comboGender.Text == "Male" ? true : false,
                    LastName = txtLname.Text,
                    Password = txtPwd.Text,
                    Username = txtUname.Text,
                };
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(url + "Users/Add", userAddDto);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Insertion process completed successfully");
                    await DataGridViewFill();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("An error occurred during the insertion process");
                }
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                UserUpdateDto userUpdateDto = new UserUpdateDto()
                {
                    Id = selectedID,
                    FirstName = txtFname.Text,
                    Address = txtAdrs.Text,
                    DateOfBirth = Convert.ToDateTime(dtpDOB.Text),
                    Email = txtEmail.Text,
                    Gender = comboGender.Text == "Male" ? true : false,
                    LastName = txtLname.Text,
                    Password = txtPwd.Text,
                    Username = txtUname.Text,
                };
                HttpResponseMessage response = await httpClient.PutAsJsonAsync(url + "Users/Update", userUpdateDto);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Edit process completed successfully");
                    await DataGridViewFill();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("An error occurred during the edit process");
                }
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(url + "Users/Delete/" + selectedID);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Delete process completed successfully");
                    await DataGridViewFill();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("An error occurred during the delete process");
                }
            }
        }

        private async void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            selectedID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            using (HttpClient httpClient = new HttpClient())
            {
                var user = await httpClient.GetFromJsonAsync<UserDto>(url + "Users/GetById/" + selectedID);

                txtAdrs.Text = user.Address;
                comboGender.SelectedValue = user.Gender == true ? 1 : 2;
                txtFname.Text = user.FirstName;
                txtLname.Text = user.LastName;
                txtPwd.Text = String.Empty;
                txtUname.Text = user.Username;
                txtEmail.Text = user.Email;
                dtpDOB.Value = user.DateOfBirth;
            }
            btnAdd.Enabled = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }
        #endregion

        #region Methods
        private void cmbGenderFill()
        {
            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender() { Id = 1, GenderName = "Male" });
            genders.Add(new Gender() { Id = 2, GenderName = "Female" });
            comboGender.DataSource = genders;
            comboGender.DisplayMember = "GenderName";
            comboGender.ValueMember = "Id";
        }
        class Gender
        {
            public int Id { get; set; }
            public string GenderName { get; set; }
        }
        private async Task DataGridViewFill()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var users = await httpClient.GetFromJsonAsync<List<UserDetailDto>>(url + "Users/GetList");
                dataGridView1.DataSource = users;
            }
        }
        void ClearForm()
        {
            txtAdrs.Text = String.Empty;
            comboGender.SelectedValue = 0;
            txtFname.Text = String.Empty;
            txtLname.Text = String.Empty;
            txtPwd.Text = String.Empty;
            txtUname.Text = String.Empty;
            txtEmail.Text = String.Empty;
            dtpDOB.Value = DateTime.Now;
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
        #endregion
    }
}