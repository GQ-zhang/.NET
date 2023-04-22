using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Dao.Model.Entities
{
    /// <summary>
    /// 用户信息实体
    /// </summary>
    [SugarTable("t_systemuser")]
    public class SystemUserDao
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键")]
        public string Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(ColumnDescription = "用户名")]
        public string UserName { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        [SugarColumn(ColumnDescription = "登录账号")]
        public string UserAccount { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [SugarColumn(ColumnDescription = "登录密码")]
        public string UserPassword { get; set; }
        /// <summary>
        /// 用户角色 1-管理员 2-普通用户
        /// </summary>
        [SugarColumn(ColumnDescription = "用户角色 1-管理员 2-普通用户")]
        public int UserRole { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDescription = "更新时间")]
        public DateTime UpdateDate { get; set; }

    }
}
