using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebApplication
{
    public partial class Pattern
    {
        public Pattern()
        {
            ExcurionGuide = new HashSet<ExcurionGuide>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "Тривалість")]
        public TimeSpan Duration { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "Макс. к-сть людей")]
        public int MaxSize { get; set; }

        public virtual ICollection<ExcurionGuide> ExcurionGuide { get; set; }
    }
}
