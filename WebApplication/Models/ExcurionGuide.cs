using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication
{
    public partial class ExcurionGuide
    {
        public ExcurionGuide()
        {
            Excursion = new HashSet<Excursion>();
        }

        public int Id { get; set; }
        public int Idguide { get; set; }
        public int Idpattern { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Guide")]
        public virtual Guide IdguideNavigation { get; set; }
        [Display(Name = "Pattern")]
        public virtual Pattern IdpatternNavigation { get; set; }
        public virtual ICollection<Excursion> Excursion { get; set; }
    }
}
