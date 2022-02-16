using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    public  class Files
    {
        private int id;

        private string namefile;

        private string pathfile;

        private string type;

        private int catalogid;

        [Column("Id")]
        public int Id
        {
            get => id;
            set => id = value;
        }


        [Column("namefile")]

        public string NameFile
        {
            get => namefile;
            set => namefile = value;

        }
        [Column("pathfile")]
        public string PathFile
        {
            get => pathfile;
            set => pathfile = value;
        }
        [Column("typemask")]
        public string Type
        {
            get => type;
            set => type = value;
        }
        [Column("catalogid")]
        public int CatalogId
        {
            get => catalogid;
            set => catalogid = value;
        }





    }
}
