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

        public async Task<bool> InsertUserInfo(SystemUserDto info)
        {
            return  await base.InsertAsync(_mapper.Map<SystemUserDao>(info));
        }

        public async  Task<bool> UpdateUserInfo(SystemUserDto info)
        {
            return await base.UpdateAsync(m=> new SystemUserDao() 
            {
                UpdateDate = DateTime.Now,
                UserName = info.UserName,
                UserPassword = info.UserPassword,
            },s=>s.Id==info.Id);
        }

        public  async Task<bool> DeleteUserInfo(SystemUserDto info)
        {
            return await base.DeleteByIdAsync(info.Id);
        }

   
    }
}
