using System.ComponentModel.DataAnnotations;

namespace Model.Efatura.UpdateModels
{
    public class UpdateModelCo
    {
        [Required]
        public string UUID { get; set; }
        public string DocumentId { get; set; }
        public string Status { get; set; }
        [Required]
        public bool Sent { get; set; }
    }
}
