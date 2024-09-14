using System.Net;

namespace Games.Services.Models
{
    public static class ErrorCtr
    {
        public static void Reject(HttpStatusCode statusCode, string error, string description)
        {
            var ex = new Exception(string.Format("{0}", description));
            ex.Data.Add("status", statusCode);
            ex.Data.Add("error", error);
            ex.Data.Add("description", description);
            throw ex;
        }
        public static void InvalidForeignKey(string description = "Lỗi liên kết khóa ngoại, không thể cập nhật lại được database!")
        {
            var ex = new Exception(string.Format("{0}", description));
            ex.Data.Add("status", HttpStatusCode.BadRequest);
            ex.Data.Add("error", ErrorCode.invalid_foreign_key);
            ex.Data.Add("description", description);
            throw ex;
        }
        public static void DataExits(string description = "Dữ liệu đã có. Vui lòng kiểm tra lại thông tin!")
        {
            var ex = new Exception(string.Format("{0}", description));
            ex.Data.Add("status", HttpStatusCode.BadRequest);
            ex.Data.Add("error", ErrorCode.data_exits);
            ex.Data.Add("description", description);
            throw ex;
        }
        public static void NotFound(string description = "Dữ liệu không có trong hệ thống. Vui lòng kiểm tra lại thông tin!")
        {
            var ex = new Exception(string.Format("{0}", description));
            ex.Data.Add("status", HttpStatusCode.NotFound);
            ex.Data.Add("error", ErrorCode.not_found);
            ex.Data.Add("description", description);
            throw ex;
        }
        public static ErrorInfo ExtractErrorInfo(Exception exc)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string error = "An internal error has occurred";
            var ex = exc;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            string description = ex.Message;
            if (exc.Data.Contains("status"))
            {
                statusCode = (HttpStatusCode)exc.Data["status"];
            }
            if (exc.Data.Contains("error"))
            {
                error = Convert.ToString(exc.Data["error"]);
            }
            if (exc.Data.Contains("description"))
            {
                description = Convert.ToString(exc.Data["description"]);
            }
            return new ErrorInfo
            {
                statusCode = statusCode,
                error = error,
                description = description

            };

        }


    }
    public static class ErrorCode
    {
        public const string invalid_foreign_key = "invalid_foreign_key";
        public const string data_exits = "data_exits";
        public const string not_found = "not_found";
    }

    public class ErrorInfo
    {
        public ErrorInfo() { }
        public ErrorInfo(HttpStatusCode statusCode, string error, string description)
        {
            this.statusCode = statusCode;
            this.error = error;
            this.description = description;
        }
        public HttpStatusCode statusCode { get; set; }
        public string error { get; set; }
        public string description { get; set; }
    }

    public class ResponseList<T>
    {
        public Meta meta { get; set; }
        public List<T> data { get; set; }
        public ResponseList()
        {
        }
        public ResponseList(Meta _meta, List<T> _data)
        {
            this.meta = _meta;
            this.data = _data;
        }
    }
    public class Meta
    {
        public int page { get; set; }
        public int page_size { get; set; }
        public Ranger ranger { get; set; }
        public int total { get; set; }
        public int total_page { get; set; }
        public Meta() { }
        public Meta(int _page, int _page_size, int _total)
        {
            ranger = new Ranger();
            page = _page;
            page_size = _page_size;
            total = _total;
            if (_page_size == 0 || _page == 0)
            {
                total_page = 1;
                if (_total == 0)
                {
                    ranger.from = 0;
                }
                else
                {
                    ranger.from = 1;
                }
                ranger.to = _total;
            }
            else
            {
                total_page = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_total) / Convert.ToDouble(_page_size)));
                if (_total == 0)
                {
                    ranger.from = 0;
                }
                else
                {
                    ranger.from = (_page - 1) * _page_size + 1;
                }
                ranger.to = (_page - 1) * _page_size + _page_size;
                if (_total < ranger.to)
                    ranger.to = _total;
                if (total_page < page)
                {
                    ranger.from = 0;
                    ranger.to = 0;
                }
            }
        }
    }
    public class Ranger
    {
        public int from { get; set; }
        public int to { get; set; }
    }
    public class ApiResponseDTO<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public T? Response { get; set; }
    }
    public class ResponseDTO
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
