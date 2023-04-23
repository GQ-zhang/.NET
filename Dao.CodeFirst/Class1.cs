using Dao.Model.Entities;
using Dao.Repository;

namespace Dao.CodeFirst
{
    public class DatabaseInit:BaseRepository<SystemUserDao>
    {
        /// <summary>
        /// 建表
        /// </summary>
        public void Init()
        {
            Context.DbMaintenance.CreateDatabase();
            Context.CodeFirst.SetStringDefaultLength(200)
                .InitTables(typeof(SystemUserDao));
        }
    }
}