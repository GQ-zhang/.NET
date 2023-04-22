using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model.Dto
{

    public class SystemUserDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
   
        public string UserName { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
      
        public string UserAccount { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
    
        public string UserPassword { get; set; }
        /// <summary>
        /// 用户角色 1-管理员 2-普通用户
        /// </summary>
       
        public int UserRole { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
      
        public DateTime UpdateDate { get; set; }
    }
}
