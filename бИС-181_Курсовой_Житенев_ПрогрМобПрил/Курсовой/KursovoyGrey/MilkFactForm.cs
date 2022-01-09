using MilkFactoryLibrary1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursovoyGrey
{
    public partial class MilkFactForm : Form
    {
        Loader loader;
        Warehouse warehouse ;
        public delegate void DelegateForTime(Label label);
        DelegateForTime DelTime;
        Thread t1;
        public MilkFactForm()
        {
            InitializeComponent();
            DelTime = new DelegateForTime(StartTime);
            loader = new Loader(pbPogr);
            warehouse = new Warehouse(loader,textBox1,lbsost);
        }
        private void MilkFactForm_Load(object sender, EventArgs e)
        {
            t1 = new Thread(LabelTime); // создаем поток  
            t1.IsBackground = true; // задаем фоновый режым  
            t1.Priority = ThreadPriority.Lowest; // указываем свмый низкий приоритет  
            t1.Start(); // 
            warehouse.mechs.Add(new Mechanic("Иванов", "Иван", 32));
            warehouse.mechs.Add(new Mechanic("Сидаров", "Дмитрий", 34));
            dataGridView1.DataSource = warehouse.mechs;

        }
        void LabelTime()
        {
            // безконечный цыкл  
            while (true)
            {
                Invoke(DelTime, lbTime);// запускаем метод с главного потока             
            }
        }
        void StartTime(Label label)
        {
            // выводим всегда две цыфры   
            // (00:00)  
            string s = DateTime.Now.Hour.ToString("00");
            s += " : ";
            s += DateTime.Now.Minute.ToString("00");

            s += " : " + DateTime.Now.Second.ToString("00");
            label.Text = s;
        }
        private void ClStr(object sender, EventArgs e)
        {
            lbsost.Text = "Производство работает";
            warehouse.Open();
            
        }

        private void ClStp(object sender, EventArgs e)
        {
            lbsost.Text = "Производство остановлено";
            warehouse.Close();              
            //dataGridView2.DataSource = warehouse.ot;
        }

        private void ClUpd(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = warehouse.otchet;
        }

        private void MilkFactForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(warehouse.IsWork())
            warehouse.Close();
            t1.Abort();
        }
    }
}
