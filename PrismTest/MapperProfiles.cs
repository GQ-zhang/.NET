using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dao.Model.Dto;
using Dao.Model.Entities;

namespace PrismTest
{
    public class MapperProfiles:Profile
    {
        public MapperProfiles()
        {
            CreateMap<SystemUserDto, SystemUserDao>();
            CreateMap<SystemUserDao, SystemUserDto>();
        }
    }
}
