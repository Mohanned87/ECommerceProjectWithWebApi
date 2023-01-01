using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;

        public UserService(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<IEnumerable<UserDetailDto>> GetListAsync()
        {
            List<UserDetailDto> UserDetailDtos = new List<UserDetailDto>();
            var response = await _userDal.GetListAsync();
            foreach (var item in response.ToList())
            {
                UserDetailDtos.Add(new UserDetailDto()
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender == true ? "Male" : "Female",
                    DateOfBirth = item.DateOfBirth,
                    Username = item.Username,
                    Address = item.Address,
                    Email = item.Email,
                    Id = item.Id
                });
            }
            return UserDetailDtos;
        }
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user =await _userDal.GetAsync(x => x.Id == id);
            UserDto userDto = new UserDto()
            {
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                Gender = user.Gender,
                Id = user.Id,
                LastName = user.LastName,
                Username = user.Username,
            };
            return userDto;
        }

        public async Task<UserDto> AddAsync(UserAddDto userAddDto)
        {
            User user = new User()
            {
                LastName = userAddDto.LastName,
                Address = userAddDto.Address,
                //Todo:CreatedDate & CreatedUserId will edit.
                CreatedDate = DateTime.Now,
                CreatedUserId = 1,
                DateOfBirth = userAddDto.DateOfBirth,
                Email = userAddDto.Email,
                FirstName = userAddDto.FirstName,
                Gender= userAddDto.Gender,
                Password = userAddDto.Password,
                Username = userAddDto.Username
            };

            var userAdd = await _userDal.AddAsync(user);
            UserDto userDto = new UserDto()
            {
                LastName = userAdd.LastName,
                Address = userAdd.Address,
                DateOfBirth = userAdd.DateOfBirth,
                Email = userAdd.Email,
                FirstName = userAdd.FirstName,
                Gender = userAdd.Gender,
                Username = userAdd.Username,
                Id= userAdd.Id,
            };
            return userDto;
        }

        public async Task<UserUpdateDto> UpdateAsync(UserUpdateDto userUpdateDto)
        {
            var getUser=await _userDal.GetAsync(x=> x.Id==userUpdateDto.Id);
            User user = new User()
            {
                LastName = userUpdateDto.LastName,
                Address = userUpdateDto.Address,
                DateOfBirth = userUpdateDto.DateOfBirth,
                Email = userUpdateDto.Email,
                FirstName = userUpdateDto.FirstName,
                Gender = userUpdateDto.Gender,
                Username = userUpdateDto.Username,
                Id = userUpdateDto.Id,
                CreatedDate=getUser.CreatedDate,
                CreatedUserId=getUser.CreatedUserId,
                Password=userUpdateDto.Password,
                UpdatedDate=DateTime.Now,
                UpdatedUserId=1,
            };
            var userUpdate = await _userDal.UpdateAsync(user);
            UserUpdateDto newUserUpdateDto = new UserUpdateDto()
            {
                LastName = userUpdate.LastName,
                Address = userUpdate.Address,
                DateOfBirth = userUpdate.DateOfBirth,
                Email = userUpdate.Email,
                FirstName = userUpdate.FirstName,
                Gender = userUpdate.Gender,
                Username = userUpdate.Username,
                Id = userUpdate.Id,
                Password = userUpdate.Password,
            };
            return newUserUpdateDto;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _userDal.DeleteAsync(id);
        }

    }
}
