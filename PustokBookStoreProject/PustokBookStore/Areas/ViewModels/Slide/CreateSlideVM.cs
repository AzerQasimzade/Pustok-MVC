using System.ComponentModel.DataAnnotations.Schema;

namespace PustokBookStore.Areas.ViewModels
{
    public class CreateSlideVM
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Desc { get; set; }
        public int Order { get; set; }
        public IFormFile Photo { get; set; }
    }
}
