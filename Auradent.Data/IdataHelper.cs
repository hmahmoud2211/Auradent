using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public interface IdataHelper<Table>
    {
        public List<Table> GetAllData();

        public List<Table> Search(string searchItem);

        public Table Find(Table item);

        public bool CheckIfIdExists(int id);



        //Writing
        public List<Table> Add_list (Table table);
        public int Add(Table table);
        public int Update(Table table);
        public int Delete(Table item);
    }

}

