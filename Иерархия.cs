using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Иерархия_классов
{
    interface IWork
    {
        DateTime Begintime { get; set; } //свойство - начало работы
        DateTime Endtime { get; set; } //свойство - конец работы
        void BeginWork(); //метод начала работы
        void EndWork(); //метод окончания работы
    } //интерфейс для всех работников
    interface IEmployer
    {
        void GetPay(uint salary); //метод получения зп
        void Withdraw(uint sum); //метод вывода со счёта
    } //интерфейс для работников
    interface IBuhgalter
    {
        int summary { get; set; } //вся сумма выплат предприятия
        void MathSalary(CEmployer s); //метод расчёта зп с налогами
        void SummaryWithdraw(uint salary); //метод подсчёта всех выплат
    } //интерфейс для бухгалтера

    abstract class CEmployer:IEmployer,IWork
    {
        public string Name; //имя
        public string Surname; //фамилия
        public string MiddleName; //отчество
        public byte experience; //стаж
        public byte Age { get; set; } //возраст
        public uint Salary { get; set; } //зарплата (счёт)
        public byte WorkWeek { get; set; } //рабочая неделя
        public string status { get; set; } //статус работы
        public  DateTime Begintime { get; set; } //начало работы (время)
        public  DateTime Endtime { get; set; } //конец работы (время)

        public void Initialize(string Name, string surname, string middleName, byte age, byte exp) 
        {
                this.Name = Name;
                Surname = surname;
                MiddleName = middleName;
                Age = age;
                experience = exp;
        } //инициализация (вместо конструктора)
        public abstract void GetPay(uint salary); //метод получения зп с реализацией в производных классах (полиморфизм)  
        public abstract void BeginWork(); //метод начала работы
        public abstract void EndWork(); //метод окончания работы
        public void Withdraw(uint sum)
        {
            if (Salary > sum)
            {
                Salary -= sum;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        } //вывод денег со счёта клиента
    } //класс кадры (общий для всех работников)

    class CWorker : CEmployer
    {
        public CWorker()
        {
            WorkWeek = 5;
        }
        public override void GetPay(uint salary) 
        {
            if (salary < 12500 || salary > 60000)
            {
                Salary += (uint)new Random().Next(12500, 60000);
            }
            else Salary += salary;
        } //переопределение абстрактного метода

        //реализация интерфейса
        public override void BeginWork()
        {
            status = "Рабочий работает";
            Begintime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
        } //переопределение абстрактно-реализованного метода интерфейса
        public override void EndWork()
        {
            status = "Рабочий закончил";
            Endtime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
        } //переопределение абстрактно-реализованного метода интерфейса
    } //класс рабочий

    class CEngineer : CEmployer
    {
        public CEngineer()
        {
            WorkWeek = 4;
        }
        public override void GetPay(uint salary) //переопределение абстрактного метода
        {
            if (salary < 30000 || salary > 100000)
            {
                Salary += (uint)new Random().Next(30000, 100000);
            }
            else Salary += salary;
        }

        //реализация интерфейса
        public override void BeginWork()
        {
            status = "Инженер работает";
            Begintime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
        } //переопределение абстрактно-реализованного метода интерфейса
        public override void EndWork()
        {
            status = "Инженер закончил";
            Endtime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 21, 0, 0);
        } //переопределение абстрактно-реализованного метода интерфейса
    } //класс инженер

    static class CAdministration
    {
        public static Action BeginWork; //многоадресный делегат начала работы
        public static Action EndWork; //многоадресный делегат окончания работы

        public static List<CEmployer> listOfEmployees = new List<CEmployer>(); //список сотрудников
        public static Buhgalter b = new Buhgalter(); //статический объект класса бухгалтера     

        public static void Dismissal(string Surname) 
        {
            foreach(CEmployer s in listOfEmployees)
            {
                if (s.Surname == Surname)
                {
                    listOfEmployees.Remove(s);
                    break;
                }
            }
        }  //увольнение сотрудника
        public static void AddToList(CEmployer employer) 
        {
            try
            {
                listOfEmployees.Add(employer);
                BeginWork += employer.BeginWork; //добавление методов нач.работы к делегату
                EndWork += employer.EndWork; //добавление методов окон.работы к делегату
            }
            catch(ArrayTypeMismatchException)
            {
                MessageBox.Show("НЕподходящий тип для сохранения");
            }
        } //найм сотрудника
        public static void Withdraw(string surname, uint salary)
        {
            try
            {
                for (int i = 0; i < listOfEmployees.Count; i++)
                {
                    if (listOfEmployees[i].Surname == surname)
                    {
                        listOfEmployees[i].GetPay(salary); //выплата без налогов
                        b.SummaryWithdraw(listOfEmployees[i].Salary); //прибавляем к общим затратам предприятия
                        b.MathSalary(listOfEmployees[i]); //выплата c налогами
                    }
                }
            }
            catch(IndexOutOfRangeException)
            {
                MessageBox.Show("Индекс находится вне границ!");
            }
        } //выплата зарплаты
    } //статический класс администрации

    class Buhgalter : CEmployer,IBuhgalter
    {
        public Buhgalter()
        {
            WorkWeek = 6;
        }

        public int summary { get; set; } //сумма всех выплат
        public override void BeginWork()
        {
            status = "Бухгалтер работает";
            Begintime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0);
        } //переопределение абстрактно-реализованного метода интерфейса
        public override void EndWork()
        {
            status = "Бухгалтер закончил";
            Endtime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 22, 0, 0);
        } //переопределение абстрактно-реализованного метода интерфейса
        public override void GetPay(uint salary)
        {
            if (salary < 55000 || salary > 95000)
            {
                Salary += (uint)new Random().Next(55000, 95000);
            }
            else Salary += salary;
        } //переопределение абстрактного метода
        public void MathSalary(CEmployer employer)
        {
            employer.Salary -= (uint)(employer.Salary * 0.13);
        } //вычет налога 13%
        public void SummaryWithdraw(uint salary)
        {
            summary += Convert.ToInt32(salary);
        } //подсчёт суммы всех выплат
    } //класс бухгалтера
}
