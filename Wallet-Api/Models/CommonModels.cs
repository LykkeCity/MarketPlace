
using System;

namespace Wallet_Api.Models
{


    public class MyWebException : Exception
    {

        public MyWebException(ResponseModel.ErrorCodeType codeType, string message):base(message)
        {
            CodeType = codeType;
        }

        public ResponseModel.ErrorCodeType CodeType { get; private set; }
    }


    public class ResponseModel
    {
        public enum ErrorCodeType
        {
            InvalidInputField, InconsistentData, NotAuthenticated
        }


        public class ErrorModel
        {


            public ErrorCodeType Code { get; set; }
            /// <summary>
            /// In case ErrorCoderType = 0
            /// </summary>
            public string Field { get; set; }
            /// <summary>
            /// Localized Error message
            /// </summary>
            public string Message { get; set; }
        }

        public ErrorModel Error { get; set; }

        public static ResponseModel CreateInvalidFieldError(string field, string message)
        {
            return new ResponseModel
            {
               Error = new ErrorModel
               {
                   Code = ErrorCodeType.InvalidInputField,
                   Field = field,
                   Message = message
               }
            };
        }

        public static ResponseModel CreateFail(ErrorCodeType errorCodeType, string message)
        {
            return new ResponseModel
            {
                Error = new ErrorModel
                {
                    Code = errorCodeType,
                    Message = message
                }
            };
        }

        private static readonly ResponseModel OkInstance = new ResponseModel();

        public static ResponseModel CreateOk()
        {
            return OkInstance;
        }

    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Result { get; set; }

        public static ResponseModel<T> CreateOk(T result)
        {
            return new ResponseModel<T>
            {
                Result = result
            };
        }

        public new static ResponseModel<T> CreateInvalidFieldError(string field, string message)
        {
            return new ResponseModel<T>
            {
                Error = new ErrorModel
                {
                    Code = ErrorCodeType.InvalidInputField,
                    Field = field,
                    Message = message
                }
            };
        }

        public new static ResponseModel<T> CreateFail(ErrorCodeType errorCodeType, string message)
        {
            return new ResponseModel<T>
            {
                Error = new ErrorModel
                {
                    Code = errorCodeType,
                    Message = message
                }
            };
        }
    }




    public class OkResponseModel
    {
        public string result { get; set; }
        public static readonly OkResponseModel Instance = new OkResponseModel {result = "ok"};
    }

    public class OkResponseModel<T> : OkResponseModel
    {
        public T data { get; set; }

        public static OkResponseModel<T> Create(T data)
        {
            return new OkResponseModel<T>
            {
                data = data,
                result = "ok-bool"
            };
            
        }
    }

}