using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mc
{
    public partial class Form1 : Form
    {
        private AppDbContext _db;
        public Form1()
        {
            InitializeComponent();
            _db = new AppDbContext();
            InitializeDatabase();
            LoadCategories();
            LoadRecords();
        }
        private void InitializeDatabase()
        {
            // Создаем базу данных, если она не существует
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
            _db.Database.Initialize(true);
        }
        private void LoadCategories()
        {
            // Загружаем существующие категории из базы
            var categories = _db.Records
                              .Select(r => r.Category)
                              .Distinct()
                              .Where(c => c != null)
                              .ToList();

            cmbCategory.Items.AddRange(categories.ToArray());
        }
        private void LoadRecords()
        {
            // Загружаем записи в DataGridView
            _db.Records.Load();
            dgvRecords.DataSource = _db.Records.Local.ToBindingList();
        }
        private void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название");
                return;
            }

            var record = new Record
            {
                Title = txtTitle.Text,
                Date = dtpDate.Value,
                Category = cmbCategory.Text,
                Description = txtDescription.Text
            };

            _db.Records.Add(record);
            _db.SaveChanges();

            // Обновляем список категорий, если добавили новую
            if (!cmbCategory.Items.Contains(cmbCategory.Text))
                cmbCategory.Items.Add(cmbCategory.Text);

            // Очищаем поля
            txtTitle.Clear();
            txtDescription.Clear();

            MessageBox.Show("Запись сохранена!");
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _db?.Dispose();
        }

        private void dgvRecords_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Отключаем автосоздание колонок (если они заданы вручную)
            dgvRecords.AutoGenerateColumns = false;

            // Привязываем данные
            dgvRecords.DataSource = _db.Records.Local.ToBindingList();

            // Принудительно обновляем отображение
            dgvRecords.Refresh();
        }
    }
}
