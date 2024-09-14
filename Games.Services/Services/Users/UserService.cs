using AutoMapper;
using Games.Data.Models;
using Games.Data.Models.QTHT;
using Games.Services.Common;
using Games.Services.Models;
using Games.Services.Services.DieuHuong.DTO;
using Games.Services.Services.Users.DTO;

namespace Games.Services.Services.Users
{
    public class UserService : GenericService<NguoiDung>, IUserService
    {
        public UserService(AppDbContext context) : base(context)
        {
            _mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BaseProfile>();
                cfg.AddProfile<UsersProfile>();
                cfg.AddProfile<DieuHuongProfile>();
            });
            _mapper = _mapperCfg.CreateMapper();
            _mapperCfg.AssertConfigurationIsValid();
        }

        protected override IQueryable<NguoiDung> QueryBuilder(IQueryable<NguoiDung> query, dynamic filter, string search)
        {
            query = query.Where(x => x.super_admin != true);
            if (filter != null)
            {
                String username = filter.username;
                if (username != null && username != "")
                {
                    username = username.ToLower().Trim();
                    query = query.Where(x => x.UserName.ToLower().Contains(username));
                }
                String email = filter.email;
                if (!string.IsNullOrEmpty(email))
                {
                    email = email.ToLower().Trim();
                    query = query.Where(x => x.Email.ToLower().Contains(email));
                }
                String phonenumber = filter.phonenumber;
                if (!string.IsNullOrEmpty(phonenumber))
                {
                    phonenumber = phonenumber.ToLower().Trim();
                    query = query.Where(x => x.PhoneNumber.ToLower().Contains(phonenumber));
                }
                String nnd_id = filter.nnd_id;
                if (!string.IsNullOrEmpty(nnd_id))
                {
                    Guid nhomNguoiDungId = Guid.Parse(nnd_id);
                    query = query.Where(x => x.ds_nhom_nguoi_dung.Any(y => y.nhom_nguoi_dung_id == nhomNguoiDungId));
                }
            }
            if (search != null && search != "")
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.UserName.ToLower().Contains(search) || x.Email.ToLower().Contains(search) ||
                                     x.PhoneNumber.ToLower().Contains(search));
            }
            return query;
        }

        public UserDTO GetById(Guid id)
        {
            var user = new UserDTO();
            user = _mapper.Map<UserDTO>(getUser(id));
            return user;
        }

        public void Create(UserDTO model)
        {

        }

        public UserDTO Update(Guid id, UserDTO model)
        {
            var user = getUser(id);
            user.Email = model.email;
            user.PhoneNumber = model.phonenumber;

            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return _mapper.Map<UserDTO>(user);
        }

        public void Delete(Guid id)
        {
            var user = getUser(id);
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public void Deletes(List<Guid> ids)
        {
            var users = _dbContext.Users.Where(x => ids.Contains(x.Id));
            _dbContext.Users.RemoveRange(users);
            _dbContext.SaveChanges();
        }

        // helper methods

        private NguoiDung getUser(Guid id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public List<string> GetPermissionUser(Guid id)
        {
            try
            {
                var res = new List<PermissionUserDTO>();
                var currentUser = _repo.Find(id);

                if (currentUser.super_admin == true)
                {
                    var ltsDieuHuong = _dbContext.DieuHuong
                    .Where(x => x.is_router == true)
                    .OrderBy(x => x.so_thu_tu).ToList();
                    if (ltsDieuHuong.Any())
                    {
                        res = _mapper.Map<List<Data.Models.QTHT.DieuHuong>, List<PermissionUserDTO>>(ltsDieuHuong);
                    }
                }
                else
                {
                    var ltsDieuHuong = _dbContext.DieuHuong
                    .Where(x => x.is_router == true &&
                                x.ds_nhom_nguoi_dung.Any(y => y.nhom_nguoi_dung.ds_nguoi_dung.Any(t => t.nguoi_dung_id == id)))
                    .OrderBy(x => x.so_thu_tu).ToList();
                    if (ltsDieuHuong.Any())
                    {
                        res = _mapper.Map<List<Data.Models.QTHT.DieuHuong>, List<PermissionUserDTO>>(ltsDieuHuong);
                    }
                }

                return res.Select(x => x.ma_quyen).OfType<string>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<NguoiDungSelectDTO> GetAllNguoiDung()
        {
            try
            {
                var query = _repo.Where(x => x.super_admin != true).AsQueryable().ToList();
                var res = _mapper.Map<List<NguoiDung>, List<NguoiDungSelectDTO>>(query);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
