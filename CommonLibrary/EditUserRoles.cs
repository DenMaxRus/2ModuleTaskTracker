using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CommonLibrary.database;
using CommonLibrary.entities;
using CommonLibrary.ModuleFramework;
using System.Drawing;
using System.Linq;

namespace CommonLibrary
{
    public class EditUserRoles
    {
        private List<Module> modules;

        private TextBox formAddName;
        private Form formAdd;
        private Form formMain;
        private Form formEdit;
        private Panel rolesPanel;

        public static void Show(List<Module> modules)
        {
            EditUserRoles r = new EditUserRoles(modules);
        }

        private EditUserRoles(List<Module> modules)
        {
            this.modules = modules;
            RecreateMain();
        }

        private void RecreateMain()
        {
            bool needShowDialog = false;

            if (formMain == null)
            {
                formMain = new Form();
                needShowDialog = true;
            }

            formMain.Controls.Clear();

            formMain.Text = "Edit user roles";

            var roles = DatabaseManager.Instance.GetDatabase<UserRole>().Select();

            rolesPanel = new Panel();
            rolesPanel.Parent = formMain;
            rolesPanel.Dock = DockStyle.Fill;
            rolesPanel.AutoScroll = true;

            foreach (var role in roles)
            {
                CreatePanelForRole(role);
            }

            Button buttonAdd = new Button();
            buttonAdd.Text = "Create new role";
            buttonAdd.Parent = formMain;
            buttonAdd.Dock = DockStyle.Bottom;

            buttonAdd.Click += OnButtonAddClick;

            formMain.StartPosition = FormStartPosition.CenterScreen;

            if (needShowDialog)
                formMain.ShowDialog();
        }

        private void CreateEdit(UserRole role, Label nameLabel)
        {
            if (formEdit == null)
                formEdit = new Form();
            
            formEdit.Controls.Clear();

            formEdit.Text = "Edit role";

            Panel panel = new Panel ();
            panel.Parent = formEdit;
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            TextBox tbName = new TextBox();
            tbName.Parent = formEdit;
            tbName.Dock = DockStyle.Top;
            tbName.Text = role.Name;

            Label name = new Label();
            name.Text = "Name";
            name.Parent = formEdit;
            name.Dock = DockStyle.Top;

            List<CheckBox> checkboxes = new List<CheckBox> ();

            foreach (var module in modules)
            {
                Label title = new Label();
                title.Text = module.Name;
                title.Font = new Font (FontFamily.GenericSansSerif, 10, FontStyle.Bold);
                title.Parent = panel;
                title.Dock = DockStyle.Top;
                title.AutoSize = false;
                title.BringToFront();

                foreach (var action in module.Actions)
                {
                    CheckBox t = new CheckBox();
                    string [] split = action.Split ('.');
                    t.Checked = role.IsHaveAccessTo (split [0], split [1]);
                    t.Text = action;
                    t.Parent = panel;
                    t.Dock = DockStyle.Top;
                    t.AutoSize = false;
                    t.BringToFront();
                    checkboxes.Add(t);
                }
            }

            Button apply = new Button ();
            apply.Text = "Apply";
            apply.Dock = DockStyle.Bottom;
            apply.Parent = formEdit;

            apply.Click += (sender, e) => {
                ChangeRole (role, tbName, checkboxes);
                nameLabel.Text = tbName.Text;
                formEdit.Close();
                formEdit.Dispose();
                formEdit = null;
            };

            formEdit.StartPosition = FormStartPosition.CenterScreen;
            formEdit.ShowDialog();
        }

        private void ChangeRole(UserRole role, TextBox tbName, List<CheckBox> checkboxes)
        {
            role.Name = tbName.Text;

            foreach (var box in checkboxes) 
            {
                string[] split = box.Text.Split('.');
                role.SetAccess (split [0], split [1], box.Checked);
            }

            DatabaseManager.Instance.GetDatabase<UserRole> ().Update(role);
        }

        private void CreatePanelForRole(UserRole role)
        {
            Panel panel = new Panel();
            panel.Parent = rolesPanel;
            panel.Dock = DockStyle.Top;
            panel.Height = 20;

            Label name = new Label();
            name.Text = role.Name;
            name.Parent = panel;
            name.Dock = DockStyle.Fill;
            name.TextAlign = ContentAlignment.MiddleCenter;
            name.AutoSize = false;

            Label roleId = new Label();
            roleId.Text = role.Id;
            roleId.Parent = panel;
            roleId.Dock = DockStyle.Left;
            roleId.TextAlign = ContentAlignment.MiddleLeft;
            roleId.AutoSize = true;

            bool enableButtons = !role.Id.Equals("role_1");

            Button remove = new Button();
            remove.Text = "Remove";
            remove.Parent = panel;
            remove.Dock = DockStyle.Right;
            remove.Enabled = enableButtons;

            remove.Click += (sender, e) => { RemovePanel(role, panel); };

            Button edit = new Button();
            edit.Text = "Edit";
            edit.Parent = panel;
            edit.Dock = DockStyle.Right;
            edit.Enabled = enableButtons;

            edit.Click += (sender, e) => { CreateEdit(role, name); };
        }

        private void RemovePanel(UserRole role, Panel panel)
        {
            DatabaseManager.Instance.GetDatabase<UserRole>().Remove(role);
            rolesPanel.Controls.Remove(panel);
        }

        private void OnButtonAddClick(object sender, EventArgs e)
        {
            if (formAdd == null)
                formAdd = new Form();

            formAdd.Controls.Clear();

            formAdd.Text = "Add new role";

            Button ok = new Button();
            ok.Parent = formAdd;
            ok.Text = "Create";
            ok.Dock = DockStyle.Top;

            formAddName = new TextBox();
            formAddName.Parent = formAdd;
            formAddName.Dock = DockStyle.Top;

            Label name = new Label();
            name.Text = "Name:";
            name.Parent = formAdd;
            name.Dock = DockStyle.Top;

            ok.Click += OnButtonAddOkClick;

            formAdd.StartPosition = FormStartPosition.CenterScreen;
            formAdd.ShowDialog();
        }

        private void OnButtonAddOkClick(object sender, EventArgs e)
        {
            UserRole role = new UserRole();
            role.Name = formAddName.Text;
            DatabaseManager.Instance.GetDatabase<UserRole>().Add(role);

            formAdd.Close();
            formAdd.Dispose();
            formAdd = null;

            CreatePanelForRole(role);
        }
    }
}
