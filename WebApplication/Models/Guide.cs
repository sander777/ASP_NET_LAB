using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication
{
    public partial class Guide
    {
        public Guide()
        {
            ExcurionGuide = new HashSet<ExcurionGuide>();
        }
        

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "ПІП")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ExcurionGuide> ExcurionGuide { get; set; }
    }
    public class box
    {
        public Pattern pattern { get; set; }
        public decimal price { get; set; }
        public bool check { get; set; }
    };
}
