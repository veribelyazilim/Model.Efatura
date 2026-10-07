using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Efatura.EDefter
{
    public class EDefterCo
    {
        /// <summary>Dönem başlangıcı (dahil).</summary>
        [Required]
        public DateTime? StartDate { get; set; }

        /// <summary>Dönem bitişi (dahil).</summary>
        [Required]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// İsteğe bağlı sayfa numarası (1'den başlar). Boşsa ilk sayfa. PageSize ile birlikte kullanılır.
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// İsteğe bağlı sayfa boyutu (SATIR sayısı). Boş bırakılırsa dönemin tamamı tek yanıtta döner.
        /// Bir yevmiyenin satırları iki sayfaya yayılabilir.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Önceki dönemin son satır sayacı (LINENUMBERCOUNTER). Yıl başı için 0.
        /// Bu dönemin sayaçları PreviousLineNumber + 1'den başlar. Sayfalar arasında değişmez.
        /// </summary>
        public int PreviousLineNumber { get; set; }

        /// <summary>Prosedürün veri doğrulamalarını (dengesiz fiş, eksik yevmiye no vb.) çalıştırır.</summary>
        public bool Validate { get; set; } = true;
    }
}
