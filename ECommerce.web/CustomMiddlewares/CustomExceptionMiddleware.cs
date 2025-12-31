using ECommerce.Domain.Exceptions;
using ECommerce.Shared.ErrorModels;
using Microsoft.Extensions.Logging;

namespace ECommerce.web.CustomMiddlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<CustomExceptionMiddleware> logger;

        public CustomExceptionMiddleware(RequestDelegate Next,ILogger<CustomExceptionMiddleware> logger)
        {
            next = Next;
            this.logger = logger;
        }
        public async Task Invoke ( HttpContext context)
        {
            try
            {
                await next.Invoke(context); // بنادي علي المديلوير الللي بعدي 
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                   #region Response Body
                    //Response Object 
                    var Response = new ErrorToReturn()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"EndPoint{context.Request.Path} Is Not Found",

                    };
                    //Return Object As JSON
                    await context.Response.WriteAsJsonAsync(Response);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);

                //Set Message For Response
                var Response = new ErrorToReturn()
                {
                    Message = ex.Message
                };
                //Set status Code For Response
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    BadRequestException badRequestException=> GetBadRequestErrors(badRequestException,Response),
                    _ => StatusCodes.Status500InternalServerError
                };
                Response.StatusCode = context.Response.StatusCode;

                //Set Content Type For Response
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(Response);

            }

          // before BadRequest 
           /*             try
            {
                await next.Invoke(context); // بنادي علي المديلوير الللي بعدي 
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    #region Response Body
                    //Response Object 
                    var Response = new ErrorToReturn()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"EndPoint {context.Request.Path} Is Not Found",

                    };
                    //Return Object As JSON
                    await context.Response.WriteAsJsonAsync(Response);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);

                #region Response Header
                //Set status Code For Response
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };
                //Set Content Type For Response
                context.Response.ContentType = "application/json";
                #endregion

                #region Response Body
                //Response Object 
                var Response = new ErrorToReturn()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,

                };
                //Return Object As JSON
                await context.Response.WriteAsJsonAsync(Response);
                #endregion

            }*/
             
        }

        private int GetBadRequestErrors(BadRequestException exception,ErrorToReturn response)
        {
            response.Errors = exception.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
