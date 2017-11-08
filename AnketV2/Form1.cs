using DAL;
using Entity.Models;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnketV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Yenile();
            CevaplariYenile();
        }

        public void CevaplariYenile()
        {
            dataGridView2.DataSource = null;
            //dataGridView2.DataSource = db.Cevaplar.ToList();
            dataGridView2.DataSource = db.Cevaplar.Select(x=> new CevapViewModel()
            {
                AdSoyad=x.CevabiVerenKisi.AdSoyad,
                CevapID=x.CevapID,
                Soru =x.Sorusu.SoruCumlesi,
                Cevap =x.Yanit.ToString()
            }).ToList();

        }

        AnketContext db = new AnketContext();

        private void button2_Click(object sender, EventArgs e)
        {
            Soru sorukaydi = new Soru();
            sorukaydi.SoruCumlesi = textBox2.Text;

            db.Sorular.Add(sorukaydi);
            db.SaveChanges();

            MessageBox.Show("Soru Eklendi");
            Yenile();
        }
        public void Yenile()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = db.Sorular.ToList();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Clear();
            foreach (Soru soru in db.Sorular)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = soru.SoruCumlesi;
                lbl.BackColor = Color.Red;
                flowLayoutPanel1.Controls.Add(lbl);

                flowLayoutPanel1.SetFlowBreak(lbl, true);

                RadioButton r1 = new RadioButton();
                r1.Name = "Soru_" + soru.SoruID;
                r1.Text = "Evet";
                r1.Height = 50;
                // flowLayoutPanel1.Controls.Add(r1);

                RadioButton r2 = new RadioButton();
                r2.Name = "Soru_" + soru.SoruID;
                r2.Text = "Hayir";
                r2.Height = 50;
                //  flowLayoutPanel1.Controls.Add(r2);                
                // flowLayoutPanel1.SetFlowBreak(r2, true);

                FlowLayoutPanel p = new FlowLayoutPanel();
                p.Controls.Add(r1);
                p.Controls.Add(r2);
                p.BackColor = Color.Blue;
                flowLayoutPanel1.Controls.Add(p);
                flowLayoutPanel1.SetFlowBreak(p, true);

                //ComboBox c1 = new ComboBox();
                //c1.Items.Add("Evet");
                //c1.Items.Add("Hayır");
                //flowLayoutPanel1.Controls.Add(c1);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            foreach (Control pnl in flowLayoutPanel1.Controls)
            {
                if (pnl is FlowLayoutPanel)
                {
                    foreach (RadioButton item in ((FlowLayoutPanel)pnl).Controls)
                    {
                        
                        
                            RadioButton r = item;
                            if (r.Checked)
                            {
                            //Soru_15 --->15
                                string soruID = item.Name.Replace("Soru_", "");
                                int SID = Convert.ToInt32(soruID);
                                Cevap c = new Cevap();
                                c.SoruID = SID;
                                c.Yanit = r.Text == "Evet" ? Yanit.Evet : Yanit.Hayir;
                                Kisi k = db.Kisiler.Where(x => x.AdSoyad == textBox1.Text).FirstOrDefault();
                                if (k != null)
                                    c.KisiID = k.KisiID;
                                else
                                {
                                    k = new Kisi();
                                    k.AdSoyad = textBox1.Text;
                                    db.Kisiler.Add(k);
                                    db.SaveChanges();
                                    c.KisiID = k.KisiID;
                                }
                                db.Cevaplar.Add(c);
                                db.SaveChanges();
                            }








                        
                    }
                    CevaplariYenile();

                    MessageBox.Show("Eklendi");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//soru sil
            if (dataGridView1.SelectedRows.Count == 0)
                MessageBox.Show("Soru seçiniz");
            else
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    int SoruID = (int)item.Cells[0].Value;
                    Soru silinecek = db.Sorular.Find(SoruID);
                    db.Sorular.Remove(silinecek);
                }
                db.SaveChanges();
                Yenile();
                CevaplariYenile();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//düzenle
            if (dataGridView1.SelectedRows.Count == 0)
                MessageBox.Show("Soru seçiniz");
            else
            {
                SoruDuzenle sd = new SoruDuzenle();
                foreach(DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    int SoruID = (int)item.Cells[0].Value;
                    //int secilenID = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    Soru iletilecek = db.Sorular.Find(SoruID);
                    sd.GelenSoru = iletilecek;
                    
                    sd.Show();
                    

                }
                
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {//cevap sil
            if (dataGridView2.SelectedRows.Count == 0)
                MessageBox.Show("Cevap seçiniz");
            else
            {
                List<Cevap> silinecekler = new List<Cevap>();

                foreach (DataGridViewRow item in dataGridView2.SelectedRows)
                {
                    var silinecek = db.Cevaplar.ToList()[item.Index];
                    silinecekler.Add(silinecek);
                    //int CevapID = (int)item.Cells[0].Value;
                    //Cevap silinecek = db.Cevaplar.Find(CevapID);
                    //db.Cevaplar.Remove(silinecek);
                }
                db.Cevaplar.RemoveRange(silinecekler);
                db.SaveChanges();
                //db.SaveChanges();
                //Yenile();
                CevaplariYenile();
            }
        }
    }
}
