using DataAccess.Models;
using Service;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DentalClinic
{
    public partial class AddOrEditUserWindow : Window
    {
        private readonly UserService _userService;
        private readonly User _currentUser;
        private readonly User _userToEdit;
        private readonly bool _isEditMode;

        public AddOrEditUserWindow(User currentUser, User userToEdit = null)
        {
            InitializeComponent();
            _userService = new UserService();
            _currentUser = currentUser;
            _userToEdit = userToEdit;
            _isEditMode = userToEdit != null;

            LoadRoles();
            LoadGenders();

            if (_isEditMode)
            {
                LoadUserData();
                txtUserId.Visibility = Visibility.Visible;
              
                Title = "Sửa người dùng";
            }
            else
            {
                Title = "Thêm người dùng mới";
                chkIsActive.IsChecked = true;
                txtUserId.Visibility = Visibility.Collapsed;
                
            }
        }

        private void LoadRoles()
        {
            cbRole.Items.Clear();
            cbRole.Items.Add(new ComboBoxItem { Content = "Admin", Tag = 1 });
            cbRole.Items.Add(new ComboBoxItem { Content = "Bác sĩ", Tag = 2 });
            cbRole.Items.Add(new ComboBoxItem { Content = "Bệnh nhân", Tag = 3 });
        }

        private void LoadGenders()
        {
            cbGender.Items.Clear();
            cbGender.Items.Add(new ComboBoxItem { Content = "Nam" });
            cbGender.Items.Add(new ComboBoxItem { Content = "Nữ" });
            cbGender.Items.Add(new ComboBoxItem { Content = "Khác" });
            cbGender.SelectedIndex = 0;
        }

        private void LoadUserData()
        {
            if (_userToEdit != null)
            {
                txtUserId.Text = _userToEdit.UserId.ToString();
                txtUsername.Text = _userToEdit.Username ?? "";
                txtFullName.Text = _userToEdit.FullName ?? "";
                txtEmail.Text = _userToEdit.Email ?? "";
                txtPhone.Text = _userToEdit.Phone ?? "";
                chkIsActive.IsChecked = _userToEdit.IsActive;
                dpDateOfBirth.SelectedDate = _userToEdit.DateOfBirth?.ToDateTime(TimeOnly.MinValue);
                txtAddress.Text = _userToEdit.Address ?? "";

                // Gán giới tính
                foreach (ComboBoxItem item in cbGender.Items)
                {
                    if (item.Content.ToString() == _userToEdit.Gender)
                    {
                        cbGender.SelectedItem = item;
                        break;
                    }
                }

                // Gán vai trò
                foreach (ComboBoxItem item in cbRole.Items)
                {
                    if ((int)item.Tag == _userToEdit.RoleId)
                    {
                        cbRole.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                if (_isEditMode)
                    UpdateUser();
                else
                    AddUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");
                txtUsername.Focus();
                return false;
            }
            if (!_isEditMode && string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                txtPassword.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                txtFullName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!");
                txtEmail.Focus();
                return false;
            }
            if (cbRole.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò!");
                cbRole.Focus();
                return false;
            }

            return true;
        }

        private void AddUser()
        {
            var selectedRole = cbRole.SelectedItem as ComboBoxItem;
            var selectedGender = cbGender.SelectedItem as ComboBoxItem;

            var newUser = new User
            {
                Username = txtUsername.Text.Trim(),
                PasswordHash = txtPassword.Password,
                FullName = txtFullName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                RoleId = (int)selectedRole.Tag,
                IsActive = chkIsActive.IsChecked == true,
                Gender = selectedGender?.Content.ToString(),
                DateOfBirth = dpDateOfBirth.SelectedDate.HasValue
                    ? DateOnly.FromDateTime(dpDateOfBirth.SelectedDate.Value)
                    : null,
                Address = txtAddress.Text.Trim()
            };

            bool success = _userService.AddUser(newUser);
            if (success)
            {
                MessageBox.Show("Thêm người dùng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm người dùng thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateUser()
        {
            if (_userToEdit == null) return;

            var selectedRole = cbRole.SelectedItem as ComboBoxItem;
            var selectedGender = cbGender.SelectedItem as ComboBoxItem;

            _userToEdit.Username = txtUsername.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                _userToEdit.PasswordHash = txtPassword.Password;
            }
            _userToEdit.FullName = txtFullName.Text.Trim();
            _userToEdit.Email = txtEmail.Text.Trim();
            _userToEdit.Phone = txtPhone.Text.Trim();
            _userToEdit.RoleId = (int)selectedRole.Tag;
            _userToEdit.IsActive = chkIsActive.IsChecked == true;
            _userToEdit.Gender = selectedGender?.Content.ToString();
            _userToEdit.DateOfBirth = dpDateOfBirth.SelectedDate.HasValue
                ? DateOnly.FromDateTime(dpDateOfBirth.SelectedDate.Value)
                : null;
            _userToEdit.Address = txtAddress.Text.Trim();

            bool success = _userService.UpdateUser(_userToEdit);
            if (success)
            {
                MessageBox.Show("Cập nhật người dùng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật người dùng thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
