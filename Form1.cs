using System;
using System.Windows.Forms;

namespace Иерархия_классов
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //найм сотрудника
            try
            {
                switch(comboBox1.SelectedItem.ToString())
                {
                    case "Инженер":
                        CEngineer engineer = new CEngineer();
                        engineer.Initialize(Name.Text, Surname.Text, MiddleName.Text, Convert.ToByte(Age.Text), Convert.ToByte(Experience.Text));
                        CAdministration.AddToList(engineer);
                        break;
                    case "Рабочий":
                        CEmployer worker = new CWorker();
                        worker.Initialize(Name.Text, Surname.Text, MiddleName.Text, Convert.ToByte(Age.Text), Convert.ToByte(Experience.Text));
                        CAdministration.AddToList(worker);
                        break;
                    case "Бухгалтер":
                        CAdministration.b.Initialize(Name.Text, Surname.Text, MiddleName.Text, Convert.ToByte(Age.Text), Convert.ToByte(Experience.Text));
                        CAdministration.AddToList(CAdministration.b);
                        comboBox1.Items.Remove("Бухгалтер"); //удаляем чтобы был только 1 бухгалтер
                        break;
                }
                SetToDataGrid();
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Объект не был создан!");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string delete = Microsoft.VisualBasic.Interaction.InputBox("Введите фамилию сотрудника:");
            CAdministration.Dismissal(delete); //увольнение сотрудника
            SetToDataGrid();
            if (delete == CAdministration.b.Surname) comboBox1.Items.Add("Бухгалтер");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            SetToDataGrid(); //обновление таблицы 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                CAdministration.Withdraw(textBox1.Text, uint.Parse(textBox2.Text));
                SetToDataGrid(); //обновление таблицы 
            }
            catch(FormatException)
            {
                MessageBox.Show("Неверно заполнены поля!");
            }
        }
        private void SetToDataGrid()
        {
            dataGridView.Rows.Clear(); //очистка прошлой таблицы
            for (int i = 0; i < CAdministration.listOfEmployees.Count; i++)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = CAdministration.listOfEmployees[i].Surname;
                dataGridView.Rows[i].Cells[1].Value = CAdministration.listOfEmployees[i].Name;
                dataGridView.Rows[i].Cells[2].Value = CAdministration.listOfEmployees[i].MiddleName;
                dataGridView.Rows[i].Cells[3].Value = CAdministration.listOfEmployees[i].Age;
                dataGridView.Rows[i].Cells[4].Value = CAdministration.listOfEmployees[i].experience;
                dataGridView.Rows[i].Cells[5].Value = CAdministration.listOfEmployees[i].WorkWeek;
                dataGridView.Rows[i].Cells[6].Value = CAdministration.listOfEmployees[i].Salary;
                dataGridView.Rows[i].Cells[7].Value = CAdministration.listOfEmployees[i].status;
                dataGridView.Rows[i].Cells[8].Value = CAdministration.listOfEmployees[i].Begintime;
                dataGridView.Rows[i].Cells[9].Value = CAdministration.listOfEmployees[i].Endtime;
            }
        } //метод заполнения таблицы
        private void button5_Click(object sender, EventArgs e)
        {
            CAdministration.BeginWork?.Invoke();
            SetToDataGrid();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            CAdministration.EndWork?.Invoke();
            SetToDataGrid();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сумма всех выплат работникам: " + CAdministration.b.summary);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var c in CAdministration.listOfEmployees)
                {
                    if (c.Surname == textBox3.Text)
                    {
                        c.Withdraw(uint.Parse(textBox4.Text));
                        MessageBox.Show("Деньги успешно сняты!");
                        break;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Сумма денег недоступна для снятия");
            }
        }
    }
}
