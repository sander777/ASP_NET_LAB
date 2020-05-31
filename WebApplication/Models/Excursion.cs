using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication
{
    public partial class Excursion
    {
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "Гид - Паттерн")]
        public int IdexcursionGuide { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле на може бути порожнім")]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        public virtual ExcurionGuide IdexcursionGuideNavigation { get; set; }
    }
    public class ExGdBox
    {
        public ExcurionGuide ed { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
    };

    public class ExGdPtBox
    {
        public Excursion ex { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    };
}
