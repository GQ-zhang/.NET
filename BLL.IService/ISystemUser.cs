using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Model.Dto;

namespace BLL.IService
{
    public interface ISystemUser
    {
        Task<SystemUserDto> SearchUserInfo();

        Task<bool> InsertUserInfo(SystemUserDto info);

        Task<bool> UpdateUserInfo(SystemUserDto info);

        Task<bool> DeleteUserInfo(SystemUserDto info);
    }
}
