using DAL;
using Entity.Models;
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

namespace AnketV2
{
    public partial class SoruDuzenle : Form
    {
        public Soru GelenSoru { get; set; }

        public SoruDuzenle()
        {
            InitializeComponent();
        }

        private void SoruDuzenle_Load(object sender, EventArgs e)
        {
            textBox2.Text = GelenSoru.SoruCumlesi;
        }

        private void button2_Click(object sender, EventArgs e)
        {//soru düzenle kaydet
            //EF bir kayıtta değişiklik yapabilmesi CONTEXT üzerinden geliyorsa mümkün
            AnketContext db = new AnketContext();
            var duzenlenecek = db.Sorular.Find(GelenSoru.SoruID);
            duzenlenecek.SoruCumlesi = textBox2.Text;
            db.Entry(duzenlenecek).State = EntityState.Modified;
            db.SaveChanges();
            Form1 f =(Form1) Application.OpenForms["Form1"];
            f.Yenile();
            f.CevaplariYenile();
            this.Close();
        }
    }
}
