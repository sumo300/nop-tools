using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace Sumo.Nop.MediaToolsCore.Models {
    [Table("Setting")]
    public partial class Setting {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public int StoreId { get; set; }
    }
}