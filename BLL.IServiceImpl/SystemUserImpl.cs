using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.IService;
using Dao.Model.Dto;
using Dao.Model.Entities;
using Dao.Repository;

namespace BLL.IServiceImpl
{
    public class SystemUserImpl:BaseRepository<SystemUserDao>, ISystemUser
    {
        private  readonly IMapper _mapper;
        public SystemUserImpl(IMapper mapper)
        {
            _mapper=mapper;
        }
        public async Task<List<SystemUserDto> > SearchUserInfo()
        {
            var result = await base.GetListAsync();
            return _mapper.Map<List<SystemUserDto>>(result);
        }

        public Task<bool> InsertUserInfo(SystemUserDto info)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserInfo(SystemUserDto info)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserInfo(SystemUserDto info)
        {
            throw new NotImplementedException();
        }
    }
}
