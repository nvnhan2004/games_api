using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Games.Data.Models;
using Games.Data.Models.QTHT;
//using Games.Services.MailKit;
//using Games.Services.Models.MailKit;
//using Games.Services.Services.QTHT;
//using Games.Services.Services.TaiChinh;
//using Games.Services.Services.TaiChinh.DanhMuc.ChiTieu;
//using Games.Services.Services.TaiChinh.DanhMuc.ThuNhap;
//using Games.Services.Services.Users;
using System.Data;
using System;
using System.Text;
using Games.Services.Services.MailKit.DTO;
using Games.Services.Services.Authentication;
using Games.Services.Services.MailKit;
using Microsoft.Extensions.Options;
using Games.Services.Services.Users;
using Games.Services.Services.DieuHuong;
using Games.Services.Services.NhomNguoiDung;
using Games.Services.Services.Categories;
using Games.Services.Services.Games;
using Games.Services.Services.Categories.DTO;

var builder = WebApplication.CreateBuilder(args);
IHttpContextAccessor accessor;

// For Entity Framework
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr"));
});

// For Identity
builder.Services.AddIdentity<NguoiDung, Roles>(options => options.Stores.ProtectPersonalData = false)
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

// Add config for required Email
builder.Services.Configure<IdentityOptions>(options => options.SignIn.RequireConfirmedEmail = true);

builder.Services.AddHttpContextAccessor();
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:4000", "https://games-six-delta.vercel.app")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Auto Mapper Configurations
//var mappingConfig = new MapperConfiguration(mc =>
//{
//    mc.AddProfile(new UsersProfile());
//    mc.AddProfile(new DieuHuongProfile());
//});
//IMapper mapper = mappingConfig.CreateMapper();
//builder.Services.AddSingleton(mapper);

// thời hạn hết hạn của 1 số token liên quan đến đặt lại pass, xác nhận tài khoản (mặc định là 1 ngày)
builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(10));

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // yêu cầu user xác thực khi chưa login hoặc yêu cầu quyền cụ thể khi user chưa có

    //options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme; // từ chối, cấm
    //options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme; // login
    //options.DefaultSignOutScheme= JwtBearerDefaults.AuthenticationScheme; // logout

    // sử dụng DefaultScheme nếu muốn khai báo tất cả các Default còn lại chung 1 giá trị
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options => {
    options.SaveToken = true; // cho phép lư token dưới trình duyệt
    options.RequireHttpsMetadata = false; // cho phép xác thực token với http
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, // kiểm tra token còn thời hạn ko
        ValidateIssuerSigningKey = true, // kiểm tra xem chữ ký của token có được tạo bằng khóa ký bí mật (Secret) hợp lệ của bên cấp hành hay không
        /*
         * ClockSkew: xác định khoảng thời gian lệch clock được phép giữa server xác nhận token và server ủy quyền đã tạo token
         * server ủy quyền (Identity Provider (IdP) hoặc Authentication Server): 
         *      - Xác thực người dùng dựa trên thông tin đăng nhập (tên người dùng, mật khẩu, v.v.)
         *      - Cấp token JWT cho người dùng đã được xác thực thành công
         * server xác nhận (API Server hoặc Resource Server): xác nhận tính hợp lệ của token JWT được gửi bởi người dùng (browser)
         *      - Xác nhận tính hợp lệ của token JWT được gửi bởi người dùng khi họ truy cập tài nguyên
         *      - Giải mã token JWT để lấy thông tin về danh tính và quyền truy cập của người dùng
         *      - Kiểm tra xem người dùng có quyền truy cập vào tài nguyên được yêu cầu hay không
         *      - Cho phép hoặc từ chối truy cập tài nguyên dựa trên kết quả kiểm tra
         * cả 2 đều nằm phía server ko phải client
         * sử dụng ClockSkew để bù đắp thời gian lệch khi các máy chủ ko cùng múi giờ
         */
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
    //options.Events = new JwtBearerEvents
    //{
    //    OnAuthenticationFailed = context =>
    //    {
    //        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
    //        {
    //            //context.Response.Headers.Add("Token-Expired", "True");
    //            //context.Response.StatusCode = 200;
    //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //            context.Response.ContentType = "application/json";
    //            context.Response.WriteAsync("EXPIRED_TOKEN");
    //        }
    //        return Task.CompletedTask;
    //    }
    //};
});

// Add email config
builder.Services.AddSingleton(builder.Configuration.GetSection("EmailConfiguaration").Get<EmailConfiguarationDTO>());
builder.Services.AddSingleton(builder.Configuration.GetSection("CategoriesConfiguaration").Get<CategoriesConfiguarationDTO>());
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDieuHuongService, DieuHuongService>();
builder.Services.AddScoped<INhomNguoiDungService, NhomNguoiDungService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IGamesService, GamesService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


#region config swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "NhanNV API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
