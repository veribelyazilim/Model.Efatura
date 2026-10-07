using System.Collections.Generic;

namespace Model.Efatura.EDefter
{
    public class EDefterResultDto
    {
        public List<EDefterLineDto> Items { get; set; } = new();

        /// <summary>Dönemdeki toplam satır sayısı (sayfadan bağımsız, her yanıtta dolu).</summary>
        public int TotalCount { get; set; }

        /// <summary>Sayfalama kullanıldıysa toplam sayfa sayısı (ceil(TotalCount / PageSize)), kullanılmadıysa boş.</summary>
        public int? PageCount { get; set; }

        /// <summary>
        /// Dönemin son satır sayacı (PreviousLineNumber + TotalCount). Bir sonraki dönem çağrılırken
        /// PreviousLineNumber olarak verilmelidir.
        /// </summary>
        public int LastLineNumber { get; set; }
    }
}
