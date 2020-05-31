using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication
{
    public partial class Category
    {
        public Category()
        {
            Guide = new HashSet<Guide>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display( Name = "Назва")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "Заробітна Плата")]
        public decimal Salary { get; set; }

        public virtual ICollection<Guide> Guide { get; set; }
    }
}
