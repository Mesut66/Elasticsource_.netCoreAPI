using System.Net;

namespace Elasticsource.API.DTO
{
    public record ResponseDto<T>
    {
        public T? Data { get; set; }

        public List<string>? Errors { get; set; }
        public HttpStatusCode Status { get; set; }

        //bu static factory method, başarılı bir yanıt oluşturmak için kullanılabilir. Verilen veri ve durum kodu ile yeni bir ResponseDto nesnesi döndürür.
        public static ResponseDto<T> Success(T data,HttpStatusCode status)
        {
            return new ResponseDto<T>
            {
                Data = data,
                Status = status,
            };
        }

        public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode status)
        {
            return new ResponseDto<T>
            {
                Errors = errors,
                Status = status,
            };
        }
    }
}
