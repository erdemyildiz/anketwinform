using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    
    class AnketKayit
    {
    }
    [Table("tblSorular")] //db de oluşacak tablonun adı
    public class Soru
    {
        [Key]
        public int SoruID { get; set; }
        [Required] //SoruCumlesi zorunlu
        public string SoruCumlesi { get; set; }


    }
    [Table("tblKisiler")]
    public class Kisi
    {
        public int KisiID { get; set; }
        [Required]
        public string AdSoyad { get; set; }

    }
    [Table("tblCevaplar")]
    public class Cevap
    {
        [Key]
        public int CevapID { get; set; }
        [ForeignKey("CevabiVerenKisi")] //bu FK nın dolduracağı virtual Navigation Property CevabiVerenKisi'dir.
        public int KisiID { get; set; }
        [ForeignKey("Sorusu")]
        public int SoruID { get; set; }
        public Yanit Yanit {get;set;}

        public virtual Kisi CevabiVerenKisi { get; set; }
        public virtual Soru Sorusu { get; set; }

    }
    public enum Yanit
    { Hayir,
      Evet
    }
}
