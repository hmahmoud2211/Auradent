using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public interface IdataHelper<Table>
    {
        // read all data from the table
        Task<List<Table>> GetAllData();
        // read data from the table by id
        Task<List<Table>> GetDataById(int id);
        // insert data into the table
        Task InsertData(Table data);
        // update data in the table
        Task UpdateData(Table data);
        // delete data from the table
        Task DeleteData(int id);
        // search data in the table
        Task<List<Table>> SearchData(string search);
        // check if data exists in the table
        Task<bool> DataExists(int id);
        // find the data in the table
        Task<Table> FindData(int id);
    }

}

