using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using SqlSugar.IOC;

namespace Dao.Repository
{
    public class BaseRepository<T> : SimpleClient<T> where T : class,new()
    {
        public BaseRepository(ISqlSugarClient context = null) : base(context)
        {
            //该模式下无需关注线程
            Context = DbScoped.SugarScope;
        }
    }
   
}
