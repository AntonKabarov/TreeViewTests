using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    [Table("catalog")]
    public class Catalog
    {
        private int id;

        private string name;

        [Column("Id")]
        public int Id
        {
            get => id;
            set => id = value;
        }
        [Column("Names")]
        public string Name
        {
            get => name;
            set => name = value;
        }

    }
}
